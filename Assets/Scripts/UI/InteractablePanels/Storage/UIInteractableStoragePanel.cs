using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractableStoragePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    private I_Storage storage;

    [SerializeField] private Image resourceImage;
    [SerializeField] private TextMeshProUGUI currentAmountText;
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

    public void Initialize(I_Storage storage)
    {
        this.storage = storage;

        nameText.text = storage.nameActor;
        hpText.text = storage.GetCurrentHP().ToString();

        foreach(ResourceImage current in resourceImages)
        {
            if(storage.GetTypeResource() == current.resource)
            {
                resourceImage.sprite = current.image;
                break;
            }
        }

        currentAmountText.text = storage.GetCurrentAmount().ToString();
        maxAmountText.text = storage.GetMaxAmount().ToString();

        storage.UpdateCurrentAmount += RefreshCurrentAmount;
    }
    
    private void RefreshCurrentAmount()
    {
        currentAmountText.text = storage.GetCurrentAmount().ToString();
    }
}