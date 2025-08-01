using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scaner))]
public class Base : MonoBehaviour
{
    [Header("Статистика")]
    [SerializeField] private List<Unit> _units = new List<Unit>();

    private List<Resource> _availableResources = new List<Resource>();
    private List<Resource> _occupiedResources = new List<Resource>();
    private List<Resource> _resources = new List<Resource>();
    
    private Scaner _scaner;

    public event Action ResourceChanged;

    public int Resources => _resources.Count;
    public int Units => _units.Count;

    private void Awake()
    {
        _scaner = GetComponent<Scaner>();
    }

    public void StartScan()
    {
        _scaner.ScanForResources(ref _availableResources);

        if (_availableResources.Count > 0)
        {
            AssignTasks();
        }
    }

    private void AssignTasks()
    {
        foreach (Resource resource in _availableResources)
        {
            if (_occupiedResources.Contains(resource))
            {
                continue;
            }

            foreach (Unit unit in _units)
            {
                if (unit.IsIdle)
                {
                    _occupiedResources.Add(resource);
                    _availableResources.Remove(resource);

                    unit.Finished += AddResource;
                    unit.AssignTask(resource, transform.position);

                    return;
                }
            }
        }
    }

    private void AddResource(Resource resource, Unit unit)
    {
        unit.Finished -= AddResource;

        _resources.Add(resource);
        Destroy(resource.gameObject);

        ResourceChanged?.Invoke();
    }
}
