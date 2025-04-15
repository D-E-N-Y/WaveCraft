using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Storage : UI_InteractablePanel
{
    public override Type PanelType => typeof(I_Storage);
    private I_Storage storage;

    [SerializeField] private Image ui_resourceImage;
    [SerializeField] private TextMeshProUGUI ui_currentAmount;
    [SerializeField] private TextMeshProUGUI ui_maxAmount;
    [SerializeField] private List<ResourceImage> ui_resourceImages;

    public override void Initialize(Actor _actor)
    {
        base.Initialize(_actor);

        storage = (I_Storage)_actor;

        foreach(ResourceImage current in ui_resourceImages)
        {
            if(storage.GetTypeResource() == current.resource)
            {
                ui_resourceImage.sprite = current.image;
                break;
            }
        }

        ui_currentAmount.text = storage.GetCurrentAmount().ToString();
        ui_maxAmount.text = storage.GetMaxAmount().ToString();

        storage.UpdateCurrentAmount += RefreshCurrentAmount;
    }

    protected override void UnsubscriptionActions()
    {
        base.UnsubscriptionActions();
        storage.UpdateCurrentAmount -= RefreshCurrentAmount;
    }

    private void RefreshCurrentAmount()
    {
        ui_currentAmount.text = storage.GetCurrentAmount().ToString();
    }
}