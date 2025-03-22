using UnityEngine;
using UnityEngine.SceneManagement;

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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
