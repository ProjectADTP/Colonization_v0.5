using UnityEngine;

public class ResourceType: MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private int _spawnWeight = 1;

    public int SpawnWeight => _spawnWeight;

    public Resource GetPrefab()
    { 
        return _prefab;
    }
}