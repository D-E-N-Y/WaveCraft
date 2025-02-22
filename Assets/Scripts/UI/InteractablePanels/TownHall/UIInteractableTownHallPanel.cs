using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInteractableTownHallPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI spawnOrderText;
    private B_TownHall townHall;
    
    
    [System.Serializable]
    private struct ResourceText
    {
        public E_Resource resource;
        public TextMeshProUGUI amountText;

        public ResourceText(E_Resource resource, TextMeshProUGUI amountText)
        {
            this.resource = resource;
            this.amountText = amountText;
        }
    }
    [SerializeField] private List<ResourceText> processedResourceTexts;
    [SerializeField] private List<ResourceText> rawResourceTexts;
    
    [SerializeField] private List<ResourceText> curresntStorageResourceTexts;
    [SerializeField] private List<ResourceText> maxStorageResourceTexts;

    public void Initialize(B_TownHall townHall)
    {
        this.townHall = townHall;

        nameText.text = townHall.nameActor;
        hpText.text = townHall.GetCurrentHP().ToString();
        spawnOrderText.text = townHall.spawnOrder.ToString();

        townHall.UpdateSpawnOrder += RefreshSpawnOrder;
        
        foreach(ResourceText current in processedResourceTexts)
        {
            TH_Processor processor = townHall.GetProcessor(current.resource);
            current.amountText.text = processor.GetProcessedAmount().ToString();
            processor.UpdadeProcessedAmount += RefreshProcessedResourceAmount;
        }

        foreach(ResourceText current in rawResourceTexts)
        {
            
            TH_Processor processor = townHall.GetProcessor(current.resource);
            current.amountText.text = processor.GetRawAmount().ToString();
            processor.UpdateRawAmount += RefreshRawResourceAmount;
        }


        foreach(ResourceText current in curresntStorageResourceTexts)
        {
            TH_Storage storage = townHall.GetStorage(current.resource);
            current.amountText.text = storage.GetCurrentAmount().ToString();
            storage.UpdateCurrentAmount += RefreshResourceAmountStorage;
        }

        foreach(ResourceText current in maxStorageResourceTexts)
        {
            TH_Storage storage = townHall.GetStorage(current.resource);
            current.amountText.text = storage.GetMaxAmount().ToString();
        }
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
        spawnOrderText.text = townHall.spawnOrder.ToString();
    }

    private void RefreshProcessedResourceAmount(E_Resource resource)
    {
        foreach(ResourceText current in processedResourceTexts)
        {
            if(current.resource == resource)
            {
                current.amountText.text = townHall.GetProcessor(current.resource).GetProcessedAmount().ToString();
            }
        }
    }

    private void RefreshRawResourceAmount(E_Resource resource)
    {
        foreach(ResourceText current in rawResourceTexts)
        {
            if(current.resource == resource)
            {
                current.amountText.text = townHall.GetProcessor(current.resource).GetRawAmount().ToString();
            }
        }
    }

    private void RefreshResourceAmountStorage(E_Resource resource)
    {
        foreach(ResourceText current in curresntStorageResourceTexts)
        {
            current.amountText.text = townHall.GetStorage(current.resource).GetCurrentAmount().ToString();
        }
    }

    public void Store(int numberResource)
    {
        E_Resource resource = (E_Resource)numberResource;
        TH_Processor processor = townHall.GetProcessor(resource);
        
        if(processor.GetProcessedAmount() > 0 && StorageSystem.current.CheckFreeSpace(processor.GetTypeResource()))
        {
            StoreTask task = new StoreTask(resource, processor);
            TaskSystem.current.AddTask(task);
        }
    }
}
