using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrogonController : MonoBehaviour
{
    private Rigidbody2D rigi2d;
    private Animator animator;
    private SpriteRenderer spriterender;
    private bool isJumping;
    private bool IsGround = false;
    [SerializeField] private int speed;
    [SerializeField] private int jumpforce;
    [SerializeField] private int HP = 10;
    [SerializeField] private int strikespeed;
    [SerializeField] private Collider2D crouchcollider;
    [SerializeField] private Collider2D standcollider;
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
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            animator.SetBool("IsWalking", true);
            spriterender.flipX = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            animator.SetBool("IsWalking", true);
            spriterender.flipX = false;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && IsGround)
        {
            rigi2d.velocity = Vector2.up * jumpforce;
            animator.SetBool("IsJumping", true);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            animator.SetTrigger("Attack");
        }
        else if (Input.GetKey(KeyCode.RightControl))
        {
            animator.SetBool("IsCrouching", true);
            crouchcollider.enabled = true;
            standcollider.enabled = false;
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                animator.SetTrigger("Attack");
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightShift))
        {
            animator.SetTrigger("Strike");
            if (spriterender.flipX == false)
            {
                rigi2d.velocity = Vector2.right * strikespeed;
            }
            else if (spriterender.flipX == true)
            {
                rigi2d.velocity = Vector2.left * strikespeed;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            animator.SetTrigger("Kick");
        }
        else
        {
            animator.SetBool("IsWalking", false);
            crouchcollider.enabled = false;
            standcollider.enabled = true;
            animator.SetBool("IsCrouching", false);
        }

        if (rigi2d.velocity.y < 0)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            IsGround = true;
            animator.SetBool("IsGround", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGround = false;
        }
    }
    private void Die()
    {
        animator.SetTrigger("Die");
    }    

    public void TakeDame()
    {
        currentHP -= 1;
        animator.SetTrigger("Hurt");
        Debug.Log($"{currentHP}/{HP}");
    }
}
