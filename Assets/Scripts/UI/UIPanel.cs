using UnityEngine;

public class UIPanel : MonoBehaviour 
{
    public virtual void Show() => gameObject.SetActive(true);
    public virtual void Hide() => gameObject.SetActive(false);
}