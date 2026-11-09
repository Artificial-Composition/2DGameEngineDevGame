using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;


public class Player : MonoBehaviour
{
    public int speed;
    private Vector2 move;
    private Rigidbody2D rb;
    private int direction = 1;
    private Animator animator;
    public float jumpHeight = 2.4f;
    public int jumpCB= 2;
    private int jumpCount;
    public bool isJumping = false;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameManager gm;
    public Transform firePosition;
    private int lives = 3;
    private int score = 0;


    private void Fire()
    {
        GameObject go = Instantiate(projectilePrefab, firePosition.position, firePosition.rotation);
        Projectile pro = go.GetComponent<Projectile>();
        pro.Launch(new Vector2(direction * -1, 0), 1000);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpCount = jumpCB;
    }

    void Update()
    {
        move = InputSystem.actions["Move"].ReadValue<Vector2>();

        if (move.x != 0)
        {
            direction = move.x < 0 ? 1 : -1;
            animator.SetInteger("Direction", direction);
        }
        animator.SetFloat("Move", move.x);

        if (InputSystem.actions["Jump"].IsPressed() && jumpCount > 0)
        {
            InputSystem.actions["Jump"].Reset();
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(0, Mathf.Sqrt(-2 * Physics2D.gravity.y * jumpHeight)), ForceMode2D.Impulse);
            jumpCount -= 1;
            animator.SetBool("isJumping", true);
        }

        if (InputSystem.actions["RangedAttack"].IsPressed())
        {
            InputSystem.actions["RangedAttack"].Reset();
            Fire();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rb.transform.position;
        position.x += (move.x * speed) * Time.fixedDeltaTime;
        rb.transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCount = jumpCB;
            animator.SetBool("isJumping", false);
        }
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            lives--;
            gm.updateLives(lives);

        }
    }

    public void AddRedGemCollectible()
    {
        score++;
        gm.UpdateScore(score);
        speed = speed + 2;
    }

    public void AddGreenGemCollectible()
    {
        score++;
        gm.UpdateScore(score);
        jumpCB = jumpCB + 2;
    }

    public void AddPurpleGemCollectible()
    {
        score++;
        gm.UpdateScore(score);
        jumpHeight = jumpHeight * 2;
    }

    public void AddTurquoiseGemCollectible()
    {
        score = score + 3;
        gm.UpdateScore(score);
    }
}
