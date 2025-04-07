using TMPro;
using UnityEngine;

public class UI_Wall : UI_InteractablePanel
{
    [SerializeField] private GameObject buttonPanel;
    
    private D_Wall wall;

    public override void Initialize(Actor _actor)
    {
        base.Initialize(_actor);

        wall = (D_Wall)_actor;
        
        buttonPanel.SetActive(wall.Type() == E_WallType.Column);
    }

    public void ContinueWall()
    {
        if(wall.isBuild)
        {
            BuildSystem.current.ContinueWall(wall);
        }
    }
}
