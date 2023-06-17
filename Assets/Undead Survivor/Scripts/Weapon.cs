using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabID;
    public float damage;
    public int count;
    public float speed;    // 회전 속도

    void Start()
    {
        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                break;
        }

        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(20f, 5);         // 레벨업하면 대미지가 20으로 증가
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;      // bullet 숫자를 count만큼 증가

        if(id == 0)
        {
            Batch();
        }
    }

    public void Init()
    {
        switch(id)
        {
            case 0:
                speed = 150f;
                Batch();
                break;
            default:
                break;
        }
    }

    void Batch()
    {
        for(int i = 0; i < count; i++)    //count값만큼 bullet 생성
        {
            Transform bullet;

            if(i < transform.childCount)
            {
                bullet = transform.GetChild(i);      // 이미 풀 안에 bullet이면 기존 자식 오브젝트로 있는 bullet을 사용하여 아래 로직 실행, 점차 늘어나면서 회전값을 최적화
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabID).transform;        // 인덱스가 풀 안에 있는 bullet 숫자보다 커지면 새로 생성해서 아래 로직 실행
                bullet.parent = transform;
            }

            // 일단 위치와 회전값을 기본값으로 초기화한 후
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;    // 생성한 bullet 갯수에 따라 i번째 인덱스의 회전값 결정, 0번이면 0도 -> 1번이면 30도, 이런 식으로...
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);       // localPosition 대비 위치를 (0, 1.5)만큼 이동
            bullet.GetComponent<Bullet>().Init(damage, -1);        // -1이면 무한 관통
        }
    }
}
