using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_TownHall : Building, ISpawnUnit, IResidential
{
    private void Start() 
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        nameActor = "Town Hall";

        // fill busy grid 
        BuildSystem.current.BusyTakeArea(BuildSystem.current.gridLayout.WorldToCell(GetStartPosition()), Size);
        Place();
        Built();

        // initialize storage
        storages = new List<TH_Storage>();
        foreach(STH_Storage storage in storages_param)
        {
            TH_Storage newStorage = gameObject.AddComponent<TH_Storage>();
            newStorage.Initialize(storage.resource, storage.maxAmount);
            storages.Add(newStorage);
            
            StorageSystem.current.AddStorage(newStorage, newStorage.resource);
            ResourceSystem.current.AddResources(newStorage.resource, 0);
        }
        ResourceSystem.current.AddResources(E_Resource.Food, 100);

        // ininitialize processor
        processors = new List<TH_Processor>();
        foreach(STH_Processor processor in processors_param)
        {
            TH_Processor newProcessor = gameObject.AddComponent<TH_Processor>();
            newProcessor.Initialize(processor.resource, processor.factor, processor.timeProcess);
            processors.Add(newProcessor);

            ProcessorSystem.current.AddProcessor(processor.resource, newProcessor);
        }
    } 

    #region SpawnUnit

    [SerializeField] private GameObject spawnUnitPref;
    [SerializeField] private float timeToSpawnUnit;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private S_Cost spawnCost;

    public IEnumerator SpawnUnit()
    {
        if(StorageSystem.current.CheckCountResurces(spawnCost.resourse) <= spawnCost.count)
        {
            Debug.Log($"not enought {spawnCost.resourse}");
            yield break;
        }

        ResourceSystem.current.RemoveResources(spawnCost.resourse, spawnCost.count);
        
        Debug.Log($"start spawn worker \nwait {timeToSpawnUnit} seconds");

        yield return new WaitForSeconds(timeToSpawnUnit);

        UP_Worker worker = Instantiate(spawnUnitPref, spawnPosition).GetComponent<UP_Worker>();
        worker.Initialize();

        Debug.Log("spawn worker");
    }

    #endregion

    #region Storage

    [System.Serializable]
    private struct STH_Storage
    {
        public E_Resource resource;
        public int maxAmount;

        public STH_Storage(E_Resource resource, int maxAmount)
        {
            this.resource = resource;
            this.maxAmount = maxAmount;
        }
    }

    [SerializeField] private List<STH_Storage> storages_param;
    private List<TH_Storage> storages;

    public TH_Storage GetStorage(E_Resource resource)
    {
        foreach(TH_Storage storage in storages)
        {
            if(storage.resource == resource)
            {
                return storage;
            }
        }
        
        return null;
    }
        
    #endregion

    #region Processing

    [System.Serializable]
    private struct STH_Processor
    {
        public E_Resource resource;
        public float factor;
        public float timeProcess;

        public STH_Processor(E_Resource resource, float factor, float timeProcess)
        {
            this.resource = resource;
            this.factor = factor;
            this.timeProcess = timeProcess;
        }
    }

    [SerializeField] private List<STH_Processor> processors_param;
    private List<TH_Processor> processors;

    public TH_Processor GetProcessor(E_Resource resource)
    {
        foreach(TH_Processor processor in processors)
        {
            if(processor.resource == resource)
            {
                return processor;
            }
        }

        return null;
    }
        
    #endregion

    #region Residential

    [SerializeField] private int villageAmount;

    public int GetVillageAmount()
    {
        return villageAmount;
    }    
    
    #endregion
}
