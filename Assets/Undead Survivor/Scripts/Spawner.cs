using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    float timer;
    int level;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;    // ���� �ִ� �ð��� ���� ������ŭ ���� ����ŭ ������ �߻�
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);   // 10�ʸ��� ������
        Debug.Log("Level is " + level);

        if (timer > spawnData[level].spawnTime)
        {
            Spawn();
            timer = 0;
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;    // 0�� �ε����� �ڱ� �ڽ��̱� ������ 1���� �迭 ���̸�ŭ ���� ���� ����
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
