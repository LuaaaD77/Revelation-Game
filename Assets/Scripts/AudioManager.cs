using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip ambientMusic;
    public AudioClip collectClip;
    public AudioClip completeClip;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (musicSource != null && ambientMusic != null)
        {
            musicSource.clip = ambientMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    public void PlayCollect() { if (sfxSource && collectClip) sfxSource.PlayOneShot(collectClip); }
    public void PlayChapterComplete() { if (sfxSource && completeClip) sfxSource.PlayOneShot(completeClip); }
}
