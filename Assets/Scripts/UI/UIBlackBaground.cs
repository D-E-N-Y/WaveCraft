using UnityEngine;

public class UIBlackBaground : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Hide()
    {
        _animator.SetTrigger("Hide");
    }

    public void Show()
    {
        _animator.SetTrigger("Show");
    }

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f;
    }
}