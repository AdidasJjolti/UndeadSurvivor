using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Scriptable/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal}

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;      // 근거리 무기 : 기본 생성 갯수, 원거리 무기 : 관통력
    public float[] damages;    // 무기류 : 피해량, 글러브 : 연사 속도, 신발 : 이동 속도
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;

}
