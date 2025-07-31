using System;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public event Action<Resource> FindedResorces;
    public event Action ArrivedToBase;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            FindedResorces?.Invoke(resource);
        }

        if (other.TryGetComponent<Base>(out _))
        {
            ArrivedToBase?.Invoke();
        }
    }
}