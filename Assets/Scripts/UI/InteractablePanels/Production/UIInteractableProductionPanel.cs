using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractableProductionPanel : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    private I_Production production;

    [SerializeField] private Image resourceImage;
    [SerializeField] private TextMeshProUGUI produceAmountText; 
    [SerializeField] private TextMeshProUGUI maxAmountText;
    
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

    public void Initialize(I_Production production)
    {
        this.production = production;

        nameText.text = production.nameActor;
        hpText.text = production.GetCurrentHP().ToString();

        foreach(ResourceImage current in resourceImages)
        {
            if(production.GetTypeResource() == current.resource)
            {
                resourceImage.sprite = current.image;
                break;
            }
        }

        produceAmountText.text = production.GetProduceAmount().ToString();
        maxAmountText.text = production.GetMaxAmount().ToString();

        production.UpdateCountResources += RefreshProduceAmount;
    }

    private void RefreshProduceAmount()
    {
        produceAmountText.text = production.GetProduceAmount().ToString();
    }

    public void Store()
    {
        if(production.GetProduceAmount() > 0)
        {
            StoreTask task = new StoreTask(production.GetTypeResource(), production);
            TaskSystem.current.AddTask(task);
        }
    }
}