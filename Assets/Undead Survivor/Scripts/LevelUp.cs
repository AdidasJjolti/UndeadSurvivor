using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    bool _isFirst;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        // rect.localScale = Vector3.one;
        this.gameObject.SetActive(true);   // 위 코드를 대체
        GameManager.instance.Stop();

        if (!_isFirst)
        {
            _isFirst = true;
            return;
        }

        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
        Next();
    }

    public void Hide()
    {
        //rect.localScale = Vector3.zero;
        this.gameObject.SetActive(false);   // 위 코드를 대체
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    // 게임 시작할 때 기본 무기 지급
    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        // 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        // 그 중에서 랜덤으로 3개 아이템 활성화
        int[] ran = new int[3];
        while(true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if(ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])   // 같은 아이템이 하나도 없으면 while문 탈출
            {
                break;
            }
        }

        for(int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

            if(ranItem.level == ranItem.data.damages.Length)
            {
                // 만렙 아이템은 소비 아이템(포션)으로 대체
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
