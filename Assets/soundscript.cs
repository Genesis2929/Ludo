using UnityEngine;

public class soundscript : MonoBehaviour
{

    public AudioClip dicerollaudio;
    public AudioClip piecemoveaudio;
    public AudioSource gameaudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        if (dicerollaudio != null && piecemoveaudio != null)
        {
            gameaudio.volume = 3f;
            gameaudio.PlayOneShot(piecemoveaudio);
        }
    }
}
