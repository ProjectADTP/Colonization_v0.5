using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float _scanRadius = 50f;

    public void ScanForResources(ref List<Resource> resources)
    {
        resources.Clear();

        Collider[] hits = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Resource resource))
            {
                resources.Add(resource);
            }
        }
    }
}