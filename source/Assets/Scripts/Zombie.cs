using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed;
    [SerializeField] private Animator anime;
    [SerializeField] private bool faceRight = true;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private int currentHP;
    [SerializeField] private GameObject grid;

    //attack system
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackArea;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRate = 3;
    [SerializeField] private float nextAttackTime = 0;

    private void Start()
    {
        currentHP = 5;
        player = GameObject.Find("Player").transform;
        grid = GameObject.Find("Grid");
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(player.position, transform.position) >= .7f &&
            Vector3.Distance(player.position, transform.position) <= 7f)
        {
            anime.SetBool("Walk", true);
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            Attack();
            anime.SetBool("Walk", false);
        }
        ReflectEnemy();
    }
    private void ReflectEnemy()
    {
        if (transform.position.x - player.position.x > 0 && faceRight ||
        transform.position.x - player.position.x < 0 && !faceRight)
        {
            Vector3 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;
            faceRight = !faceRight;
        }
    }
    public void getDamage(int damage)
    {
        currentHP -= damage;
        ps.Play();
        StartCoroutine(Recoil());
        if (currentHP <= 0)
            Die();
    }
    private void Die()
    {
        anime.SetTrigger("Die");
        GetComponent<BoxCollider2D>().enabled = false;
        enabled = false;
        StartCoroutine(DestroyObject());
        grid.GetComponent<MapGenerator>().spawnedZombiesAmount--;
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2);
        for (float i = 1; i >= 0; i -= 0.1f)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(.1f);
        }
        Destroy(gameObject);
    }

    private void Attack()
    {
        Collider2D player = Physics2D.OverlapCircle(attackArea.position, attackRange, playerLayer);
        if (Time.time >= nextAttackTime && player)
        {
            player.gameObject.GetComponent<PlayerController>().getDamage();
            anime.SetTrigger("Hit");
            nextAttackTime = Time.time + attackRate;
            rb.velocity = new Vector2(0, 0);
        }
    }

    private IEnumerator Recoil()
    {
        rb.AddForce(new Vector2(15 * -Vector2.Distance(transform.position, player.position), 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        rb.velocity = new Vector2(0, 0);
    }

    private void OnDrawGizmos()
    {
        if (attackArea == null)
            return;

        Gizmos.DrawWireSphere(attackArea.position, attackRange);
    }
}
