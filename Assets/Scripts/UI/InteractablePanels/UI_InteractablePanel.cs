using TMPro;
using UnityEngine;

public class UI_InteractablePanel : MonoBehaviour 
{
    [SerializeField] protected TextMeshProUGUI ui_name;
    [SerializeField] protected TextMeshProUGUI ui_currentHP;
    private Actor actor;

    public virtual void Initialize(Actor _actor)
    {
        actor = _actor;
        
        ui_name.text = actor.nameActor;
        
        ui_currentHP.text = actor.GetCurrentHP().ToString();
        actor.UpdateCurrentHP += RefreshCurrentHP;

        actor.DestroyActor += Hide;
    }

    private void RefreshCurrentHP(float _currentHP)
    {
        ui_currentHP.text = _currentHP.ToString();
    }

    private void Hide()
    {
        UnsubscriptionActions();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        UnsubscriptionActions();
    }

    protected virtual void UnsubscriptionActions()
    {
        actor.DestroyActor -= Hide;
        actor.UpdateCurrentHP -= RefreshCurrentHP;
    }
}