using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpawnZone : MonoBehaviour
{
    private List<BoxCollider> _colliders = new List<BoxCollider>();

    void Awake()
    {
        GetComponents(_colliders);
    }

    public Vector3 GetRandomPosition()
    {
        if (_colliders.Count == 0)
        {
            return transform.position;
        }

        BoxCollider randomCollider = _colliders[Random.Range(0, _colliders.Count)];

        return GetRandomPoint(randomCollider);
    }

    private Vector3 GetRandomPoint(BoxCollider box)
    {
        Vector3 center = box.transform.TransformPoint(box.center);
        Vector3 size = Vector3.Scale(box.size, box.transform.lossyScale);

        Vector3 randomPoint = new Vector3
        (
            Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2)
        );

        return center + box.transform.rotation * randomPoint;
    }
}