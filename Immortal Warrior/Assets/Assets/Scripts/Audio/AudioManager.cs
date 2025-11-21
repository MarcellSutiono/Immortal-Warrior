using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource sfx;

    public AudioClip outsideMusic;
    public AudioClip gameMusic;
    public AudioClip run;
    public AudioClip attack;
    public AudioClip rolling;
    public AudioClip charge;
    public AudioClip hit;
    public AudioClip jump;
    public AudioClip parry;

    //-------- BOSS SFX --------
    public AudioClip attackBoss;

    private void Start()
    {
        music.clip = outsideMusic;
        music.volume = 0.5f;
        music.Play();
    }

    public void changeMusic(AudioClip clip)
    {
        music.clip = clip;
        music.Play();
    }

    public void playSFX(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }

    public void stopSFX()
    {
        sfx.Stop();
    }
}
