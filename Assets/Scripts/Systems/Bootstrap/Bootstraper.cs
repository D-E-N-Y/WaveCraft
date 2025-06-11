using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstraper : MonoBehaviour 
{
    [SerializeField] private UISystem uiSystem;
    [SerializeField] private SceneLoaderSystem sceneLoaderSystem;

    private void Start() 
    {
        uiSystem.Initialize();
        sceneLoaderSystem.Initialize();

        sceneLoaderSystem.LoadMainMenu();
    }
}