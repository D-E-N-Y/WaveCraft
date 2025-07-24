using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TownHall : UI_Building
{
    public override Type PanelType => typeof(B_TownHall);
    private B_TownHall townHall;

    [SerializeField] private TextMeshProUGUI ui_spawnOrder;
    [SerializeField] private TextMeshProUGUI ui_spawnCost;
    [SerializeField] private Image ui_spawnProgress;    
    
    
    [System.Serializable]
    private struct ResourceText
    {
        public E_Resource resource;
        public TextMeshProUGUI ui_amount;

        public ResourceText(E_Resource resource, TextMeshProUGUI ui_amount)
        {
            this.resource = resource;
            this.ui_amount = ui_amount;
        }
    }
    [SerializeField] private List<ResourceText> ui_processedResources;
    [SerializeField] private List<ResourceText> ui_rawResources;

    [SerializeField] private Button ui_storeWoodButton;
    [SerializeField] private Button ui_storeStoneButton;
    [SerializeField] private Button ui_storeFoodButton;

    [SerializeField] private List<ResourceText> ui_curresntStorageResources;
    [SerializeField] private List<ResourceText> ui_maxStorageResources;

    public override void InitializeInfo(Actor _actor)
    {
        base.InitializeInfo(_actor);

        townHall = (B_TownHall)_actor;

        ui_spawnOrder.text = townHall.spawnOrder.ToString();
        ui_spawnCost.text = townHall.GetSpawnCost().count.ToString();
        ui_spawnProgress.fillAmount = 1f;

        townHall.UpdateSpawnOrder += RefreshSpawnOrder;
        townHall.UpdateSpawnProgress += RefreshSpawnProgressIgame;

        foreach(ResourceText current in ui_processedResources)
        {
            TH_Processor processor = townHall.GetProcessor(current.resource);
            current.ui_amount.text = processor.GetProcessedAmount().ToString();
            processor.UpdadeProcessedAmount += RefreshProcessedResourceAmount;
        }

        foreach(ResourceText current in ui_rawResources)
        {
            
            TH_Processor processor = townHall.GetProcessor(current.resource);
            current.ui_amount.text = processor.GetRawAmount().ToString();
            processor.UpdateRawAmount += RefreshRawResourceAmount;
        }

        ui_storeWoodButton.onClick.AddListener(() => Store(E_Resource.Wood));
        ui_storeStoneButton.onClick.AddListener(() => Store(E_Resource.Stone));
        ui_storeFoodButton.onClick.AddListener(() => Store(E_Resource.Food));

        foreach (ResourceText current in ui_curresntStorageResources)
        {
            TH_Storage storage = townHall.GetStorage(current.resource);
            current.ui_amount.text = storage.GetCurrentAmount().ToString();
            storage.UpdateCurrentAmount += RefreshResourceAmountStorage;
        }

        foreach(ResourceText current in ui_maxStorageResources)
        {
            TH_Storage storage = townHall.GetStorage(current.resource);
            current.ui_amount.text = storage.GetMaxAmount().ToString();
        }
    }

    protected override void UnsubscriptionActions()
    {
        base.UnsubscriptionActions();

        townHall.UpdateSpawnOrder -= RefreshSpawnOrder;
        townHall.UpdateSpawnProgress -= RefreshSpawnProgressIgame;

        foreach (E_Resource resource in Enum.GetValues(typeof(E_Resource)))
        {
            TH_Processor processor = townHall.GetProcessor(resource);
            processor.UpdadeProcessedAmount -= RefreshProcessedResourceAmount;
            processor.UpdateRawAmount -= RefreshRawResourceAmount;

            TH_Storage storage = townHall.GetStorage(resource);
            storage.UpdateCurrentAmount -= RefreshResourceAmountStorage;
        }

        ui_storeWoodButton.onClick.RemoveAllListeners();
        ui_storeStoneButton.onClick.RemoveAllListeners();
        ui_storeFoodButton.onClick.RemoveAllListeners();
    }

    public void SpawnWorker()
    {
        townHall.SpawnUnit();
    }

    public void CancelSpawn()
    {
        townHall.CancelSpawnUnit();
    }

    private void RefreshSpawnOrder()
    {
        ui_spawnOrder.text = townHall.spawnOrder.ToString();
    }

    private void RefreshSpawnProgressIgame(float value)
    {
        ui_spawnProgress.fillAmount = value;
    }

    private void RefreshProcessedResourceAmount(E_Resource resource)
    {
        foreach(ResourceText current in ui_processedResources)
        {
            if(current.resource == resource)
            {
                current.ui_amount.text = townHall.GetProcessor(current.resource).GetProcessedAmount().ToString();
            }
        }
    }

    private void RefreshRawResourceAmount(E_Resource resource)
    {
        foreach(ResourceText current in ui_rawResources)
        {
            if(current.resource == resource)
            {
                current.ui_amount.text = townHall.GetProcessor(current.resource).GetRawAmount().ToString();
            }
        }
    }

    private void RefreshResourceAmountStorage(E_Resource resource)
    {
        foreach(ResourceText current in ui_curresntStorageResources)
        {
            current.ui_amount.text = townHall.GetStorage(current.resource).GetCurrentAmount().ToString();
        }
    }

    private void Store(E_Resource resource)
    {
        TH_Processor processor = townHall.GetProcessor(resource);

        if (processor.GetProcessedAmount() > 0)
        {
            if (StorageSystem.current.CheckFreeSpace(processor.GetTypeResource()))
            {
                StoreTask task = new StoreTask(resource, townHall);
                taskSystem.AddTask(task);
            }
            else
            {
                MessageSystem.current.AddMessage($"No available space for resource: {processor.GetTypeResource()}");
            }
        }
    }
}