using UnityEngine;
using UnityEngine.UI;

public class UI_InteractableIndicator : UIPanel
{
    [SerializeField] private Image ui_iconAcor;
    [SerializeField] private Button ui_focusToActorButton;
    private UI_InteractablePanel ui_interactablePanel;

    private Actor actor;

    private Renderer topYRenderer;
    private Vector3 topWorldPoint;
    private Vector2 minPos, maxPos;

    private FocusSystem focusSystem;

    private Camera _camera;
    private RectTransform _rectTransform;
    private RectTransform _parentRect;

    public void Initialize(RectTransform canvas)
    {
        focusSystem = FocusSystem.current;

        _camera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
        _parentRect = canvas;

        isShow = false;
    }

    public void InitializeInfo(Actor actor, Sprite iconActor, UI_InteractablePanel ui_interactablePanel, Renderer topYRenderer, Vector3 topWorldPoint)
    {
        ui_iconAcor.sprite = iconActor;

        ui_focusToActorButton.onClick.RemoveAllListeners();
        ui_focusToActorButton.onClick.AddListener(() => focusSystem.FocusToPoint(topWorldPoint));

        this.actor = actor;
        this.ui_interactablePanel = ui_interactablePanel;

        this.topYRenderer = topYRenderer;
        this.topWorldPoint = topWorldPoint;

        minPos = new Vector2(
            -_parentRect.rect.width / 2 + _rectTransform.rect.width / 2,
            -_parentRect.rect.height / 2
        );

        maxPos = new Vector2(
            _parentRect.rect.width / 2 - _rectTransform.rect.width / 2,
            _parentRect.rect.height / 2 - _rectTransform.rect.height
        );

        SubscriptionActions();
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

        if ((localPoint.x > -_parentRect.rect.width / 2 && localPoint.x < _parentRect.rect.width / 2) &&
            (localPoint.y > -_parentRect.rect.height / 2 && localPoint.y < _parentRect.rect.height / 2))
        {
            ui_interactablePanel.SetLocalPosition(localPoint);
            ui_interactablePanel.InitializeInfo(actor);
            ui_interactablePanel.Show();

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

    private void UnsubscriptionActions()
    {
        actor.DestroyActor -= Hide;
    }

    private void SubscriptionActions()
    {
        actor.DestroyActor += Hide;
    }

    public override void Hide()
    {
        UnsubscriptionActions();
        base.Hide();
    }

    void OnDisable()
    {
        UnsubscriptionActions();
    }
}