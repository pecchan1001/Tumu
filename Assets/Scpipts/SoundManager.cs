using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //BGM
    [SerializeField] AudioSource audioSourceBGM = default;
    [SerializeField] AudioSource audioSourceSE = default;

    //Clip
    [SerializeField] AudioClip[] audioClips = default;
    [SerializeField] AudioClip[] seClips = default;

    public enum BGM
    {
        Title,
        Main
    }

    public enum SE
    {
        Destroy,
        Touch
    }

    private void Start()
    {
        PlayBGM(BGM.Title);
    }

    public void PlayBGM (BGM bgm)
    {
        audioSourceBGM.clip = audioClips[(int)bgm];
        audioSourceBGM.Play();
    }

    public void PlaySE (SE se)
    {
        audioSourceSE.PlayOneShot(seClips[(int)se]);
    }

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
