using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBlackBackground : MonoBehaviour
{
    private Animator _animator;
    private string _scene;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Hide()
    {
        gameObject.SetActive(true);
        _animator.SetTrigger("Hide");
    }

    public void Show()
    {
        _animator.SetTrigger("Show");
    }

    public void SetLoadScene(string _scene) => this._scene = _scene;

    public void LoadScene()
    {
        if (Application.CanStreamedLevelBeLoaded(_scene))
        {
            SceneManager.LoadScene(_scene);
        }
        else
        {
            Application.Quit();
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}