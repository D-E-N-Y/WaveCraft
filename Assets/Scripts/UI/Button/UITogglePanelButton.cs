using UnityEngine;
using UnityEngine.UI;

public class UITogglePanelButton : MonoBehaviour 
{
    [SerializeField] private string panelName;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => UISystem.current.TogglePanelByName(panelName));
    }
}