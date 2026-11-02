using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float walkTime;
    [SerializeField] private float idleTime;
    private float elapsedTime;
    private int direction = 1;
    private bool isWalking = false;
    private Rigidbody2D rb;
    private Animator animator;


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

    }
}
