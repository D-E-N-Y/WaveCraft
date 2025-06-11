using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResourcePanel : MonoBehaviour
{
    [Serializable]
    private struct AmountText
    {
        public E_Resource resource;
        public TextMeshProUGUI textMesh;

        public AmountText(E_Resource resource, TextMeshProUGUI textMesh)
        {
            this.resource = resource;
            this.textMesh = textMesh;
        }
    }

    [SerializeField] private List<AmountText> currentAmountText;
    [SerializeField] private List<AmountText> maxAmountText;

    [SerializeField] private TextMeshProUGUI currentAmountVillageText;
    [SerializeField] private TextMeshProUGUI maxAmountVillageText;

    private StorageSystem storageSystem;
    private ResourceSystem resourceSystem;
    private VillageSystem villageSystem;

    public void Initialize()
    {
        storageSystem = StorageSystem.current;
        resourceSystem = ResourceSystem.current;
        villageSystem = VillageSystem.current;

        foreach (E_Resource resource in Enum.GetValues(typeof(E_Resource)))
        {
            RefreshCurrentAmount(resource);
            RefreshMaxAmount(resource);
        }

        RefreshCurrentVillageAmount();
        RefreshMaxVillageAmount();
    }

    private void OnEnable()
    {
        resourceSystem.UpdateCurrentCount += RefreshCurrentAmount;
        storageSystem.UpdateMaxCount += RefreshMaxAmount;

        villageSystem.UpdateCurrentAmount += RefreshCurrentVillageAmount;
        villageSystem.UpdateMaxAmount += RefreshMaxVillageAmount;
    }

    private void OnDisable()
    {
        resourceSystem.UpdateCurrentCount -= RefreshCurrentAmount;
        storageSystem.UpdateMaxCount -= RefreshMaxAmount;

        villageSystem.UpdateCurrentAmount -= RefreshCurrentVillageAmount;
        villageSystem.UpdateMaxAmount -= RefreshMaxVillageAmount;
    }

    private void RefreshCurrentAmount(E_Resource resource)
    {
        foreach(AmountText current in currentAmountText)
        {
            if(current.resource == resource)
            {
                current.textMesh.text = CorrectFormat(storageSystem.CheckCountResurces(resource));
                break;
            }
        }
    }

    private void RefreshMaxAmount(E_Resource resource) 
    {
        foreach(AmountText current in maxAmountText)
        {
            if(current.resource == resource)
            {
                current.textMesh.text = CorrectFormat(storageSystem.CheckMaxCountResources(resource));
                break;
            }
        }
    }

    private void RefreshCurrentVillageAmount()
    {
        currentAmountVillageText.text = villageSystem.GetCurrentAmount().ToString();
    }

    private void RefreshMaxVillageAmount()
    {
        maxAmountVillageText.text = villageSystem.GetMaxAmount().ToString();
    }

    private string CorrectFormat(int value)
    {
        if(value < 1000)
        {
            return value.ToString();
        }
        else if(value < 1000000)
        {
            return ((float)value / 1000).ToString("F2") + "k"; 
        }
        else
        {
            return ((float)value / 1000000).ToString("F2") + "M"; 
        }
    }
}