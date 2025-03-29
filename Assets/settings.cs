using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class settings : MonoBehaviour
{
    public GameObject settinggameobject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void settingclick()
    {
        settinggameobject.SetActive(true);  
    }

    public void settingcross()
    {
        settinggameobject.SetActive(false);
    }

    public void backbutton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

    }
    public void nextgame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
    public Slider slider; // Reference to the UI Slider
    private const string SliderPrefKey = "SliderValue"; // Key for PlayerPrefs

    void Start()
    {
        // Load the stored slider value (default to 1 if not found)
        if(slider != null)
        {
            slider.value = PlayerPrefs.GetFloat(SliderPrefKey, 1f);

            // Add listener to detect changes
            slider.onValueChanged.AddListener(delegate { SaveSliderValue(); });

        }

        //if (PlayerPrefs.HasKey("MusicVolume"))
        //{
        //    musicslider.value = PlayerPrefs.GetInt("MusicVolume");
        //}
        //if (PlayerPrefs.HasKey("MusicVolume"))
        //{
        //    musicslider.value = PlayerPrefs.GetInt("MusicVolume");
        //    gameaudio.volume = musicslider.value;
        //}
        //if (PlayerPrefs.HasKey("AudioVolume"))
        //{
        //    audioslider.value = PlayerPrefs.GetInt("AudioVolume");
        //    gameaudio.volume = audioslider.value;
        //}
    }
    //public Slider audioslider, musicslider;
    //public AudioSource bgaudio, gameaudio;
    public void SaveSliderValue()
    {
        PlayerPrefs.SetFloat(SliderPrefKey, slider.value);
        PlayerPrefs.Save(); // Save changes immediately
    }
}
