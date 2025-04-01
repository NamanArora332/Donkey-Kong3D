using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource ambienceSource;

    [Header("Background Music (BGM) Clips")]
    public AudioClip introClip;     
    public AudioClip ambientClip_Level1;
    public AudioClip ambientClip_Level2;
    public AudioClip winClip;
    public AudioClip gameOverClip;
    public AudioClip stageClearClip; 
    public AudioClip gameStartClip;
    public AudioClip pauseClip; 

    [Header("Sound Effects (SFX) - Mario Clips")]
    public AudioClip kongClip;
    public AudioClip climbClip;
    public AudioClip jumpClip;
    public AudioClip loseLifeClip;
    public AudioClip jumpOverClip; 
    public AudioClip getCoinClip; 
    public AudioClip getItemClip; 
    public AudioClip powerUpClip; 
    public AudioClip bigMarioClip; 
    public AudioClip fightOffClip; 
    public AudioClip powerDownClip;

    private Dictionary<AudioClip, AudioSource> activeSounds = new Dictionary<AudioClip, AudioSource>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make AudioManager persistent across scenes
        }
        else{
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        Debug.Log("Starting Ambient Sound: " + (ambientClip_Level1 != null ? "Clip Found" : "Clip Missing"));
    }
    
    public void PlayAmbientSound(AudioClip clip)
    {
        if (ambienceSource == null)
        {
            Debug.LogError("AmbienceSource is NULL!");
            return;
        }

        if (clip == null)
        {
            Debug.LogError("Ambient clip is NULL!");
            return;
        }

        ambienceSource.clip = clip;
        ambienceSource.loop = true;
        ambienceSource.Play();
    }

    public void StopAmbientSound()
    {
        if (ambienceSource != null && ambienceSource.isPlaying)
        {
            ambienceSource.Stop();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            if (sfxSource == null)
            {
                Debug.LogError("sfxSource is NULL!");
                return;
            }

            sfxSource.clip = clip;
            sfxSource.Play();
        }
    }

    public void StopSound(AudioClip clip)
    {
        if (activeSounds.ContainsKey(clip))
        {
            AudioSource source = activeSounds[clip];
            if (source != null && source.isPlaying)
            {
                source.Stop();
                Destroy(source); // Cleanup
            }
            activeSounds.Remove(clip);
        }
    }
}
