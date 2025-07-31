using UnityEngine;
using System;

[RequireComponent(typeof(UnitAnimator))]
[RequireComponent(typeof(CollisionDetector))]
[RequireComponent(typeof(Mover))]

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private GameObject _pocket;

    private UnitAnimator _animator;
    private CollisionDetector _collisionDetector;
    private Mover _mover;

    private UnitState _currentState = UnitState.Idle;
    private Resource _targetResource;

    public event Action<Resource,Unit> Finished;

    private enum UnitState { Idle, MovingToResource, MovingToBase }

    public bool IsIdle => _currentState == UnitState.Idle;

    private void Awake()
    {
        _animator = GetComponent<UnitAnimator>();
        _collisionDetector = GetComponent<CollisionDetector>();
        _mover = GetComponent<Mover>();
    }

    private void CollectResource(Resource resource)
    {
        if (resource != _targetResource)
        {
            return;
        }

        _collisionDetector.FindedResorces -= CollectResource;

        _mover.StopActiveCoroutine();

        TakeResource(_targetResource);

        SwitchState(UnitState.MovingToBase);

        _collisionDetector.ArrivedToBase += DeliverResource;

        _mover.MoveToTarget(_base.transform.position);
    }

    public void DeliverResource()
    {
        _collisionDetector.ArrivedToBase -= DeliverResource;

        _mover.StopActiveCoroutine();

        SwitchState(UnitState.Idle);

        Finished?.Invoke(_targetResource, this);
    }

    public void AssignTask(Resource resource)
    {
        _targetResource = resource;

        SwitchState(UnitState.MovingToResource);

        _collisionDetector.FindedResorces += CollectResource;

        _mover.MoveToTarget(_targetResource.transform.position);
    }

    private void TakeResource(Resource resource)
    {
        resource.transform.SetParent(_pocket.transform);
        resource.transform.localPosition = Vector3.zero;
        resource.transform.localRotation = Quaternion.Euler(0f,90f,0f);
        resource.transform.localScale = Vector3.one * 0.5f;
    }

    private void SwitchState(UnitState state)
    {
        _currentState = state;

        switch (state) 
        {
            case UnitState.Idle:
                _animator.SetIdle();
                break;
            case UnitState.MovingToResource:
                _animator.SetWalk();
                break;
            case UnitState.MovingToBase:
                _animator.SetWalkBack();
                break;
            default:
                break;
        } 
    }
}
