using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public AudioSource introSource, loopSource;
    void Start()
    {
        introSource.Play();
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
