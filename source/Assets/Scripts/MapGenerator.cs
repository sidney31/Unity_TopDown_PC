using System.Collections;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] public int fieldSize;
    [SerializeField] private Vector2 way;
    [SerializeField] private GameObject[] trees;
    [SerializeField] private Sprite[] tiles;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector2 offset;
    [Range(-10f, 10f)]
    [SerializeField] private float scale;

    [SerializeField] private LayerMask treesLayer;

    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] public int spawnedZombiesAmount;
    [SerializeField] private int maxZombiesAmount;

    private void Awake()
    {
        way.x = Random.Range(0, fieldSize);
        way.y = way.x - 3;
        StartCoroutine(GenerateObjects());
        GenerateMap();

    }

    private void FixedUpdate()
    {
        ZombieSpawner();
    }

    public void GenerateMap()
    {
        DestroyAllTiles();
        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                float noiseValue = (GenerateNoiseValue(x, y, scale) + GenerateNoiseValue(x, y, scale)) / 2;
                int intNoiseValue = noiseValue < 0.5 ? 0 : 1;
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile.GetComponent<SpriteRenderer>().color = new Color(intNoiseValue, intNoiseValue, intNoiseValue, 1);
                tile.name = $"tile{x}:{y}";
                tile.transform.SetParent(transform);
            }
        }

    }

    private float GenerateNoiseValue(int x, int y, float scale)
    {
        offset = new Vector2(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
        return Mathf.PerlinNoise(x * scale + offset.x, y * scale + offset.y);
    }

    private void DestroyAllTiles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    IEnumerator GenerateObjects()
    {
        int _treesSpawned = 0;
        Vector2 currentTreeCords;

        while (_treesSpawned < 50)
        {
            for (int x = 3; x < fieldSize - 3; x++)
            {
                for (int y = 3; y < fieldSize - 3; y++)
                {
                    float xOffset = Random.Range(-1000f, 1000f);
                    float yOffset = Random.Range(-1000f, 1000f);

                    float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);

                    if (noiseValue > 0)
                    {
                        currentTreeCords = new Vector2(x, y);
                        if (Physics2D.OverlapCircleAll(currentTreeCords, 6, treesLayer).Length == 0)
                        {
                            Instantiate(trees[Random.Range(0, 4)], currentTreeCords, Quaternion.identity);
                            _treesSpawned++;
                            yield return null;
                        }
                    }
                }
            }
            if (_treesSpawned < 50)
            {
                break;
            }
        }
    }
    private void ZombieSpawner()
    {
        if (spawnedZombiesAmount < maxZombiesAmount)
        {
            Instantiate(zombiePrefab, new Vector2(Random.Range(1, fieldSize - 1), Random.Range(1, fieldSize - 1)), Quaternion.identity);
            spawnedZombiesAmount++;
        }
    }
}