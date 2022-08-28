using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public static GridSpawner Instance;

    public GameObject PlatformPrefab;
    public int GridX;
    public int GridY;
    public float GridSpacingOffset = 5f;
    public Vector3 GridOrigin = Vector3.zero;

    public List<GameObject> Platforms;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnGrid();
    }

    private int NumberName;
    private void SpawnGrid()
    {
        for (int x = 0; x < GridX; x++)
        {
            for (int y = 0; y < GridY; y++)
            {
                NumberName++;
                Vector3 spawnPosition = new Vector3(x * GridSpacingOffset, 0, y * GridSpacingOffset) + GridOrigin;
                PickAndSpawn(spawnPosition, Quaternion.identity, NumberName);
            }
        }

        NumberName = 0;
    }

    private void PickAndSpawn(Vector3 positionToSpawn, Quaternion rotationToSpawn, int NumberName)
    {
        GameObject clone = Instantiate(PlatformPrefab, positionToSpawn, rotationToSpawn);
        clone.transform.parent = transform;
        clone.name = NumberName.ToString();

        Platforms.Add(clone);
    }
}
