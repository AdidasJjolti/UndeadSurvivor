using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    // 캐릭터에 따라 스피드 값 다르게 설정
    public static float Speed
    {
        get
        {
            return GameManager.instance.playerId == 0 ? 1.1f : 1f;
        }
    }

    // 무기 회전 속도
    public static float WeaponSpeed
    {
        get
        {
            return GameManager.instance.playerId == 1 ? 1.1f : 1f;
        }
    }

    // 무기 연사 속도
    public static float WeaponRate
    {
        get
        {
            return GameManager.instance.playerId == 1 ? 0.9f : 1f;
        }
    }

    public static float Damage
    {
        get
        {
            return GameManager.instance.playerId == 2 ? 1.2f : 1f;
        }
    }

    public static int Count
    {
        get
        {
            return GameManager.instance.playerId == 3 ? 1 : 0;
        }
    }
}
