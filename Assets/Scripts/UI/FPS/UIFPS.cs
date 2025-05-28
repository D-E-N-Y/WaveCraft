using TMPro;
using UnityEngine;

public class UIFPS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ui_fps;
    float timer;
    int fps;

    private void Update()
    {
        timer += Time.deltaTime;
        fps++;

        if (timer >= 1)
        {
            ui_fps.text = fps.ToString();

            timer = 0;
            fps = 0;
        }
    }
}
