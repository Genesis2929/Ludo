using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class HomeOption : MonoBehaviour
{
    //public List<TMP_Text> selectplayertext = new List<TMP_Text>();
    private int state = 0;
    public List<int> buttonstate = new List<int>();
    public List<TMP_Text> buttontext = new List<TMP_Text>();

    // Reference to your UI Text component.
    public TMP_Text displayText;
    int noofplayer, noofAI;
    


    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            buttonstate.Add(0);
        }
        if(!PlayerPrefs.HasKey("PlayerNumber"))
        {
            PlayerPrefs.SetInt("PlayerNumber", 0);
            PlayerPrefs.Save();
        }

        preferenceupdate();
    }

    private void preferenceupdate()
    {
        if(PlayerPrefs.HasKey("PlayerNumber"))
        {
            //foreach(int i in buttonstate)
            //{
            //    if(i != 0)
            //    {
            //        noofplayer ++;
            //    }
            //}
            noofplayer = 0;
            noofAI= 0;
            for(int i = 0; i < buttonstate.Count; i++)
            {
                if (buttonstate[i] != 0)
                {
                    noofplayer++;

                    if (buttonstate[i] == 1)
                    {
                        isplayercheck(i, 1, 0);
                    }
                    else
                    {
                        isplayercheck(i, 0, 1);
                        noofAI++;
                    }

                }
                else
                {
                    isplayercheck(i, 0, 0);
                }
            }
            PlayerPrefs.SetInt("PlayerNumber", noofplayer);
            PlayerPrefs.Save();
            PlayerPrefs.SetInt("AINumber", noofAI);
            PlayerPrefs.Save();
        }
    }

    void isplayercheck(int i, int boolnum, int aiboolnum)
    {
        if(i == 0)
        {
            if(boolnum == 1 && aiboolnum == 0)
            {
                PlayerPrefs.SetInt("Player1", 1);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI1", 0);
                PlayerPrefs.Save();

            }
            else if(boolnum == 0 && aiboolnum == 1)
            {
                PlayerPrefs.SetInt("Player1", 1);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI1", 1);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.SetInt("Player1", 0);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI1", 0);
                PlayerPrefs.Save();
            }
        }
        else if (i == 1)
        {
            if (boolnum == 1 && aiboolnum == 0)
            {
                PlayerPrefs.SetInt("Player2", 1);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI2", 0);
                PlayerPrefs.Save();

            }
            else if (boolnum == 0 && aiboolnum == 1)
            {
                PlayerPrefs.SetInt("Player2", 1);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI2", 1);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.SetInt("Player2", 0);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI2", 0);
                PlayerPrefs.Save();
            }
        }
        else if (i == 2)
        {
            if (boolnum == 1 && aiboolnum == 0)
            {
                PlayerPrefs.SetInt("Player3", 1);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI3", 0);
                PlayerPrefs.Save();

            }
            else if (boolnum == 0 && aiboolnum == 1)
            {
                PlayerPrefs.SetInt("Player3", 1);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI3", 1);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.SetInt("Player3", 0);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI3", 0);
                PlayerPrefs.Save();
            }
        }
        else if (i == 3)
        {
            if (boolnum == 1 && aiboolnum == 0)
            {
                PlayerPrefs.SetInt("Player4", 1);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI4", 0);
                PlayerPrefs.Save();

            }
            else if (boolnum == 0 && aiboolnum == 1)
            {
                PlayerPrefs.SetInt("Player4", 1);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI4", 1);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.SetInt("Player4", 0);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("AI4", 0);
                PlayerPrefs.Save();
            }
        }


    }
    // This function will be assigned to all four buttons.
    // In the Inspector, add an OnClick event for each button that calls this function,
    // and supply the desired text (unique to each button) in the string parameter.
    public void OnAnyButtonClicked(int buttonnumber)
    {
        
        state = buttonstate[buttonnumber];
        // Cycle state variable.
        state = (state + 1) % 3;

        // If state is 0, update the UI Text with the passed buttonText.
        if (state == 0)
        {
            //displayText.text = buttonText;
            buttontext[buttonnumber].text = "None";
        }
        else if (state == 1)
        {
            // Optionally, set a different text when state is 1.
            //displayText.text = "State is One";
            buttontext[buttonnumber].text = "Human";
        }
        else if (state == 2)
        {
            // Optionally, set a different text when state is 2.
            //displayText.text = "State is Two";
            buttontext[buttonnumber].text = "Computer";
        }
        buttonstate[buttonnumber]= state;

        preferenceupdate();
        Debug.Log(PlayerPrefs.GetInt("PlayerNumber"));
        Debug.Log(PlayerPrefs.GetInt("Player1"));
        Debug.Log(PlayerPrefs.GetInt("Player2"));
        Debug.Log(PlayerPrefs.GetInt("Player3"));
        Debug.Log(PlayerPrefs.GetInt("Player4"));
        Debug.Log(PlayerPrefs.GetInt("AINumber"));
        Debug.Log(PlayerPrefs.GetInt("AI1"));
        Debug.Log(PlayerPrefs.GetInt("AI2"));
        Debug.Log(PlayerPrefs.GetInt("AI3"));
        Debug.Log(PlayerPrefs.GetInt("AI4"));
    }


    public void scenechange()
    {
        SceneManager.LoadScene(1);
    }


}
