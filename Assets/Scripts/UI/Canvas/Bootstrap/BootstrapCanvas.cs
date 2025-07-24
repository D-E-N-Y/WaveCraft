using UnityEngine;

public class BootstrapCanvas : MonoBehaviour
{
    [SerializeField] private UI_PreviewScreen ui_previewScreen;

    public void Initialize(SceneLoaderSystem sceneLoaderSystem)
    {
        ui_previewScreen.Initialize(sceneLoaderSystem);
    }
}
