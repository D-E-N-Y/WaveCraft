using TMPro;
using UnityEngine;
public class UI_Build : MonoBehaviour
{
    [SerializeField] protected GameObject building;

    [SerializeField] protected TextMeshProUGUI _name;
    [SerializeField] protected TextMeshProUGUI health;
    [SerializeField] protected TextMeshProUGUI price;

    public void BuyBuilding()
    {
        Debug.Log("building");
        
        BuildSystem.current.InitializeWithObject(building);
    }
}
