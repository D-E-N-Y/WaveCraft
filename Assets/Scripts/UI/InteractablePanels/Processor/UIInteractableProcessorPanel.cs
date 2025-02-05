using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractableProcessorPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    private I_Processor processor;

    [SerializeField] private Image resourceImage;
    [SerializeField] private TextMeshProUGUI rawAmountText; 
    [SerializeField] private TextMeshProUGUI processedAmountText;
    
    [System.Serializable]
    private struct ResourceImage
    {
        public E_Resource resource;
        public Sprite image;

        public ResourceImage(E_Resource resource, Sprite image)
        {
            this.resource = resource;
            this.image = image;
        }
    }
    [SerializeField] private List<ResourceImage> resourceImages;

    public void Initialize(I_Processor processor)
    {
        this.processor = processor;

        nameText.text = processor.nameActor;
        hpText.text = processor.GetCurrentHP().ToString();

        foreach(ResourceImage current in resourceImages)
        {
            if(processor.GetTypeResource() == current.resource)
            {
                resourceImage.sprite = current.image;
                break;
            }
        }

        rawAmountText.text = processor.rawAmount.ToString();
        processedAmountText.text = processor.processedAmount.ToString();

        processor.UpdateRawAmount += RefreshRawAmount;
        processor.UpdateProcessedAmount += RefreshProcessedAmount;
    }

    private void RefreshRawAmount()
    {
        rawAmountText.text = processor.rawAmount.ToString();
    }

    private void RefreshProcessedAmount()
    {
        processedAmountText.text = processor.processedAmount.ToString();
    }

    public void Store()
    {
        if(processor.processedAmount > 0)
        {
            StoreTask task = new StoreTask(processor.GetTypeResource(), processor);
            TaskSystem.current.AddTask(task);
        }
    }
}