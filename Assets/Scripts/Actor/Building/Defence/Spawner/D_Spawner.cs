using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class D_Spawner : B_Defence, ISpawnUnit, ICircleZone
{
    [SerializeField] private U_Village spawnUnit;
    [SerializeField] private float timeToSpawnUnit;
    [SerializeField] private Transform spawnPosition;

    public override void Initialize()
    {
        base.Initialize();

        List<SCircleZone> zones = new List<SCircleZone>();
        zones.Add(GetCircleZone());

        expansionPlaceZone.Initialize();
        expansionPlaceZone.DrawLines(zones);
        expansionPlaceZone.gameObject.SetActive(true);
    }

    public override void Place()
    {
        base.Place();
        expansionPlaceZone.gameObject.SetActive(false);
    }

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

    [SerializeField] private CircleZone expansionPlaceZone;
    [SerializeField] private float radiusExpansionPlaceZone;
    public SCircleZone GetCircleZone() => new SCircleZone(transform, radiusExpansionPlaceZone);

    #endregion
}