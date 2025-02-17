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
        foreach(I_Storage storage in GetComponents<I_Storage>())
        {
            storage.Initialize();
            storages.Add(storage);
            StorageSystem.current.AddStorage(storage, storage.GetTypeResource());
            ResourceSystem.current.AddResources(storage.GetTypeResource(), 0);
        }
        ResourceSystem.current.AddResources(E_Resource.Food, 100);

        // ininitialize processor
        processors = new List<I_Processor>();
        foreach(I_Processor processor in GetComponents<I_Processor>())
        {
            processor.Initialize();
            processors.Add(processor);
            ProcessorSystem.current.AddProcessor(processor.GetTypeResource(), processor);
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
