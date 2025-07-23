using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, IPosition
{
    public Action<float> UpdateCurrentHP;
    public Action DestroyActor;

    [SerializeField] protected GameObject mesh;
    private string selectLayer;
    private string defaultLayer;

    [SerializeField] protected List<Transform> actorPositions;

    public abstract string nameActor { get; }

    [SerializeField] protected float maxHP;
    protected float currentHP;


    public float GetMaxHP() => maxHP;
    public float GetCurrentHP() => currentHP;
    public virtual void TakeDamage(float damage) { }


    public virtual void Interaction() => SetLayerRecursively(mesh, selectLayer);
    public virtual void DisInteraction() => SetLayerRecursively(mesh, defaultLayer);
    void SetLayerRecursively(GameObject obj, string newLayer)
    {
        obj.layer = LayerMask.NameToLayer(newLayer);

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }


    public virtual void Initialize()
    {
        currentHP = maxHP;

        selectLayer = "SelectedActor";
        defaultLayer = "Interactable";
    }


    private void OnDestroy() => DestroyActor?.Invoke();
    void OnDisable() => DestroyActor?.Invoke();

    protected virtual void Death()
    {
        gameObject.SetActive(false);
    }

    public List<Transform> GetPosition() => actorPositions;
    public Actor GetActor() => this;

    public GameObject GetMesh() => mesh;
}
