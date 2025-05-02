using UnityEngine;

public class FocusSystem : GameSystem
{
    public static FocusSystem current;
    
    [SerializeField] private CameraControler cameraControler;

    public override void Initialize()
    {
        current = this;
    }

    public void FocusToObject(Actor actor)
    {
        cameraControler.FocusToObject(actor);

        InteractionSystem.current.UnSelectActor();
        InteractionSystem.current.SelectActor(actor);
    }
}