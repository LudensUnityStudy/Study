using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid; // Rigidbody2D 컴포넌트를 참조
    SpriteRenderer spriteRenderer;
    Animator animator;

    public int nextMove; // 다음 움직임 방향 (-1: 왼쪽, 0: 정지, 1: 오른쪽)

    void Awake() // 게임 오브젝트가 생성되거나 활성화될 때 실행
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y); // 현재 방향(nextMove)에 따라 x축 속도를 설정 (y축 속도는 그대로 유지)

        animator.SetBool("IsRunning", nextMove != 0); // 즉, 캐릭터가 nextMove == 0일 때는 Idle anim 아니면 Run anim 재생

        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1; // nextMove == 1이면 오른쪽 이동 중이므로 방향 전환

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2f, LayerMask.GetMask("Enemy"));
        if (rayHit.collider == null)
        {
            nextMove = nextMove * -1;
            CancelInvoke();
            Invoke("Think", 5);
        }    
    
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2); // 범위: -1, 0, 1 중 무작위 값을 nextMove에 설정 (왼쪽, 정지, 오른쪽)

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime); // 2 ~ 5초 후에 다시 Think() 호출 → 방향을 주기적으로 계속 바꿈
    }
} 
