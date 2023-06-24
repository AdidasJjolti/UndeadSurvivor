using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;    // 대미지
    public int per;         // 적 관통 가능 숫자

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if(per >= 0 )    // 관통하지 않는 원거리 무기에만 방향 부여
        {
            rigid.velocity = dir * 15f;    // 적의 방향으로 날아갈 속도
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Enemy") || per == -100)
        {
            return;
        }

        per--;

        if(per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
        {
            return;
        }

        // 플레이어 영역에서 나가면 총알 비활성화
        gameObject.SetActive(false);
    }
}
