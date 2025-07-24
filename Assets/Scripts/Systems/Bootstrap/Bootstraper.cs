using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstraper : MonoBehaviour 
{
    [SerializeField] private UISystem uiSystem;
    [SerializeField] private SceneLoaderSystem sceneLoaderSystem;

    [SerializeField] private BootstrapCanvas bootstrapCanvas;

    private void Start()
    {
        uiSystem.Initialize();
        sceneLoaderSystem.Initialize();

        bootstrapCanvas.Initialize(sceneLoaderSystem);
    }
}