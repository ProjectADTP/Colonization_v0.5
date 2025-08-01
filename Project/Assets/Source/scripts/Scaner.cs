using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float _scanRadius = 50f;

    [SerializeField] private LayerMask _resourceLayer;

    private Collider[] _hits = new Collider[20];

    public void ScanForResources(Storage storage)
    {
        storage.ClearAvailableResources();

        Physics.OverlapSphereNonAlloc(transform.position, _scanRadius, _hits, _resourceLayer);

        foreach (Collider hit in _hits)
        {
            if (hit == null)
            {
                continue;
            }

            if (hit.TryGetComponent(out Resource resource))
            {
                storage.RegisterResource(resource);
            }
        }
    }
}