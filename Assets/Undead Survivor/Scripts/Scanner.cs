using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);  // 원형 캐스트 쏘기 : 시작 위치, 반지름, 방향, 길이, 대상 레이어
        nearestTarget = GetNearest();
    }


    // 가장 가까운 적의 위치를 파악
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100f;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;                     // 플레이어의 위치
            Vector3 targetPos = target.transform.position;          // 적의 위치
            float curDiff = Vector3.Distance(myPos, targetPos);

            if(curDiff < diff)
            {
                diff = curDiff;                                    // 저장된 거리보다 현재 적의 거리가 더 가까우면 값 교체
                result = target.transform;
            }
        }

        return result;
    }
}
