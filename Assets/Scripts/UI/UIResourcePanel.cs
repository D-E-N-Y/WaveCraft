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

    private void OnEnable()
    {
        ResourceSystem.current.UpdateCurrentCount += RefreshCurrentAmount;
        StorageSystem.current.UpdateMaxCount += RefreshMaxAmount;

        VillageSystem.current.UpdateCurrentAmount += RefreshCurrentVillageAmount;
        VillageSystem.current.UpdateMaxAmount += RefreshMaxVillageAmount;
    }

    private void OnDisable()
    {
        ResourceSystem.current.UpdateCurrentCount -= RefreshCurrentAmount;
        StorageSystem.current.UpdateMaxCount -= RefreshMaxAmount;

        VillageSystem.current.UpdateCurrentAmount -= RefreshCurrentVillageAmount;
        VillageSystem.current.UpdateMaxAmount -= RefreshMaxVillageAmount;
    }

    private void RefreshCurrentAmount(E_Resource resource)
    {
        foreach(AmountText current in currentAmountText)
        {
            if(current.resource == resource)
            {
                current.textMesh.text = CorrectFormat(StorageSystem.current.CheckCountResurces(resource));
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
                current.textMesh.text = CorrectFormat(StorageSystem.current.CheckMaxCountResources(resource));
                break;
            }
        }
    }

    private void RefreshCurrentVillageAmount()
    {
        currentAmountVillageText.text = VillageSystem.current.GetCurrentAmount().ToString();
    }

    private void RefreshMaxVillageAmount()
    {
        maxAmountVillageText.text = VillageSystem.current.GetMaxAmount().ToString();
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