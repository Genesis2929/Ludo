using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class optionscript : MonoBehaviour
{
    public static bool startatoneorsix = false; //false means startatone
    public static bool sixgiveanotherturn = true;
    public static bool sixalsobringcoinout = false;
    public static bool sixtostart = false;
    public static bool threeonecut = true;
    public static bool threeoneskip = false;
    public static bool threesixstart = true;
    public static bool showstar = false; // incomplete
    public static bool cutgainturn = false; //
    public static bool homegainturn = true; //

    //public static bool mustcut = false; //
    public static bool mustcuttable = false; //

    public static bool mustbringcoinout = false; //

    public static bool barrier = false; //optional
    public static bool mustcuttoenterhome = false; //optional



    public static bool onealsogiveturn = false;
    public static bool onealsobringcoinout = false;
    public static bool threesixcut = false;
    public static bool threesixskip= false;
    public static bool threeonestart = false;

   
    public static bool flingenable = true;

    public static int difficultylevel = 1;

    public static int coinnumber = 4;
    public static bool start3timesskip = false;
    public static bool other3timesskip = false;
    public static bool start3timescut = false;

    public static bool continueroll = true;
    public List<GameObject> starobject = new List<GameObject>();
    public GameObject starparent = null;


    public static bool loadingenable = true;



    [Header("Basic Settings")]
    public bool _startAtOneOrSix = false; // false means start at one
    public bool _sixGiveAnotherTurn = true;
    public bool _sixAlsoBringCoinOut = false;
    public bool _sixToStart = false;

    [Header("Rule Settings")]
    public bool _threeOneCut = true;
    public bool _threeOneSkip = false;
    public bool _threeSixStart = true;
    public bool _showStar = false; // incomplete
    public bool _cutGainTurn = false;
    public bool _homeGainTurn = true;

    [Header("Must Rules")]
    public bool _mustCutTable = false;      // corresponds to static mustcuttable
    public bool _mustBringCoinOut = false;
    public bool _mustCutToEnterHome = false;

    [Header("Optional Rules")]
    public bool _barrier = false;

    [Header("Additional Rules")]
    public bool _oneAlsoGiveTurn = false;
    public bool _oneAlsoBringCoinOut = false;
    public bool _threeSixCut = false;
    public bool _threeSixSkip = false;
    public bool _threeOneStart = false;

    [Header("Gameplay Settings")]
    public bool _flingEnable = true;
    public int _difficultyLevel = 1;
    public int _coinNumber = 4;
    public bool _continueroll = false;

    [Header("Three Times Rules")]
    public bool _start3TimesSkip = false;
    public bool _other3TimesSkip = false;
    public bool _start3TimesCut = false;


    public static void LoadAllOptions()
    {
        // Game mode settings
        startatoneorsix = PlayerPrefs.GetInt("OPT_StartMode", 1) == 6;
        sixgiveanotherturn = PlayerPrefs.GetInt("OPT_SixTurn", 1) == 1;
        sixalsobringcoinout = PlayerPrefs.GetInt("OPT_SixBringOut", 0) == 1;
        sixtostart = PlayerPrefs.GetInt("OPT_SixStart", 0) == 1;

        // Rule settings
        threeonecut = PlayerPrefs.GetInt("OPT_3OneCut", 1) == 1;
        threeoneskip = PlayerPrefs.GetInt("OPT_3OneSkip", 0) == 1;
        threesixstart = PlayerPrefs.GetInt("OPT_3SixStart", 1) == 1;
        showstar = PlayerPrefs.GetInt("OPT_ShowStars", 0) == 1;
        cutgainturn = PlayerPrefs.GetInt("OPT_CutTurn", 0) == 1;
        homegainturn = PlayerPrefs.GetInt("OPT_HomeTurn", 1) == 1;

        // Must rules
        mustcuttable = PlayerPrefs.GetInt("OPT_MustCutTable", 0) == 1;
        mustbringcoinout = PlayerPrefs.GetInt("OPT_MustBringOut", 0) == 1;
        barrier = PlayerPrefs.GetInt("OPT_BarrierRule", 0) == 1;
        mustcuttoenterhome = PlayerPrefs.GetInt("OPT_CutToEnter", 0) == 1;

        // Additional rules
        onealsogiveturn = PlayerPrefs.GetInt("OPT_OneTurn", 0) == 1;
        onealsobringcoinout = PlayerPrefs.GetInt("OPT_OneBringOut", 0) == 1;
        threesixcut = PlayerPrefs.GetInt("OPT_3SixCut", 0) == 1;
        threesixskip = PlayerPrefs.GetInt("OPT_3SixSkip", 0) == 1;
        threeonestart = PlayerPrefs.GetInt("OPT_3OneStart", 0) == 1;

        // Gameplay settings
        flingenable = PlayerPrefs.GetInt("OPT_Fling", 1) == 1;
        difficultylevel = PlayerPrefs.GetInt("OPT_Difficulty", 1);
        coinnumber = PlayerPrefs.GetInt("OPT_CoinCount", 4);
        continueroll = PlayerPrefs.GetInt("OPT_Continue", 1) == 1;

        // Three times rules
        start3timesskip = PlayerPrefs.GetInt("OPT_3StartSkip", 0) == 1;
        other3timesskip = PlayerPrefs.GetInt("OPT_3OtherSkip", 0) == 1;
        start3timescut = PlayerPrefs.GetInt("OPT_3StartCut", 0) == 1;





        PlayerPrefs.SetInt("OneOrSix", startatoneorsix ? 6 : 1);
        PlayerPrefs.SetInt("SixTurn", sixgiveanotherturn ? 1 : 0);
        PlayerPrefs.SetInt("SixOut", sixalsobringcoinout ? 1 : 0);
        //PlayerPrefs.SetInt("OPT_SixStart", sixtostart ? 1 : 0);

        // Rule settings
        PlayerPrefs.SetInt("3onecut", threeonecut ? 1 : 0);
        PlayerPrefs.SetInt("3oneskip", threeoneskip ? 1 : 0);
        PlayerPrefs.SetInt("3sixout", threesixstart ? 1 : 0);
        PlayerPrefs.SetInt("ShowStar", showstar ? 1 : 0);
        PlayerPrefs.SetInt("cutturn", cutgainturn ? 1 : 0);
        PlayerPrefs.SetInt("hometurn", homegainturn ? 1 : 0);

        // Must rules
        PlayerPrefs.SetInt("Mustcutcuttable", mustcuttable ? 1 : 0);
        PlayerPrefs.SetInt("Mustout", mustbringcoinout ? 1 : 0);
        PlayerPrefs.SetInt("2coinbarrier", barrier ? 1 : 0);
        PlayerPrefs.SetInt("Mustcuthome", mustcuttoenterhome ? 1 : 0);

        // Additional rules
        PlayerPrefs.SetInt("OneTurn", onealsogiveturn ? 1 : 0);
        PlayerPrefs.SetInt("OneOut", onealsobringcoinout ? 1 : 0);
        PlayerPrefs.SetInt("3sixcut", threesixcut ? 1 : 0);
        PlayerPrefs.SetInt("3sixskip", threesixskip ? 1 : 0);
        PlayerPrefs.SetInt("3oneout", threeonestart ? 1 : 0);

        // Gameplay settings
        PlayerPrefs.SetInt("Fling", flingenable ? 1 : 0);
        PlayerPrefs.SetInt("Difflevel", difficultylevel);
        PlayerPrefs.SetInt("CoinNumber", coinnumber);
        PlayerPrefs.SetInt("Continueroll", continueroll ? 1 : 0);

        // Three times rules
        //PlayerPrefs.SetInt("OPT_3StartSkip", start3timesskip ? 1 : 0);
        //PlayerPrefs.SetInt("OPT_3OtherSkip", other3timesskip ? 1 : 0);
        //PlayerPrefs.SetInt("OPT_3StartCut", start3timescut ? 1 : 0);
        // System
        //loadingenable = PlayerPrefs.GetInt("OPT_Loading", 0) == 1;
    }
    public static void SaveAllOptions()
    {
        // Game mode settings
        PlayerPrefs.SetInt("OPT_StartMode", startatoneorsix ? 6 : 1);
        PlayerPrefs.SetInt("OPT_SixTurn", sixgiveanotherturn ? 1 : 0);
        PlayerPrefs.SetInt("OPT_SixBringOut", sixalsobringcoinout ? 1 : 0);
        PlayerPrefs.SetInt("OPT_SixStart", sixtostart ? 1 : 0);

        // Rule settings
        PlayerPrefs.SetInt("OPT_3OneCut", threeonecut ? 1 : 0);
        PlayerPrefs.SetInt("OPT_3OneSkip", threeoneskip ? 1 : 0);
        PlayerPrefs.SetInt("OPT_3SixStart", threesixstart ? 1 : 0);
        PlayerPrefs.SetInt("OPT_ShowStars", showstar ? 1 : 0);
        PlayerPrefs.SetInt("OPT_CutTurn", cutgainturn ? 1 : 0);
        PlayerPrefs.SetInt("OPT_HomeTurn", homegainturn ? 1 : 0);

        // Must rules
        PlayerPrefs.SetInt("OPT_MustCutTable", mustcuttable ? 1 : 0);
        PlayerPrefs.SetInt("OPT_MustBringOut", mustbringcoinout ? 1 : 0);
        PlayerPrefs.SetInt("OPT_BarrierRule", barrier ? 1 : 0);
        PlayerPrefs.SetInt("OPT_CutToEnter", mustcuttoenterhome ? 1 : 0);

        // Additional rules
        PlayerPrefs.SetInt("OPT_OneTurn", onealsogiveturn ? 1 : 0);
        PlayerPrefs.SetInt("OPT_OneBringOut", onealsobringcoinout ? 1 : 0);
        PlayerPrefs.SetInt("OPT_3SixCut", threesixcut ? 1 : 0);
        PlayerPrefs.SetInt("OPT_3SixSkip", threesixskip ? 1 : 0);
        PlayerPrefs.SetInt("OPT_3OneStart", threeonestart ? 1 : 0);

        // Gameplay settings
        PlayerPrefs.SetInt("OPT_Fling", flingenable ? 1 : 0);
        PlayerPrefs.SetInt("OPT_Difficulty", difficultylevel);
        PlayerPrefs.SetInt("OPT_CoinCount", coinnumber);
        PlayerPrefs.SetInt("OPT_Continue", continueroll ? 1 : 0);

        // Three times rules
        PlayerPrefs.SetInt("OPT_3StartSkip", start3timesskip ? 1 : 0);
        PlayerPrefs.SetInt("OPT_3OtherSkip", other3timesskip ? 1 : 0);
        PlayerPrefs.SetInt("OPT_3StartCut", start3timescut ? 1 : 0);

        // System
        //PlayerPrefs.SetInt("OPT_Loading", loadingenable ? 1 : 0);

        PlayerPrefs.Save();
    }
    // In Awake(), assign instance values to the static ones
    void assigning()
    {
        _startAtOneOrSix = startatoneorsix;
        _sixGiveAnotherTurn = sixgiveanotherturn;
        _sixAlsoBringCoinOut = sixalsobringcoinout;
        _sixToStart = sixtostart;
        _threeOneCut = threeonecut;
        _threeOneSkip = threeoneskip;
        _threeSixStart = threesixstart;
        _showStar = showstar;
        _cutGainTurn = cutgainturn;
        _homeGainTurn = homegainturn;
        _mustCutTable = mustcuttable;
        _mustBringCoinOut = mustbringcoinout;
        _barrier = barrier;
        _mustCutToEnterHome = mustcuttoenterhome;
        _oneAlsoGiveTurn = onealsogiveturn;
        _oneAlsoBringCoinOut = onealsobringcoinout;
        _threeSixCut = threesixcut;
        _threeSixSkip = threesixskip;
        _threeOneStart = threeonestart;
        _flingEnable = flingenable;
        _difficultyLevel = difficultylevel;
        _coinNumber = coinnumber;
        _start3TimesSkip = start3timesskip;
        _other3TimesSkip = other3timesskip;
        _start3TimesCut = start3timescut;
        _continueroll = continueroll;
    }


    public static bool saveenable = false;
    public void start3timeupdate()
    {
        if(startatoneorsix == false)
        {
            if(threeonecut == true)
            {
                start3timescut = true;
            }
            else
            {
                start3timescut=false;
            }

        }
        else if(startatoneorsix == true)
        {
            if (threesixcut == true)
            {
                start3timescut = true;
            }
            else
            {
                start3timescut = false;
            }

        }

        if (PlayerPrefs.HasKey("Continueroll"))
        {
            if (PlayerPrefs.GetInt("Continueroll") == 1)
            {
                continueroll = true;
            }
            else
            {
                continueroll=false;
            }
        }


    }

    private void Awake()
    {
        if(PlayerPrefs.HasKey("LoadingEnable"))
            if(PlayerPrefs.GetFloat("LoadingEnable") == 1)
            {
                loadingenable = true;
            }
            else
            {
                loadingenable = false;
            }

        if (PlayerPrefs.HasKey("SaveEnable"))
            if (PlayerPrefs.GetFloat("SaveEnable") == 1)
            {
                saveenable = true;
            }
            else
            {
                saveenable = false;
            }
        prefupdate();
        start3timeupdate();
        showingstar();
    }
    void showingstar()
    {

        if (!showstar)
        {
            if(starobject.Count > 0)
            {
                foreach(GameObject gm in starobject)
                {
                    gm.SetActive(false);
                }

            }
        }
        else
        {
            if (starobject.Count > 0)
            {
                foreach (GameObject gm in starobject)
                {
                    gm.SetActive(true);
                }

            }
        }
    }
    private void prefupdate()
    {

        if(PlayerPrefs.HasKey("SixTurn"))
        {
            int forturn = PlayerPrefs.GetInt("SixTurn");
            if(forturn == 1)
            {
                sixgiveanotherturn =true;
            }
            else if(forturn == 0)
            {
                sixgiveanotherturn=false;
            }
        }

        if(PlayerPrefs.HasKey("OneTurn"))
        {
            int forturn = PlayerPrefs.GetInt("OneTurn");
            if (forturn == 1)
            {
                onealsogiveturn = true;
            }
            else if (forturn == 0)
            {
                onealsogiveturn = false;
            }
        }

        if(PlayerPrefs.HasKey("OneOut"))
        {
            int forturn = PlayerPrefs.GetInt("OneOut");

            if (forturn == 1)
            {
                onealsobringcoinout = true;
            }
            else if (forturn == 0)
            {
                onealsobringcoinout = false;
            }

        }


        if (PlayerPrefs.HasKey("SixOut"))
        {
            int forturn = PlayerPrefs.GetInt("SixOut");

            if (forturn == 1)
            {
                sixalsobringcoinout = true;
            }
            else if (forturn == 0)
            {
                sixalsobringcoinout = false;
            }

        }

        if (PlayerPrefs.HasKey("ShowStar"))
        {
            int forturn = PlayerPrefs.GetInt("ShowStar");

            if (forturn == 1)
            {
                showstar = true;
            }
            else if (forturn == 0)
            {
                showstar = false;
            }

        }

        if (PlayerPrefs.HasKey("3onecut"))
        {
            int forturn = PlayerPrefs.GetInt("3onecut");

            if (forturn == 1)
            {
                threeonecut = true;
            }
            else if (forturn == 0)
            {
                threeonecut = false;
            }

        }

        if (PlayerPrefs.HasKey("3sixout"))
        {
            int forturn = PlayerPrefs.GetInt("3sixout");

            if (forturn == 1)
            {
               threesixstart = true;
            }
            else if (forturn == 0)
            {
                threesixstart = false;
            }

        }

        if (PlayerPrefs.HasKey("3oneout"))
        {
            int forturn = PlayerPrefs.GetInt("3oneout");

            if (forturn == 1)
            {
                threeonestart = true;
            }
            else if (forturn == 0)
            {
                threeonestart = false;
            }

        }

        if (PlayerPrefs.HasKey("3sixcut"))
        {
            int forturn = PlayerPrefs.GetInt("3sixcut");

            if (forturn == 1)
            {
                threesixcut = true;
            }
            else if (forturn == 0)
            {
                threesixcut = false;
            }

        }

        if (PlayerPrefs.HasKey("3oneskip"))
        {
            int forturn = PlayerPrefs.GetInt("3oneskip");

            if (forturn == 1)
            {
                threeoneskip = true;
            }
            else if (forturn == 0)
            {
                threeoneskip = false;
            }

        }

        if (PlayerPrefs.HasKey("3sixskip"))
        {
            int forturn = PlayerPrefs.GetInt("3sixskip");

            if (forturn == 1)
            {
                threesixskip = true;
            }
            else if (forturn == 0)
            {
                threesixskip = false;
            }

        }

        if (PlayerPrefs.HasKey("cutturn"))
        {
            int forturn = PlayerPrefs.GetInt("cutturn");

            if (forturn == 1)
            {
                cutgainturn = true;
            }
            else if (forturn == 0)
            {
                cutgainturn = false;
            }

        }

        if (PlayerPrefs.HasKey("hometurn"))
        {
            int forturn = PlayerPrefs.GetInt("hometurn");

            if (forturn == 1)
            {
                homegainturn = true;
            }
            else if (forturn == 0)
            {
                homegainturn = false;
            }

        }

        if (PlayerPrefs.HasKey("Mustcuthome"))
        {
            int forturn = PlayerPrefs.GetInt("Mustcuthome");

            if (forturn == 1)
            {
                mustcuttoenterhome = true;
            }
            else if (forturn == 0)
            {
                mustcuttoenterhome = false;
            }

        }

        if (PlayerPrefs.HasKey("Mustcutcuttable"))
        {
            int forturn = PlayerPrefs.GetInt("Mustcutcuttable");

            if (forturn == 1)
            {
                mustcuttable = true;
            }
            else if (forturn == 0)
            {
                mustcuttable = false;
            }

        }

        if (PlayerPrefs.HasKey("Mustout"))
        {
            int forturn = PlayerPrefs.GetInt("Mustout");

            if (forturn == 1)
            {
                mustbringcoinout = true;
            }
            else if (forturn == 0)
            {
                mustbringcoinout = false;
            }

        }

        if (PlayerPrefs.HasKey("2coinbarrier"))
        {
            int forturn = PlayerPrefs.GetInt("2coinbarrier");

            if (forturn == 1)
            {
                barrier = true;
            }
            else if (forturn == 0)
            {
                barrier = false;
            }

        }
        if (PlayerPrefs.HasKey("OneOrSix"))
        {
            int onesixcheck = PlayerPrefs.GetInt("OneOrSix");
            if (onesixcheck == 1)
            {
                startatoneorsix = false;
                onealsogiveturn = false;
                onealsobringcoinout=false;
                threesixcut = false;
                //threeoneskip = false;
                threesixskip = false;
                threeonestart = false;

            }
            else if (onesixcheck == 6)
            {
                startatoneorsix = true;
                sixgiveanotherturn = false;
                sixalsobringcoinout = false;
                threeoneskip = false;
                threeonecut = false;
                threesixstart = false;
            }
        }
    }

    public LudoDice2D dicesystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        assigning();


        if(loadingenable)
        {
            LoadAllOptions();
            dicesystem.playerAIorhumandecide();
        }
    }

    int counter = 0;
    // Update is called once per frame
    void Update()
    {
        if(counter == 0)
        {
            counter++;
            loadingenable = true;
            PlayerPrefs.SetFloat("LoadingEnable", 1);
            PlayerPrefs.SetFloat("SaveEnable", 1);
        }
    }
}
