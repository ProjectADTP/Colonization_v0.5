using UnityEngine;
using System;

[RequireComponent(typeof(UnitAnimator))]
[RequireComponent(typeof(CollisionDetector))]
[RequireComponent(typeof(Mover))]

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject _pocket;

    private UnitAnimator _animator;
    private CollisionDetector _collisionDetector;
    private Mover _mover;

    private UnitState _currentState = UnitState.Idle;
    private Resource _targetResource;
    private Vector3 _dropOffPoint;

    private bool haveResource;

    public event Action<Resource,Unit> Finished;

    private enum UnitState { Idle, MovingToPoint }

    public bool IsIdle => _currentState == UnitState.Idle;

    private void Awake()
    {
        _animator = GetComponent<UnitAnimator>();
        _collisionDetector = GetComponent<CollisionDetector>();
        _mover = GetComponent<Mover>();
    }

    public void AssignTask(Resource resource, Vector3 dropOffPoint)
    {
        _targetResource = resource;
        _dropOffPoint = dropOffPoint;

        SwitchState(UnitState.MovingToPoint);

        _collisionDetector.FindedResorces += CollectResource;

        _mover.MoveToTarget(_targetResource.transform.position);
    }

    private void DeliverResource()
    {
        _collisionDetector.ArrivedToBase -= DeliverResource;

        _mover.StopActiveCoroutine();

        haveResource = false;

        SwitchState(UnitState.Idle);

        Finished?.Invoke(_targetResource, this);
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

        haveResource = true;

        SwitchState(UnitState.MovingToPoint);

        _collisionDetector.ArrivedToBase += DeliverResource;

        _mover.MoveToTarget(_dropOffPoint);
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
            case UnitState.MovingToPoint:
                if (haveResource) 
                    _animator.SetWalkBack();
                else
                    _animator.SetWalk();
                break;
            default:
                break;
        } 
    }
}
