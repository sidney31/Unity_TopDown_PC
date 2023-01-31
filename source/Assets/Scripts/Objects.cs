using System.Collections;
using UnityEngine;

public class Objects : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Animator anime;
    [SerializeField] public int currentHp;
    [SerializeField] private PlayerController player;
    [SerializeField] public enum objectType {Tree, Stone}
    [SerializeField] public objectType typeName;
    [SerializeField] private Sprite[] stoneSprites;
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private Texture2D stoneCursor, treeCursor, defaultCursor;

    [SerializeField] private Item stone, wood;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponentInChildren<PlayerController>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    public void getDamage(float posX)
    {
        currentHp--;
        if (currentHp > 0)
        {
            if (typeName == objectType.Tree)
            {
                anime.SetTrigger("TreeDamage");
                particle.Play();
            }
            if (typeName == objectType.Stone)
            {
                sr.sprite = stoneSprites[currentHp];
            }
        }
        else
        {
            if (typeName == objectType.Tree)
            {
                Die(posX);
            }
            if (typeName == objectType.Stone)
            {
                StartCoroutine(animateDie());
                Die();
            }
        }
    }

    private void Die(float posX = 0)
    {
        if (typeName == objectType.Tree)
        {
            anime.SetTrigger(transform.position.x > posX ? "FallInRight" : "FallInLeft");
            InventoryManager.Instance.AddItem(wood);
        }
        if (typeName == objectType.Stone)
        {
            InventoryManager.Instance.AddItem(stone);
        }
        StartCoroutine(DestroyObject());
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2);
        for (float i = 1; i >= 0; i-=0.1f)
        {
            GetComponentInChildren<SpriteRenderer>().material.color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(.1f);
        }
        Destroy(gameObject);
    }

    private void OnMouseOver()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= 2f)
        {
            Item item = InventoryManager.Instance.getItemByIndex(InventoryManager.Instance.selectedSlot, false);
            if (item)
            {
                if (typeName == objectType.Tree && item.type == ItemType.axe) //last changes!
                {
                    Cursor.SetCursor(treeCursor, Vector2.zero, CursorMode.ForceSoftware);
                    if (Input.GetMouseButtonDown(0))
                    {
                        player.objectInteractive();
                    }
                }
                else if (typeName == objectType.Stone && item.type == ItemType.pickaxe)
                {
                    Cursor.SetCursor(stoneCursor, Vector2.zero, CursorMode.ForceSoftware);
                    if (Input.GetMouseButtonDown(0))
                    {
                        player.objectInteractive();
                    }
                }
            }
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    private IEnumerator animateDie()
    {
        for (int i = 3; i < stoneSprites.Length; i++)
        {
            sr.sprite = stoneSprites[i];
            yield return new WaitForSeconds(.09f);

        }
    }

}
