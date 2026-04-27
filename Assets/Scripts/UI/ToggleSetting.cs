using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Synchronizes a UI toggle with the audio settings state.
/// </summary>
public class ToggleSetting : MonoBehaviour
{
    public ToggleType toggleType = ToggleType.Music;

    void Start()
    {
        // Initializes the toggle state from the audio manager settings.
        Toggle toggle = GetComponent<Toggle>();

        if (toggleType == ToggleType.Music)
        {
            toggle.isOn = !AudioManager.Instance.musicOn;
        }
        else if (toggleType == ToggleType.SFX)
        {
            toggle.isOn = !AudioManager.Instance.sfxOn;
        }
    }

    public void OnToggleValueChanged()
    {
        // Flips the selected audio setting when the UI toggle changes.
        if (toggleType == ToggleType.Music)
        {
            AudioManager.Instance.ToggleMusic();
        }
        else if (toggleType == ToggleType.SFX)
        {
            AudioManager.Instance.ToggleSFX();
        }
    }
}