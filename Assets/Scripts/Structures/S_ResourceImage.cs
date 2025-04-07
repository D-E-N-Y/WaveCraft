using UnityEngine;

[System.Serializable]
public struct ResourceImage
{
    public E_Resource resource;
    public Sprite image;

    public ResourceImage(E_Resource resource, Sprite image)
    {
        this.resource = resource;
        this.image = image;
    }
}