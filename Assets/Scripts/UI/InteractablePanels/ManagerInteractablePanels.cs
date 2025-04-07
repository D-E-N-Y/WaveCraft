using UnityEngine;

public class ManagerInteractablePanels : MonoBehaviour
{
    [SerializeField] private GameObject resourcePanel;
    
    // building panels
    [SerializeField] private GameObject townHallPanel;
    [SerializeField] private GameObject storagePanel;
    [SerializeField] private GameObject processorPanel;
    [SerializeField] private GameObject productionPanel;
    [SerializeField] private GameObject wallPanel;
    [SerializeField] private GameObject gatePanel;

    // unit panels
    [SerializeField] private GameObject workerPanel;

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

                openPanel.GetComponent<UI_Resource>().Initialize(actor);
                break;

            case B_TownHall:
                openPanel = townHallPanel;
                openPanel.SetActive(true);

                openPanel.GetComponent<UI_TownHall>().Initialize(actor);
                break;

            case I_Storage:
                openPanel = storagePanel;
                openPanel.SetActive(true);

                openPanel.GetComponent<UI_Storage>().Initialize(actor);
                break;

            case I_Processor:
                openPanel = processorPanel;
                openPanel.SetActive(true);

                openPanel.GetComponent<UI_Processor>().Initialize(actor);
                break;

            case I_Production:
                openPanel = productionPanel;
                openPanel.SetActive(true);

                openPanel.GetComponent<UI_Production>().Initialize(actor);
                break;

            case UP_Worker:
                openPanel = workerPanel;
                openPanel.SetActive(true);

                openPanel.GetComponent<UI_Worker>().Initialize(actor);
                break;

            case D_Wall:
                openPanel = wallPanel;
                openPanel.SetActive(true);

                openPanel.GetComponent<UI_Wall>().Initialize(actor);
                break;
            
            case D_Gate:
                openPanel = gatePanel;
                openPanel.SetActive(true);

                openPanel.GetComponent<UI_Gate>().Initialize(actor);
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
