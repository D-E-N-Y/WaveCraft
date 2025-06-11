using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : UIPanel
{
    public Action completeMainLoading, completePartLoading;

    [SerializeField] private TextMeshProUGUI ui_initilize;
    [SerializeField] private Image ui_mainProgress, ui_partProgress;
    private float mainMaxProgress, partMaxProgress;

    public void Initialize()
    {
        ui_mainProgress.fillAmount = 0f;
        ui_partProgress.fillAmount = 0f;
    }

    public void SetInitializeText(string _initialize)
    {
        ui_initilize.text = _initialize;
    }

    public void AddMainProgress()
    {
        ui_mainProgress.fillAmount += 1f / mainMaxProgress;

        if (ui_mainProgress.fillAmount >= 1f)
        {
            completeMainLoading?.Invoke();
        }
    }

    public void AddPartProgress()
    {
        ui_partProgress.fillAmount += 1f / partMaxProgress;

        if (ui_partProgress.fillAmount >= 1f)
        {
            completePartLoading?.Invoke();
        }
    }

    public void SetMaxMainProgress(int value)
    {
        mainMaxProgress = Math.Max(value, 1);
        ui_mainProgress.fillAmount = 0f;
    }

    public void SetMaxPartProgress(int value)
    {
        partMaxProgress = Math.Max(value, 1);
        ui_partProgress.fillAmount = 0f;
    }
}
