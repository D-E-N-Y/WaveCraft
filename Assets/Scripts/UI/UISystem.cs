using UnityEngine;

public class UISystem : GameSystem
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
