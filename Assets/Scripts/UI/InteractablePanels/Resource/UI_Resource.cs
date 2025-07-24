using System;

public class UI_Resource : UI_InteractablePanel
{
    public override Type PanelType => typeof(Resource);   
    private Resource resource;

    public override void InitializeInfo(Actor _actor)
    {
        base.InitializeInfo(_actor);

        resource = (Resource)_actor;
    }

    public void Mine()
    {
        MiningTask task = new MiningTask(resource);
        TaskSystem.current.AddTask(task);
    }
}
