using UnityEngine;

/// <summary>
/// Plays sound effects and controls global audio state for music and SFX.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip click;
    public AudioClip place;
    public AudioClip win;
    public AudioClip popup;

    public bool musicOn = true;
    public bool sfxOn = true;

    void Awake()
    {
        Instance = this;

        musicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        sfxOn = PlayerPrefs.GetInt("SFX", 1) == 1;
        bgmSource.mute = !musicOn;
    }

    public void ToggleMusic()
    {
        bool value = !musicOn;
        musicOn = value;
        bgmSource.mute = !value;
        PlayerPrefs.SetInt("Music", value ? 1 : 0);
    }

    public void ToggleSFX()
    {
        bool value = !sfxOn;
        sfxOn = value;
        PlayerPrefs.SetInt("SFX", value ? 1 : 0);

        if (value)
            sfxSource.PlayOneShot(click);
    }

    public void PlaySFX(string type)
    {
        if (!sfxOn)
            return;

        switch (type)
        {
            case "click":
                sfxSource.PlayOneShot(click);
                break;
            case "place":
                sfxSource.PlayOneShot(place);
                break;
            case "win":
                sfxSource.PlayOneShot(win);
                break;
            case "popup":
                sfxSource.PlayOneShot(popup);
                break;
        }
    }
}