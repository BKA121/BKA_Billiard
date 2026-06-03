using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;

    // Cue sound
    public AudioClip cueHit;

    // Ball sound
    public AudioClip ballHit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayCueHitSound(float force)
    {

        if (1f < force && force < 3f)
        {
            sfxSource.pitch = 1f;
        }
        else if (3f <= force && force <= 4.5f)
        {
            sfxSource.pitch = 1.5f;
        }
        else if (force > 4.5f)
        {
            sfxSource.pitch = 1.8f;
        }
        sfxSource.volume = Mathf.Clamp(force / 6f, 0.1f, 1f);
        sfxSource.PlayOneShot(cueHit);
    }

    public void PlayBallHitSound(float force)
    {
        Debug.Log(force);
        sfxSource.volume = Mathf.Clamp(force / 6f, 0.1f, 1f);

        if(sfxSource.volume > 0.8f) sfxSource.pitch = 1.5f;
        else sfxSource.pitch = 1f;

        sfxSource.PlayOneShot(ballHit);
    }

    //public void PlayPocketSound()
    //{
    //    if (pocketClip != null)
    //    {
    //        sfxSource.PlayOneShot(pocketClip);
    //    }
    //}
}
