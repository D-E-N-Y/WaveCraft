using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIResourcePanel : MonoBehaviour
{
    [System.Serializable]
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

    private void Awake() 
    {
        ResourceSystem.current.UpdateCurrentCount += RefreshCurrentAmount;
        StorageSystem.current.UpdateMaxCount += RefreshMaxAmount;
    }

    private void RefreshCurrentAmount(E_Resource resource)
    {
        foreach(AmountText current in currentAmountText)
        {
            if(current.resource == resource)
            {
                current.textMesh.text = CorrectFormat(StorageSystem.current.CheckCountResurces(resource));
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
            }
        }
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