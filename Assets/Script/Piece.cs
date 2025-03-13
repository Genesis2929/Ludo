using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
//using UnityEngine.AdaptivePerformance.VisualScripting;

public class Piece : MonoBehaviour
{
    public List<Transform> redhomepos = new List<Transform>();
    public List<Transform> greenhomepos = new List<Transform>();
    public List<Transform> bluehomepos = new List<Transform>();
    public List<Transform> yellowhomepos = new List<Transform>();
    public int colornum = 0;
    public int CurrentPosition = -1; // -1 = in base
    public bool IsInBase = true;
     public bool IsInHome = false;

    public bool ismoving = false;
    public int lastposition = -1;
    public bool wasjustmoving = false;


    public bool ispressedbyotherpiece = false;


    //public List<Transform> homeposition = new List<Transform>();
    [Header("Visuals")]
    [SerializeField] private GameObject highlightEffect;

    public static bool alreadyselected = false;
    public static GameObject selectedpiece = null;

    public int piecenumber = 0;

    public static bool greenoneappear = false;
    public static bool redoneappear = false;
    public static bool blueoneappear = false;
    public static bool yellowoneappear = false;

    public static bool homeorcutturn = false;

    public int toppositionnumber = 0;

    public int olddictionaryposition = 0;

    public static Dictionary<int, List<GameObject>> samePosDictionary = new Dictionary<int, List<GameObject>>();

    public void samepositionmaintainlist(int pieceposnumber, GameObject psobject)
    {

        

        if (!samePosDictionary.ContainsKey(CurrentPosition))
        {
            samePosDictionary[CurrentPosition] = new List<GameObject>();
        }

        // Check if the list already contains this GameObject.
        if (!samePosDictionary[CurrentPosition].Contains(psobject))
        {
            samePosDictionary[CurrentPosition].Add(psobject);
        }
        if (!samePosDictionary[CurrentPosition].Contains(gameObject))
        {
            samePosDictionary[CurrentPosition].Add(gameObject);
        }
        olddictionaryposition = CurrentPosition;

        samePosDictionary[CurrentPosition].Sort((go1, go2) =>
    go1.GetComponent<Piece>().toppositionnumber.CompareTo(go2.GetComponent<Piece>().toppositionnumber)
);
        foreach (GameObject obj in samePosDictionary[CurrentPosition])
        {
            int tempnumber = obj.GetComponent<Piece>().toppositionnumber;
            Debug.Log("Top position number: " + tempnumber);
        }

        //samePosDictionary[CurrentPosition].Add(gameObject);
        //samePosDictionary[pieceposnumber].Add(psobject);
    }

    public void sameposswapping()
    {
        // Ensure the key exists in the dictionary
        if (!samePosDictionary.ContainsKey(CurrentPosition))
        {
            Debug.LogError("No objects found at CurrentPosition.");
            return;
        }

        List<GameObject> objectsAtPosition = samePosDictionary[CurrentPosition];

        // Ensure the list is sorted by toppositionnumber before swapping
        objectsAtPosition.Sort((go1, go2) =>
            go1.GetComponent<Piece>().toppositionnumber.CompareTo(go2.GetComponent<Piece>().toppositionnumber)
        );

        // Find the index of the current GameObject in that list
        int currentIndex = objectsAtPosition.IndexOf(gameObject);
        if (currentIndex <= 0)
        {
            Debug.Log("Current object is at the lowest position; cannot swap.");
            return;
        }

        // Get the GameObject that has one less toppositionnumber in the sorted order
        GameObject targetObject = objectsAtPosition[currentIndex - 1];

        // Get the Piece component for both objects
        Piece currentPiece = gameObject.GetComponent<Piece>();
        Piece targetPiece = targetObject.GetComponent<Piece>();

        // Swap the toppositionnumber values
        int temp = currentPiece.toppositionnumber;
        currentPiece.toppositionnumber = targetPiece.toppositionnumber;
        targetPiece.toppositionnumber = temp;

        movesortorder(gameObject, targetObject, true);

        if(currentPiece.toppositionnumber == 0)
        {
            currentPiece.ispressedbyotherpiece = false;
        }
        if(targetPiece.toppositionnumber == 0)
        {
            targetPiece.ispressedbyotherpiece = false;
        }

        Debug.Log($"Swapped toppositionnumber between {gameObject.name} and {targetObject.name}");
    }

