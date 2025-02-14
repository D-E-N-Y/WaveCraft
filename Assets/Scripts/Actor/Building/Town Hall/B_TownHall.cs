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
        storages = new List<I_Storage>();
        foreach(S_Storage storage in storages_param)
        {
            I_Storage newStorage = gameObject.AddComponent<I_Storage>();
            newStorage.Initialize(storage.resource, storage.maxAmount, actorPositions);
            storages.Add(newStorage);
            
            StorageSystem.current.AddStorage(newStorage, newStorage.GetTypeResource());
            ResourceSystem.current.AddResources(newStorage.GetTypeResource(), 0);
        }
        ResourceSystem.current.AddResources(E_Resource.Food, 100);

        // ininitialize processor
        processors = new List<I_Processor>();
        foreach(S_Processor processor in processors_param)
        {
            I_Processor newProcessor = gameObject.AddComponent<I_Processor>();
            newProcessor.Initialize(processor.resource, processor.factor, processor.timeProcess, actorPositions);
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
    private struct S_Storage
    {
        public E_Resource resource;
        public int maxAmount;

        public S_Storage(E_Resource resource, int maxAmount)
        {
            this.resource = resource;
            this.maxAmount = maxAmount;
        }
    }

    [SerializeField] private List<S_Storage> storages_param;
    private List<I_Storage> storages;

    public I_Storage GetStorage(E_Resource resource)
    {
        foreach(I_Storage storage in storages)
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

    [System.Serializable]
    private struct S_Processor
    {
        public E_Resource resource;
        public float factor;
        public float timeProcess;

        public S_Processor(E_Resource resource, float factor, float timeProcess)
        {
            this.resource = resource;
            this.factor = factor;
            this.timeProcess = timeProcess;
        }
    }

    [SerializeField] private List<S_Processor> processors_param;
    private List<I_Processor> processors;

    public I_Processor GetProcessor(E_Resource resource)
    {
        foreach(I_Processor processor in processors)
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

    public int GetVillageAmount()
    {
        return villageAmount;
    }    
    
    #endregion
}
