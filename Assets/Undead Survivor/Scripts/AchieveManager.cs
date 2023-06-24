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
            PlayerPrefs.SetInt(achieve.ToString(), 0);    // 0 : 해금 안됨, 1 : 해금
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
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;    // 데이터가 있으면 업적 완료한 상태(true)
            lockCharacter[i].SetActive(!isUnlock);                   // 캐릭터 잠금 버튼은 비활성화
            unlockCharacter[i].SetActive(isUnlock);                  // 오픈한 캐릭터 버튼은 활성화
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

        // 각 해금 조건을 체크
        switch(achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.instance.kill >= 10;   // 킬 수가 10 이상이면 해금
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;    // 게임 타임과 최대 게임 타임이 같을 때 해금 : 끝까지 살아남음
                break;
        }

        if(isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)    // 아직 해금 상태가 저장되지 않은 경우에만 해금 로직 실행
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);    // 업적 해금 상태를 저장

            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achieve;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);    // 업적과 맞는 자식 오브젝트를 활성화
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        // 업적 달성 팝업 5초 동안 노출 후 비활성화
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;
        uiNotice.SetActive(false);
    }
}
