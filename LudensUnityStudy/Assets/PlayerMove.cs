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

        // isWalking 설정
        if (!Isjumping)
        {
            animator.SetBool("isWalking", rigid.velocity.x > 0.1f || rigid.velocity.x < 0.1f);
        }
           

    }

    void OnCollisionEnter2D(Collision2D collision)
    {       
        animator.SetBool("isJumping", false);
        Isjumping = false;

        if(collision.gameObject.tag == "Enemy") // 부딪힌 오브젝트의 태그가 "Enemy"인지 확인
        {
            OnDamaged(collision.transform.position); // 적의 위치를 OnDamaged함수에 담아 전달하고 함수 실행
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        // Change Layer (Immortal Active)
        gameObject.layer = LayerMask.NameToLayer("PlayerDamaged"); // gameObject.layer = 9;

        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 스프라이트의 투명도를 0.4로 설정

        //적이 왼쪽에 있으면 오른쪽으로, 적이 오른쪽에 있으면 왼쪽으로 튕겨 나가도록 방향 계산
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; 
        rigid.AddForce(new Vector2(dirc, 1) * 5, ForceMode2D.Impulse);

        //Animation
        animator.SetTrigger("doDamaged");

        Invoke("OffDamaged", 3); // 3초 뒤에 OffDamaged() 함수를 호출시킴
    }

    void OffDamaged()
    {
        gameObject.layer = LayerMask.NameToLayer("Player"); // 레이어를 다시 Player로 전환
        spriteRenderer.color = new Color(1, 1, 1, 1); // 스프라이트의 투명도를 1로 설정
    }
}



