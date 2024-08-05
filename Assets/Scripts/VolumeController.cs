using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioSource audioSourceEffect;
    public AudioSource audioSourceMusic;
    public AudioSource footStepSource;
    public AudioClip[] audioClips;
    public enum Sounds
    {
        gameover,
        take
    }
    public Sounds soundState;

    private void Start()
    {
        float volumeEffect = PlayerPrefs.GetFloat("EffectVolume");
        float volumeMusic = PlayerPrefs.GetFloat("MusicVolume");
        audioSourceEffect.volume = volumeEffect;
        audioSourceMusic.volume = volumeMusic;
        footStepSource.volume = volumeEffect;
    }

    public void PlaySound(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.gameover:
                audioSourceEffect.PlayOneShot(audioClips[0]);
                break;
            case Sounds.take:
                audioSourceEffect.PlayOneShot(audioClips[1]);
                break;
        }
    }
}
