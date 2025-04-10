using UnityEngine;

public class UISystem : GameSystem
{
    private GameObject openPanel;
    
    public override void Initialize()
    {
        base.Initialize();
    }

    public void OpenPanel(GameObject panel)
    {
        if(openPanel == panel)
        {
            ClosePanel(openPanel);
            return;
        }
        else if(openPanel != null)
        {
            ClosePanel(openPanel);
        }
        
        panel.SetActive(true);
        openPanel = panel;
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        openPanel = null;
    }

    public void CloseOpenPanel()
    {
        if(openPanel != null)
        {
            ClosePanel(openPanel);
        }
    }
}
