using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_TownHall : Building, ISpawnUnit, IResidential
{
    public override void Initialize()
    {
        base.Initialize();

        nameActor = "Town Hall";
        spawnOrder = 0;

        // fill busy grid 
        BuildSystem.current.BusyTakeArea(this);
        Place();
        Built();

        // initialize storage
        storages = new List<TH_Storage>();
        foreach(TH_Storage storage in GetComponents<TH_Storage>())
        {
            storage.Initialize(this);
            storages.Add(storage);
            StorageSystem.current.AddStorage(storage, storage.GetTypeResource());
            ResourceSystem.current.AddResources(storage.GetTypeResource(), 0);
        }
        ResourceSystem.current.AddResources(E_Resource.Food, 100);
        ResourceSystem.current.AddResources(E_Resource.Stone, 100);
        ResourceSystem.current.AddResources(E_Resource.Wood, 100);

        // ininitialize processor
        processors = new List<TH_Processor>();
        foreach(TH_Processor processor in GetComponents<TH_Processor>())
        {
            processor.Initialize(this);
            processors.Add(processor);
            ProcessorSystem.current.AddProcessor(processor.GetTypeResource(), processor);
        }

        // intialize residential
        VillageSystem.current.AddResidential(this);
    } 

    #region SpawnUnit

    public Action UpdateSpawnOrder;
    public Action<float> UpdateSpawnProgress;

    [SerializeField] private GameObject spawnUnitPref;
    [SerializeField] private float timeToSpawnUnit;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private S_Cost spawnCost;
    private Coroutine spawning;
    public int spawnOrder { get; private set; }

    public void SpawnUnit()
    {
        spawnOrder++;
        UpdateSpawnOrder?.Invoke();

        if(spawnOrder == 1)
        {
            spawning = StartCoroutine(Spawning());
        }
    }

    public void CancelSpawnUnit()
    {
        spawnOrder = Mathf.Max(0, spawnOrder - 1);
        UpdateSpawnOrder?.Invoke();

        if(spawnOrder == 0 && spawning != null)
        {
            ResourceSystem.current.AddResources(spawnCost.resourse, spawnCost.count);
            UpdateSpawnProgress?.Invoke(1f);

            StopCoroutine(spawning);
            spawning = null;
        }
    }

    private IEnumerator Spawning()
    {
        while(spawnOrder > 0 && VillageSystem.current.CheckFreeSpace())
        {
            if(StorageSystem.current.CheckCountResurces(spawnCost.resourse) <= spawnCost.count)
            {
                break;
            }
            
            ResourceSystem.current.RemoveResources(spawnCost.resourse, spawnCost.count);

            float timer = 0;

            while(timer < timeToSpawnUnit)
            {
                timer += Time.deltaTime;
                UpdateSpawnProgress?.Invoke(timer / timeToSpawnUnit);
                yield return null;
            }

            UP_Worker worker = Instantiate(spawnUnitPref, spawnPosition).GetComponent<UP_Worker>();
            worker.Initialize();

            spawnOrder--;
            UpdateSpawnOrder?.Invoke();
        }

        spawnOrder = 0;
        UpdateSpawnOrder?.Invoke();

        spawning = null;
    }

    public S_Cost GetSpawnCost() => spawnCost;

    #endregion

    #region Storage
    
    private List<TH_Storage> storages;

    public TH_Storage GetStorage(E_Resource resource)
    {
        foreach(TH_Storage storage in storages)
        {
            if(storage.GetTypeResource() == resource)
            {
                return storage;
            }
        }
        
        return null;
    }
        
    #endregion

    #region Processor
    
    private List<TH_Processor> processors;

    public TH_Processor GetProcessor(E_Resource resource)
    {
        foreach(TH_Processor processor in processors)
        {
            if(processor.GetTypeResource() == resource)
            {
                return processor;
            }
        }

        return null;
    }
        
    #endregion

    #region Residential

    [SerializeField] private int villageAmount;

    public int GetVillageAmount() => villageAmount;
    
    #endregion
}
