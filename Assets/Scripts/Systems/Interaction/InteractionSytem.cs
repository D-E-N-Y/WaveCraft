using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractionSystem : GameSystem
{
    public static InteractionSystem current;
    public Action<Actor> Select;
    public Action UnSelect;

    [SerializeField] private InputActionReference interactionAction;

    private int layerInteractable; 
    private int layerSelect;
    private Actor selectActor;

    public override void Initialize()
    {
        current = this;

        layerInteractable = LayerMask.NameToLayer("Interactable");
        layerSelect = LayerMask.NameToLayer("SelectedActor");

        interactionAction.action.started += OnInteraction;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit raycastHit))
            return raycastHit.point;
        else
            return Vector3.zero;
    }

    public bool HasSelectedActor() => selectActor != null;
    
    public void SelectActor(Actor actor)
    {
        selectActor = actor;
        selectActor.Interaction();

        Select?.Invoke(selectActor);
    }

    public void UnSelectActor()
    {
        if(HasSelectedActor())
        {
            selectActor.DisInteraction();
            selectActor = null;

            UnSelect?.Invoke();
        }
    }

    private void OnInteraction(InputAction.CallbackContext context) 
    {
        if (EventSystem.current.IsPointerOverGameObject()) 
        {
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999))
        {
            UnSelectActor();

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

                SelectActor(actor.gameObject.GetComponent<Actor>());
            }
        }
    }
}