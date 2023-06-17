using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabID;
    public float damage;
    public int count;
    public float speed;    // ȸ�� �ӵ�

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
            LevelUp(20f, 5);         // �������ϸ� ������� 20���� ����
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;      // bullet ���ڸ� count��ŭ ����

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
        for(int i = 0; i < count; i++)    //count����ŭ bullet ����
        {
            Transform bullet;

            if(i < transform.childCount)
            {
                bullet = transform.GetChild(i);      // �̹� Ǯ �ȿ� bullet�̸� ���� �ڽ� ������Ʈ�� �ִ� bullet�� ����Ͽ� �Ʒ� ���� ����, ���� �þ�鼭 ȸ������ ����ȭ
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabID).transform;        // �ε����� Ǯ �ȿ� �ִ� bullet ���ں��� Ŀ���� ���� �����ؼ� �Ʒ� ���� ����
                bullet.parent = transform;
            }

            // �ϴ� ��ġ�� ȸ������ �⺻������ �ʱ�ȭ�� ��
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;    // ������ bullet ������ ���� i��° �ε����� ȸ���� ����, 0���̸� 0�� -> 1���̸� 30��, �̷� ������...
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);       // localPosition ��� ��ġ�� (0, 1.5)��ŭ �̵�
            bullet.GetComponent<Bullet>().Init(damage, -1);        // -1�̸� ���� ����
        }
    }
}