    public void listmaintain()
    {
        if (samePosDictionary.ContainsKey(olddictionaryposition))
        {
            if (olddictionaryposition != CurrentPosition)
            {
                samePosDictionary[olddictionaryposition].Remove(gameObject);
            
                foreach(GameObject obj in samePosDictionary[olddictionaryposition])
                {
                    movesortorder(obj, obj, false);

                    Piece objpiece  = obj.GetComponent<Piece>();
                    if(objpiece.toppositionnumber >= 1)
                    objpiece.toppositionnumber = objpiece.toppositionnumber - 1 ;

                    if(objpiece.toppositionnumber == 0)
                    {
                        objpiece.ispressedbyotherpiece = false;
                    }
                }

                olddictionaryposition = CurrentPosition;
            }

        }
    }

    private void Start()
    {
        
        checkplayerenable();
        IsInBase = true;
        lastposition = CurrentPosition;
        initialpiecesetup();

    }

    void checkplayerenable()
    {
        if (LudoDice2D.playernum == 3)
        {
            if (colornum ==  LudoDice2D.player1 || colornum == LudoDice2D.player2 || colornum == LudoDice2D.player3)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }            
        }
        else if(LudoDice2D.playernum == 2)
        {
            if (colornum == LudoDice2D.player1 || colornum == LudoDice2D.player2)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
    void homereached()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        IsInHome = true;
    }

    void initialpiecesetup()
    {
        if (colornum == 3)
        {
            if (piecenumber == 0)
            {
                transform.position = redhomepos[0].position;
            }
            else if (piecenumber == 1)
            {
                transform.position = redhomepos[1].position;
            }
            else if (piecenumber == 2)
            {
                transform.position = redhomepos[2].position;
            }
            else if (piecenumber == 3)
            {
                transform.position = redhomepos[3].position;
            }

        }
        else if (colornum == 2)
        {
            if (piecenumber == 0)
            {
                transform.position = bluehomepos[0].position;
            }
            else if (piecenumber == 1)
            {
                transform.position = bluehomepos[1].position;
            }
            else if (piecenumber == 2)
            {
                transform.position = bluehomepos[2].position;
            }
            else if (piecenumber == 3)
            {
                transform.position = bluehomepos[3].position;
            }

        }
        else if (colornum == 1)
        {
            if (piecenumber == 0)
            {
                transform.position = yellowhomepos[0].position;
            }
            else if (piecenumber == 1)
            {
                transform.position = yellowhomepos[1].position;
            }
            else if (piecenumber == 2)
            {
                transform.position = yellowhomepos[2].position;
            }
            else if (piecenumber == 3)
            {
                transform.position = yellowhomepos[3].position;
            }

        }
        else if (colornum == 0)
        {
            if (piecenumber == 0)
            {
                transform.position = greenhomepos[0].position;
            }
            else if (piecenumber == 1)
            {
                transform.position = greenhomepos[1].position;
            }
            else if (piecenumber == 2)
            {
                transform.position = greenhomepos[2].position;
            }
            else if (piecenumber == 3)
            {
                transform.position = greenhomepos[3].position;
            }

        }
    }

    public void Update()
    {
        if(toppositionnumber == 0)
        {
            ispressedbyotherpiece = false;
        }


        if (colornum == 0)
        {
            if (CurrentPosition == 0)
            {
                greenoneappear = true;
            }

        }
        else if (colornum == 1)
        {
            if (CurrentPosition == 0)
            {
                yellowoneappear = true;
            }
        }
        else if (colornum == 2)
        {
            if (CurrentPosition == 0)
            {
                blueoneappear = true;
            }

        }
        else if (colornum == 3)
        {
            if (CurrentPosition == 0)
            {
                redoneappear = true;
            }
        }


        if (!wasjustmoving)
        {
            lastposition = CurrentPosition;
        }
    }
    public void ToggleHighlight(bool state)
    {
        highlightEffect.SetActive(state);
    }

