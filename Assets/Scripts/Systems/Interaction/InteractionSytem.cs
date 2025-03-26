using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionSystem : GameSystem
{
    public static InteractionSystem current;
    public Action<Actor> Select;
    public Action UnSelect;

    private int layerInteractable; 
    private int layerSelect;
    private Actor selectActor;

    public override void Initialize()
    {
        base.Initialize();

        current = this;

        layerInteractable = LayerMask.NameToLayer("Interactable");
        layerSelect = LayerMask.NameToLayer("SelectedActor");
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit raycastHit))
            return raycastHit.point;
        else
            return Vector3.zero;
    }

    private void Update() 
    {
        if (EventSystem.current.IsPointerOverGameObject()) 
        {
            return;
        }
        
        if(Input.GetMouseButtonUp((int)MouseButton.Left))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, 999))
            {
                if(selectActor)
                {
                    selectActor.DisInteraction();
                    selectActor = null;

                    UnSelect?.Invoke();
                }

                if(raycastHit.transform.gameObject.layer == layerInteractable || raycastHit.transform.gameObject.layer == layerSelect)
                {
                    Transform actor = raycastHit.transform;
                    
                    while (true)
                    {
                        if(actor.transform.parent == null || actor.gameObject.GetComponent<Actor>() != null)
                        {
                            break;
                        }
                        
                        actor = actor.transform.parent;
                    }

                    selectActor = actor.gameObject.GetComponent<Actor>();
                    selectActor.Interaction();

                    Select?.Invoke(selectActor);
                }
            }
        }
    }
}