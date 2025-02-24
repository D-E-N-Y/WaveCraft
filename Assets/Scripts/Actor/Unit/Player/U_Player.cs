public class U_Player : Unit
{
    public override void Initialize()
    {
        base.Initialize();

        VillageSystem.current.AddVillage(this);
    }
}