using UnityEngine;

public class UIInteractablePanel : MonoBehaviour
{
    [SerializeField] private GameObject resourcePanel;
    [SerializeField] private GameObject townHallPanel;
    [SerializeField] private GameObject otherPanel;
    private GameObject openPanel;

    private void Awake() 
    {
        InteractionSystem.current.Select += OpenPanel;
        InteractionSystem.current.UnSelect += ClosePanel;
    }

    private void OpenPanel(Actor actor)
    {
        switch(actor)
        {
            case Resource:
                openPanel = resourcePanel;
                openPanel.SetActive(true);

                openPanel.GetComponent<UIInteractableResourcePanel>().Initialize((Resource)actor);
                break;

            case B_TownHall:
                openPanel = townHallPanel;
                openPanel.SetActive(true);

                openPanel.GetComponent<UIInteractableTownHallPanel>().Initialize((B_TownHall)actor);
                break;

            default:
                openPanel = otherPanel;
                openPanel.SetActive(true);
                break;
        }
    }

    private void ClosePanel()
    {
        if(openPanel)
        {
            openPanel.SetActive(false);
            openPanel = null;
        }
    }
}
