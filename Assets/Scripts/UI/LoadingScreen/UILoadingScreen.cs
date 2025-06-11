using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoadingScreen : UIPanel
{
    public Action finalLoading;

    [SerializeField] private Image _progressImage;
    [SerializeField, Range(0.1f, 1f)] float _loadSpeed;
    private float maxFill = 1f;

    private Coroutine loading;

    public void Initialize()
    {
        _progressImage.fillAmount = 0f;
    }

    public void StartLoading()
    {
        if (loading != null)
            StopCoroutine(loading);

        loading = StartCoroutine(nameof(Loading));
    }

    private IEnumerator Loading()
    {
        while (_progressImage.fillAmount != maxFill)
        {
            _progressImage.fillAmount += _loadSpeed * Time.deltaTime;

            yield return null;
        }

        StopLoading();
    }

    public void StopLoading()
    {
        StopCoroutine(loading);

        finalLoading?.Invoke();
    }
}
