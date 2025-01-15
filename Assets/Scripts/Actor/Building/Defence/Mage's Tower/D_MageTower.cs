using System.Collections;
using UnityEngine;

public class D_MageTower : B_Defence, ISpawnUnit
{
    [SerializeField] private GameObject spawnUnitPref;
    [SerializeField] private float timeToSpawnUnit;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private S_Cost spawnCost;
    
    public void SpawnUnit()
    {
        StartCoroutine(nameof(Spawn));
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(timeToSpawnUnit);

        Unit unit = Instantiate(spawnUnitPref, spawnPosition).GetComponent<Unit>();
        unit.Initialize();

        Debug.Log("spawn mage");
    }
}
