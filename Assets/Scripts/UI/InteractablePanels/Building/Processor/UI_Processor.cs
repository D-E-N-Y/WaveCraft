using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Processor : UI_Building
{
    public override Type PanelType => typeof(I_Processor);
    private I_Processor processor;

    [SerializeField] private Image ui_resourceImage;
    [SerializeField] private TextMeshProUGUI ui_rawAmount;
    [SerializeField] private TextMeshProUGUI ui_processedAmount;
    [SerializeField] private List<ResourceImage> ui_resourceImages;

    public override void Initialize(Actor _actor)
    {
        base.Initialize(_actor);

        processor = (I_Processor)_actor;

        foreach(ResourceImage current in ui_resourceImages)
        {
            if(processor.GetTypeResource() == current.resource)
            {
                ui_resourceImage.sprite = current.image;
                break;
            }
        }

        ui_rawAmount.text = processor.rawAmount.ToString();
        ui_processedAmount.text = processor.processedAmount.ToString(); 

        processor.UpdateRawAmount += RefreshRawAmount;
        processor.UpdateProcessedAmount += RefreshProcessedAmount;
    }

    protected override void UnsubscriptionActions()
    {
        base.UnsubscriptionActions();
        processor.UpdateRawAmount -= RefreshRawAmount;
        processor.UpdateProcessedAmount -= RefreshProcessedAmount;
    }

    private void RefreshRawAmount()
    {
        ui_rawAmount.text = processor.rawAmount.ToString();
    }

    private void RefreshProcessedAmount()
    {
        ui_processedAmount.text = processor.processedAmount.ToString();
    }

    public void Store()
    {
        if(processor.processedAmount > 0 && StorageSystem.current.CheckFreeSpace(processor.GetTypeResource()))
        {
            StoreTask task = new StoreTask(processor.GetTypeResource(), processor);
            taskSystem.AddTask(task);
        }
    }
}