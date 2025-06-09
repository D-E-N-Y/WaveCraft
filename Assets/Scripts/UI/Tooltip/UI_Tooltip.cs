using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement), typeof(RectTransform))]
public class UI_Tooltip : UIPanel
{
    [SerializeField] protected TextMeshProUGUI ui_header, ui_content;
    private LayoutElement layoutElement;
    private RectTransform rectTransform;
    [SerializeField, Range(1, 250)] private int characterWrapLimit;

    public void Initialize()
    {
        layoutElement = GetComponent<LayoutElement>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetContent(string content, string header = "")
    {
        SetText(content, header);
        SetPosition();
    }

    protected void SetText(string content, string header)
    {
        ui_header.gameObject.SetActive(!string.IsNullOrEmpty(header));
        ui_header.text = header;
        ui_content.text = content;

        int headerLength = ui_header.text.Length;
        int contentLength = ui_content.text.Length;

        layoutElement.enabled = headerLength > characterWrapLimit || contentLength > characterWrapLimit;
    }

    protected void SetPosition()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}