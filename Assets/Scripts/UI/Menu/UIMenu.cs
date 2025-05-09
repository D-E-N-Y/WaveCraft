using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UIPanel
{
    [SerializeField] private Image selectMenu;
    
    [SerializeField] private Color normal, select;
    
    private GameObject openSection;
    private GameObject selectTabSection;

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
            tab.GetComponent<Image>().color = select;
        }
    }

    private void UnSelect(GameObject tab)
    {
        if(tab)
        {
            tab.GetComponent<Image>().color = normal; 
        }
    }
}
