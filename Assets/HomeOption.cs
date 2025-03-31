using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
public class HomeOption : MonoBehaviour
{
    //public List<TMP_Text> selectplayertext = new List<TMP_Text>();
    private int state = 0;
    public List<int> buttonstate = new List<int>();
    public List<TMP_Text> buttontext = new List<TMP_Text>();

    // Reference to your UI Text component.
    public TMP_Text displayText;
    int noofplayer, noofAI;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

    }
    public void exitbutton()
    {
        Application.Quit();
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        for (int i = 0; i < 4; i++)
        {
            buttonstate.Add(0);
        }
        if(!PlayerPrefs.HasKey("PlayerNumber"))
        {
            PlayerPrefs.SetInt("PlayerNumber", 0);
            PlayerPrefs.Save();
        }



        preferenceinitial();
        preferencenumberofplayer();
        LoadRulesFromPlayerPrefs();
        preferenceupdate();
        UpdateAllRules();
    }
    public void preferenceinitial()
    {
        if (PlayerPrefs.HasKey("CoinNumber"))
        {
            coinnum = PlayerPrefs.GetInt("CoinNumber");
            if (PlayerPrefs.GetInt("CoinNumber") == 1)
            {
                coinimage1.SetActive(true);
                coinimage2.SetActive(false);
                coinimage3.SetActive(false);
                coinimage4.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("CoinNumber") == 2)
            {
                coinimage1.SetActive(false);
                coinimage2.SetActive(true);
                coinimage3.SetActive(false);
                coinimage4.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("CoinNumber") == 3)
            {
                coinimage1.SetActive(false);
                coinimage2.SetActive(false);
                coinimage3.SetActive(true);
                coinimage4.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("CoinNumber") == 4)
            {
                coinimage1.SetActive(false);
                coinimage2.SetActive(false);
                coinimage3.SetActive(false);
                coinimage4.SetActive(true);
            }
        }
        else
        {
            coinselect(4);
        }
        if (PlayerPrefs.HasKey("OneOrSix"))
        {
            if (PlayerPrefs.GetInt("OneOrSix") == 1)
            {
                selectdiceone.SetActive(true);
                selectdicesix.SetActive(false);
                onesix = false;

                oneorsixtext(true);
            }
            else
            {
                selectdiceone.SetActive(false);
                selectdicesix.SetActive(true);
                onesix = true;

                oneorsixtext(false);
            }
        }
        else
        {
            startcoinat(true);
        }
        if(PlayerPrefs.HasKey("Fling"))
        {
            if(PlayerPrefs.GetInt("Fling")== 1)
            {
                fling = false;
                //flingimage.sprite = flingsprite;
                flingtext.text = "Fling";
            }
            else
            {
                fling = true;
                //flingimage.sprite = touchsprite;
                flingtext.text = "Touch";
            }
        }
        else
        {
            flingenable();
        }

        if (PlayerPrefs.HasKey("Continueroll"))
        {
            if (PlayerPrefs.GetInt("Continueroll") == 1)
            {
                cont = false;
                //contimage.sprite = contspriteon;
                conttext.text = "On";
            }
            else
            {
                cont = true;
                //contimage.sprite = contspriteoff;
                conttext.text = "Off";
            }
        }
        else
        {

            contenable();
        }

        if (PlayerPrefs.HasKey("Difflevel"))
        {
             difflevel = PlayerPrefs.GetInt("Difflevel");
            if (PlayerPrefs.GetInt("Difflevel") == 0)
            {
                //diffimage.sprite = easy;
                difftext.text = "Easy";
            }
            else if (PlayerPrefs.GetInt("Difflevel") == 1)
            {
                //diffimage.sprite = medium;
                difftext.text = "Medium";
            }
            else
            {
                //diffimage.sprite = hard;
                difftext.text = "Hard";
            }
        }
        else
        {
            difficultyset();
        }
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


    void preferencenumberofplayer()
    {
        if(PlayerPrefs.HasKey("PlayerNumber"))
        {
            noofplayer = PlayerPrefs.GetInt("PlayerNumber");
        }
        if (PlayerPrefs.HasKey("AINumber"))
        {
            noofAI = PlayerPrefs.GetInt("AINumber");
        }

        if(PlayerPrefs.HasKey("Player1") && PlayerPrefs.HasKey("AI1"))
        {
            if(PlayerPrefs.GetInt("Player1") == 0 && PlayerPrefs.GetInt("AI1") == 0)
            {
                buttonstate[0] = 0;
                //OnAnyButtonClicked(0);
                buttontextupdate(0,0);
            }
            else if(PlayerPrefs.GetInt("Player1") == 1 && PlayerPrefs.GetInt("AI1") == 0)
            {
                buttonstate[0] = 1;
                //OnAnyButtonClicked(0);
                buttontextupdate(1, 0);
            }
            else
            {
                buttonstate[0] = 2;
                //OnAnyButtonClicked(0);
                buttontextupdate(2, 0);
            }
        }
        if (PlayerPrefs.HasKey("Player2") && PlayerPrefs.HasKey("AI2"))
        {
            if (PlayerPrefs.GetInt("Player2") == 0 && PlayerPrefs.GetInt("AI2") == 0)
            {
                buttonstate[1] = 0;
                buttontextupdate(0, 1);
            }
            else if (PlayerPrefs.GetInt("Player2") == 1 && PlayerPrefs.GetInt("AI2") == 0)
            {
                buttonstate[1] = 1;
                buttontextupdate(1, 1);
            }
            else
            {
                buttonstate[1] = 2;
                buttontextupdate(2, 1);
            }
        }
        if (PlayerPrefs.HasKey("Player3") && PlayerPrefs.HasKey("AI3"))
        {
            if (PlayerPrefs.GetInt("Player3") == 0 && PlayerPrefs.GetInt("AI3") == 0)
            {
                buttonstate[2] = 0;
                buttontextupdate(0, 2);
            }
            else if (PlayerPrefs.GetInt("Player3") == 1 && PlayerPrefs.GetInt("AI3") == 0)
            {
                buttonstate[2] = 1;
                buttontextupdate(1, 2);
            }
            else
            {
                buttonstate[2] = 2;
                buttontextupdate(2, 2);
            }
        }
        if (PlayerPrefs.HasKey("Player4") && PlayerPrefs.HasKey("AI4"))
        {
            if (PlayerPrefs.GetInt("Player4") == 0 && PlayerPrefs.GetInt("AI4") == 0)
            {
                buttonstate[3] = 0;
                buttontextupdate(0, 3);
            }
            else if (PlayerPrefs.GetInt("Player4") == 1 && PlayerPrefs.GetInt("AI4") == 0)
            {
                buttonstate[3] = 1;
                buttontextupdate(1, 3);
            }
            else
            {
                buttonstate[3] = 2;
                buttontextupdate(2, 3);
            }
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

    void buttontextupdate(int state, int buttonnumber)
    {
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
        buttontextupdate(state, buttonnumber);

        buttonstate[buttonnumber]= state;

        preferenceupdate();
        //Debug.Log(PlayerPrefs.GetInt("PlayerNumber"));
        //Debug.Log(PlayerPrefs.GetInt("Player1"));
        //Debug.Log(PlayerPrefs.GetInt("Player2"));
        //Debug.Log(PlayerPrefs.GetInt("Player3"));
        //Debug.Log(PlayerPrefs.GetInt("Player4"));
        //Debug.Log(PlayerPrefs.GetInt("AINumber"));
        //Debug.Log(PlayerPrefs.GetInt("AI1"));
        //Debug.Log(PlayerPrefs.GetInt("AI2"));
        //Debug.Log(PlayerPrefs.GetInt("AI3"));
        //Debug.Log(PlayerPrefs.GetInt("AI4"));
    }
    public GameObject continuegameunfinished;
    
    public void scenechange()
    {
        if (PlayerPrefs.HasKey("PlayerNumber"))
        {
            noofplayer = PlayerPrefs.GetInt("PlayerNumber");
            noofAI = PlayerPrefs.GetInt("AINumber");

            if (noofplayer >= 2)
            {
                if(noofAI < noofplayer)
                {
                    if(PlayerPrefs.HasKey("SaveEnable"))
                    {
                        if(PlayerPrefs.GetFloat("SaveEnable") == 1)
                        {
                            continuegameunfinished.SetActive(true);
                        }
                        else
                        {
                            SceneManager.LoadScene(1);
                        }
                    }
                    else
                    {
                        SceneManager.LoadScene(1);
                    }
                }
                else
                {
                    foreach (TMP_Text text in buttontext)
                    {
                        GameObject gm = text.gameObject;
                        Animator anim = gm.GetComponent<Animator>();

                        anim.SetTrigger("Textanim");

                    }
                }
            }
            else
            {
                foreach(TMP_Text text in buttontext)
                {
                    GameObject gm = text.gameObject;
                    Animator anim = gm.GetComponent<Animator>();

                    anim.SetTrigger("Textanim");

                }
            }
        }
        else
        {
            foreach (TMP_Text text in buttontext)
            {
                GameObject gm = text.gameObject;
                Animator anim = gm.GetComponent<Animator>();
                //anim.SetTrigger("Textbuttonanim");
                anim.SetTrigger("Textanim");


            }
        }
    }


    public GameObject coinimage1, coinimage2, coinimage3, coinimage4;
    public int coinnum = 0;

    public void coinselect(int coinnumber)
    {
        if(coinnumber == 1)
        {
            coinimage1.SetActive(true);
            coinimage2.SetActive(false);
            coinimage3.SetActive(false);
            coinimage4.SetActive(false);
        }
        if (coinnumber == 2)
        {
            coinimage1.SetActive(false);
            coinimage2.SetActive(true);
            coinimage3.SetActive(false);
            coinimage4.SetActive(false);
        }
        if (coinnumber == 3)
        {
            coinimage1.SetActive(false);
            coinimage2.SetActive(false);
            coinimage3.SetActive(true);
            coinimage4.SetActive(false);
        }
        if (coinnumber == 4)
        {
            coinimage1.SetActive(false);
            coinimage2.SetActive(false);
            coinimage3.SetActive(false);
            coinimage4.SetActive(true);
        }
        coinnum = coinnumber;


        PlayerPrefs.SetInt("CoinNumber", coinnum);
        PlayerPrefs.Save();
    }

    public int difflevel = 2;
    public Image diffimage;
    public Sprite easy, medium, hard;
    public TMP_Text difftext;
    public void difficultyset()
    {
        difflevel = (difflevel + 1) % 3;
        if(difflevel == 0)
        {
            //diffimage.sprite = easy;
            difftext.text = "Easy";
        }
        else if(difflevel == 1)
        {
            //diffimage.sprite = medium;
            difftext.text = "Medium";
        }
        else if (difflevel == 2)
        {
            //diffimage.sprite = hard;
            difftext.text = "Hard";
        }

        PlayerPrefs.SetInt("Difflevel", difflevel);
        PlayerPrefs.Save();
    }

    public Image flingimage;
    public Sprite flingsprite, touchsprite;
    public TMP_Text flingtext;
    bool fling = true;
    public void flingenable()
    {
        if(fling)
        {

            //flingimage.sprite = flingsprite;
            flingtext.text = "Fling";
            PlayerPrefs.SetInt("Fling", 1);
            PlayerPrefs.Save();
            fling = false;
        }
        else
        {
            //flingimage.sprite = touchsprite;
            flingtext.text = "Touch";
            PlayerPrefs.SetInt("Fling", 0);
            PlayerPrefs.Save();
            fling = true;
        }
    }

    public Image contimage;
    public Sprite contspriteoff, contspriteon;
    public TMP_Text conttext;
    bool cont = false;
    public void contenable()
    {
        if (cont)
        {

            //contimage.sprite = contspriteon;
            conttext.text = "On";
            PlayerPrefs.SetInt("Continueroll", 1);
            PlayerPrefs.Save();

            cont = false;
        }
        else
        {
            //contimage.sprite = contspriteoff;
            conttext.text = "Off";
            PlayerPrefs.SetInt("Continueroll", 0);
            PlayerPrefs.Save();
            cont = true;
        }
    }

    public GameObject selectdiceone, selectdicesix;
    bool onesix = false;
    public void startcoinat(bool oneorsix)
    {
        if(oneorsix)
        {
            PlayerPrefs.SetInt("OneOrSix", 1);
            PlayerPrefs.Save();
            selectdiceone.SetActive(true);
            selectdicesix.SetActive(false);
            onesix = false;
        }
        else
        {
            PlayerPrefs.SetInt("OneOrSix", 6);
            PlayerPrefs.Save();
            selectdiceone.SetActive(false);
            selectdicesix.SetActive(true);
            onesix = true;
        }

        UpdateAllRules();
        oneorsixtext(oneorsix);
    }

    public TMP_Text rule1text, rule2text, rule4text, rule5text, rule6text, rule11text;

    void oneorsixtext(bool oneorsix)
    {
        if(oneorsix)
        {
            rule1text.text = "6 gives another turn";
            rule2text.text = "6 also brings a coin out";
            rule4text.text = "3 consecutive rolls of 1 cuts one own coin";
            rule5text.text = "Skip a turn on 3 consecutive rolls of 1";
            rule6text.text = "3 consecutive rolls of 6 brings a coin out";
            rule11text.text = "Must bring a coin out on 1";
        }
        else
        {
            rule1text.text = "1 gives another turn";
            rule2text.text = "1 also brings a coin out";
            rule4text.text = "3 consecutive rolls of 6 cuts one own coin";
            rule5text.text = "Skip a turn on 3 consecutive rolls of 6";
            rule6text.text = "3 consecutive rolls of 1 brings a coin out";
            rule11text.text = "Must bring a coin out on 6";
        }
    }
    public void UpdateAllRules()
    {
        // --- Rule 1 ---
        if (rule1)
        {
            rule1image.sprite = rightsprite;
            if (!onesix)
                PlayerPrefs.SetInt("SixTurn", 1);
            else
                PlayerPrefs.SetInt("OneTurn", 1);
        }
        else
        {
            rule1image.sprite = wrongsprite;
            rule2 = false;
            rule2text.color = Color.gray;
            rule6 = false;
            rule6text.color = Color.gray;

            if (!onesix)
                PlayerPrefs.SetInt("SixTurn", 0);
            else
                PlayerPrefs.SetInt("OneTurn", 0);
        }

        // --- Rule 2 ---
        if (rule2)
        {
            rule2image.sprite = rightsprite;
            rule6 = false;
            rule6text.color = Color.gray;
            
            if (!onesix)
            {
                PlayerPrefs.SetInt("SixOut", 1);
                // Also update rule1 as in your original logic
                rule1 = true;
                rule1image.sprite = rightsprite;
                PlayerPrefs.SetInt("SixTurn", 1);
            }
            else
            {
                PlayerPrefs.SetInt("OneOut", 1);
                rule1 = true;
                rule1image.sprite = rightsprite;
                PlayerPrefs.SetInt("OneTurn", 1);
            }
        }
        else
        {
            rule2image.sprite = wrongsprite;
            if (!onesix)
                PlayerPrefs.SetInt("SixOut", 0);
            else
                PlayerPrefs.SetInt("OneOut", 0);
        }

        // --- Rule 3 ---
        if (rule3)
        {
            rule3image.sprite = rightsprite;
            PlayerPrefs.SetInt("ShowStar", 1);
        }
        else
        {
            rule3image.sprite = wrongsprite;
            PlayerPrefs.SetInt("ShowStar", 0);
        }

        // --- Rule 4 ---
        if (rule4)
        {
            rule4image.sprite = rightsprite;
            if (!onesix)
                PlayerPrefs.SetInt("3onecut", 1);
            else
                PlayerPrefs.SetInt("3sixcut", 1);
        }
        else
        {
            rule4image.sprite = wrongsprite;
            if (!onesix)
                PlayerPrefs.SetInt("3onecut", 0);
            else
                PlayerPrefs.SetInt("3sixcut", 0);
        }

        // --- Rule 5 ---
        if (rule5)
        {
            rule5image.sprite = rightsprite;
            if (!onesix)
                PlayerPrefs.SetInt("3oneskip", 1);
            else
                PlayerPrefs.SetInt("3sixskip", 1);
        }
        else
        {
            rule5image.sprite = wrongsprite;
            if (!onesix)
                PlayerPrefs.SetInt("3oneskip", 0);
            else
                PlayerPrefs.SetInt("3sixskip", 0);
        }

        // --- Rule 6 ---
        if (rule6)
        {
            if(rule2 == false)
            {
                rule6image.sprite = rightsprite;
                if (!onesix)
                    PlayerPrefs.SetInt("3sixout", 1);
                else
                    PlayerPrefs.SetInt("3oneout", 1);
            }
            else
            {
                rule6image.sprite = wrongsprite;
                if (!onesix)
                    PlayerPrefs.SetInt("3sixout", 0);
                else
                    PlayerPrefs.SetInt("3oneout", 0);
            }
        }
        else
        {
            rule6image.sprite = wrongsprite;
            if (!onesix)
                PlayerPrefs.SetInt("3sixout", 0);
            else
                PlayerPrefs.SetInt("3oneout", 0);
        }

        // --- Rule 7 ---
        if (rule7)
        {
            rule7image.sprite = rightsprite;
            PlayerPrefs.SetInt("cutturn", 1);
        }
        else
        {
            rule7image.sprite = wrongsprite;
            PlayerPrefs.SetInt("cutturn", 0);
        }

        // --- Rule 8 ---
        if (rule8)
        {
            rule8image.sprite = rightsprite;
            PlayerPrefs.SetInt("hometurn", 1);
        }
        else
        {
            rule8image.sprite = wrongsprite;
            PlayerPrefs.SetInt("hometurn", 0);
        }

        // --- Rule 9 ---
        if (rule9)
        {
            rule9image.sprite = rightsprite;
            PlayerPrefs.SetInt("Mustcuthome", 1);
        }
        else
        {
            rule9image.sprite = wrongsprite;
            PlayerPrefs.SetInt("Mustcuthome", 0);
        }

        // --- Rule 10 ---
        if (rule10)
        {
            rule10image.sprite = rightsprite;
            PlayerPrefs.SetInt("Mustcutcuttable", 1);
        }
        else
        {
            rule10image.sprite = wrongsprite;
            PlayerPrefs.SetInt("Mustcutcuttable", 0);
        }

        // --- Rule 11 ---
        if (rule11)
        {
            rule11image.sprite = rightsprite;
            PlayerPrefs.SetInt("Mustout", 1);
        }
        else
        {
            rule11image.sprite = wrongsprite;
            PlayerPrefs.SetInt("Mustout", 0);
        }

        // --- Rule 12 ---
        if (rule12)
        {
            rule12image.sprite = rightsprite;
            PlayerPrefs.SetInt("2coinbarrier", 1);
        }
        else
        {
            rule12image.sprite = wrongsprite;
            PlayerPrefs.SetInt("2coinbarrier", 0);
        }

        // Finally, save all changes at once.
        PlayerPrefs.Save();
    }
    public void LoadRulesFromPlayerPrefs()
    {
        // --- Rule 1: "SixTurn" or "OneTurn" based on onesix ---
        if (!onesix)
        {
            if (PlayerPrefs.HasKey("SixTurn"))
                rule1 = PlayerPrefs.GetInt("SixTurn") == 1;
            else
                rule1 = false;
        }
        else
        {
            if (PlayerPrefs.HasKey("OneTurn"))
                rule1 = PlayerPrefs.GetInt("OneTurn") == 1;
            else
                rule1 = false;
        }

        // --- Rule 2: "SixOut" or "OneOut" ---
        if (!onesix)
        {
            if (PlayerPrefs.HasKey("SixOut"))
                rule2 = PlayerPrefs.GetInt("SixOut") == 1;
            else
                rule2 = false;
        }
        else
        {
            if (PlayerPrefs.HasKey("OneOut"))
                rule2 = PlayerPrefs.GetInt("OneOut") == 1;
            else
                rule2 = false;
        }

        // --- Rule 3: "ShowStar" ---
        if (PlayerPrefs.HasKey("ShowStar"))
            rule3 = PlayerPrefs.GetInt("ShowStar") == 1;
        else
            rule3 = false;

        // --- Rule 4: "3onecut" or "3sixcut" ---
        if (!onesix)
        {
            if (PlayerPrefs.HasKey("3onecut"))
                rule4 = PlayerPrefs.GetInt("3onecut") == 1;
            else
                rule4 = false;
        }
        else
        {
            if (PlayerPrefs.HasKey("3sixcut"))
                rule4 = PlayerPrefs.GetInt("3sixcut") == 1;
            else
                rule4 = false;
        }

        // --- Rule 5: "3oneskip" or "3sixskip" ---
        if (!onesix)
        {
            if (PlayerPrefs.HasKey("3oneskip"))
                rule5 = PlayerPrefs.GetInt("3oneskip") == 1;
            else
                rule5 = false;
        }
        else
        {
            if (PlayerPrefs.HasKey("3sixskip"))
                rule5 = PlayerPrefs.GetInt("3sixskip") == 1;
            else
                rule5 = false;
        }

        // --- Rule 6: "3sixout" or "3oneout" ---
        if (!onesix)
        {
            if (PlayerPrefs.HasKey("3sixout"))
                rule6 = PlayerPrefs.GetInt("3sixout") == 1;
            else
                rule6 = false;
        }
        else
        {
            if (PlayerPrefs.HasKey("3oneout"))
                rule6 = PlayerPrefs.GetInt("3oneout") == 1;
            else
                rule6 = false;
        }

        // --- Rule 7: "cutturn" ---
        if (PlayerPrefs.HasKey("cutturn"))
            rule7 = PlayerPrefs.GetInt("cutturn") == 1;
        else
            rule7 = false;

        // --- Rule 8: "hometurn" ---
        if (PlayerPrefs.HasKey("hometurn"))
            rule8 = PlayerPrefs.GetInt("hometurn") == 1;
        else
            rule8 = false;

        // --- Rule 9: "Mustcuthome" ---
        if (PlayerPrefs.HasKey("Mustcuthome"))
            rule9 = PlayerPrefs.GetInt("Mustcuthome") == 1;
        else
            rule9 = false;

        // --- Rule 10: "Mustcutcuttable" ---
        if (PlayerPrefs.HasKey("Mustcutcuttable"))
            rule10 = PlayerPrefs.GetInt("Mustcutcuttable") == 1;
        else
            rule10 = false;

        // --- Rule 11: "Mustout" ---
        if (PlayerPrefs.HasKey("Mustout"))
            rule11 = PlayerPrefs.GetInt("Mustout") == 1;
        else
            rule11 = false;

        // --- Rule 12: "2coinbarrier" ---
        if (PlayerPrefs.HasKey("2coinbarrier"))
            rule12 = PlayerPrefs.GetInt("2coinbarrier") == 1;
        else
            rule12 = false;
    }


    void rule6func()
    {
        if(rule1 == true && rule2 == false)
        {
            if(rule6 == false)
            {
                rule6 = true;
                rule6image.sprite = rightsprite;

                if (onesix == false)
                {
                    PlayerPrefs.SetInt("3sixout", 1);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("3oneout", 1);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                rule6 = false;
                rule6image.sprite = wrongsprite;

                if (onesix == false)
                {
                    PlayerPrefs.SetInt("3sixout", 0);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("3oneout", 0);
                    PlayerPrefs.Save();
                }
            }
        }
        else if((rule1 == true && rule2 == true) || rule1 == false)
        {
            rule6 = false;
            rule6image.sprite = wrongsprite;

            if (onesix == false)
            {
                PlayerPrefs.SetInt("3sixout", 0);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.SetInt("3oneout", 0);
                PlayerPrefs.Save();
            }
        }
        
    }

    public bool rule1 = true, rule2 = false, rule3 = true, rule4 = true, rule5 = false, rule6 = false, rule7 = true, rule8 = true, rule9 = false, rule10 = false, rule11 = false, rule12 = false;
    public Image rule1image, rule2image, rule3image, rule4image, rule5image, rule6image, rule7image, rule8image, rule9image, rule10image, rule11image, rule12image;
    public Sprite rightsprite, wrongsprite;
    public void gamerule(int ruleno)
    {
        if(ruleno == 1)
        {
            if(rule1 == false)
            {
                rule1 = true;
                rule1image.sprite = rightsprite;
                rule2text.color = Color.black;
                if (onesix == false)
                {
                    PlayerPrefs.SetInt("SixTurn", 1);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("OneTurn", 1);
                    PlayerPrefs.Save();
                }

                if(rule2 == false)
                {
                    rule6text.color = Color.black;
                }
            }
            else
            {
                rule1 = false;
                rule1image.sprite = wrongsprite;

                rule2 = false;
                rule2image.sprite = wrongsprite;
                rule2text.color = Color.gray;

                rule6image.sprite = wrongsprite;

                rule6text.color = Color.gray;
                rule6 = false;

                if (onesix == false)
                {
                    PlayerPrefs.SetInt("SixTurn", 0);
                    PlayerPrefs.Save();
                    PlayerPrefs.SetInt("SixOut", 0);
                    PlayerPrefs.Save();

                    PlayerPrefs.SetInt("3sixout", 0);
                    PlayerPrefs.Save();

                }
                else
                {
                    PlayerPrefs.SetInt("OneTurn", 0);
                    PlayerPrefs.Save();
                    PlayerPrefs.SetInt("OneOut", 0);
                    PlayerPrefs.Save();


                    PlayerPrefs.SetInt("3oneout", 0);
                    PlayerPrefs.Save();
                }
            }
        }
        else if (ruleno == 2)
        {
            if(rule1 == true)
            {
                if (rule2 == false)
                {
                    rule2 = true;
                    rule2image.sprite = rightsprite;
                    rule6image.sprite = wrongsprite;

                    rule6text.color = Color.gray;
                    rule6 = false;

                    if (onesix == false)
                    {
                        PlayerPrefs.SetInt("SixOut", 1);
                        PlayerPrefs.Save();

                        rule1 = true;
                        rule1image.sprite = rightsprite;
                        PlayerPrefs.SetInt("SixTurn", 1);
                        PlayerPrefs.Save();

                        PlayerPrefs.SetInt("3sixout", 0);
                        PlayerPrefs.Save();

                    }
                    else
                    {
                        PlayerPrefs.SetInt("OneOut", 1);
                        PlayerPrefs.Save();

                        rule1 = true;
                        rule1image.sprite  = rightsprite;
                        PlayerPrefs.SetInt("OneTurn", 1);
                        PlayerPrefs.Save();

                        PlayerPrefs.SetInt("3oneout", 0);
                        PlayerPrefs.Save();
                    }
                }
                else
                {
                    rule2 = false;
                    rule2image.sprite = wrongsprite;

                    rule6text.color = Color.black;


                    if (onesix == false)
                    {
                        PlayerPrefs.SetInt("SixOut", 0);
                        PlayerPrefs.Save();
                    }
                    else
                    {
                        PlayerPrefs.SetInt("OneOut", 0);
                        PlayerPrefs.Save();
                    }
                }

            }
            else
            {
                rule2 = false;
                rule2image.sprite = wrongsprite;
                rule6text.color = Color.gray;

                if (onesix == false)
                {
                    PlayerPrefs.SetInt("SixOut", 0);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("OneOut", 0);
                    PlayerPrefs.Save();
                }
            }
        }
        else if (ruleno == 3)
        {
            if (rule3 == false)
            {
                rule3 = true;
                rule3image.sprite = rightsprite;

                PlayerPrefs.SetInt("ShowStar", 1);
                PlayerPrefs.Save();
            }
            else
            {
                rule3 = false;
                rule3image.sprite = wrongsprite;
                PlayerPrefs.SetInt("ShowStar", 0);
                PlayerPrefs.Save();
            }
        }
       else if (ruleno == 4)
        {
            if (rule4 == false)
            {
                rule4 = true;
                rule4image.sprite = rightsprite;
                if (onesix == false)
                {
                    PlayerPrefs.SetInt("3onecut", 1);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("3sixcut", 1);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                rule4 = false;
                rule4image.sprite = wrongsprite;

                if (onesix == false)
                {
                    PlayerPrefs.SetInt("3onecut", 0);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("3sixcut", 0);
                    PlayerPrefs.Save();
                }
            }
        }
       else  if (ruleno == 5)
        {
            if (rule5 == false)
            {
                rule5 = true;
                rule5image.sprite = rightsprite;
                if (onesix == false)
                {
                    PlayerPrefs.SetInt("3oneskip", 1);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("3sixskip", 1);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                rule5 = false;
                rule5image.sprite = wrongsprite;

                if (onesix == false)
                {
                    PlayerPrefs.SetInt("3oneskip", 0);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("3sixskip", 0);
                    PlayerPrefs.Save();
                }
            }
        }
        else if (ruleno == 6)
        {
            rule6func();
            //if (rule6 == false)
            //{
            //    if(rule2 == false)
            //    {
            //        rule6 = true;
            //        rule6image.sprite = rightsprite;

            //        if (onesix == false)
            //        {
            //            PlayerPrefs.SetInt("3sixout", 1);
            //            PlayerPrefs.Save();
            //        }
            //        else
            //        {
            //            PlayerPrefs.SetInt("3oneout", 1);
            //            PlayerPrefs.Save();
            //        }
            //    }
            //    else
            //    {
            //        rule6 = false;
            //        rule6image.sprite = wrongsprite;

            //        if (onesix == false)
            //        {
            //            PlayerPrefs.SetInt("3sixout", 0);
            //            PlayerPrefs.Save();
            //        }
            //        else
            //        {
            //            PlayerPrefs.SetInt("3oneout", 0);
            //            PlayerPrefs.Save();
            //        }
            //    }
            //}
            //else
            //{
            //    rule6 = false;
            //    rule6image.sprite = wrongsprite;
            //    if (onesix == false)
            //    {
            //        PlayerPrefs.SetInt("3sixout", 0);
            //        PlayerPrefs.Save();
            //    }
            //    else
            //    {
            //        PlayerPrefs.SetInt("3oneout", 0);
            //        PlayerPrefs.Save();
            //    }
            //}
        }
        else if (ruleno == 7)
        {
            if (rule7 == false)
            {
                rule7 = true;
                rule7image.sprite = rightsprite;

                //if (onesix == false)
                {
                    PlayerPrefs.SetInt("cutturn", 1);
                    PlayerPrefs.Save();
                }

            }
            else
            {
                rule7 = false;
                rule7image.sprite = wrongsprite;
                PlayerPrefs.SetInt("cutturn", 0);
                PlayerPrefs.Save();
            }
        }
       else if (ruleno == 8)
        {
            if (rule8 == false)
            {
                rule8 = true;
                rule8image.sprite = rightsprite;

                //if (onesix == false)
                {
                    PlayerPrefs.SetInt("hometurn", 1);
                    PlayerPrefs.Save();
                }

            }
            else
            {
                rule8 = false;
                rule8image.sprite = wrongsprite;
                PlayerPrefs.SetInt("hometurn", 0);
                PlayerPrefs.Save();
            }
        }
        else if (ruleno == 9)
        {
            if (rule9 == false)
            {
                rule9 = true;
                rule9image.sprite = rightsprite;

                //if (onesix == false)
                {
                    PlayerPrefs.SetInt("Mustcuthome", 1);
                    PlayerPrefs.Save();
                }

            }
            else
            {
                rule9 = false;
                rule9image.sprite = wrongsprite;
                PlayerPrefs.SetInt("Mustcuthome", 0);
                PlayerPrefs.Save();
            }
        }
        else if (ruleno == 10)
        {
            if (rule10 == false)
            {
                rule10 = true;
                rule10image.sprite = rightsprite;

                //if (onesix == false)
                {
                    PlayerPrefs.SetInt("Mustcutcuttable", 1);
                    PlayerPrefs.Save();
                }

            }
            else
            {
                rule10 = false;
                rule10image.sprite = wrongsprite;
                PlayerPrefs.SetInt("Mustcutcuttable", 0);
                PlayerPrefs.Save();
            }
        }
       else if (ruleno == 11)
        {
            if (rule11 == false)
            {
                rule11 = true;
                rule11image.sprite = rightsprite;

                //if (onesix == false)
                {
                    PlayerPrefs.SetInt("Mustout", 1);
                    PlayerPrefs.Save();
                }

            }
            else
            {
                rule11 = false;
                rule11image.sprite = wrongsprite;
                PlayerPrefs.SetInt("Mustout", 0);
                PlayerPrefs.Save();
            }
        }
        else if (ruleno == 12)
        {
            if (rule12 == false)
            {
                rule12 = true;
                rule12image.sprite = rightsprite;

                //if (onesix == false)
                {
                    PlayerPrefs.SetInt("2coinbarrier", 1);
                    PlayerPrefs.Save();
                }

            }
            else
            {
                rule12 = false;
                rule12image.sprite = wrongsprite;
                PlayerPrefs.SetInt("2coinbarrier", 0);
                PlayerPrefs.Save();
            }
        }
    }

}
