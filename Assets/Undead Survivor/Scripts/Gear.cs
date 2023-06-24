using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch(type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    // 연사력 증가
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0:   // 근거리 무기
                    float speed = 150 * Character.WeaponSpeed;    // 캐릭터별 기본 무기 속도를 곱연산
                    weapon.speed = speed + (speed * rate);
                    break;
                default:  // 원거리 무기
                    speed = 0.5f * Character.WeaponRate;           // 캐릭터별 기본 연사 속도를 곱연산
                    weapon.speed = speed * (1 - rate);
                    break;
            }
        }
    }

    // 이동 속도 증가
    void SpeedUp()
    {
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
