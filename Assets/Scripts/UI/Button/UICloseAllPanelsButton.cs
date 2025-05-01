using UnityEngine;
using UnityEngine.UI;

public class UICloseAllPanelsButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => UISystem.current.CloseAllPanels());
    }
}