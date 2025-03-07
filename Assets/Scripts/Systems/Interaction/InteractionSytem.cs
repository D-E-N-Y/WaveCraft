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

    [SerializeField] private GameObject wall_1, wall_2;
    private List<D_Wall> walls;

    public override void Initialize()
    {
        base.Initialize();

        current = this;

        layerInteractable = LayerMask.NameToLayer("Interactable");
        layerSelect = LayerMask.NameToLayer("SelectedActor");
        walls = new List<D_Wall>();
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
                    selectActor = raycastHit.transform.gameObject.GetComponent<Actor>();
                    selectActor.Interaction();

                    Select?.Invoke(selectActor);
                }

                walls.Add(Instantiate(wall_1, raycastHit.point, Quaternion.identity).GetComponent<D_Wall>());

                if(walls.Count == 2)
                {
                    int countWalls = (int)(Vector3.Distance(walls[0].transform.position, walls[1].transform.position) / wall_2.GetComponent<D_Wall>().GetWallLength()) * 2;
                    Vector3 V = walls[0].transform.position - walls[1].transform.position;

                    Debug.Log($"{countWalls} {wall_2.GetComponent<D_Wall>().GetWallLength()}");

                    for (int i = 1; i < countWalls; i += 2)
                    {
                        Vector3 spawnPosition = walls[0].transform.position - V * (float)i / countWalls;
                    
                        GameObject wall = Instantiate(wall_2, spawnPosition, Quaternion.identity);
                        wall.transform.LookAt(walls[1].transform.position);
                    }

                    walls.Clear();
                }
            }
        }
    }
}