using UnityEngine;

public class UIPanel : MonoBehaviour 
{
    public bool isShow { protected set; get; }

    public bool isCanClose { protected set; get; } = true;
    
    public virtual void Show()
    {
        gameObject.SetActive(true);
        isShow = true;
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        isShow = false;
    }
}