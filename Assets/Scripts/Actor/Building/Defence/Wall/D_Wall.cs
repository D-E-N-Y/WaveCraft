using TMPro;
using UnityEngine;

public class D_Wall : B_Defence
{
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    [SerializeField] private TextMeshProUGUI ui_text;

    public float GetWallLength() => Vector3.Distance(startTransform.position, endTransform.position);

    public void SetTextUI(int value)
    {
        ui_text.text = value.ToString();
    }
}
