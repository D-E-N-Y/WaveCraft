using UnityEngine;
using UnityEngine.UI;

public class UIClosePanelButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => UISystem.current.CloseCurrentPanel());
    }
}