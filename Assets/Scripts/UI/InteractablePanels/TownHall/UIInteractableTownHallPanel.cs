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
            I_Processor processor = townHall.GetProcessor(current.resource);
            current.amountText.text = processor.processedAmount.ToString();
            processor.UpdateProcessedAmount += RefreshProcessedResourceAmount;
        }

        foreach(ResourceText current in rawResourceTexts)
        {
            I_Processor processor = townHall.GetProcessor(current.resource);
            current.amountText.text = processor.rawAmount.ToString();
            processor.UpdateRawAmount += RefreshRawResourceAmount;
        }


        foreach(ResourceText current in curresntStorageResourceTexts)
        {
            I_Storage storage = townHall.GetStorage(current.resource);
            current.amountText.text = storage.GetCurrentAmount().ToString();
            storage.UpdateCurrentAmount += RefreshResourceAmountStorage;
        }

        foreach(ResourceText current in maxStorageResourceTexts)
        {
            I_Storage storage = townHall.GetStorage(current.resource);
            current.amountText.text = storage.GetMaxAmount().ToString();
        }
    }

    public void SpawnWorker()
    {
        townHall.StartCoroutine(townHall.SpawnUnit());
    }

    private void RefreshProcessedResourceAmount()
    {
        foreach(ResourceText current in processedResourceTexts)
        {
            current.amountText.text = townHall.GetProcessor(current.resource).processedAmount.ToString();
        }
    }

    private void RefreshRawResourceAmount()
    {
        foreach(ResourceText current in rawResourceTexts)
        {
            current.amountText.text = townHall.GetProcessor(current.resource).rawAmount.ToString();
        }
    }

    private void RefreshResourceAmountStorage()
    {
        foreach(ResourceText current in curresntStorageResourceTexts)
        {
            current.amountText.text = townHall.GetStorage(current.resource).GetCurrentAmount().ToString();
        }
    }

    public void Store(int numberResource)
    {
        E_Resource resource = (E_Resource)numberResource;
        I_Processor processor = townHall.GetProcessor(resource);
        
        if(processor.processedAmount > 0 && StorageSystem.current.CheckFreeSpace(processor.GetTypeResource()))
        {
            StoreTask task = new StoreTask(resource, processor);
            TaskSystem.current.AddTask(task);
        }
    }
}
