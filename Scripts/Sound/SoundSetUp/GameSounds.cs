using UnityEngine;
/* This will only be for 2d sounds. 3d sounds can be made as a child under the SoundFXHolder as an empty GO,
 * with their own AudioSource and file dropped directly in there. */

public class GameSounds : MonoBehaviour
{
    public AudioClip[] _clips;
    public AudioSource _audioSource;
    public static GameSounds Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayOneShotAudio(SoundType type, float volume) => _audioSource.PlayOneShot(_clips[(int)type], volume);

    #region Direct Methods Required for Stopping Audio (Can't Stop Audio with PlayOneShot)
    public void PlayWindUpSFX()
    {
        _audioSource.clip = _clips[(int)SoundType.windingUp];
        _audioSource.loop = false;
        _audioSource.Play();
    }
    public void StopWindUpSFX()
    {
        _audioSource.clip = _clips[(int)SoundType.windingUp];
        _audioSource.Stop();
    }

    public void PlayGeneralBoopSFX()
    {
        _audioSource.clip = _clips[(int)SoundType.generalBoop];
        _audioSource.loop = false;
        _audioSource.Play();
    }

    public void PlayFirstTimeFishCaughtSFX()
    {
        _audioSource.clip = _clips[(int)SoundType.firstTimeFishCaught];
        _audioSource.loop = false;
        _audioSource.Play();
    }

    public void PlayMaxSizeFishCaughtSFX()
    {
        _audioSource.clip = _clips[(int)SoundType.maxSizeFishCaught];
        _audioSource.loop = false;
        _audioSource.Play();
    }
    #endregion
}

public enum SoundType
{
    // Steps
    walkingFootsteps,
    sprintingFootsteps,

    // Fishing
    windingUp,
    castingSwish,
    lineGoingOut,
    bobberHitWater,
    bobberHitGround,
    lineNoisesWaitingForFish,
    reelingInNoCaught,
    fishOnLineSplashingInWater,
    maxSizeFishCaught,
    firstTimeFishCaught,
    generalBoop
}
