using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
