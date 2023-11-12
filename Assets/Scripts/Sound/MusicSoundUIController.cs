using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class MusicSoundUIController : MonoBehaviour
{
    public static UnityEvent<bool> musicEvent = new UnityEvent<bool>();
    public static UnityEvent<bool> soundEvent = new UnityEvent<bool>();

    [SerializeField] private AudioSource musicSource;
    [SerializeField] public AudioMixer audioMixer;

    [Header("Buttons")]
    [SerializeField] private Image musicButton;
    [SerializeField] private Image soundButton;

    private string musicKey = "music";
    private string soundKey = "sound";

    private bool isMusic;
    private bool isSound;

    private void Awake()
    {
        // CheckStatus
        int musicInt = PlayerPrefs.GetInt(musicKey, 1); // Assuming 1 is the default value for music
        int soundInt = PlayerPrefs.GetInt(soundKey, 1); // Assuming 1 is the default value for sound

        isMusic = IntToBool(musicInt);
        isSound = IntToBool(soundInt);

        SetButtons();

        EventsController.StartEvent.AddListener(OnStart);
    }

    private void OnStart()
    {
        SetButtons();
    }

    public void ToggleMusic()
    {
        isMusic = !isMusic;
        SetButtons();
    }

    public void ToggleSound()
    {
        isSound = !isSound;
        SetButtons();
    }

    private void SetButtons()
    {
        Debug.Log($"Music: {isMusic}, Sound: {isSound}");

        musicButton.color = GetColor(isMusic);
        soundButton.color = GetColor(isSound);

        musicEvent.Invoke(isMusic);
        soundEvent.Invoke(isSound);

        musicSource.mute = !isMusic;        

        if (isSound)
        {
            Debug.Log("SOUND ON");
            audioMixer.SetFloat("Vol", 0f);
        }            
        else
        {
            Debug.Log("SOUND OFF");
            audioMixer.SetFloat("Vol", -80f);
        }
            

        PlayerPrefs.SetInt(musicKey, BoolToInt(isMusic));
        PlayerPrefs.SetInt(soundKey, BoolToInt(isSound));
    }

    private bool IntToBool(int num) => num != 0;
    private int BoolToInt(bool value) => value ? 1 : 0;

    public Color GetColor(bool isTrue)
    {
        float alpha = isTrue ? 1.0f : 0.5f;
        return new Color(1.0f, 1.0f, 1.0f, alpha);
    }
}
