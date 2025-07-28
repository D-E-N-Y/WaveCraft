using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_TownHall : Building, ISpawnUnit, IResidential, ICircleZone
{
    public override string nameActor => "Town Hall";

    public override void Initialize()
    {
        base.Initialize();

        spawnOrder = 0;

        // fill busy grid 
        BuildSystem.current.BusyTakeArea(this);
        Place();
        Built();

        // initialize storage
        storages = new List<TH_Storage>();
        foreach (TH_Storage storage in GetComponents<TH_Storage>())
        {
            storage.Initialize(this);
            storages.Add(storage);
            StorageSystem.current.AddStorage(storage, storage.GetTypeResource());
            ResourceSystem.current.AddResourceByType(storage.GetTypeResource(), 0);
        }
        ResourceSystem.current.AddResourceByType(E_Resource.Food, 10000);
        ResourceSystem.current.AddResourceByType(E_Resource.Stone, 10000);
        ResourceSystem.current.AddResourceByType(E_Resource.Wood, 10000);

        // ininitialize processor
        processors = new List<TH_Processor>();
        foreach (TH_Processor processor in GetComponents<TH_Processor>())
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

    [SerializeField] private U_Village spawnUnit;
    [SerializeField] private float timeToSpawnUnit;
    [SerializeField] private Transform spawnPosition;
    private Coroutine spawning;
    public int spawnOrder { get; private set; }

    public void SpawnUnit()
    {
        if (StorageSystem.current.CheckCountResurces(spawnUnit.GetSpawnCost().resourse) <= spawnUnit.GetSpawnCost().count)
        {
            MessageSystem.current.AddMessage($"Not enough resources to spawn {spawnUnit.nameActor}");
            return;
        }

        spawnOrder++;
        UpdateSpawnOrder?.Invoke();

        if (spawnOrder == 1)
        {
            spawning = StartCoroutine(Spawning());
        }
    }

    public void CancelSpawnUnit()
    {
        spawnOrder = Mathf.Max(0, spawnOrder - 1);
        UpdateSpawnOrder?.Invoke();

        if (spawnOrder == 0 && spawning != null)
        {
            ResourceSystem.current.AddResourceByType(spawnUnit.GetSpawnCost().resourse, spawnUnit.GetSpawnCost().count);
            UpdateSpawnProgress?.Invoke(1f);

            StopCoroutine(spawning);
            spawning = null;
        }
    }

    private IEnumerator Spawning()
    {
        while (spawnOrder > 0 && VillageSystem.current.CheckFreeSpace())
        {
            if (StorageSystem.current.CheckCountResurces(spawnUnit.GetSpawnCost().resourse) <= spawnUnit.GetSpawnCost().count)
            {
                break;
            }

            ResourceSystem.current.RemoveResourceByType(spawnUnit.GetSpawnCost().resourse, spawnUnit.GetSpawnCost().count);

            float timer = 0;

            while (timer < timeToSpawnUnit)
            {
                timer += Time.deltaTime;
                UpdateSpawnProgress?.Invoke(timer / timeToSpawnUnit);
                yield return null;
            }

            UV_Worker worker = (UV_Worker)Instantiate(spawnUnit, spawnPosition);
            worker.Initialize();

            spawnOrder--;
            UpdateSpawnOrder?.Invoke();
        }

        spawnOrder = 0;
        UpdateSpawnOrder?.Invoke();

        spawning = null;
    }

    public S_Cost GetSpawnCost() => spawnUnit.GetSpawnCost();

    #endregion

    #region Storage

    private List<TH_Storage> storages;

    public TH_Storage GetStorage(E_Resource resource)
    {
        foreach (TH_Storage storage in storages)
        {
            if (storage.GetTypeResource() == resource)
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
        foreach (TH_Processor processor in processors)
        {
            if (processor.GetTypeResource() == resource)
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

    #region Expansion Plaze Zone

    [SerializeField] private SCircleZone expansionPlaceZone;
    public SCircleZone GetCircleZone() => expansionPlaceZone;

    #endregion
}
