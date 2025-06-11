using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSystem : GameSystem
{
    public static SceneLoaderSystem current;

    public override void Initialize()
    {
        current = this;
        DontDestroyOnLoad(this);
    }

    public void LoadScene(ELoadScene scene) => SceneManager.LoadScene(scene.ToString());
    public void LoadMainMenu() => SceneManager.LoadScene(ELoadScene.MainMenuScene.ToString());
    public void LoadGame() => SceneManager.LoadScene(ELoadScene.GameScene.ToString());
}