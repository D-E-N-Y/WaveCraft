using Unity.VisualScripting;
using UnityEngine;

public class UI_PreviewScreen : UIPanel
{
    private SceneLoaderSystem sceneLoaderSystem;

    public void Initialize(SceneLoaderSystem sceneLoaderSystem)
    {
        this.sceneLoaderSystem = sceneLoaderSystem;
    }

    public void LoadMainMenu() => sceneLoaderSystem.LoadMainMenu();
}
