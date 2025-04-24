using UnityEngine;

public class UIPanel : MonoBehaviour 
{
    public bool isCanClose { protected set; get; } = true;
    
    public virtual void Show() => gameObject.SetActive(true);
    public virtual void Hide() => gameObject.SetActive(false);
}