using System.Collections;
using UnityEngine;

public class TileLogic : MonoBehaviour
{
    [SerializeField] private GameObject border;
    [SerializeField] private SpriteRenderer borderSR;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private PlayerController player;

    [SerializeField] private Sprite[] _grass;
    [SerializeField] private Sprite _sand;
    [SerializeField] private Sprite _plowed;
    [SerializeField] private Sprite _seed;

    [SerializeField] private Sprite[] _growStages;
    [SerializeField] private float nextGrowTime;
    [SerializeField] private float growRate = 1;

    [SerializeField] private GameObject bush;
    [SerializeField] private SpriteRenderer bushSR;

    [SerializeField] private int growLevel = 0;

    [SerializeField] private Texture2D shovelCursor;
    [SerializeField] private Texture2D seedCursor;
    [SerializeField] private Texture2D defaultCursor;

    [SerializeField] private Item Wheat, Seed;

    [SerializeField] private Transform circle;
    [Range(0, 5f)]
    [SerializeField] private float circleRadius;
    [SerializeField] private LayerMask tilesLayer;
    [SerializeField] private int SmoothItteration;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponentInChildren<PlayerController>();
        borderSR = border.GetComponent<SpriteRenderer>();
        bushSR = bush.GetComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Smooth());
    }

    private void FixedUpdate()
    {
        if (Time.time >= nextGrowTime && sr.sprite == _seed && growLevel < _growStages.Length)
        {
            bushSR.sprite = _growStages[growLevel++];
            nextGrowTime = Time.time + growRate + Random.Range(.1f, 3f);
        }
    }

    private IEnumerator Smooth()
    {
        for (int i = 0; i < SmoothItteration; i++)
        {
            int blackNeighbors = 0, whiteNeighbors = 0;
            Collider2D[] neighbors = Physics2D.OverlapCircleAll(circle.position, circleRadius, tilesLayer);

            foreach (Collider2D neighbor in neighbors)
            {
                if (neighbor.GetComponent<SpriteRenderer>().color == Color.black)
                {
                    blackNeighbors++;
                }
                else
                {
                    whiteNeighbors++;
                }
            }
            if (blackNeighbors > 4)
            {
                sr.color = Color.black;
            }
            else
            {
                sr.color = Color.white;
            }
            yield return null;
        }
        ReplaceTileSprite();
    }

    private void ReplaceTileSprite()
    {
        if (sr.color == Color.black)
        {
            sr.sprite = _grass[Random.Range(0, _grass.Length)];
            sr.color = Color.white;
        }
        else if (sr.color == Color.white)
        {
            sr.sprite = _sand;
        }
    }

    private void OnDrawGizmos()
    {
        if (circle == null)
            return;

        Gizmos.DrawWireSphere(circle.position, circleRadius);
    }
    private void OnMouseOver()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= .7f)
        {
            Item item = InventoryManager.Instance.getItemByIndex(InventoryManager.Instance.selectedSlot, false);
            if (item && sr.sprite != _sand)
            {
                border.SetActive(item.type == ItemType.shovel);
            }
            foreach (Sprite grass in _grass)
            {
                if (item)
                {
                    if (item.type == ItemType.shovel)
                    {
                        if (sr.sprite == grass)
                        {
                            Cursor.SetCursor(shovelCursor, Vector2.zero, CursorMode.ForceSoftware);
                            ///**/ Debug.Log($"{sr.sprite} == {grass}");
                            borderSR.sprite = _plowed;
                        }
                    }
                    else if (item.type == ItemType.seed)
                    {
                        if (sr.sprite == _plowed)
                        {
                            Cursor.SetCursor(seedCursor, Vector2.zero, CursorMode.ForceSoftware);
                            ///**/ Debug.Log($"{sr.sprite} == {_plowed}");
                            borderSR.sprite = _seed;
                        }
                        if (sr.sprite == _seed)
                        {
                            Cursor.SetCursor(seedCursor, Vector2.zero, CursorMode.ForceSoftware);
                            ///**/ Debug.Log($"{sr.sprite} == {_plowed}");
                            borderSR.sprite = _plowed;
                        }
                    }
                    else
                    {
                        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (sr.sprite != _sand)
                        {
                            if (item.type == ItemType.shovel)
                            {
                                if (borderSR.sprite == _plowed)
                                {
                                    sr.sprite = borderSR.sprite;

                                }
                                if (bushSR.sprite == _growStages[4])
                                {
                                    bushSR.sprite = null;
                                    sr.sprite = _plowed;
                                    for (int i = 0; i < 2; i++)
                                    {
                                        InventoryManager.Instance.AddItem(Seed);
                                    }
                                    InventoryManager.Instance.AddItem(Wheat);
                                    nextGrowTime = 0.1f;
                                    growLevel = 0;

                                }
                                player.anime.SetTrigger("Dig");
                                return;
                            }
                        }
                        if (item.type == ItemType.seed && borderSR.sprite == _seed)
                        {
                            Debug.Log("seeded");
                            player.anime.SetTrigger("Sow");
                            sr.sprite = borderSR.sprite;
                            InventoryManager.Instance.getItemByIndex(InventoryManager.Instance.selectedSlot, true);
                            return;
                        }
                    }
                }
            }
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    private void OnMouseExit()
    {
        border.SetActive(false);
    }

}