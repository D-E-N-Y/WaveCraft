using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : UISystem
{
    private GameObject openMenu;
    private GameObject openSection;

    private GameObject selectMenu;
    private GameObject selectSection;

    public void OpenMenu(GameObject menu)
    {
        Close(openMenu);
        
        openMenu = menu;
        openMenu.SetActive(true);
    }

    private void Close(GameObject menu)
    {
        if(menu != null)
        {
            menu.SetActive(false);
        }
    }

    public void OpenSection(GameObject section)
    {
        Close(openSection);

        openSection = section;
        openSection.SetActive(true);
    }

    
    public void SelectMenu(GameObject button)
    {
        UnSelect(selectMenu);

        selectMenu = button;
        Select(selectMenu);
    }

    public void SelectSection(GameObject button)
    {
        UnSelect(selectSection);

        selectSection = button;
        Select(selectSection);
    }

    private void Select(GameObject button)
    {
        if(button)
        {
            Color color = button.GetComponent<Image>().color;
            color = new Color(125f / 255f, 101f / 255f, 101f / 255f);
            button.GetComponent<Image>().color = color;
        }
    }

    private void UnSelect(GameObject button)
    {
        if(button)
        {
            Color color = button.GetComponent<Image>().color;
            color = new Color(1f, 1f, 1f);
            button.GetComponent<Image>().color = color; 
        }
    }
}
