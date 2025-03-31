using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using TMPro;
using System.Net.NetworkInformation;
using UnityEngine.LowLevel;
using System.Linq;
using System;

public class PieceManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LudoDice2D diceSystem;
    [SerializeField] private BoardPath boardPath;

    [Header("Player Pieces")]
    public GameObject greenpiece, bluepiece, redpiece, yellowpiece;
    [SerializeField] private List<Piece> player1Pieces;
    [SerializeField] private List<Piece> player2Pieces;
    [SerializeField] private List<Piece> player3Pieces;
    [SerializeField] private List<Piece> player4Pieces;

    public static List<Piece> p1piece;
    public static List<Piece> p2piece;
    public static List<Piece> p3piece;
    public static List<Piece> p4piece;
    [Header("Selection UI")]
    [SerializeField] private GameObject selectionPrompt;
    [SerializeField] private float moveDuration = 0.5f;

    public List<Piece> movablePieces = new List<Piece>();
    private bool isWaitingForSelection = false;
    private int currentDiceValue = 0;

    public BoxCollider2D dicecollider;

    public static bool selectedpiecemove = false;
    public int dicenum = 0;

    public int oneorsix = 1;
    public int colorchoose = 0;

    public static bool AIenable = false;


    public  bool isAI = false;
    public bool AIroll = false;
    public static Piece lastmovepiece;

    public List<Piece> currentPlayerPieces = new List<Piece>();

    public Dictionary<int, List<GameObject>> samePosDictionary = new Dictionary<int, List<GameObject>>();
    public List<GameObject> playerList = new List<GameObject>();
    public int currpos;

    public static int rednum, greennum, bluenum , yellownum;
    public int secondoneorsix;
    bool forceturnchange = false;



    public void SaveGameState()
    {
        // Basic variables
        PlayerPrefs.SetInt("currentDiceValue", currentDiceValue);
        PlayerPrefs.SetInt("dicenum", dicenum);
        PlayerPrefs.SetInt("colorchoose", colorchoose);
        PlayerPrefs.SetInt("dicecontinuerollcount", dicecontinuerollcount);
        PlayerPrefs.SetInt("totalplayer", totalplayer);
        PlayerPrefs.SetInt("oneorsix", oneorsix);
        PlayerPrefs.SetInt("secondoneorsix", secondoneorsix);

        // Booleans
        PlayerPrefs.SetInt("isAI", isAI ? 1 : 0);
        PlayerPrefs.SetInt("AIenable", AIenable ? 1 : 0);
        PlayerPrefs.SetInt("selectedpiecemove", selectedpiecemove ? 1 : 0);
        PlayerPrefs.SetInt("threeconsecutivecut", threeconsecutivecut ? 1 : 0);
        PlayerPrefs.SetInt("coinoutboolenable", coinoutboolenable ? 1 : 0);
        PlayerPrefs.SetInt("isWaitingForSelection", isWaitingForSelection ? 1 : 0);
        PlayerPrefs.SetInt("forceturnchange", forceturnchange ? 1 : 0);

        // Dice collider state
        PlayerPrefs.SetInt("dicecolliderEnabled", dicecollider.enabled ? 1 : 0);

        // Lists
        SaveList(storedicevalue, "storedicevalue");
        SavePieceList(movablePieces, "movablePieces");

        // Dictionary
        List<string> dictEntries = new List<string>();
        foreach (var pair in samePosDictionary)
        {
            string entry = pair.Key + ":";
            entry += string.Join("|", pair.Value.Select(p => p.GetComponent<Piece>().UniqueID));
            dictEntries.Add(entry);
        }
        PlayerPrefs.SetString("samePosDictionary", string.Join(";", dictEntries));

        // Player pieces
        SavePlayerPieces(player1Pieces, "Green");
        SavePlayerPieces(player2Pieces, "Yellow");
        SavePlayerPieces(player3Pieces, "Blue");
        SavePlayerPieces(player4Pieces, "Red");

        // Rankings
        PlayerPrefs.SetInt("FirstRank", firstranked);
        PlayerPrefs.SetInt("SecondRank", secondranked);
        PlayerPrefs.SetInt("ThirdRank", thirdranked);
        PlayerPrefs.SetInt("FourthRank", fourthranked);

        PlayerPrefs.Save();
    }

    void SaveList<T>(List<T> list, string key)
    {
        PlayerPrefs.SetString(key, string.Join(",", list));
    }

    void SavePieceList(List<Piece> pieces, string key)
    {
        PlayerPrefs.SetString(key, string.Join(",", pieces.Select(p => p.UniqueID)));
    }

    void SavePlayerPieces(List<Piece> pieces, string color)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            Piece p = pieces[i];
            string prefix = $"{color}_Piece_{i}";
            p.SaveGame();
            PlayerPrefs.SetInt($"{prefix}_Position", p.CurrentPosition);
            PlayerPrefs.SetInt($"{prefix}_InBase", p.IsInBase ? 1 : 0);
            PlayerPrefs.SetInt($"{prefix}_InHome", p.IsInHome ? 1 : 0);
            PlayerPrefs.SetInt($"{prefix}_Barrier", p.barrieron ? 1 : 0);
            PlayerPrefs.SetInt($"{prefix}_AlreadySelected", Piece.alreadyselected ? 1 : 0);
            PlayerPrefs.SetInt($"{prefix}_Colornum", p.colornum);
            PlayerPrefs.SetInt($"{prefix}_Piecenum", p.piecenumber);
        }
    }

    public void LoadGameState()
    {
        // Basic variables
        currentDiceValue = PlayerPrefs.GetInt("currentDiceValue", 0);
        dicenum = PlayerPrefs.GetInt("dicenum", 0);
        colorchoose = PlayerPrefs.GetInt("colorchoose", 0);
        dicecontinuerollcount = PlayerPrefs.GetInt("dicecontinuerollcount", 0);
        totalplayer = PlayerPrefs.GetInt("totalplayer", 4);
        oneorsix = PlayerPrefs.GetInt("oneorsix", 1);
        secondoneorsix = PlayerPrefs.GetInt("secondoneorsix", 6);

        // Booleans
        isAI = PlayerPrefs.GetInt("isAI", 0) == 1;
        AIenable = PlayerPrefs.GetInt("AIenable", 0) == 1;
        selectedpiecemove = PlayerPrefs.GetInt("selectedpiecemove", 0) == 1;
        threeconsecutivecut = PlayerPrefs.GetInt("threeconsecutivecut", 0) == 1;
        coinoutboolenable = PlayerPrefs.GetInt("coinoutboolenable", 0) == 1;
        isWaitingForSelection = PlayerPrefs.GetInt("isWaitingForSelection", 0) == 1;
        forceturnchange = PlayerPrefs.GetInt("forceturnchange", 0) == 1;

        // Dice collider
        dicecollider.enabled = PlayerPrefs.GetInt("dicecolliderEnabled", 1) == 1;

        
        // Lists
        storedicevalue = LoadList<int>("storedicevalue");
        movablePieces = LoadPieceList("movablePieces");

        // Dictionary
        samePosDictionary.Clear();
        string dictData = PlayerPrefs.GetString("samePosDictionary", "");
        if (!string.IsNullOrEmpty(dictData))
        {
            foreach (string entry in dictData.Split(';'))
            {
                string[] parts = entry.Split(':');
                if (parts.Length == 2)
                {
                    int key = int.Parse(parts[0]);
                    List<GameObject> value = parts[1].Split('|')
                        .Select(FindPieceByID).Where(p => p != null).ToList();
                    samePosDictionary[key] = value;
                }
            }
        }

        // Player pieces
        LoadPlayerPieces(player1Pieces, "Green");
        LoadPlayerPieces(player2Pieces, "Yellow");
        LoadPlayerPieces(player3Pieces, "Blue");
        LoadPlayerPieces(player4Pieces, "Red");

        // Rankings
        firstranked = PlayerPrefs.GetInt("FirstRank", 1000);
        secondranked = PlayerPrefs.GetInt("SecondRank", 1000);
        thirdranked = PlayerPrefs.GetInt("ThirdRank", 1000);
        fourthranked = PlayerPrefs.GetInt("FourthRank", 1000);

        // Rebuild game state
        StartCoroutine(RebuildGameState());
    }

    List<T> LoadList<T>(string key)
    {
        string data = PlayerPrefs.GetString(key, "");
        return string.IsNullOrEmpty(data) ? new List<T>() :
            data.Split(',').Select(s => (T)Convert.ChangeType(s, typeof(T))).ToList();
    }

    List<Piece> LoadPieceList(string key)
    {
        string data = PlayerPrefs.GetString(key, "");
        return string.IsNullOrEmpty(data) ? new List<Piece>() :
            data.Split(',').Select(FindPieceByID).Where(p => p != null)
                .Select(p => p.GetComponent<Piece>()).ToList();
    }

    void LoadPlayerPieces(List<Piece> pieces, string color)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            string prefix = $"{color}_Piece_{i}";
            Piece p = pieces[i];
            p.LoadGame();
            p.CurrentPosition = PlayerPrefs.GetInt($"{prefix}_Position", -1);
            p.IsInBase = PlayerPrefs.GetInt($"{prefix}_InBase", 1) == 1;
            p.IsInHome = PlayerPrefs.GetInt($"{prefix}_InHome", 0) == 1;
            p.barrieron = PlayerPrefs.GetInt($"{prefix}_Barrier", 0) == 1;
            p.colornum = PlayerPrefs.GetInt($"{prefix}_Colornum", 0);
            p.piecenumber = PlayerPrefs.GetInt($"{prefix}_Piecenum", 0);
            Piece.alreadyselected = PlayerPrefs.GetInt($"{prefix}_AlreadySelected", 0) == 1;

            // Update visual state
            //if (!p.IsInBase && !p.IsInHome)
            //{
            //    boardPath.UpdatePiecePosition(p, p.CurrentPosition);
            //}
        }
    }

    IEnumerator RebuildGameState()
    {
        yield return null; // Wait one frame

        // Recalculate movable pieces
        movablePieces = GetMovablePieces(
            GetPlayerPieces(colorchoose),
            currentDiceValue
        );

        if(dicecollider.enabled == false)
        {
            pieceanim(true, movablePieces);  
        }

        // Update dice system state
        //diceSystem.currentPlayerIndex = colorchoose;
        //diceSystem.changedicecoloronturn(currentDiceValue);

        // Update AI state if needed
        //if (isAI) diceSystem.AIsubfunc(diceSystem.noofAI, false);
    }

    // Helper method to find pieces by ID
    GameObject FindPieceByID(string id)
    {
        string[] parts = id.Split('-');
        if (parts.Length != 2) return null;

        int color = int.Parse(parts[0]);
        int number = int.Parse(parts[1]);

        return GetPlayerPieces(color)
            .FirstOrDefault(p => p.piecenumber == number)?.gameObject;
    }

    void movingspeed()
    {
        moveDuration = 0.2f;
        if(PlayerPrefs.HasKey("SliderValue"))
        {
            float svalue = PlayerPrefs.GetFloat("SliderValue");
            moveDuration = 0.25f - svalue/10;
        }
    }

    void checkallinbase()
    {
        foreach(Piece mpiece in movablePieces)
        {
            if(!mpiece.IsInBase)
            {
                return;
            }
        }
        //Piece.selectedpiece = movablePieces[Random.Range(0, movablePieces.Count)].gameObject;
        Piece.selectedpiece = movablePieces[UnityEngine.Random.Range(0, movablePieces.Count)].gameObject;
        Debug.Log(Piece.selectedpiece.name);
        Piece.alreadyselected = true;

    }

    int totalplayer;
    private void Start()
    {
        movingspeed();
        rednum = 0; greennum = 0; bluenum = 0; yellownum = 0;
        if(greenpiece != null)
        {
            foreach(Transform children in greenpiece.transform)
            {

                if(greennum < optionscript.coinnumber)
                {
                    player1Pieces.Add(children.gameObject.GetComponent<Piece>());
                    greennum++;
                }
                else
                {
                    children.gameObject.SetActive(false);
                }

            }
        }
        if (yellowpiece != null)
        {
            foreach (Transform children in yellowpiece.transform)
            {

                if (yellownum < optionscript.coinnumber)
                {
                    player2Pieces.Add(children.gameObject.GetComponent<Piece>());
                    yellownum++;

                }
                else
                {
                    children.gameObject.SetActive(false);
                }
            }
        }
        if (bluepiece != null)
        {
            foreach (Transform children in bluepiece.transform)
            {
                if (bluenum < optionscript.coinnumber)
                {
                    player3Pieces.Add(children.gameObject.GetComponent<Piece>());
                    bluenum++;
                }
                else
                {
                    children.gameObject.SetActive(false);
                }
            }
        }
        if (redpiece != null)
        {
            foreach (Transform children in redpiece.transform)
            {

                if (rednum < optionscript.coinnumber)
                {
                    player4Pieces.Add(children.gameObject.GetComponent<Piece>());
                    rednum++;
                }
                else
                {
                    children.gameObject.SetActive(false);
                }
            }
        }



        if (optionscript.startatoneorsix)
        {
            oneorsix = 6;
            secondoneorsix = 1;
        }
        else
        {
            oneorsix = 1;
            secondoneorsix = 6;
        }


        p1piece = player1Pieces;
        p2piece = player2Pieces;
        p3piece = player3Pieces;
        p4piece = player4Pieces;

        totalplayer = LudoDice2D.playernum;
        pl1 = LudoDice2D.player1;
        pl2 = LudoDice2D.player2;
        pl3 = LudoDice2D.player3;
        pl4 = LudoDice2D.player4;


        if(optionscript.loadingenable && optionscript.saveenable)
        LoadGameState();
    }

    int pl1, pl2, pl3, pl4;

    //void dictioinaryshow()
    //{
    //    foreach(GameObject gm in Piece.samePosDictionary[Piece.Currentposit])
    //}
    static int firstranked = 1000;
    static int secondranked = 1000;
    static int thirdranked = 1000;
    static int fourthranked = 1000;


    public GameObject gameoverUI;
    public static void onpiecehomecomplete(List<Piece> homeps)
    {
        Debug.Log("Colornum:" + homeps[0].colornum +":AINUM:" + LudoDice2D.AIstaticnum + ":" + LudoDice2D.AIstaticnum1 + ":" + LudoDice2D.AIstaticnum2 + ":" + LudoDice2D.AIstaticnum3);
        homefunc(homeps[0].colornum);
        Debug.Log("Colornum:" + homeps[0].colornum +":AINUM:" + LudoDice2D.AIstaticnum + ":" + LudoDice2D.AIstaticnum1 + ":" + LudoDice2D.AIstaticnum2 + ":" + LudoDice2D.AIstaticnum3);
        ainumcheck(homeps[0].colornum);

        LudoDice2D.playernum--;
        if (firstranked == 1000)
        {
            firstranked = homeps[0].colornum;
   
        }
        else if(secondranked == 1000)
        {
            secondranked = homeps[0].colornum;

        }
        else if(thirdranked == 1000)
        {
            thirdranked = homeps[0].colornum;

        }
        else if(fourthranked == 1000)
        {
            fourthranked = homeps[0].colornum;
  
        }
        Debug.Log("First:" + firstranked + ":Second:" + secondranked + ":Third:" + thirdranked + ":Fourth:" + fourthranked);
        Debug.Log("PlayerNUM:" + LudoDice2D.playernum + ":" + LudoDice2D.player1 + ":" + LudoDice2D.player2 + ":" + LudoDice2D.player3 + ":" + LudoDice2D.player4);
        Debug.Log("AINUM:" + LudoDice2D.AIstaticnum + ":" + LudoDice2D.AIstaticnum1 + ":" + LudoDice2D.AIstaticnum2 + ":" + LudoDice2D.AIstaticnum3);
    }

    //turn issue..................................................................................................
    static void ainumcheck(int conum)
    {
        if(conum == LudoDice2D.AIstaticnum1)
        {
            LudoDice2D.AIstaticnum1 = LudoDice2D.AIstaticnum2;
            LudoDice2D.AIstaticnum2 = LudoDice2D.AIstaticnum3;
            LudoDice2D.AIstaticnum--;
        }
        else if(conum == LudoDice2D.AIstaticnum2)
        {
            //LudoDice2D.AIstaticnum1 = LudoDice2D.AIstaticnum2;
            LudoDice2D.AIstaticnum2 = LudoDice2D.AIstaticnum3;
            LudoDice2D.AIstaticnum--;
        }
        else if (conum == LudoDice2D.AIstaticnum3)
        {
            LudoDice2D.AIstaticnum--;
        }
    }
    static void homefunc(int conum)
    {
        if(conum == LudoDice2D.player1)
        {
            LudoDice2D.player1 = LudoDice2D.player2;
            LudoDice2D.player2 = LudoDice2D.player3;
            LudoDice2D.player3 = LudoDice2D.player4;
        }
        else if (conum == LudoDice2D.player2)
        {
            //LudoDice2D.player1 = LudoDice2D.player2;
            LudoDice2D.player2 = LudoDice2D.player3;
            LudoDice2D.player3 = LudoDice2D.player4;
        }
        else if (conum == LudoDice2D.player3)
        {
            //LudoDice2D.player1 = LudoDice2D.player2;
            //LudoDice2D.player2 = LudoDice2D.player3;
            LudoDice2D.player3 = LudoDice2D.player4;
        }
    }

    void OnEnable()
    {
        LudoDice2D.OnDiceRollCompleted += HandleDiceResult;
    }

    void OnDisable()
    {
        LudoDice2D.OnDiceRollCompleted -= HandleDiceResult;
    }
    public bool threeconsecutivecut = false;

    IEnumerator cutpiecefrom3oneorsix()
    {
        if(lastmovepiece.barrieron)
        {
            lastmovepiece.barrierbreak(false, lastmovepiece);
        }
        StartCoroutine(boardPath.movebacktohome(lastmovepiece, lastmovepiece.CurrentPosition, 0, 0.1f));
        //lastmovepiece.cutpiece(lastmovepiece.colornum, lastmovepiece.piecenumber, lastmovepiece.gameObject);
        lastmovepiece.IsInBase = true;
        yield return new WaitForSeconds(0.2f);

        lastmovepiece.CurrentPosition = -1;
        threeconsecutivecut = false;

        //diceSystem.checkformovingturn(true);
        //diceSystem.checkingturnandskip();
        StartCoroutine(delayfor5(0.5f, 2));

    }
    void HandleDiceResult(int playerIndex, int diceValue)
    {
        if(LudoDice2D.threeoneorsix)
        {
           if(optionscript.threeonecut || optionscript.threesixcut)
            {
                if(lastmovepiece != null)
                {
                    threeconsecutivecut = true;
                    //StartCoroutine(MovePiece(lastmovepiece));
                    if (threeconsecutivecut)
                    {
                        lastmovepiece.lastposition = lastmovepiece.CurrentPosition;
                        lastmovepiece.beforecurrrentposition = -1;
                        lastmovepiece.wasjustmoving = true;

                        StartCoroutine(cutpiecefrom3oneorsix());

                    }

                }

                else
                StartCoroutine(delayfor5(1.5f, 1));
                //diceSystem.EndTurn();
            }
           else
            {
                handlingpiece(playerIndex, diceValue);
            }
        }
        else
        {
            handlingpiece(playerIndex, diceValue);

        }
    }


    public List<Piece> tocheckmovable = new List<Piece>();
    bool coinoutboolenable = false;
    void handlingpiece(int playerIndex, int diceValue)
    {
        currentDiceValue = diceValue;
        //currentPlayerPieces.Clear();
        Debug.Log("Getplayerpiece");
        currentPlayerPieces = GetPlayerPieces(playerIndex);

        // Find movable pieces
        movablePieces = GetMovablePieces(currentPlayerPieces, diceValue);
        Debug.Log("GetMovablepieceinpiecemanager");
        tocheckmovable = movablePieces;

        notouchinputforotherpiece(playerIndex);

        if (movablePieces.Count == 0)
        {
            // No movable pieces - end turn
            //diceSystem.EndTurn();
            if (lastmovepiece != null)
            {
                lastmovepiece.lastposition = lastmovepiece.CurrentPosition;
                lastmovepiece.wasjustmoving = false;

            }
            foreach(Piece piece in currentPlayerPieces)
            {
                if(piece.CurrentPosition != -1)
                {
                    lastmovepiece = piece;
                    break;
                }
            }
            StartCoroutine(delayfor5(0.5f, 1));
            Debug.Log("No move");

            AIenable = true;
        }
        else if (movablePieces.Count == 1)
        {
            AIenable = false;
            // Auto-move single piece
            StartCoroutine(MovePiece(movablePieces[0]));
            Debug.Log("1 move");

            AIenable = true;
        }
        else
        {
            // Multiple pieces - enable selection
            //EnablePieceSelection(movablePieces);
            AIenable=false;
            diceSystem.collisioncheckbool = true;

            //foranim(playerIndex);
            pieceanim(true, movablePieces);
            dicecollider.enabled = false;
            selectedpiecemove = true;
            colorchoose = playerIndex;
            Debug.Log("ColorChoose:"+colorchoose);
            dicenum = diceValue;
            AIroll = true;
            AIpiecemove();

            if (optionscript.mustbringcoinout)
            {
                if (mustbringoutcheck(movablePieces))
                {
                        if (piecegm.gameObject != null)
                        {
                            Piece.selectedpiece = piecegm.gameObject;
                            Piece.alreadyselected = true;
                            coinoutboolenable = true;
                        }
                }
            }
            checkallinbase();
        }
    }

    public static bool isitreallyAI = false;
    public void AIpiecemove()
    {
        if (LudoDice2D.isAIturn)
        {
            if (AIroll)
            {
                //if(diceSystem.currentPlayerIndex == diceSystem.AIplayernum)
                //{

                //}
                Debug.Log(1);
                Debug.Log("ColorChoose:" + colorchoose + ":AIjustnumber:" + LudoDice2D.AIforPieceManagerNumber + ":DiceNum:" + dicenum);
                if (colorchoose == LudoDice2D.AIforPieceManagerNumber)
                {
                    Debug.Log(12);
                    if (movablePieces.Count > 0)
                    {
                        Debug.Log(123);
                        if(optionscript.difficultylevel == 1)
                        {
                            profitcalculcation(false);
                        }

                        if(Piece.selectedpiece == null)
                        Piece.selectedpiece = movablePieces[UnityEngine.Random.Range(0, movablePieces.Count)].gameObject;
                        Debug.Log(Piece.selectedpiece.name);
                        Piece.alreadyselected = true;

                    }


                }

                if(optionscript.mustcuttable)
                {
                    profitcalculcation(true);
                    //if(formustcutgameobject != null)
                    if(formustcutlist.Count > 0)
                    {
                        int i = UnityEngine.Random.Range(0, formustcutlist.Count);
                        Piece.selectedpiece = formustcutlist[i];
                    }
                }
                //isitreallyAI = true;
                AIroll = false;
                LudoDice2D.isAIturn = false;
            }
        }
    }

    List<int> storedicevalue = new List<int>();

    GameObject piecegm = null;
    bool mustbringoutcheck(List<Piece> outpiece)
    {
        foreach(Piece piece in outpiece)
        {
            if(piece.IsInBase)
            {
                piecegm = piece.gameObject;
                return true;
            }
        }
        return false;
    }
    public GameObject forprofitcalculation;
    public GameObject formustcutgameobject;
    public List<GameObject> formustcutlist = new List<GameObject>();
    void profitcalculcation(bool formustcut)
    {
        formustcutgameobject = null;
        formustcutlist.Clear();
        foreach (var piece in movablePieces)
        {

            int col = piece.colornum;
            int cposition = piece.CurrentPosition;

            int newposition = cposition + dicenum;

            
            if(checkingentertohome(piece) == true)
            {
                newposition = newposition % 52;
            }
            forprofitcalculation.SetActive(true);
            Debug.Log("CutCheck");
            boardPath.movetonewpos(forprofitcalculation, newposition, col);
            CircleCollider2D colliderforpiece = forprofitcalculation.GetComponent<CircleCollider2D>();
            Vector2 center = (Vector2)forprofitcalculation.transform.position + colliderforpiece.offset;

            // Calculate radius (handle scaling)
            float scale = Mathf.Max(
                Mathf.Abs(forprofitcalculation.transform.lossyScale.x),
                Mathf.Abs(forprofitcalculation.transform.lossyScale.y)
            );
            float radius = colliderforpiece.radius * scale;

            // Layer mask for "Ignore Raycast"
            LayerMask layerMask = LayerMask.GetMask("Ignore Raycast");
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, layerMask);

            // Debug output
            Debug.Log($"Hits: {hits.Length}");
            foreach (Collider2D hit in hits)
            {
                Debug.Log($"Hit: {hit.gameObject.name} (Layer: {LayerMask.LayerToName(hit.gameObject.layer)})");
            }
            //CircleCollider2D colliderforpiece = forprofitcalculation.GetComponent<CircleCollider2D>();

            //Vector2 center = (Vector2)forprofitcalculation.transform.position + colliderforpiece.offset;

            //// Calculate the effective radius (account for scaling if needed).
            //float scale = Mathf.Max(forprofitcalculation.transform.lossyScale.x, forprofitcalculation.transform.lossyScale.y);

            //float radius = colliderforpiece.radius * scale;


 

            foreach (Collider2D hit in hits)
            {
                Debug.Log("Colliderhit for profit!!!!!!!!!!!!!");
                // Ignore GameObject2 itself.
                if (hit.gameObject == forprofitcalculation)
                    continue;

                //Debug.Log("GameObject2 collided with: " + hit.gameObject.name);

                if (hit.gameObject.CompareTag("Piece"))
                {
                    Piece hitpiece = hit.gameObject.GetComponent<Piece>();  
                    if(piece.colornum != hitpiece.colornum)
                    {
                        if(optionscript.showstar)
                        if(!hitpiece.forshowstar(true, hitpiece.CurrentPosition) && !hitpiece.forshowstar(false, hitpiece.CurrentPosition))
                        {
                            if(formustcut == false)
                            {
                                Piece.selectedpiece = piece.gameObject;
                                Debug.Log("AI Cuttable found!!!!!!!!!!!!!!!!!!!!!!!!!");
                                return;

                            }

                            else
                            {
                                Debug.Log("Cuttable found!!!!!!!!!!!!!!!!!!!!!!!!!");
                                formustcutgameobject = piece.gameObject;
                                formustcutlist.Add(piece.gameObject);
                            }

                        }
                        else
                        {
                            if(!hitpiece.forshowstar(true, hitpiece.CurrentPosition))
                            {
                                    if (formustcut == false)
                                    {
                                        Piece.selectedpiece = piece.gameObject;
                                        Debug.Log("AI Cuttable found!!!!!!!!!!!!!!!!!!!!!!!!!");
                                        return;

                                    }

                                    else
                                    {
                                        Debug.Log("Cuttable found!!!!!!!!!!!!!!!!!!!!!!!!!");
                                        formustcutgameobject = piece.gameObject;
                                        formustcutlist.Add(piece.gameObject);
                                    }
                                }
                        }

                    }
                }
                // You can add any additional logic for handling the collision.
            }
            //forprofitcalculation.SetActive(false);  
        }

        if(formustcut == false)
        {
            foreach (var piece in movablePieces)
            {
            if(piece.piecehavecut())
                {

                    int col = piece.colornum;
                    int cposition = piece.CurrentPosition;

                    int newposition = cposition + dicenum;

                    if(newposition == 56)
                    {
                        Piece.selectedpiece = piece.gameObject;
                        return;
                    }

                    //forprofitcalculation.SetActive(true);

                    //boardPath.movetonewpos(forprofitcalculation, newposition, col);
                }
             }


        }



        }

    void SetLayerForPieces(List<Piece> pieces, string layerName)
    {
        List<int> curpos = new List<int>(); 
        //int toppos = 0;
        int layer = LayerMask.NameToLayer(layerName);
        foreach (Piece piece in pieces)
        {
            curpos.Add(piece.CurrentPosition);
  
            piece.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            piece.transform.GetChild(0).gameObject.layer = layer;

        }

        //for(int i = 0; i < curpos.Count; i++)
        //{
        //    for(int j = i + 1; j<curpos.Count; j++)
        //    {
        //        if (curpos[j] == curpos[i])
        //        {
        //            //same number

        //            if(pieces[i].toppositionnumber < pieces[j].toppositionnumber)
        //            {
        //                pieces[j].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        //            }
        //        }
        //    }

        //}
    }

    void pieceanim(bool animenable, List<Piece> playerpieceslist)
    {
        foreach(Piece playerps in playerpieceslist)
        {
            GameObject childgm = playerps.transform.GetChild(0).gameObject;

            Animator anim = childgm.GetComponent<Animator>();
            if (animenable)
            {
                anim.SetBool("PANIM", true);
            }
            else
            {
                anim.SetBool("PANIM", false);
            }
        }
    }
    public void notouchinputforotherpiece(int whoseturn)
    {
        if(whoseturn == 0)
        {
            SetLayerForPieces(player2Pieces, "Ignore Raycast");
            SetLayerForPieces(player3Pieces, "Ignore Raycast");
            SetLayerForPieces(player4Pieces, "Ignore Raycast");
            SetLayerForPieces(player1Pieces, "Default");


        }
        else if(whoseturn == 1)
        {
            SetLayerForPieces(player1Pieces, "Ignore Raycast");
            SetLayerForPieces(player3Pieces, "Ignore Raycast");
            SetLayerForPieces(player4Pieces, "Ignore Raycast");
            SetLayerForPieces(player2Pieces, "Default");


        }
        else if (whoseturn == 2)
        {
            SetLayerForPieces(player2Pieces, "Ignore Raycast");
            SetLayerForPieces(player1Pieces, "Ignore Raycast");
            SetLayerForPieces(player4Pieces, "Ignore Raycast");
            SetLayerForPieces(player3Pieces, "Default");


 
        }
        else if(whoseturn == 3)
        {
            SetLayerForPieces(player2Pieces, "Ignore Raycast");
            SetLayerForPieces(player3Pieces, "Ignore Raycast");
            SetLayerForPieces(player1Pieces, "Ignore Raycast");
            SetLayerForPieces(player4Pieces, "Default");


        }
    }

    public bool selectablebool = false;
    IEnumerator punishment(Piece ps)
    {
        if (lastmovepiece != null)
        {
            lastmovepiece.lastposition = lastmovepiece.CurrentPosition;
            lastmovepiece.wasjustmoving = false;

        }
        ps.beforecurrrentposition = -1;
        //ps.cutpiece(ps.colornum, ps.piecenumber, ps.gameObject);
        StartCoroutine(boardPath.movebacktohome(ps, ps.CurrentPosition, 0, 0.1f));
        ps.IsInBase = true;
        yield return new WaitForSeconds(0.2f);

        ps.CurrentPosition = -1;
        //threeconsecutivecut = false;
        Piece.alreadyselected = false;
        Piece.selectedpiece = null;
        selectedpiecemove = false;

        lastmovepiece = ps;
        pieceanim(false, player2Pieces);
        pieceanim(false, player3Pieces);
        pieceanim(false, player1Pieces);
        pieceanim(false, player4Pieces);
        StartCoroutine(delayfor5(1.5f,1));


        //diceSystem.checkingturnandskip();
        //StartCoroutine(delayfor5(1.5f));
        AIenable = true;

        selectablebool = false;
    }

    void forupdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Using Raycast, the first collider hit (based on internal order) will be returned.
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Piecechild"))
                {
                    Debug.Log("Clicked on: " + hit.collider.gameObject.name);
                    Piece hitps = hit.collider.transform.parent.gameObject.GetComponent<Piece>();

                    hitps.touchclick();

                }
                // Optionally add your handling logic here
            }
        }
        if (Piece.alreadyselected)
        {
            if(storedicevalue.Count > 0)
            {
                dicenum = storedicevalue[dicecontinuerollcount];
                dicecontinuerollcount++;

            }
            Debug.Log("selectedupdate");
            Piece ps = Piece.selectedpiece.GetComponent<Piece>();

            foreach (Piece piecemove in movablePieces)
            {
                if (piecemove.gameObject == ps.gameObject)
                {
                    selectablebool = true;
                }
            }
            if (selectablebool == false)
            {
                Piece.alreadyselected = false;
                Piece.selectedpiece = null;
                //selectedpiecemove = false;
                Debug.Log("selectedupdate2");
                return;
            }
            Debug.Log("Selected");

            if(coinoutboolenable == false)
            {
                if (optionscript.mustcuttable)
                {
                    bool checkequal = false;
                    profitcalculcation(true);
                    //if (formustcutgameobject != null)
                    if (formustcutlist.Count > 0)
                    {
                        //Piece.selectedpiece = formustcutgameobject;
                        Debug.Log("selectedupdate3");
                        foreach (GameObject gm in formustcutlist)
                        {
                            if (Piece.selectedpiece == gm)
                            {
                                Debug.Log("selectedupdate4");
                                checkequal = true;
                            }
                        }
                        if (checkequal == false)
                        {
                            StartCoroutine(punishment(ps));
                            Piece.alreadyselected = false;
                            Piece.selectedpiece = null;
                            selectedpiecemove = false;
                            selectablebool = false;
                            return;
                        }
                    }
                }
            }



            Debug.Log("ColorNum" + ps.colornum + ":colorchoose" + colorchoose);
            if (ps.colornum == colorchoose)
            {
                if (ps.IsInBase == true)
                {
                    if (dicenum == oneorsix || (optionscript.sixalsobringcoinout && dicenum == 6) || (optionscript.onealsobringcoinout && dicenum == 1)
                        || (optionscript.threesixstart && diceSystem.seconddice3times) || (optionscript.threeonestart && diceSystem.seconddice3times))
                    //if (dicenum == oneorsix)
                    {
                        StartCoroutine(MovePiece(ps));

                        Piece.alreadyselected = false;
                        Piece.selectedpiece = null;
                        selectedpiecemove = false;

                    }
                    else
                    {
                        Piece.alreadyselected = false;
                    }
                }
                else
                {
                    if (!ps.IsInHome && boardPath.CanMove(ps.CurrentPosition, dicenum, ps.colornum, ps))
                    {
                        StartCoroutine(MovePiece(ps));

                        Piece.alreadyselected = false;
                        Piece.selectedpiece = null;
                        selectedpiecemove = false;

                    }
                    else
                    {
                        Piece.alreadyselected = false;
                    }
                }

            }
            else
            {
                Piece.alreadyselected = false;
            }

            AIenable = true;

            selectablebool = false;
            coinoutboolenable = false;
        }
    }

    int dicecontinuerollcount = 0;
    private void Update()
    {
        //if ((optionscript.continueroll))
        //{
        //    if (rollingfinish)
        //    {
        //        //for (int i = 0; i < storedicevalue.Count; i++)
        //        if(dicecontinuerollcount < storedicevalue.Count)
        //        {
        //            if(Piece.selectedpiece == null)
        //            {
        //                selectedpiecemove = true;
        //                Piece.alreadyselected = false;
        //                forupdate();
        //                Piece.selectedpiece = null;

        //            }

        //        }
        //        else
        //        {
        //            storedicevalue.Clear();
        //            dicecontinuerollcount = 0;
        //        }

        //    }
        //}
        //else
        {
            forupdate();
        }
        
    }
    List<Piece> GetPlayerPieces(int playerIndex)
    {
        return playerIndex switch
        {
            0 => player1Pieces,
            1 => player2Pieces,
            2 => player3Pieces,
            3 => player4Pieces,
            _ => player1Pieces
        };
    }

    bool barrierbool = true;
    void barriercheck(Piece ps, int dicevalue)
    {
        int actualpos = ps.currentposbasedoncolor(ps.colornum, ps.CurrentPosition);
        int tarpos = (actualpos + dicevalue)%52;

        foreach(Piece pieceslist in Piece.barrierpos)
        {
            int pieceslistpos = pieceslist.currentposbasedoncolor(pieceslist.colornum, pieceslist.CurrentPosition);

            Debug.Log("ActualPos:" + actualpos + ":TargetPos:" + tarpos + ":PiecelistPos:" + pieceslistpos);
            if(actualpos < tarpos)
            {
                if (actualpos <= pieceslistpos && tarpos >= pieceslistpos)
                {
                    barrierbool = false;
                }
            }
            else
            {
                if(tarpos > pieceslistpos)
                {
                    barrierbool = false;
                }
                else
                {
                    if(actualpos < pieceslistpos)
                    {
                        barrierbool = false;
                    }
                }
            }

        }
    }
    bool checksamepiecepos(List<Piece> pieces, int post)
    {
        foreach(Piece piece in pieces)
        {
            if(piece.CurrentPosition == post)
            {
                return false;
            }
        }
        return true;
    }
    List<Piece> GetMovablePieces(List<Piece> pieces, int diceValue)
    {
        List<Piece> movable = new List<Piece>();

        foreach (Piece piece in pieces)
        {

            if(piece.barrieron)
            {
                if(!piece.pieceinsidepiece)
                {
                    if(diceValue % 2 == 0)
                    {
                        if(checksamepiecepos(pieces, piece.CurrentPosition + diceValue))
                        movable.Add(piece); 
                    }

                }
            }
            else
            {
                if (piece.IsInBase)
                {
                    if((optionscript.onealsobringcoinout && diceValue == 1)|| (optionscript.sixalsobringcoinout && diceValue == 6)
                        || (optionscript.threeonestart && diceSystem.seconddice3times) || (optionscript.threesixstart && diceSystem.seconddice3times))
                    {
                        movable.Add(piece);
                    }
                    else
                    {
                        if (diceValue == oneorsix)
                            movable.Add(piece);
                    }
                }
                else if (!piece.IsInBase && !piece.IsInHome &&
                       boardPath.CanMove(piece.CurrentPosition, diceValue, piece.colornum, piece))
                {
                    barriercheck(piece, diceValue);
                    Debug.Log("BarrierBool:"+barrierbool+";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;");
                    if(barrierbool)
                    movable.Add(piece);

                    barrierbool = true;
                }

            }
        }

        return movable;
    }


    public bool checkingentertohome(Piece ps)
    {
        if (optionscript.mustcuttoenterhome)
        {
            if (ps.colornum == 0)
            {
                if (Piece.greenhavecut == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (ps.colornum == 1)
            {
                if (Piece.yellowhavecut == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (ps.colornum == 2)
            {
                if (Piece.bluehavecut == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                if (Piece.redhavecut == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }

    void colnumupdate()
    {
        if(currentpiececolornum == 0)
        {
            onpiecehomecomplete(p1piece);
        }
        else if (currentpiececolornum == 1)
        {
            onpiecehomecomplete(p2piece);
        }
        else if (currentpiececolornum == 2)
        {
            onpiecehomecomplete(p3piece);
        }
        else if (currentpiececolornum == 3)
        {
            onpiecehomecomplete(p4piece);
        }
    }

    int currentpiececolornum = 0;
    IEnumerator MovePiece(Piece piece)
    {
        pieceanim(false, player2Pieces);
        pieceanim(false, player3Pieces);
        pieceanim(false, player1Pieces);
        pieceanim(false, player4Pieces);

        if (lastmovepiece != null)
        {
            lastmovepiece.lastposition = lastmovepiece.CurrentPosition;
            lastmovepiece.wasjustmoving = false;

        }
        currentpiececolornum = piece.colornum;
        piece.wasjustmoving = true;
        lastmovepiece = piece;
        piece.collidednum = 0;
        // Handle base exit
        //if(piece.ispressedbyotherpiece)
        //{
        //    if(currentDiceValue == oneorsix)
        //    {
        //        piece.sameposswapping();
        //    }
        //}
        //else
        if(piece.barrieron)
        {
            piece.ismoving = false;
            piece.lastposition = piece.CurrentPosition;

            int targetPosition = piece.CurrentPosition + currentDiceValue / 2;
            piece.beforecurrrentposition = targetPosition;

            //if (checkingentertohome(piece) == true)
            //{
            //    targetPosition = targetPosition % 52;
            //}

            if(checkingentertohome(piece) == false)
            {
                if(targetPosition == 56)
                {
                    forceturnchange = piece.gamewin(1);
                }

            }

            yield return boardPath.MovePieceAlongPath(piece, piece.CurrentPosition, targetPosition, moveDuration);
            piece.SetPosition(targetPosition);
            if (piece.CurrentPosition == targetPosition)
            {
                piece.ismoving = false;
                Debug.Log("ismovingfalse");
            }
        }
        else
        {
            if (piece.IsInBase)
            {
                if(currentDiceValue == oneorsix || (optionscript.sixalsobringcoinout && currentDiceValue == 6) 
                || (optionscript.onealsobringcoinout && currentDiceValue == 1) || (optionscript.threeonestart && diceSystem.seconddice3times) 
                || (optionscript.threesixstart && diceSystem.seconddice3times))
                {
                    diceSystem.seconddice3times = false;
                    //LudoDice2D.threeconsecappear = true;
                    piece.ismoving = true;
                    piece.lastposition = piece.CurrentPosition;
                    piece.ExitBase();
                    piece.beforecurrrentposition = 0;

                    //bool ch = checkingentertohome();


                    yield return boardPath.MovePieceToStart(piece, moveDuration);
                    if (piece.CurrentPosition == 0)
                    {
                        piece.ismoving = false;
                        Debug.Log("ismovingfalse");
                    }

                }
            }
            else
            {
                piece.ismoving = true;
                piece.lastposition = piece.CurrentPosition;

                int targetPosition = piece.CurrentPosition + currentDiceValue;
                piece.beforecurrrentposition = targetPosition;

                //if (checkingentertohome(piece) == true)
                //{
                //    targetPosition = targetPosition % 52;
                //}
                //Debug.Log(piece.ismoving);
                if (checkingentertohome(piece) == false)
                {
                    if (targetPosition == 56)
                    {
                        forceturnchange = piece.gamewin(1);
                    }

                }
                // Regular movement
                yield return boardPath.MovePieceAlongPath(piece, piece.CurrentPosition, targetPosition, moveDuration);
                piece.SetPosition(targetPosition);
                if (piece.CurrentPosition == targetPosition)
                {
                    piece.ismoving = false;
                    Debug.Log("ismovingfalse");
                }
            }


        }

        StartCoroutine(delayfor5(0.5f, 1));
        Piece.endtriggeronetime = false;

    }

    void turningupdate(int prevcurpos, int ainum, int fordifferentdelay)
    {
        diceSystem.checkformovingturn(true);
        if (fordifferentdelay == 2)
        {
            diceSystem.checkingturnandskip();
        }
        if (isitreallyAI)
        {
            if (diceSystem.turnchangecheck)
            {
                diceSystem.turnchangecheck = false;
            }
            else
            {

                diceSystem.AIsubfunc(diceSystem.noofAI, true);
            }
            Debug.Log("::::::::::::::::::::::::::::::::::::" + LudoDice2D.AIjustnumber);
        }

        if (forceturnchange)
        {
            diceSystem.currentPlayerIndex = prevcurpos;
            LudoDice2D.AIjustnumber = ainum;

            //if (prevcurpos == diceSystem.currentPlayerIndex)
            {
                diceSystem.turnchange(true);
                if (isitreallyAI)
                {
                    if (diceSystem.turnchangecheck)
                    {
                        diceSystem.turnchangecheck = false;
                    }
                    else
                    {

                        diceSystem.AIsubfunc(diceSystem.noofAI, true);
                    }

                }
            }
            colnumupdate();

            if (LudoDice2D.playernum <= 1)
            {
                if (gameoverUI != null)
                {
                    gameoverfunc();
                    Time.timeScale = 0f;
                }
            }
            forceturnchange = false;
        }
     
    }

    void lastranked()
    {
        List<int> playersls = new List<int>();
        playersls.Add(pl1);
        if (totalplayer >= 2)
            playersls.Add(pl2);
        if (totalplayer >= 3)
            playersls.Add(pl3);
        if (totalplayer == 4)
            playersls.Add(pl4);

        if (totalplayer == 2)
        {
            // For two players, choose the one that isn't already first.
            foreach (int p in playersls)
            {
                if (p != firstranked)
                {
                    secondranked = p;
                    break;
                }
            }
        }
        else if (totalplayer == 3)
        {
            // For three players, choose the one that's neither first nor second.
            foreach (int p in playersls)
            {
                if (p != firstranked && p != secondranked)
                {
                    thirdranked = p;
                    break;
                }
            }
        }
        else if (totalplayer == 4)
        {
            // For four players, choose the one that's not first, second, or third.
            foreach (int p in playersls)
            {
                if (p != firstranked && p != secondranked && p != thirdranked)
                {
                    fourthranked = p;
                    break;
                }
            }
        }
    }

    public TMP_Text text1, text2, text3, text4;
    public GameObject trophy3, trophy4, background3, background4;
    void gameoverfunc()
    {
        lastranked();
        gameoverUI.SetActive(true);
        if(firstranked == 0)
        {
            text1.text = " Green";
        }
        else if (firstranked == 1)
        {
            text1.text = " Yellow";
        }
        else if(firstranked == 2)
        {
            text1.text = " Blue";
        }
        else
        {
            text1.text = " Red";
        }
        if (secondranked == 0)
        {
            text2.text = " Green";
        }
        else if (secondranked == 1)
        {
            text2.text = " Yellow";
        }
        else if (secondranked == 2)
        {
            text2.text = " Blue";
        }
        else
        {
            text2.text = " Red";
        }
        if(thirdranked != 1000)
        {
            if (thirdranked == 0)
            {
                text3.text = " Green";
            }
            else if (thirdranked == 1)
            {
                text3.text = " Yellow";
            }
            else if (thirdranked == 2)
            {
                text3.text = " Blue";
            }
            else
            {
                text3.text = " Red";
            }
        }
        else
        {
            trophy3.SetActive(false);
            GameObject gm = text3.gameObject.transform.parent.gameObject;
            gm.SetActive(false);
            background3.SetActive(false);
            text3.text = " ";
        }
        if (fourthranked != 1000)
        {
            if (fourthranked == 0)
            {
                text4.text = " Green";
            }
            else if (fourthranked == 1)
            {
                text4.text = " Yellow";
            }
            else if (fourthranked == 2)
            {
                text4.text = " Blue";
            }
            else
            {
                text4.text = " Red";
            }
        }
        else
        {
            trophy4.SetActive(false);
            GameObject gm = text4.gameObject.transform.parent.gameObject;
            gm.SetActive(false);
            background4.SetActive(false);
            text4.text = " ";
        }


        PlayerPrefs.SetFloat("SaveEnable", 0);
        PlayerPrefs.SetFloat("LoadingEnable", 1);
    }
    IEnumerator delayfor5(float delayamount, int fordifferentdelay)
    {
        int prevcurpos = diceSystem.currentPlayerIndex;
        int ainum = LudoDice2D.AIjustnumber;
        yield return new WaitForSeconds(delayamount);

        if(optionscript.continueroll)
        {
            diceSystem.continuerollcounter++;
            diceSystem.continuerollbool = false;
            LudoDice2D.isAIturn = false;
             //if(dicecontinuerollcount == storedicevalue.Count)
            if(diceSystem.continuerollcounter == diceSystem.dicevaluestore.Count)
            {
                diceSystem.AIagain = false;
                diceSystem.turnchangecheck = false;
                diceSystem.continuerollcounter = 0;
                diceSystem.rollingfinish = false;
                diceSystem.dicevaluestore.Clear();
                turningupdate(prevcurpos, ainum, fordifferentdelay);
                diceSystem.changedicecoloronturn(currentDiceValue);
                isitreallyAI = false;
            }
        }
        else
        {
            turningupdate(prevcurpos, ainum, fordifferentdelay);
            diceSystem.changedicecoloronturn(currentDiceValue);
            isitreallyAI = false;
        }
        if (oneorsix == 1)
        {
            // Check for special conditions (e.g., 6 gives another turn)
            if (currentDiceValue != 1)
            {
                //piece.wasjustmoving = false;
                diceSystem.EndTurn();
            }
            else
            {
                diceSystem.EnableDiceInteraction();
            }
        }
        else if (oneorsix == 6)
        {
            // Check for special conditions (e.g., 6 gives another turn)
            if (currentDiceValue != 6)
            {
                //piece.wasjustmoving = false;
                diceSystem.EndTurn();
            }
            else
            {
                diceSystem.EnableDiceInteraction();
            }
        }

        diceSystem.EnableDiceInteraction();
        dicecollider.enabled = true;
        diceSystem.keeptouchingbool = false;

        diceSystem.collisiontruepiece = false;
        //diceSystem.collisioncheckbool = false;
        diceSystem.SetTransparency(1f);
        diceSystem.piecemanagercodecomplete = true;


    }
    // Called when player clicks a piece
    //public void PieceClicked(Piece clickedPiece)
    //{
    //    if (isWaitingForSelection)
    //    {
    //        OnPieceSelected(clickedPiece);
    //    }
    //}
}