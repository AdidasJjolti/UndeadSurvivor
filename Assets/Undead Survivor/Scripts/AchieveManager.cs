using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    enum Achieve
    {
        UnlockPotato, UnlockBean
    }

    Achieve[] achieves;
    WaitForSecondsRealtime wait;

    void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5f);

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);    // 0 : �ر� �ȵ�, 1 : �ر�
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for(int i = 0; i< lockCharacter.Length; i++)
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;    // �����Ͱ� ������ ���� �Ϸ��� ����(true)
            lockCharacter[i].SetActive(!isUnlock);                   // ĳ���� ��� ��ư�� ��Ȱ��ȭ
            unlockCharacter[i].SetActive(isUnlock);                  // ������ ĳ���� ��ư�� Ȱ��ȭ
        }
    }

    void LateUpdate()
    {
        foreach (Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        // �� �ر� ������ üũ
        switch(achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.instance.kill >= 10;   // ų ���� 10 �̻��̸� �ر�
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;    // ���� Ÿ�Ӱ� �ִ� ���� Ÿ���� ���� �� �ر� : ������ ��Ƴ���
                break;
        }

        if(isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)    // ���� �ر� ���°� ������� ���� ��쿡�� �ر� ���� ����
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);    // ���� �ر� ���¸� ����

            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achieve;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);    // ������ �´� �ڽ� ������Ʈ�� Ȱ��ȭ
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        // ���� �޼� �˾� 5�� ���� ���� �� ��Ȱ��ȭ
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;
        uiNotice.SetActive(false);
    }
}
