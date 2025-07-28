using System.Collections;
using UnityEngine;

public abstract class D_Spawner : B_Defence, ISpawnUnit, ICircleZone
{
    [SerializeField] private U_Village spawnUnit;
    [SerializeField] private float timeToSpawnUnit;
    [SerializeField] private Transform spawnPosition;

    public void SpawnUnit()
    {
        ResourceSystem.current.RemoveResourceByType(spawnUnit.GetSpawnCost().resourse, spawnUnit.GetSpawnCost().count);

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

    public EVillageProfession GetProfessionUnit() => spawnUnit.Profession();
    public float GetTimeToSpawnUnit() => timeToSpawnUnit;
    public int GetCostSpawnUnit() => spawnUnit.GetSpawnCost().count;

    #region Expansion Plaze Zone

    [SerializeField] private SCircleZone expansionPlaceZone;
    public SCircleZone GetCircleZone() => expansionPlaceZone;

    #endregion
}