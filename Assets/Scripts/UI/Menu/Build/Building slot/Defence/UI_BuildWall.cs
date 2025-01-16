public class UI_BuildWall : UI_BuildingSlot
{
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        D_Wall wall = building.GetComponent<D_Wall>();
        
        _name.text = "Wall";
        health.text = wall.GetMaxHP().ToString();

        foreach(S_CostUI cost in cost)
        {
            cost.amount.text = wall.GetCostByResource(cost.resourse).ToString();
        }
    }
}