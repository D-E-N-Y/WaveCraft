using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected string header;
    [SerializeField, Multiline()] protected string content;

    protected static LTDescr delay;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(0.5f, () =>
        {
            TooltipSystem.current.Show(content, header);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.current.Hide();
    }

    private void OnDisable()
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.current.Hide();
    }
}