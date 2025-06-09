using System.Collections;
using UnityEngine;

public abstract class D_Spawner : B_Defence, ISpawnUnit
{
    [SerializeField] private U_Player spawnUnit;
    [SerializeField] private float timeToSpawnUnit;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private S_Cost spawnCost;

    public void SpawnUnit()
    {
        ResourceSystem.current.RemoveResourceByType(E_Resource.Food, spawnCost.count);

        StartCoroutine(Spawning());
    }

    public void CancelSpawnUnit()
    {

    }

    private IEnumerator Spawning()
    {
        yield return new WaitForSeconds(timeToSpawnUnit);

        Unit unit = Instantiate(spawnUnit, spawnPosition);
        unit.Initialize();

        Debug.Log("spawn unit");
    }

    public EVillageType GetTypeUnit() => spawnUnit.Type();
    public float GetTimeToSpawnUnit() => timeToSpawnUnit;
    public int GetCostSpawnUnit() => spawnCost.count;
}