using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;      // 여러개 효과음을 재생할 채널 갯수 선언
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx
    {
        Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win
    }

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {

        // 배경음 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;   // 하이패스 필터에 적용받지 않도록 세팅
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlaying)
    {
        if(isPlaying)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;    // channelIndex : 마지막으로 사용한 효과음 재생 플레이어의 인덱스, loopIndex는 마지막으로 사용한 재생 플레이어 인덱스 그 다음

            if(sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee)   // 2개 이상 있는 효과음은 랜덤 숫자 넣어서 무작위 재생
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];   // 2개 중에서 1개를 무작위 재생
            sfxPlayers[loopIndex].Play();
            break;
        }

    }
}
