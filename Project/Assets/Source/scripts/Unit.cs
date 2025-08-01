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
    private Resource _targetResource;

    private Vector3 _dropOffPoint;

    public bool IsIdle { get; private set; } = true;
    public bool HaveResource { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<UnitAnimator>();
        _collisionDetector = GetComponent<CollisionDetector>();
        _mover = GetComponent<Mover>();
    }

    public Resource GetResource() => _targetResource;

    public void Release()
    {
        IsIdle = true;
        HaveResource = false;
        _targetResource = null;

        _mover.StopActiveCoroutine();
        _animator.SetIdle();
    }

    public void AssignTask(Resource resource, Vector3 dropOffPoint)
    {
        _dropOffPoint = dropOffPoint;
        _targetResource = resource;
        IsIdle = false;
        HaveResource = false;

        _collisionDetector.FindedResorces += CollectResource;

        _mover.MoveToTarget(_targetResource.transform.position);
        _animator.SetWalk();
    }

    private void CollectResource(Resource resource)
    {
        if (resource != _targetResource)
        {
            return;
        }

        _collisionDetector.FindedResorces -= CollectResource;

        TakeResource(_targetResource);

        _mover.MoveToTarget(_dropOffPoint);
        _animator.SetWalkBack();
    }

    private void TakeResource(Resource resource)
    {
        HaveResource = true;

        resource.transform.SetParent(_pocket.transform);
        resource.transform.localPosition = Vector3.zero;
        resource.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        resource.transform.localScale = Vector3.one * 0.5f;
    }
}
