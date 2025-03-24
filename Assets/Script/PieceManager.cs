using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using TMPro;
using System.Net.NetworkInformation;

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

    void movingspeed()
    {
        moveDuration = 0.2f;
        if(PlayerPrefs.HasKey("SliderValue"))
        {
            float svalue = PlayerPrefs.GetFloat("SliderValue");
            moveDuration = 0.2f - svalue/10;
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
        Piece.selectedpiece = movablePieces[Random.Range(0, movablePieces.Count)].gameObject;
        Debug.Log(Piece.selectedpiece.name);
        Piece.alreadyselected = true;

    }
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
    }



    //void dictioinaryshow()
    //{
    //    foreach(GameObject gm in Piece.samePosDictionary[Piece.Currentposit])
    //}
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
        lastmovepiece.cutpiece(lastmovepiece.colornum, lastmovepiece.piecenumber, lastmovepiece.gameObject);
        lastmovepiece.IsInBase = true;
        yield return new WaitForSeconds(0.2f);

        lastmovepiece.CurrentPosition = -1;
        threeconsecutivecut = false;

        diceSystem.checkingturnandskip();
        StartCoroutine(delayfor5(1.5f));

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
                StartCoroutine(delayfor5(1.5f));
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

    void foranim(int pindex)
    {
        if(pindex ==0 )
        {
            pieceanim(true, player1Pieces);
        }
        else if(pindex ==1 )
        {
            pieceanim(true, player2Pieces);
        }
        else if(pindex ==2 )
        {
            pieceanim(true, player3Pieces);
        }
        else
        {
            pieceanim(true, player4Pieces);
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
            StartCoroutine(delayfor5(1.5f));
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

            foranim(playerIndex);
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
            //if (optionscript.continueroll)
            //{
            //    continuerollon(diceValue);

            //    if (diceValue == 2 || diceValue == 3 || diceValue == 4 || diceValue == 5 || diceValue == secondoneorsix)
            //    {
            //        if (optionscript.sixgiveanotherturn || optionscript.onealsogiveturn)
            //        {
            //            if (diceValue != secondoneorsix)
            //            {
            //                rollingfinish = true;
            //            }
            //            else
            //            {
            //                rollingfinish = false;
            //            }
            //        }
            //        else
            //        {
            //            rollingfinish = true;
            //        }
            //    }
            //}
        }
    }
    
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
                        Piece.selectedpiece = movablePieces[Random.Range(0, movablePieces.Count)].gameObject;
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
                        int i = Random.Range(0, formustcutlist.Count);
                        Piece.selectedpiece = formustcutlist[i];
                    }
                }
                AIroll = false;
                LudoDice2D.isAIturn = false;
            }
        }
    }

    List<int> storedicevalue = new List<int>();
    bool rollingfinish = false;
    void continuerollon(int dicevalue)
    {
        storedicevalue.Add(dicevalue);
        StartCoroutine(delayfor5(1.5f));
    }
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
  
            piece.gameObject.layer = layer;
      
        }

        for(int i = 0; i < curpos.Count; i++)
        {
            for(int j = i + 1; j<curpos.Count; j++)
            {
                if (curpos[j] == curpos[i])
                {
                    //same number

                    if(pieces[i].toppositionnumber < pieces[j].toppositionnumber)
                    {
                        pieces[j].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    }
                }
            }

        }
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
        ps.cutpiece(ps.colornum, ps.piecenumber, ps.gameObject);
        ps.IsInBase = true;
        yield return new WaitForSeconds(0.2f);

        ps.CurrentPosition = -1;
        //threeconsecutivecut = false;
        Piece.alreadyselected = false;
        Piece.selectedpiece = null;
        selectedpiecemove = false;

        lastmovepiece = ps;
        StartCoroutine(delayfor5(1.5f));


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
                if (hit.collider.CompareTag("Piece"))
                {
                    Debug.Log("Clicked on: " + hit.collider.gameObject.name);
                    Piece hitps = hit.collider.gameObject.GetComponent<Piece>();

                    hitps.touchclick();

                }
                // Optionally add your handling logic here
            }
        }
        if (Piece.alreadyselected)
        {
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
    private void Update()
    {
        //if((optionscript.continueroll))
        //{
        //    if(rollingfinish)
        //    {
        //        for (int i = 0; i < storedicevalue.Count; i++)
        //        {
        //            selectedpiecemove = true;
        //            forupdate();

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

            if (actualpos <= pieceslistpos && tarpos >= pieceslistpos)
            {
                barrierbool = false;
            }
            else if(actualpos >= pieceslistpos && tarpos <= pieceslistpos)
            {
                barrierbool = false;
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

        StartCoroutine(delayfor5(0.5f));
        Piece.endtriggeronetime = false;

    }

    IEnumerator delayfor5(float delayamount)
    {
        yield return new WaitForSeconds(delayamount);
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
        diceSystem.changedicecoloronturn(currentDiceValue);
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