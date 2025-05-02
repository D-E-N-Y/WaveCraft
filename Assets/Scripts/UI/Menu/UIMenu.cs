using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UIPanel
{
    [SerializeField] Image selectMenu;
    
    private GameObject openSection;
    private GameObject selectTabSection;

    private Color normalColor;

    void OnEnable()
    {
        selectMenu.color = new Color(selectMenu.color.r, selectMenu.color.g, selectMenu.color.b, 1f);
    }

    void OnDisable()
    {
        selectMenu.color = new Color(selectMenu.color.r, selectMenu.color.g, selectMenu.color.b, 0f);

        UnSelect(selectTabSection);
        CloseSection(openSection);

        openSection = selectTabSection = null;
    }

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
            section.SetActive(false);
        }
    }

    public void CloseCurrentSection()
    {
        if(openSection != null)
        {
            openSection.SetActive(false);
            openSection = null;
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
            normalColor = tab.GetComponent<Image>().color;
            
            Color color = tab.GetComponent<Image>().color;
            color = new Color(125f / 255f, 101f / 255f, 101f / 255f);
            tab.GetComponent<Image>().color = color;
        }
    }

    private void UnSelect(GameObject tab)
    {
        if(tab)
        {
            tab.GetComponent<Image>().color = normalColor; 
        }
    }
}
