using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;    // 대미지
    public int per;         // 적 관통 가능 숫자

    public void Init(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
}