    public void EnableSelection()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    public void DisableSelection()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    public void ExitBase()
    {
        IsInBase = false;
        CurrentPosition = 0;
    }

    public void SetPosition(int newPosition)
    {
        CurrentPosition = newPosition;
    }

    private void OnMouseDown()
    {

        //if(!LudoDice2D.isAIturn)
        if(LudoDice2D.numberofAI ==  0)
        {
            if (PieceManager.selectedpiecemove)
            {
                if (alreadyselected == false)
                {
                    selectedpiece = gameObject;
                    alreadyselected = true;
                }

                //PieceManager.selectedpiecemove = false;
            }
        }
        else
        {
            if(LudoDice2D.AIjustnumber != colornum)
            {
                if (PieceManager.selectedpiecemove)
                {
                    if (alreadyselected == false)
                    {
                        selectedpiece = gameObject;
                        alreadyselected = true;
                    }

                    //PieceManager.selectedpiecemove = false;
                }

            }
        }
    }

    //public void samepositionmaintainlist(int pieceposnumber, GameObject psobject)
    //{
    //    sameposlist.Add(gameObject);
    //    sameposlist.Add(psobject);
    //}
    public void movesortorder(GameObject gm, GameObject gm1, bool swap)
    {

        // Here we assume that "certainpiece" (this gameObject) has exactly two children.
        SpriteRenderer childRenderer1 = gm.transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer childRenderer2 = gm.transform.GetChild(1).GetComponent<SpriteRenderer>();

        if(swap)
        {
            SpriteRenderer childRenderer11 = gm1.transform.GetChild(0).GetComponent<SpriteRenderer>();
            SpriteRenderer childRenderer22 = gm1.transform.GetChild(1).GetComponent<SpriteRenderer>();

            int tempsortingorder;
            // Safety check in case one of the children is missing a SpriteRenderer.
            if (childRenderer1 != null && childRenderer2 != null && childRenderer11 != null && childRenderer22 != null)
            {
                tempsortingorder = childRenderer1.sortingOrder;
                childRenderer1.sortingOrder = childRenderer11.sortingOrder;
                childRenderer11.sortingOrder = tempsortingorder;

                tempsortingorder = childRenderer2.sortingOrder;
                childRenderer2.sortingOrder = childRenderer22.sortingOrder;
                childRenderer22.sortingOrder = tempsortingorder;
            }

        }
        else
        {
            childRenderer1.sortingOrder = childRenderer1.sortingOrder - 1;
            childRenderer2.sortingOrder = childRenderer2.sortingOrder - 1;
        }

        

        
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Piece"))
        {
            if(other.gameObject.GetComponent<Piece>().colornum != colornum)
                 listmaintain();

        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {

        //if(wasjustmoving == true)
        if (lastposition != CurrentPosition)
        {
            if (ismoving == false)
            {
                // Check if the collided object is tagged "Piece"
                if (collision.gameObject.CompareTag("Piece"))
                {
                    Piece ps = collision.gameObject.GetComponent<Piece>();
                    if (colornum != ps.colornum)
                    {
                        if (ps.ismoving == false)
                        {
                            if (CurrentPosition == 0 || CurrentPosition == 8 || CurrentPosition == 13
                    || CurrentPosition == 21 || CurrentPosition == 26 || CurrentPosition == 34
                    || CurrentPosition == 39 || CurrentPosition == 47 || CurrentPosition == -1)
                            {
                                // Get all child SpriteRenderer components from the Piece object.
                                SpriteRenderer[] pieceChildRenderers = collision.gameObject.GetComponentsInChildren<SpriteRenderer>();

                                // Ensure that we have at least 2 child SpriteRenderers in the Piece object.
                                if (pieceChildRenderers.Length >= 2)
                                {
                                    // Get the child SpriteRenderer components from "certainpiece"
                                    // Here we assume that "certainpiece" (this gameObject) has exactly two children.
                                    SpriteRenderer childRenderer1 = transform.GetChild(0).GetComponent<SpriteRenderer>();
                                    SpriteRenderer childRenderer2 = transform.GetChild(1).GetComponent<SpriteRenderer>();

                                    // Safety check in case one of the children is missing a SpriteRenderer.
                                    if (childRenderer1 != null && childRenderer2 != null)
                                    {
                                        // Assign the sorting order from the Piece's children to the corresponding children in "certainpiece".
                                        childRenderer1.sortingOrder = pieceChildRenderers[0].sortingOrder + 1;
                                        childRenderer2.sortingOrder = pieceChildRenderers[1].sortingOrder + 1;
                                        //ps.gameObject.GetComponent<Collider2D>().enabled = false;
                                        ps.ispressedbyotherpiece = true;
                                        ps.toppositionnumber++;

                               
                                        samepositionmaintainlist(ps.toppositionnumber, ps.gameObject);
                                        Debug.Log("PieceColornum:" + colornum+":++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                                    }

                                    else
                                    {
                                        Debug.LogWarning("One or both children of 'certainpiece' are missing a SpriteRenderer component.");
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning("The collided 'Piece' does not have enough child SpriteRenderers.");
                                }
                            }
                            //    }
                            //if (collision.gameObject.CompareTag("Piece"))
                            //{
                            else
                            {


                                //GameObject gobj = ps.gameObject;

                                cutpiece(ps.colornum, ps.piecenumber, ps.gameObject);

                                ps.IsInBase = true;
                                ps.lastposition = ps.CurrentPosition;
                                ps.CurrentPosition = -1;

                                homeorcutturn = true;

                            }

                        }
                        Debug.Log("CurrentPosition:" + CurrentPosition);
                        //Debug.Log("Collided with an object tagged as 'Piece'");
                        // Add your logic here for when the object with the tag "Piece" collides.
                    }

                }
                else if (collision.gameObject.CompareTag("Home"))
                {
                    homereached();
                    homeorcutturn = true;
                }

                wasjustmoving = false;
            }

        }
    }

    public void cutpiece(int colornumber, int piecenum, GameObject gobj)
    {
        if (colornumber == 3)
        {
            if (piecenum == 0)
            {
                gobj.transform.position = redhomepos[0].position;
            }
            else if (piecenum == 1)
            {
                gobj.transform.position = redhomepos[1].position;
            }
            else if (piecenum == 2)
            {
                gobj.transform.position = redhomepos[2].position;
            }
            else if (piecenum == 3)
            {
                gobj.transform.position = redhomepos[3].position;
            }

        }
        else if (colornumber == 2)
        {
            if (piecenum == 0)
            {
                gobj.transform.position = bluehomepos[0].position;
            }
            else if (piecenum == 1)
            {
                gobj.transform.position = bluehomepos[1].position;
            }
            else if (piecenum == 2)
            {
                gobj.transform.position = bluehomepos[2].position;
            }
            else if (piecenum == 3)
            {
                gobj.transform.position = bluehomepos[3].position;
            }

        }
        else if (colornumber == 1)
        {
            if (piecenum == 0)
            {
                gobj.transform.position = yellowhomepos[0].position;
            }
            else if (piecenum == 1)
            {
                gobj.transform.position = yellowhomepos[1].position;
            }
            else if (piecenum == 2)
            {
                gobj.transform.position = yellowhomepos[2].position;
            }
            else if (piecenum == 3)
            {
                gobj.transform.position = yellowhomepos[3].position;
            }

        }
        else if (colornumber == 0)
        {
            if (piecenum == 0)
            {
                gobj.transform.position = greenhomepos[0].position;
            }
            else if (piecenum == 1)
            {
                gobj.transform.position = greenhomepos[1].position;
            }
            else if (piecenum == 2)
            {
                gobj.transform.position = greenhomepos[2].position;
            }
            else if (piecenum == 3)
            {
                gobj.transform.position = greenhomepos[3].position;
            }

        }
    }

}