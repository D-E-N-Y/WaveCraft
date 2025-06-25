using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Message : UIPanel
{
    [SerializeField] private TextMeshProUGUI ui_message;
    [SerializeField, Range(1f, 10f)] private float liveTime;
    public bool isAvaliable { get; private set; }

    public void Initialize()
    {
        isAvaliable = true;
    }

    public void InitializeMessage(string message)
    {
        Show();

        isAvaliable = false;
        ui_message.text = message;

        StartCoroutine(nameof(Live));
    }

    private IEnumerator Live()
    {
        yield return new WaitForSeconds(liveTime);

        isAvaliable = true;
        Hide();
    }
}
