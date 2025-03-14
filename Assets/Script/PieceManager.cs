using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using TMPro;

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
    private void Start()
    {
        if(greenpiece != null)
        {
            foreach(Transform children in greenpiece.transform)
            {
                player1Pieces.Add(children.gameObject.GetComponent<Piece>());
            }
        }
        if (yellowpiece != null)
        {
            foreach (Transform children in yellowpiece.transform)
            {
                player2Pieces.Add(children.gameObject.GetComponent<Piece>());
            }
        }
        if (bluepiece != null)
        {
            foreach (Transform children in bluepiece.transform)
            {
                player3Pieces.Add(children.gameObject.GetComponent<Piece>());
            }
        }
        if (redpiece != null)
        {
            foreach (Transform children in redpiece.transform)
            {
                player4Pieces.Add(children.gameObject.GetComponent<Piece>());
            }
        }



        if (optionscript.startatoneorsix)
        {
            oneorsix = 6;
        }
        else
        {
            oneorsix = 1;
        }
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

    void HandleDiceResult(int playerIndex, int diceValue)
    {
        if(LudoDice2D.threeoneorsix)
        {
           if(optionscript.threeonecut || optionscript.threesixcut)
            {
                if(lastmovepiece != null)
                {
                    lastmovepiece.cutpiece(lastmovepiece.colornum, lastmovepiece.piecenumber, lastmovepiece.gameObject);

                    lastmovepiece.IsInBase = true;
                    lastmovepiece.CurrentPosition = -1;
                }

                StartCoroutine(delayfor5(1.5f));
                diceSystem.EndTurn();
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

    void handlingpiece(int playerIndex, int diceValue)
    {
        currentDiceValue = diceValue;
        //currentPlayerPieces.Clear();
        Debug.Log("Getplayerpiece");
        currentPlayerPieces = GetPlayerPieces(playerIndex);

        // Find movable pieces
        movablePieces = GetMovablePieces(currentPlayerPieces, diceValue);
        Debug.Log("GetMovablepieceinpiecemanager");

        notouchinputforotherpiece(playerIndex);

        if (movablePieces.Count == 0)
        {
            // No movable pieces - end turn
            //diceSystem.EndTurn();
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
            dicecollider.enabled = false;
            selectedpiecemove = true;
            colorchoose = playerIndex;
            Debug.Log("ColorChoose:"+colorchoose);
            dicenum = diceValue;
            AIroll = true;
            AIpiecemove();
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
                        Piece.selectedpiece = movablePieces[Random.Range(0, movablePieces.Count)].gameObject;
                        Debug.Log(Piece.selectedpiece.name);
                        Piece.alreadyselected = true;

                    }


                }
                AIroll = false;
                LudoDice2D.isAIturn = false;
            }
        }
    }

    void SetLayerForPieces(List<Piece> pieces, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        foreach (Piece piece in pieces)
        {
            piece.gameObject.layer = layer;
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
    private void Update()
    {

        if (Piece.alreadyselected)
        {
            
            Piece ps = Piece.selectedpiece.GetComponent<Piece>();

            Debug.Log("ColorNum"+ps.colornum+":colorchoose"+colorchoose);
            if(ps.colornum == colorchoose)
            {
                if (ps.IsInBase == true)
                {
                    if(dicenum == oneorsix)
                    {
                        StartCoroutine(MovePiece(ps));

                        Piece.alreadyselected = false;
                        Piece.selectedpiece = null;
                        selectedpiecemove=false;

                    }
                    else
                    {
                        Piece.alreadyselected = false;
                    }
                }
                else
                {
                    if(!ps.IsInHome && boardPath.CanMove(ps.CurrentPosition, dicenum, ps.colornum))
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

    List<Piece> GetMovablePieces(List<Piece> pieces, int diceValue)
    {
        List<Piece> movable = new List<Piece>();

        foreach (Piece piece in pieces)
        {
            if(piece.ispressedbyotherpiece)
            {
                if(diceValue == oneorsix)
                {
                    movable.Add(piece);
                }
            }
            else
            {
                if (piece.IsInBase && diceValue == oneorsix)
                {
                    movable.Add(piece);
                }
                else if (!piece.IsInBase && !piece.IsInHome &&
                       boardPath.CanMove(piece.CurrentPosition, diceValue, piece.colornum))
                {

                    movable.Add(piece);
                }

            }
        }

        return movable;
    }

    void EnablePieceSelection(List<Piece> selectablePieces)
    {
        isWaitingForSelection = true;
        selectionPrompt.SetActive(true);

        // Highlight movable pieces
        foreach (Piece piece in selectablePieces)
        {
            piece.ToggleHighlight(true);
            piece.EnableSelection();
        }
    }

    void DisablePieceSelection()
    {
        isWaitingForSelection = false;
        selectionPrompt.SetActive(false);

        foreach (Piece piece in movablePieces)
        {
            piece.ToggleHighlight(false);
            piece.DisableSelection();
        }
    }

    public void OnPieceSelected(Piece selectedPiece)
    {
        if (!isWaitingForSelection || !movablePieces.Contains(selectedPiece)) return;

        DisablePieceSelection();
        StartCoroutine(MovePiece(selectedPiece));
    }

    IEnumerator MovePiece(Piece piece)
    {
        piece.wasjustmoving = true;
        lastmovepiece = piece;
        // Handle base exit
        if(piece.ispressedbyotherpiece)
        {
            if(currentDiceValue == oneorsix)
            {
                piece.sameposswapping();
            }
        }
        else
        {
            if (piece.IsInBase && currentDiceValue == oneorsix)
            {
                piece.ismoving = true;
                piece.lastposition = piece.CurrentPosition;
                piece.ExitBase();
                yield return boardPath.MovePieceToStart(piece, moveDuration);
                if (piece.CurrentPosition == 0)
                {
                    piece.ismoving = false;
                    Debug.Log("ismovingfalse");
                }
            }
            else
            {
                piece.ismoving = true;
                piece.lastposition = piece.CurrentPosition;

                int targetPosition = piece.CurrentPosition + currentDiceValue;
 
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

        diceSystem.piecemanagercodecomplete = true;


    }
    // Called when player clicks a piece
    public void PieceClicked(Piece clickedPiece)
    {
        if (isWaitingForSelection)
        {
            OnPieceSelected(clickedPiece);
        }
    }
}