using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipActorTrigger : TooltipTrigger 
{
    [SerializeField] private Actor actor;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(0.5f, () =>
        {
            TooltipSystem.current.Show(actor, content, header);
        });
    }
}