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
    public AudioClip ballHitTable;

    // Pocket sound
    public AudioClip[] pocketSound;

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
        sfxSource.volume = Mathf.Clamp(force / 4f, 0.04f, 1f);

        if(sfxSource.volume > 0.8f) sfxSource.pitch = 1.5f;
        else sfxSource.pitch = 1f;

        sfxSource.PlayOneShot(ballHit);
    }

    public void PlayBallHitTableSound(float force)
    {
        sfxSource.pitch = 1f;

        sfxSource.volume = Mathf.Clamp(force / 4f, 0.1f, 1f);
        sfxSource.PlayOneShot(ballHitTable);
    }

    public void PlayPocketSound(float force)
    {
        sfxSource.pitch = 1f;
        sfxSource.volume = 1f;

        if(0f < force && force < 0.01f) sfxSource.PlayOneShot(pocketSound[0]);
        else if(0.01f <= force && force < 0.02f) sfxSource.PlayOneShot(pocketSound[1]);
        else if (0.02f <= force && force < 0.03f) sfxSource.PlayOneShot(pocketSound[2]);
        else if (0.03f <= force && force < 0.035f) sfxSource.PlayOneShot(pocketSound[3]);
        else if (0.035f <= force && force < 0.042f) sfxSource.PlayOneShot(pocketSound[4]);
        else if (0.042f <= force) sfxSource.PlayOneShot(pocketSound[5]);
    }
}
