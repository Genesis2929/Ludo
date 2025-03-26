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
        SceneManager.LoadScene(0);

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
    }

    public void SaveSliderValue()
    {
        PlayerPrefs.SetFloat(SliderPrefKey, slider.value);
        PlayerPrefs.Save(); // Save changes immediately
    }
}
