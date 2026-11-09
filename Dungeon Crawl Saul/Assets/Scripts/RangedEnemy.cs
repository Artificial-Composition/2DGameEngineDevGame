using UnityEngine;

public class Enemies : MonoBehaviour
{

    [SerializeField] private float walkTime;
    [SerializeField] private float idleTime;
    [SerializeField] private float fireTimer = 0.5f;
    [SerializeField] private GameObject projectile;

    private float elapsedTime;
    private int direction = 1;
    private bool isWalking = false;
    private Rigidbody2D rb;
    private Animator animator;
    private float fireCountdown;
    private bool isInvincible;
    private float invincibleCount;
    private bool visible = true;
    private int strength = 3;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetInteger("Direction", direction);
        animator.SetFloat("Move", 0);
    }


    void Update()
    {
        if (isWalking && elapsedTime < walkTime)
        {
            Vector2 position = rb.transform.position;
            position.x += direction * Time.deltaTime;
            rb.transform.position = position;
        }
        else if (!isWalking && elapsedTime > walkTime)
        {
            isWalking = true;
            elapsedTime = 0;
            direction *= -1;
            animator.SetFloat("Move", direction);
            animator.SetInteger("Direction", direction);
        }
        else if (isWalking && elapsedTime > walkTime)
        {
            isWalking = false;
            elapsedTime = 0;
            animator.SetFloat("Move", 0);
        }

        elapsedTime += Time.deltaTime;

        if (isInvincible)
        {
            invincibleCount += Time.deltaTime;
            if (invincibleCount > 3)
            {
                isInvincible = false;
                invincibleCount = 0;
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, new Vector2(direction, 0),
            5f, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<Player>() != null)
            {
                Fire();
            }
        }
        fireCountdown += Time.fixedDeltaTime;

        if (isInvincible)
        {
            if ((int)((invincibleCount * 10) / 10) % 2 == 0)
            {
                visible = !visible;
                this.gameObject.GetComponent<SpriteRenderer>().enabled = visible;
            }

        }

    }

    private void Fire()
    {

        if (fireCountdown > fireTimer)
        {
            GameObject go = Instantiate(projectile, rb.transform.position, Quaternion.identity);
            Projectile pro = go.GetComponent<Projectile>();
            pro.Launch(new Vector2(direction, 0), 300);
            fireCountdown = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            rb.linearVelocity = new Vector2(0, 0);
            hit();
        }
    }

    private void hit()
    {
        if (!isInvincible)
        {
            strength--;

            if (strength <= 0)
            {
                Destroy(this.gameObject);
                return;
            }

            isInvincible = true;
        }
    }
}