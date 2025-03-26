using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float maxSpeed;
    public float jumpPower;
    public bool Isjumping = false;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !Isjumping)
        {
            Isjumping = true;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
            animator.SetBool("isWalking", false);
        }
            

        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        }

        if (Input.GetAxisRaw("Horizontal") == 0 && !Isjumping)
        {
            animator.SetBool("isWalking", false);
        }


    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);

        else if (rigid.velocity.x < (-1) * maxSpeed)
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);

        // isWalking ¼³Á¤
        if (!Isjumping)
        {
            animator.SetBool("isWalking", rigid.velocity.x > 0.1f || rigid.velocity.x < 0.1f);
        }
           

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
            
        animator.SetBool("isJumping", false);
        Isjumping = false;

    }
}



