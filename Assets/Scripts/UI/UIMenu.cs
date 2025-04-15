using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UIPanel
{
    private GameObject openSection;
    private GameObject selectTabSection;

    public void OpenSection(GameObject section)
    {
        if(openSection != section)
        {
            CloseSection(openSection);
            
            openSection = section;
            openSection.SetActive(true);
        }
    }

    private void CloseSection(GameObject section)
    {
        if(section)
        {
            openSection.SetActive(false);
        }
    }

    public void SelectTabSection(GameObject tab)
    {
        if(selectTabSection != tab)
        {
            UnSelect(selectTabSection);

            selectTabSection = tab;
            Select(selectTabSection);
        }
    }

    private void Select(GameObject tab)
    {
        if(tab)
        {
            Color color = tab.GetComponent<Image>().color;
            color = new Color(125f / 255f, 101f / 255f, 101f / 255f);
            tab.GetComponent<Image>().color = color;
        }
    }

    private void UnSelect(GameObject tab)
    {
        if(tab)
        {
            Color color = tab.GetComponent<Image>().color;
            color = new Color(1f, 1f, 1f);
            tab.GetComponent<Image>().color = color; 
        }
    }
}
