using UnityEngine;
using UnityEngine.UI;

public class soundscript : MonoBehaviour
{

    public AudioClip dicerollaudio;
    public AudioClip piecemoveaudio;
    public AudioClip piecemovedieaudio;
    public AudioSource gameaudio;
    public AudioSource bgaudio;


    public Slider musicslider, audioslider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            if(musicslider != null && bgaudio != null)
            {
                musicslider.value = PlayerPrefs.GetFloat("MusicVolume");
                bgaudio.volume = musicslider.value;

            }
        }
        else
        {
           if(musicslider !=null && bgaudio != null)
            {
                musicslider.value = bgaudio.volume/3;
            }
        }
        if (PlayerPrefs.HasKey("AudioVolume"))
        {
            if(audioslider!= null && gameaudio != null)
            {
                audioslider.value = PlayerPrefs.GetFloat("AudioVolume");
                gameaudio.volume = audioslider.value;

            }
        }
        else
        {
            if(audioslider != null && gameaudio != null)
            {
                audioslider.value = 1f;
                gameaudio.volume = 1f;
            }
        }
    }
    public void dicerollplay()
    {
        if(dicerollaudio != null && gameaudio !=null)
        {
            gameaudio.volume = 3f;
            gameaudio.PlayOneShot(dicerollaudio);
        }
    }

    public void piecemovesound()
    {
        if (gameaudio != null && piecemoveaudio != null)
        {
            gameaudio.volume = 3f;
            gameaudio.PlayOneShot(piecemoveaudio);
        }
    }
    public void piecemovediesound()
    {
        if (gameaudio != null && piecemovedieaudio != null)
        {
            gameaudio.volume = 3f;
            gameaudio.PlayOneShot(piecemovedieaudio);
        }
    }

    public void musicvolumeupdate()
    {
        float value = musicslider.value;
        if(bgaudio != null)
        bgaudio.volume = 3 * value;

        Debug.Log(bgaudio.volume);
        PlayerPrefs.SetFloat("MusicVolume", bgaudio.volume);

    }
    public void audiovolumeupdate()
    {
        float value = audioslider.value;

        if(gameaudio != null)
        gameaudio.volume = value;


        PlayerPrefs.SetFloat("AudioVolume", gameaudio.volume);

    }
}
