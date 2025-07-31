using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private readonly int Idle = Animator.StringToHash(nameof(Idle));
    private readonly int Walk = Animator.StringToHash(nameof(Walk));
    private readonly int WalkBack = Animator.StringToHash(nameof(WalkBack));

    [SerializeField] private Animator _animator;

    public void SetIdle()
    {
        _animator.SetTrigger(Idle);
    }

    public void SetWalk()
    {
        _animator.SetTrigger(Walk);
    }

    public void SetWalkBack()
    {
        _animator.SetTrigger(WalkBack);
    }
}
