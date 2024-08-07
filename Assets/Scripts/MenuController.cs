using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Toggle[] toggleGroup;

    [Header("UI Elements")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        InitSettings();
    }
    void Start()
    {
        startButton.onClick.AddListener(() => OnStart());
        exitButton.onClick.AddListener(() => OnExit());
    }

    private void OnStart()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    private void OnExit()
    {
        Application.Quit();
    }

    private void InitSettings()
    {
        startButton.onClick.AddListener(() => SceneManager.LoadScene(1));
        Time.timeScale = 1;
        float effectVolume = PlayerPrefs.GetFloat("EffectVolume", 1f);
        effectsSlider.value = effectVolume;        
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSlider.value = musicVolume;
        audioSource.volume = musicVolume;
        int difficult = PlayerPrefs.GetInt("difficult");
        switch (difficult)
        {
            case 1:
                toggleGroup[0].isOn = true;
                break;
            case 2:
                toggleGroup[1].isOn = true;
                break;
            case 3:
                toggleGroup[2].isOn = true;
                break;
        }
    }

    public void SetDifficult(int num)
    {
        PlayerPrefs.SetInt("difficult", num);
        Debug.Log("difficult set "+num);
    }

    public void SetEffectVolume(Slider slider)
    {
        PlayerPrefs.SetFloat("EffectVolume", slider.value);
        Debug.Log($"volime is {slider.value}");
    }

    public void SetMusicVolume(Slider slider)
    {
        PlayerPrefs.SetFloat("MusicVolume", slider.value);
        Debug.Log($"volime is {slider.value}");
        audioSource.volume = slider.value;
    }
}
