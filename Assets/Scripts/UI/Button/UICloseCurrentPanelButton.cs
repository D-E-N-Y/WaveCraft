using UnityEngine;
using UnityEngine.UI;

public class UICloseCurrentPanelButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => UISystem.current.CloseCurrentPanel());
    }
}