using System;

public class UI_Resource : UI_InteractablePanel
{
    public override Type PanelType => typeof(Resource);   
    private Resource resource;

    public override void Initialize(Actor _actor)
    {
        base.Initialize(_actor);

        resource = (Resource)_actor;
    }

    public void Mine()
    {
        MiningTask task = new MiningTask(resource);
        TaskSystem.current.AddTask(task);
    }
}
