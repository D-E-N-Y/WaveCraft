using TMPro;
using UnityEngine;
public class UI_Build : MonoBehaviour
{
    [SerializeField] protected GameObject building;

    [SerializeField] protected TextMeshProUGUI name;
    [SerializeField] protected TextMeshProUGUI health;
    [SerializeField] protected TextMeshProUGUI price;

    public void BuyBuilding()
    {
        BuildSystem.current.InitializeWithObject(building);
    }
}
