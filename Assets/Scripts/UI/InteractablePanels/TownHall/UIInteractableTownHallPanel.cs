using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIInteractableTownHallPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    private B_TownHall townHall;
    
    
    [System.Serializable]
    private struct ProcessedResourceText
    {
        public E_Resource resource;
        public TextMeshProUGUI amountText;

        public ProcessedResourceText(E_Resource resource, TextMeshProUGUI amountText)
        {
            this.resource = resource;
            this.amountText = amountText;
        }
    }
    [SerializeField] private List<ProcessedResourceText> processedResourceTexts;

    public void Initialize(B_TownHall townHall)
    {
        this.townHall = townHall;

        nameText.text = townHall.nameActor;
        hpText.text = townHall.GetCurrentHP().ToString();

        foreach(ProcessedResourceText current in processedResourceTexts)
        {
            TH_Processor processor = townHall.GetProcessor(current.resource);
            current.amountText.text = processor.processedAmount.ToString();
            processor.UpdadeProcessedAmount += RefreshProcessedResourceAmount;
        }
    }

    public void SpawnWorker()
    {
        townHall.StartCoroutine(townHall.SpawnUnit());
    }

    private void RefreshProcessedResourceAmount(E_Resource resource)
    {
        foreach(ProcessedResourceText current in processedResourceTexts)
        {
            if(current.resource == resource)
            {
                current.amountText.text = townHall.GetProcessor(resource).processedAmount.ToString();
                break;
            }
        }
    }

    public void Store()
    {
        // StoreTask task = new StoreTask();
    }
}
