using TMPro;
using UnityEngine;

public class UIInteractableTownHallPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    private B_TownHall townHall;
    
    public void Initialize(B_TownHall townHall)
    {
        this.townHall = townHall;

        nameText.text = townHall.nameActor;
        hpText.text = townHall.GetCurrentHP().ToString();
    }

    public void SpawnWorker()
    {
        townHall.StartCoroutine(townHall.SpawnUnit());
    }
}
