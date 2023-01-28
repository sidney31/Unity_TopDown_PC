using UnityEngine;
using System.Collections;

public class StartCutSceneLogin : MonoBehaviour
{
    [SerializeField] private enum intityTypesEnum {Player, Zombie};
    [SerializeField] private intityTypesEnum entityType;

    [SerializeField] private Animator anime;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int speed;

    [SerializeField] private Transform player;
    [SerializeField] private Transform zombie;

    private void Start()
    {
        speed = 1;
    }

    private void Update()
    {
        Debug.Log(Vector3.Distance(player.position, zombie.position));
        if (Vector3.Distance(player.position, zombie.position) >= 1)
        {
            if (entityType == intityTypesEnum.Zombie)
            {
                anime.SetBool("Walk", true);
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }
        else
        {
            if (entityType == intityTypesEnum.Player)
            {
                Attack();
            }
            if (entityType == intityTypesEnum.Zombie)
            {
                StartCoroutine(Recoil());
            }
        }
    }

    private void Attack()
    {
        anime.SetTrigger("Hit");
    }

    private IEnumerator Recoil()
    {
        ps.Play();
        rb.AddForce(new Vector2(-5 * transform.localScale.x, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        rb.velocity = new Vector2(0, 0);
    }
}
