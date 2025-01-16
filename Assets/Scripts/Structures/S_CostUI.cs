using TMPro;

[System.Serializable]
public struct S_CostUI
{
    public E_Resource resourse;
    public TextMeshProUGUI amount;

    public S_CostUI (E_Resource resourse, TextMeshProUGUI amount)
    {
        this.resourse = resourse;
        this.amount = amount;
    }
}