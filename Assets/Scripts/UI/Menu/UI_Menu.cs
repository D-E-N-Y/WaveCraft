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
        SetOpasity(selectMenu, 40f);

        selectMenu = button;
        SetOpasity(selectMenu, 100f);
    }

    public void SelectSection(GameObject button)
    {
        SetOpasity(selectSection, 40f);

        selectSection = button;
        SetOpasity(selectSection, 100f);
    }

    private void SetOpasity(GameObject button, float a)
    {
        if(button)
        {
            Color color = button.GetComponent<Image>().color;
            color = new Color(color.r, color.g, color.b, a / 255f);
            button.GetComponent<Image>().color = color;
        }
    }
}
