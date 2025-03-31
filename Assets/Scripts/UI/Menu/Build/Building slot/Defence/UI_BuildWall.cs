public class UI_BuildWall : UI_BuildingSlot
{
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        Building building = this.building.GetComponent<Building>();

        _name.text = building.nameActor;
        health.text = building.GetMaxHP().ToString();

        foreach(S_CostUI cost in cost)
        {
            cost.amount.text = building.GetCostByResource(cost.resourse).ToString();
        }
    }
}