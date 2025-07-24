using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class UI_InteractablePanel : UIPanel
{
    [SerializeField] private Sprite iconActor;

    [SerializeField] protected TextMeshProUGUI ui_name;
    [SerializeField] protected TextMeshProUGUI ui_currentHP;
    public abstract Type PanelType { get; }
    private Actor actor;

    private Camera _camera;
    private RectTransform _rectTransform;
    private RectTransform _parentRect;

    private UI_InteractableIndicator ui_indicator;

    private Coroutine live;

    public void Initialize(RectTransform canvas, UI_InteractableIndicator ui_indicator)
    {
        _camera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();

        _parentRect = canvas;
        this.ui_indicator = ui_indicator;

        isShow = false;
    }

    public virtual void InitializeInfo(Actor _actor)
    {
        actor = _actor;

        ui_name.text = actor.nameActor;

        ui_currentHP.text = actor.GetCurrentHP().ToString();
        actor.UpdateCurrentHP += RefreshCurrentHP;

        actor.DestroyActor += Hide;

        CalculateTopPoint();
    }

    private void Update()
    {
        if (topWorldPoint == null) return;

        if (actor as Unit)
        {
            topWorldPoint = new Vector3(
                topYRenderer.bounds.center.x,
                topYRenderer.bounds.max.y,
                topYRenderer.bounds.center.z
            );
        }

        Vector2 screenPos = _camera.WorldToScreenPoint(topWorldPoint);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentRect,
            screenPos,
            null,
            out Vector2 localPoint
        );

        if (localPoint.x < -_parentRect.rect.width * 0.75 || localPoint.x > _parentRect.rect.width * 0.75 ||
            localPoint.y < -_parentRect.rect.height * 0.75 || localPoint.y > _parentRect.rect.height * 0.75)
        {
            ui_indicator.SetLocalPosition(localPoint);
            ui_indicator.InitializeInfo(actor, iconActor, this, topYRenderer, topWorldPoint);
            ui_indicator.Show();

            Hide();
        }

        localPoint.x = Mathf.Clamp(localPoint.x, minPos.x + 10, maxPos.x - 10);
        localPoint.y = Mathf.Clamp(localPoint.y, minPos.y + 70, maxPos.y - 60);

        SetLocalPosition(localPoint);
    }

    public void SetLocalPosition(Vector2 localPoint)
    {
        _rectTransform.localPosition = localPoint;
    }

    private void RefreshCurrentHP(float _currentHP)
    {
        ui_currentHP.text = _currentHP.ToString();
    }

    public override void Hide()
    {
        UnsubscriptionActions();

        if (ui_indicator.isShow && !isShow)
        {
            ui_indicator.Hide();
        }

        base.Hide();
    }

    private void OnDisable()
    {
        UnsubscriptionActions();

        if (ui_indicator.isShow && !isShow)
        {
            ui_indicator.Hide();
        }
    }

    protected virtual void UnsubscriptionActions()
    {
        actor.DestroyActor -= Hide;
        actor.UpdateCurrentHP -= RefreshCurrentHP;
    }

    private Renderer[] meshRenderers;
    private Renderer topYRenderer;
    private Vector3 topWorldPoint;
    private Vector2 minPos, maxPos;
    private void CalculateTopPoint()
    {
        if (actor.GetMesh().TryGetComponent(out Renderer renderer))
            meshRenderers = new Renderer[] { renderer };
        else
            meshRenderers = actor.GetMesh().GetComponentsInChildren<Renderer>();

        if (meshRenderers.Length > 0)
        {
            topYRenderer = meshRenderers.OrderByDescending(x => x.bounds.max.y).FirstOrDefault();

            topWorldPoint = new Vector3(
                topYRenderer.bounds.center.x,
                topYRenderer.bounds.max.y,
                topYRenderer.bounds.center.z
            );

            minPos = new Vector2(
                -_parentRect.rect.width / 2 + _rectTransform.rect.width / 2,
                -_parentRect.rect.height / 2
            );

            maxPos = new Vector2(
                _parentRect.rect.width / 2 - _rectTransform.rect.width / 2,
                _parentRect.rect.height / 2 - _rectTransform.rect.height
            );
        }
    }
}