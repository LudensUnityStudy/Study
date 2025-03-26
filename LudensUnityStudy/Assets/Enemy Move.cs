using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid; // Rigidbody2D ������Ʈ�� ����
    SpriteRenderer spriteRenderer;
    Animator animator;

    public int nextMove; // ���� ������ ���� (-1: ����, 0: ����, 1: ������)

    void Awake() // ���� ������Ʈ�� �����ǰų� Ȱ��ȭ�� �� ����
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y); // ���� ����(nextMove)�� ���� x�� �ӵ��� ���� (y�� �ӵ��� �״�� ����)

        animator.SetBool("IsRunning", nextMove != 0); // ��, ĳ���Ͱ� nextMove == 0�� ���� Idle anim �ƴϸ� Run anim ���

        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1; // nextMove == 1�̸� ������ �̵� ���̹Ƿ� ���� ��ȯ

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
        nextMove = Random.Range(-1, 2); // ����: -1, 0, 1 �� ������ ���� nextMove�� ���� (����, ����, ������)

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime); // 2 ~ 5�� �Ŀ� �ٽ� Think() ȣ�� �� ������ �ֱ������� ��� �ٲ�
    }
} 
