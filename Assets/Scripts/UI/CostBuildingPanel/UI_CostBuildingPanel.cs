using TMPro;
using UnityEngine;

public class UI_CostBuildingPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ui_woodCost;
    [SerializeField] private TextMeshProUGUI ui_stoneCost;

    public void UpdateCost(int wood, int stone)
    {
        ui_woodCost.text = wood.ToString();
        ui_stoneCost.text = stone.ToString();
    }
}
