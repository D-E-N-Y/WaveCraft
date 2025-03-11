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
                    float totalDistance = Vector3.Distance(walls[0].transform.position, walls[1].transform.position);
                    float wallLength = wall_2.GetComponent<D_Wall>().GetWallLength();

                    float countWalls = (totalDistance - wallLength) / wallLength;
                    countWalls = MathF.Round(countWalls);
                    
                    Vector3 direction = (walls[0].transform.position - walls[1].transform.position).normalized;
                    Vector3 middlePosition = (walls[1].transform.position + walls[0].transform.position) / 2;
                    Vector3 startPosition = middlePosition + direction * (countWalls * wallLength / 2);

                    for (int i = 0; i < countWalls + 1; i++)
                    {
                        Vector3 spawnPosition = startPosition - direction * (i * wallLength);
                    
                        GameObject wall = Instantiate(wall_2, spawnPosition, Quaternion.identity);
                        wall.transform.LookAt(walls[1].transform.position);
                    }

                    walls.Clear();
                }
            }
        }
    }
}