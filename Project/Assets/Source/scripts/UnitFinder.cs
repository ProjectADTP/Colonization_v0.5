using System;
using UnityEngine;

public class UnitFinder : MonoBehaviour
{
    public event Action<Unit> UnitEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            UnitEntered?.Invoke(unit);
        }
    }
}
