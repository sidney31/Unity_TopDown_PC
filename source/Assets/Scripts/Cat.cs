using UnityEngine;
public class Cat : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed;
    [SerializeField] private Animator anime;
    [SerializeField] private bool faceRight = true;
    [SerializeField] private bool pursuit = true;

    [SerializeField] private Texture2D catCursor;

    private void Start()
    {
        transform.position = player.position + new Vector3(2, 0, 0);
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, transform.position) >= 1f && pursuit)
        {
            anime.SetBool("Run", true);
            if (anime.GetCurrentAnimatorStateInfo(0).IsName("DogRun"))
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }
        else
        {
            anime.SetBool("Run", false);
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

    public void OnMouseOver()
    {
        if (Vector3.Distance(player.position, transform.position) <= .8f)
        {
            Cursor.SetCursor(catCursor, Vector2.zero, CursorMode.ForceSoftware);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                anime.SetTrigger("Pet");
                player.GetComponent<PlayerController>().anime.SetTrigger("Pet");
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                pursuit = !pursuit;
            }
        }
    }
}
