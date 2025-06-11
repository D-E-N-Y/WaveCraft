using System;
using System.Collections;
using UnityEngine;

public class TerrainResourceSpawner : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private GameObject[] resourcePrefabs;
    private TreeInstance[] savedTreeData; 

    public IEnumerator Initializing(UILoadingScreen ui_loadingScreen)
    {
        TerrainData terrainData = terrain.terrainData;
        TreeInstance[] resources = terrainData.treeInstances;

        int _count = 0;
        ui_loadingScreen.SetMaxPartProgress(resources.Length / 100);

        foreach (TreeInstance _resource in resources)
        {
            if (_resource.prototypeIndex >= resourcePrefabs.Length) continue;

            Vector3 _position = ConvertToWorldPosition(_resource.position);
            Quaternion _rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            Resource resource = Instantiate(resourcePrefabs[_resource.prototypeIndex], _position, _rotation).GetComponent<Resource>();
            resource.transform.localScale = new Vector3(_resource.heightScale, _resource.heightScale, _resource.heightScale);
            resource.transform.SetParent(this.transform);
            resource.Initialize();

            _count++;

            if (_count < 100) continue;

            ui_loadingScreen.AddPartProgress();
            _count = 0;

            yield return null;
        }

        savedTreeData = resources;
        terrainData.treeInstances = new TreeInstance[0];
    }

    private Vector3 ConvertToWorldPosition(Vector3 terrainPos)
    {
        Vector3 worldPos = new Vector3(
            terrainPos.x * terrain.terrainData.size.x + terrain.transform.position.x,
            terrainPos.y * terrain.terrainData.size.y,
            terrainPos.z * terrain.terrainData.size.z + terrain.transform.position.z
        );
        return worldPos;
    }

    void OnDisable()
    {
        TerrainData terrainData = terrain.terrainData;
        terrainData.treeInstances = savedTreeData;
    }
}
