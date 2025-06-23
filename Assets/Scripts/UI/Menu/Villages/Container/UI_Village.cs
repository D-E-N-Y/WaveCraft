using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Village : UIPanel
{
    [SerializeField] private Image ui_image;
    [SerializeField] private TextMeshProUGUI ui_name;
    [SerializeField] private TextMeshProUGUI ui_profession;

    private U_Village village;

    public void Initialize(U_Village village, Sprite sprite)
    {
        this.village = village;

        ui_image.sprite = sprite;
        ui_name.text = village.nameActor;
        ui_profession.text = village.Profession().ToString();
    }
}
