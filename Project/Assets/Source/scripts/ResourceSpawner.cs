using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField, Range(10, 20)] private int _maxResources = 20;

    [Header("Зона спавна")]
    [SerializeField] private SpawnZone _spawnZone;

    [Header("Дополнительные настройки")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _spawnHeightOffset = 0f;
    [SerializeField] private float _minSpawnDistance = 3f;

    [Header("Типы ресурсов")]
    [SerializeField] private List<Resource> _resources = new List<Resource>();

    private Collider[] _colliderBuffer = new Collider[10];
    private int _minResources = 5;

    private void Start()
    {
        SpawnInitialResources();
    }

    private void SpawnInitialResources()
    {
        int initialCount = Random.Range(_minResources, _maxResources);

        for (int i = 0; i < initialCount; i++)
        {
            SpawnResource();
        }
    }

    private void SpawnResource()
    {
        if (_resources.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, _resources.Count);

        Resource prefabToSpawn = _resources[randomIndex];

        if (_spawnZone == null || prefabToSpawn == null)
        {
            return;
        }

        int attempts = 10;

        Vector3 spawnPos;
        Quaternion spawnRot = Quaternion.identity;

        while (attempts-- > 0)
        {
            spawnPos = _spawnZone.GetRandomPosition();

            if (Physics.Raycast(spawnPos + Vector3.up * 5, Vector3.down, out RaycastHit hit, 100f, _groundLayer))
            {
                spawnPos = hit.point + Vector3.up * _spawnHeightOffset;
                spawnRot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }

            if (IsPositionValid(spawnPos, _minSpawnDistance))
            {
                Instantiate(prefabToSpawn, spawnPos, spawnRot, transform);

                return;
            }
        }
    }

    private bool IsPositionValid(Vector3 position, float minDistance)
    {
        int count = Physics.OverlapSphereNonAlloc(position, minDistance, _colliderBuffer);

        for (int i = 0; i < count; i++)
        {
            Collider col = _colliderBuffer[i];

            if (col.TryGetComponent<Resource>(out _) == true)
            {
                return false;
            }
        }

        return true;
    }
}
