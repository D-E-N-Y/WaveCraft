using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ui_header, ui_content;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField, Range(1, 250)] private int characterWrapLimit;

    public void SetText(string content, string header = "")
    {
        ui_header.gameObject.SetActive(!string.IsNullOrEmpty(header));
        ui_header.text = header;
        ui_content.text = content;

        int headerLength = ui_header.text.Length;
        int contentLength = ui_content.text.Length;

        layoutElement.enabled = headerLength > characterWrapLimit || contentLength > characterWrapLimit;

        SetPosition();
    }

    private void SetPosition()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}