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
    [SerializeField] private float _spawnHeightOffset = 0.5f;
    [SerializeField] private float _minSpawnDistance = 2f;

    [Header("Типы ресурсов")]
    [SerializeField] private List<ResourceType> _resourceTypes = new List<ResourceType>();

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
        if (_resourceTypes.Count == 0)
        {
            return;
        }

        int totalWeight = 0;

        foreach (var type in _resourceTypes)
        {
            totalWeight += type.SpawnWeight;
        }

        int randomWeight = Random.Range(0, totalWeight);

        Resource prefabToSpawn = null;

        foreach (var type in _resourceTypes)
        {
            if (randomWeight < type.SpawnWeight)
            {
                prefabToSpawn = type.GetPrefab();
                break;
            }

            randomWeight -= type.SpawnWeight;
        }

        if (_spawnZone == null || prefabToSpawn == null)
        {
            return;
        }

        int attempts = 10;
        bool spawned = false;
        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        while (attempts-- > 0 && !spawned)
        {
            spawnPos = _spawnZone.GetRandomPosition();

            if (Physics.Raycast(spawnPos + Vector3.up * 5, Vector3.down, out RaycastHit hit, 100f, _groundLayer))
            {
                spawnPos = hit.point + Vector3.up * _spawnHeightOffset;
                spawnRot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }

            if (IsPositionValid(spawnPos, _minSpawnDistance))
            {
                spawned = true;
            }
        }

        if (spawned == false)
        {
            return;
        }

        Instantiate(prefabToSpawn, spawnPos, spawnRot, transform);
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
