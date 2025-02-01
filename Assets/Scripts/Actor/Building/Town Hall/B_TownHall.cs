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

        // fill busy grid 
        BuildSystem.current.BusyTakeArea(BuildSystem.current.gridLayout.WorldToCell(GetStartPosition()), Size);
        Place();
        Built();

        // initialize storage
        storages = new List<TH_Storage>();
        foreach(STH_Storage storage in storages_param)
        {
            TH_Storage newStorage = new TH_Storage(storage.resource, storage.maxAmount);
            
            storages.Add(newStorage);
            StorageSystem.current.AddStorage(newStorage, newStorage.resource);
            ResourceSystem.current.AddResources(newStorage.resource, newStorage.GetMaxAmount());
        }

        // ininitialize processor
        processors = new List<TH_Processor>();
        foreach(STH_Processor processor in processors_param)
        {
            processors.Add(new TH_Processor(processor.resource, processor.factor, processor.timeProcess));
        }
    } 

    #region SpawnUnit

    [SerializeField] private GameObject spawnUnitPref;
    [SerializeField] private float timeToSpawnUnit;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private S_Cost spawnCost;
    
    public void SpawnUnit()
    {
        StartCoroutine(nameof(Spawn));
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(timeToSpawnUnit);

        Unit unit = Instantiate(spawnUnitPref, spawnPosition).GetComponent<Unit>();
        unit.Initialize();

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
        
    #endregion

    #region Residential

    [SerializeField] private int villageAmount;

    public int GetVillageAmount()
    {
        return villageAmount;
    }    
    
    #endregion
}
