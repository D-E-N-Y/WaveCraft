using System;
using UnityEngine;

public class UIBlackout : UIPanel
{
    private Animator _animator;

    public Action finalUp, finalDown;

    public void Initialize()
    {
        _animator = GetComponent<Animator>();
    }

    public void Down()
    {
        Show();
        _animator.SetTrigger("Down");
    }

    public void Up()
    {
        Show();
        _animator.SetTrigger("Up");
    }

    public void ClearFinalUpActions() => finalUp = null;
    public void ClearFinalDownActions() => finalDown = null;

    public void FinalUp() => finalUp?.Invoke();
    public void FinalDown() => finalDown?.Invoke();
}