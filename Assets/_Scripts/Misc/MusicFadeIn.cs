using UnityEngine;
[RequireComponent(typeof(Oscillator))]
public class MusicFadeIn : MonoBehaviour
{
    private Oscillator oscillator;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        oscillator = GetComponent<Oscillator>();
        oscillator.MaxValue = GameInfo.MusicVolume + 0.2f;

        GameObject[] musicPlayers = GameObject.FindGameObjectsWithTag("Music");
        if (musicPlayers.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {

        if (audioSource.volume < GameInfo.MusicVolume)
        {
            audioSource.volume = oscillator.Progress;
        }
        else
        {
            enabled = false;
        }
    }
}
