using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoadingScreen : UIPanel
{
    [SerializeField] private UIBlackBaground ui_BlackBaground;
    
    [SerializeField] private Image _progressImage;

    [SerializeField, Range(0.1f, 1f)] float _loadSpeed;
    private float maxFill = 1f;

    public void Initialize()
    {
        _progressImage.fillAmount = 0f;
        StartCoroutine(nameof(Loading));
    }

    private IEnumerator Loading()
    {
        while(_progressImage.fillAmount != maxFill)
        {
            _progressImage.fillAmount += _loadSpeed * Time.deltaTime;
            
            yield return null;
        }

        ui_BlackBaground.Hide();

        yield return new WaitForSeconds(1f);

        while(!ui_BlackBaground.IsAnimationFinished())
        {
            yield return null;
        }

        SceneManager.LoadScene("GameScene");
    }
}
