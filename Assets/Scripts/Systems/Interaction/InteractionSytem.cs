using Unity.VisualScripting;
using UnityEngine;

public class InteractionSystem : GameSystem
{
    public static InteractionSystem current;

    private int layerInteractable; 
    private Actor selectActor;

    public override void Initialize()
    {
        base.Initialize();

        current = this;

        layerInteractable = LayerMask.NameToLayer("Interactable"); 
    }

    private void Update() 
    {
        if(Input.GetMouseButtonUp((int)MouseButton.Left))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, 999))
            {
                if(selectActor)
                {
                    selectActor.DisInteraction();
                    selectActor = null;
                }

                if(raycastHit.transform.gameObject.layer == layerInteractable)
                {
                    selectActor = raycastHit.transform.gameObject.GetComponent<Actor>();
                    selectActor.Interaction();
                }
            }
        }
    }
}