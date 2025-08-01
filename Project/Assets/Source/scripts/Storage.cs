using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Storage : MonoBehaviour
{
    private List<Resource> _availableResources = new List<Resource>();
    private List<Resource> _occupiedResources = new List<Resource>();
    private List<Resource> _resources = new List<Resource>();

    public event Action ResourceChanged;

    public int Resources => _resources.Count;

    public void RegisterResource(Resource resource)
    {
        if (_availableResources.Contains(resource) == false && _occupiedResources.Contains(resource) == false)
        {
            _availableResources.Add(resource);
        }
    }

    public bool TryOccupyResource(out Resource resource)
    {
        resource = null;

        if (_availableResources.Count == 0)
        {
            return false;
        }
        else
        {
            resource = _availableResources.FirstOrDefault();

            _availableResources.Remove(resource);
            _occupiedResources.Add(resource);

            return true;
        } 
    }

    public void RemoveResource(Resource resource)
    {
        _resources.Add(resource);

        ResourceChanged?.Invoke();

        _occupiedResources.Remove(resource);

        Destroy(resource.gameObject);
    }

    public void ClearAvailableResources()
    {
        _availableResources.Clear();
    }

}
