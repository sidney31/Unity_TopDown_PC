using System;
using System.Collections;
using UnityEngine;
using static Objects;


public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    //others
    [SerializeField] private float speed;
    [SerializeField] private Vector2 movement;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool faceRight;
    [SerializeField] private bool freeze;
    [SerializeField] public Animator anime;
    [SerializeField] private int fieldSize;
    [SerializeField] public int currentHP;
    [SerializeField] private int maxHP = 5;
    [SerializeField] private GameObject PauseMenu;

    //actions
    [SerializeField] private LayerMask objectsLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float actionRange;
    [SerializeField] private Transform actionZone;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackZone;
    [SerializeField] private float actionRate = 1;
    [SerializeField] private float nextActionTime;
    [SerializeField] private int damage = 1;

    //inventory
    [SerializeField] public int stoneCount;
    [SerializeField] public int treeCount;

    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private GameObject DieMenu;


    private void Start()
    {
        GameObject.Find("Grid");
        rb = GetComponent<Rigidbody2D>();
        fieldSize = GameObject.Find("Grid").GetComponent<MapGenerator>().fieldSize;
        transform.position = new Vector3(fieldSize / 2, fieldSize / 2, 0);
        freeze = false;
        currentHP = maxHP;
    }

    private void FixedUpdate()
    {
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        if (Math.Abs(movement.x) >= .7f && Math.Abs(movement.y) >= .7f)
        {
            movement = movement.normalized;
        }
        if (!freeze)
        {
            transform.position += new Vector3(movement.x, movement.y, 0) * speed * Time.deltaTime;
            anime.SetBool("Walk", movement.x == 0 && movement.y == 0 ? false : true);
            ReflectPlayer();
        }
        transform.position = new Vector3(
            transform.position.x < 0 ? 0 : transform.position.x > fieldSize ? fieldSize : transform.position.x,
            transform.position.y < 0 ? 0 : transform.position.y > fieldSize ? fieldSize : transform.position.y,
            0);

        freeze = (!(anime.GetCurrentAnimatorStateInfo(0).IsName("Walk") || anime.GetCurrentAnimatorStateInfo(0).IsName("Idle")) || PauseMenu.activeSelf);

        if (Input.GetMouseButton(0) &&
            Time.time >= nextActionTime &&
            InventoryManager.Instance.getItemByIndex(InventoryManager.Instance.selectedSlot, false) &&
            InventoryManager.Instance.getItemByIndex(InventoryManager.Instance.selectedSlot, false).type == ItemType.sword &&
            Physics2D.OverlapCircleAll(attackZone.position, attackRange, enemyLayer) != null)
        {
            anime.SetTrigger("Hit");
            Attack();
            nextActionTime = Time.time + actionRate;
        }
    }

    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
        }
    }
    public void ReflectPlayer()
    {
        if (movement.x < 0 && faceRight == false ||
            movement.x > 0 && faceRight == true)
        {
            Vector3 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;
            faceRight = !faceRight;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (actionZone == null)
            return;
        if (attackZone == null)
            return;
        Gizmos.DrawWireSphere(actionZone.position, actionRange);
        Gizmos.DrawWireSphere(attackZone.position, attackRange);
    }

    public void objectInteractive()
    {
        if (Time.time >= nextActionTime)
        {
            Collider2D[] objects = Physics2D.OverlapCircleAll(actionZone.position, actionRange, objectsLayer);
            foreach (Collider2D obj in objects)
            {
                obj.GetComponentInParent<Objects>().getDamage(transform.position.x);
                anime.SetTrigger(obj.GetComponentInParent<Objects>().typeName == objectType.Tree ? "Chop" : "Mine");
                nextActionTime = Time.time + actionRate;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D triggerObject)
    {
        if (triggerObject && triggerObject.gameObject.layer == 3 && triggerObject.GetComponentInParent<Objects>().currentHp > 0)
        {
            StartCoroutine(enterTree(triggerObject.GetComponentInChildren<SpriteRenderer>()));
        }
    }
    private void OnTriggerExit2D(Collider2D triggerObject)
    {
        if (triggerObject && triggerObject.gameObject.layer == 3 && triggerObject.GetComponentInParent<Objects>().currentHp > 0)
        {
            StartCoroutine(exitTree(triggerObject.GetComponentInChildren<SpriteRenderer>()));
        }
    }
    private IEnumerator enterTree(SpriteRenderer sr)
    {
        for (float i = 1f; i >= .3f; i-=.1f)
        {
            sr.material.color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator exitTree(SpriteRenderer sr)
    {
        for (float i = .3f; i <= 1; i += .1f)
        {
            sr.material.color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(.1f);
        }
    }
    private void Attack()
    {
        Collider2D[] enemysArray = Physics2D.OverlapCircleAll(attackZone.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in enemysArray)
        {
            enemy.gameObject.GetComponent<Zombie>().getDamage(damage);
        }
    }
    public void getDamage(Transform zombieTransform = null)
    {
        currentHP--;
        if (zombieTransform)
            StartCoroutine(Recoil(zombieTransform));
        if (currentHP <= 0)
            Die();
    }
    private void Die()
    {
        anime.SetTrigger("Die");
        DieMenu.SetActive(true);
    }

    private IEnumerator Recoil(Transform zombieTransform)
    {
        rb.AddForce(new Vector2(15 * -Vector2.Distance(transform.position, zombieTransform.position), 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        rb.velocity = new Vector2(0, 0);
    }
}