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

    float timer;
    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

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
                timer += Time.deltaTime;
                if(timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
        }

        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(10f, 1);         // 레벨업하면 대미지가 10으로 증가
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
                speed = 150f;        // 근거리 무기인 경우 플레이어를 회전하는 속도로 설정
                Batch();
                break;
            default:
                speed = 0.3f;        // 원거리 무기인 경우 연사 쿨타임으로 설정
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
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);        // -1이면 무한 관통
        }
    }

    void Fire()
    {
        if(!player.scanner.nearestTarget)
        {
            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabID).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);   // 목표 지점을 향해 회전
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
