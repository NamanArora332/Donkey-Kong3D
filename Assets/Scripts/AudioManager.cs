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
        }
        else{
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        Debug.Log("Starting Ambient Sound: " + (ambientClip_Level1 != null ? "Clip Found" : "Clip Missing"));
        PlayAmbientSound();
    }
    
    public void PlayAmbientSound()
    {
        if (ambienceSource == null)
        {
            Debug.LogError("AmbienceSource is NULL!");
            return;
        }
        else if (ambientClip_Level1 == null)
        {
            Debug.LogError("ambientClip_level1 is NULL!");
            return;
        }
        ambienceSource.clip = ambientClip_Level1;
        ambienceSource.loop = true;
        ambienceSource.Play();
        Debug.Log("Ambient Sound Playing...");
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
