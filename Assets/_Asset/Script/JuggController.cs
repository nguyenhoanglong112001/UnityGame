using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggController : MonoBehaviour
{
    private Rigidbody2D rigi2d;
    private Animator animator;
    private SpriteRenderer spriterender;
    private bool isJumping;
    private bool IsGround = false;
    [SerializeField] private int speed ;
    [SerializeField] private int jumpforce;
    [SerializeField] private int runspeed;
    [SerializeField] private int rollspeed;
    [SerializeField] private int HP = 10;
    [SerializeField] private Collider2D rollcollider;
    [SerializeField] private Collider2D standcollider;
    [SerializeField] private Collider2D attackcollider;
    private int currentHP;
    // Start is called before the first frame update
    void Start()
    {
        rigi2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriterender = GetComponent<SpriteRenderer>();
        currentHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(Vector2.right * runspeed * Time.deltaTime);
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsWalking", false);
                spriterender.flipX = false;
            }
            else
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsWalking", true);
                spriterender.flipX = false;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(Vector2.left * runspeed * Time.deltaTime);
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsWalking", false);
                spriterender.flipX = true;
            }
            else
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsWalking", true);
                spriterender.flipX = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) && IsGround)
        {
            rigi2d.velocity = Vector2.up * jumpforce;
            animator.SetBool("IsJumping", true);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Attack");
            attackcollider.enabled = true;
            Invoke("HideCollder", 100);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Attack2");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("Summon");
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetTrigger("Roll");
            rollcollider.enabled = true;
            standcollider.enabled = false;
            if (spriterender.flipX == false)
            {
                rigi2d.velocity = Vector2.right * rollspeed;
            }
            else if(spriterender.flipX == true)
            {
                rigi2d.velocity = Vector2.left * rollspeed;
            }
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            animator.SetTrigger("Kick");
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
            rollcollider.enabled = false;
            standcollider.enabled = true;
        }
        if (rigi2d.velocity.y < 0)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }
    }
    void HideCollder()
    {
        attackcollider.enabled = false;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            IsGround = true;
            animator.SetBool("IsGround", true);
            animator.SetBool("IsFalling", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dragon"))
        {
            DrogonController dragon = collision.GetComponent<DrogonController>();
            dragon.TakeDame();
        }
    }
    void Death()
    {
        animator.SetTrigger("Death");
        return;
    }

    void TakeDame(int Dame)
    {
        animator.SetTrigger("Hurt");
        currentHP -= Dame;
        Debug.Log($"{currentHP}/HP");
    }
}
