using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Настройки")]
    public int initialUnits = 3;
    public GameObject unitPrefab;
    public GameObject resourcePrefab;

    [Header("Ссылки")]
    public Base baseController;
    public ResourceSpawner resourceSpawner;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < initialUnits; i++)
        {
            Vector3 spawnPos = baseController.transform.position + Random.insideUnitSphere * 2f;
            spawnPos.y = 0;
            Instantiate(unitPrefab, spawnPos, Quaternion.identity);
        }
    }
}