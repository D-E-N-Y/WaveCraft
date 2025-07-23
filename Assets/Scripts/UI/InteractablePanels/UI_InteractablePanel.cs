using System;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class UI_InteractablePanel : UIPanel
{
    [SerializeField] protected TextMeshProUGUI ui_name;
    [SerializeField] protected TextMeshProUGUI ui_currentHP;
    public abstract Type PanelType { get; }
    private Actor actor;
    private Camera _camera;
    RectTransform _rectTransform;
    RectTransform _parentRect;

    public virtual void Initialize(Actor _actor)
    {
        actor = _actor;

        ui_name.text = actor.nameActor;

        ui_currentHP.text = actor.GetCurrentHP().ToString();
        actor.UpdateCurrentHP += RefreshCurrentHP;

        actor.DestroyActor += Hide;

        _camera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
        _parentRect = _rectTransform.parent.parent as RectTransform;

        CalculateTopPoint();
    }

    private void Update()
    {
        if (topYRenderer == null) return;

        Vector3 topWorldPoint = new Vector3(
            topYRenderer.bounds.center.x,
            topYRenderer.bounds.max.y,
            topYRenderer.bounds.center.z
        );

        Vector2 screenPos = _camera.WorldToScreenPoint(topWorldPoint);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentRect,
            screenPos,
            null,
            out Vector2 localPoint
        );

        Vector2 minPos = new Vector2(
            -_parentRect.rect.width / 2 + 10 + _rectTransform.rect.width / 2,
            -_parentRect.rect.height / 2 + 70
        );

        Vector2 maxPos = new Vector2(
            _parentRect.rect.width / 2 - 10 - _rectTransform.rect.width / 2,
            _parentRect.rect.height / 2 - 60 - _rectTransform.rect.height
        );

        localPoint.x = Mathf.Clamp(localPoint.x, minPos.x, maxPos.x);
        localPoint.y = Mathf.Clamp(localPoint.y, minPos.y, maxPos.y);

        _rectTransform.localPosition = localPoint;
    }

    private void RefreshCurrentHP(float _currentHP)
    {
        ui_currentHP.text = _currentHP.ToString();
    }

    public override void Hide()
    {
        UnsubscriptionActions();
        base.Hide();
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

    private Renderer[] meshRenderers;
    private Renderer topYRenderer;
    private void CalculateTopPoint()
    {
        if (actor.GetMesh().TryGetComponent(out Renderer renderer))
            meshRenderers = new Renderer[] { renderer };
        else
            meshRenderers = actor.GetMesh().GetComponentsInChildren<Renderer>();

        if (meshRenderers.Length > 0)
        {
            topYRenderer = meshRenderers.OrderByDescending(x => x.bounds.max.y).FirstOrDefault();
        }
    }
}