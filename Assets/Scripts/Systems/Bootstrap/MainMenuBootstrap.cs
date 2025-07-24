using UnityEngine;

public class MainMenuBootstrap : MonoBehaviour
{
    [SerializeField] private MainMenuCanvas mainMenuCanvas;

    void Start()
    {
        mainMenuCanvas.Initialize();
    }
}
