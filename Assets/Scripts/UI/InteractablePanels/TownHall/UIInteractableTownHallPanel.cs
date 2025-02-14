using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInteractableTownHallPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
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

        
        foreach(ResourceText current in processedResourceTexts)
        {
            TH_Processor processor = townHall.GetProcessor(current.resource);
            current.amountText.text = processor.processedAmount.ToString();
            processor.UpdadeProcessedAmount += RefreshProcessedResourceAmount;
        }

        foreach(ResourceText current in rawResourceTexts)
        {
            TH_Processor processor = townHall.GetProcessor(current.resource);
            current.amountText.text = processor.rawAmount.ToString();
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
        townHall.StartCoroutine(townHall.SpawnUnit());
    }

    private void RefreshProcessedResourceAmount(E_Resource resource)
    {
        foreach(ResourceText current in processedResourceTexts)
        {
            if(current.resource == resource)
            {
                current.amountText.text = townHall.GetProcessor(resource).processedAmount.ToString();
                break;
            }
        }
    }

    private void RefreshRawResourceAmount(E_Resource resource)
    {
        foreach(ResourceText current in rawResourceTexts)
        {
            if(current.resource == resource)
            {
                current.amountText.text = townHall.GetProcessor(resource).rawAmount.ToString();
                break;
            }
        }
    }

    private void RefreshResourceAmountStorage(E_Resource resource)
    {
        foreach(ResourceText current in curresntStorageResourceTexts)
        {
            if(current.resource == resource)
            {
                current.amountText.text = townHall.GetStorage(resource).GetCurrentAmount().ToString();
                break;
            }
        }
    }

    public void Store(int numberResource)
    {
        E_Resource resource = (E_Resource)numberResource;
        TH_Processor processor = townHall.GetProcessor(resource);
        
        if(processor.processedAmount > 0 && StorageSystem.current.CheckFreeSpace(processor.resource))
        {
            StoreTask task = new StoreTask(resource, processor);
            TaskSystem.current.AddTask(task);
        }
    }
}
