using TMPro;
using UnityEngine;

public class UIInteractableResourcePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    private Resource resource;

    public void Initialize(Resource resource)
    {
        this.resource = resource;
        
        nameText.text = resource.nameActor;
        hpText.text = resource.GetCurrentHP().ToString();
    }

    public void Mine()
    {
        MiningTask task = new MiningTask(resource);
        TaskSystem.current.AddTask(task);
    }
}
