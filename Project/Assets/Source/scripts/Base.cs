using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Base : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float _scanRadius = 50f;

    [Header("Статистика")]
    [SerializeField] private List<Unit> _units = new List<Unit>();

    private List<Resource> _availableResources = new List<Resource>();
    private List<Resource> _resources = new List<Resource>();

    public event Action ResourceChanged;

    public event Action OnBaseClicked;

    public int Resources => _resources.Count;
    public int Units => _units.Count;

    public void ScanForResources()
    {
        _availableResources.Clear();

        Collider[] hits = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Resource resource))
            {
                _availableResources.Add(resource);
            }
        }

        if (_availableResources.Count > 0) 
        {
            AssignTasks();
        }
    }

    private void OnMouseDown()
    {
        OnBaseClicked?.Invoke();
    }

    private void AssignTasks()
    {
        foreach (Resource resource in _availableResources)
        {
            if (resource == null)
            {
                _availableResources.Remove(resource);

                continue;
            }

            if (resource.IsAvailable == false)
            {
                continue;
            }

            foreach (Unit unit in _units)
            {
                if (unit.IsIdle)
                {
                    resource.Reserve();

                    _availableResources.Remove(resource);

                    unit.Finished += AddResource;
                    unit.AssignTask(resource);

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