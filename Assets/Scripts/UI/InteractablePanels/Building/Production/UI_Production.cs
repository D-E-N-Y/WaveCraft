using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Production : UI_Building 
{
    public override Type PanelType => typeof(I_Production);
    private I_Production production;

    [SerializeField] private Image ui_resourceImage;
    [SerializeField] private TextMeshProUGUI ui_produceAmount; 
    [SerializeField] private TextMeshProUGUI ui_maxAmount;
    [SerializeField] private List<ResourceImage> ui_resourceImages;


    public override void InitializeInfo(Actor _actor)
    {
        base.InitializeInfo(_actor);

        production = (I_Production)_actor;

        foreach(ResourceImage current in ui_resourceImages)
        {
            if(production.GetTypeResource() == current.resource)
            {
                ui_resourceImage.sprite = current.image;
                break;
            }
        }

        ui_produceAmount.text = production.GetProduceAmount().ToString();
        ui_maxAmount.text = production.GetMaxAmount().ToString();

        production.UpdateCountResources += RefreshProduceAmount;
    }

    protected override void UnsubscriptionActions()
    {
        base.UnsubscriptionActions();
        production.UpdateCountResources -= RefreshProduceAmount;
    }

    private void RefreshProduceAmount()
    {
        ui_produceAmount.text = production.GetProduceAmount().ToString();
    }

    public void Store()
    {
        if(production.GetProduceAmount() > 0)
        {
            StoreTask task = new StoreTask(production.GetTypeResource(), production);
            taskSystem.AddTask(task);
        }
    }
}