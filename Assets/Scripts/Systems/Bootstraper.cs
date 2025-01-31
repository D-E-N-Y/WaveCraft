using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstraper : MonoBehaviour 
{
    [SerializeField] private GameSystem[] systems;

    private void Awake() 
    {
        foreach (GameSystem systemPrefab in systems)
        {
            GameSystem systemInstance = Instantiate(systemPrefab);
            systemInstance.Initialize();

            DontDestroyOnLoad(systemInstance.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() 
    {
        SceneManager.LoadScene("GameScene");
    }
}