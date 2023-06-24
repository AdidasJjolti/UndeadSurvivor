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
        this.gameObject.SetActive(true);   // �� �ڵ带 ��ü
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
        this.gameObject.SetActive(false);   // �� �ڵ带 ��ü
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    // ���� ������ �� �⺻ ���� ����
    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        // ��� ������ ��Ȱ��ȭ
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        // �� �߿��� �������� 3�� ������ Ȱ��ȭ
        int[] ran = new int[3];
        while(true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if(ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])   // ���� �������� �ϳ��� ������ while�� Ż��
            {
                break;
            }
        }

        for(int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

            if(ranItem.level == ranItem.data.damages.Length)
            {
                // ���� �������� �Һ� ������(����)���� ��ü
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
