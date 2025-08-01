using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scaner))]
[RequireComponent(typeof(UnitFinder))]
[RequireComponent(typeof(Storage))]
public class Base : MonoBehaviour
{
    [Header("Статистика")]
    [SerializeField] private List<Unit> _units = new List<Unit>();

    private UnitFinder _unitFinder;
    private Scaner _scaner;
    private Storage _storage;

    public int Units => _units.Count;

    private void Awake()
    {
        _storage = GetComponent<Storage>();
        _scaner = GetComponent<Scaner>();
        _unitFinder = GetComponent<UnitFinder>();
    }

    private void OnEnable()
    {
        _unitFinder.UnitEntered += OnUnitEntered;
    }

    private void OnDisable()
    {
        _unitFinder.UnitEntered -= OnUnitEntered;
    }

    public void StartScan()
    {
        _scaner.ScanForResources(_storage);

        AssignTasks();
    }

   private void AssignTasks()
   {
        foreach (Unit unit in _units)
        {
            if (unit.IsIdle)
            {
                if (_storage.TryOccupyResource(out Resource resource))
                {
                    unit.AssignTask(resource, transform.position);

                    return;
                }
            }
        }
   }

    private void OnUnitEntered(Unit unit)
    {
        if (_units.Contains(unit) && unit.HaveResource)
        {
            Resource resource = unit.GetResource();

            _storage.RemoveResource(resource);

            unit.Release();
        }
    }
}
