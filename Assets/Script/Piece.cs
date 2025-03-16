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

    public int beforecurrrentposition = -1;
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

    int currentposbasedoncolor(int cnum, int currentposnumber)
    {
        Debug.Log("CNUM:" + cnum +":Currentposnumber:"+currentposnumber);
        int newnum = 0;
        if(cnum == 0)
        {
            newnum = currentposnumber;   
        }
        else if(cnum == 1)
        {
            newnum = currentposnumber + 39;
            newnum = newnum % 52;
        }
        else if(cnum == 2)
        {
            newnum = currentposnumber + 26;
            newnum = newnum % 52;
        }
        else if(cnum == 3)
        {
            newnum = currentposnumber + 13;
            newnum = newnum % 52;
        }
        return newnum;
    }
    public void samepositionmaintainlist(int pieceposnumber, GameObject psobject, Piece collidedpiece)
    {

        int nenum = currentposbasedoncolor(collidedpiece.colornum, collidedpiece.CurrentPosition);

        if (!samePosDictionary.ContainsKey(nenum))
        {
            samePosDictionary[nenum] = new List<GameObject>();
        }


        // Check if the list already contains this GameObject.
        if (!samePosDictionary[nenum].Contains(psobject))
        {
            samePosDictionary[nenum].Add(psobject);
            collidedpiece.olddictionaryposition = collidedpiece.CurrentPosition;
        }

        if (!samePosDictionary[nenum].Contains(gameObject))
        {
            samePosDictionary[nenum].Add(gameObject);
            
        }
        olddictionaryposition = CurrentPosition;

        samePosDictionary[nenum].Sort((go1, go2) =>
    go1.GetComponent<Piece>().toppositionnumber.CompareTo(go2.GetComponent<Piece>().toppositionnumber));
            int totalcount = samePosDictionary[nenum].Count;
            int newcount = totalcount;
        foreach (GameObject obj in samePosDictionary[nenum])
        {
            int tempnumber = obj.GetComponent<Piece>().toppositionnumber;
            if(obj != gameObject)
            {
                Piece pss = obj.GetComponent<Piece>();
                pss.toppositionnumber = tempnumber + 1;
                //if(totalcount > 2)
                //{
                //    movesortorder(obj, obj, false, true);
                //    //newcount--;
                //}
                pss.ispressedbyotherpiece = true;
                
            }
            else
            {
                toppositionnumber = 0;
                for(int i=1 ; i <= totalcount-1; i++)
                {
                    movesortorder(obj, obj, false, true);
                }
            }
            tempnumber = obj.GetComponent<Piece>().toppositionnumber;
            Debug.Log("Top position number: " + tempnumber);
        }
        samePosDictionary[nenum].Sort((go1, go2) =>
        go1.GetComponent<Piece>().toppositionnumber.CompareTo(go2.GetComponent<Piece>().toppositionnumber));

        PrintSamePosDictionaryContents(nenum);

        //samePosDictionary[CurrentPosition].Add(gameObject);
        //samePosDictionary[pieceposnumber].Add(psobject);
    }
    public void PrintSamePosDictionaryContents(int nenum)
    {
        foreach (KeyValuePair<int, List<GameObject>> kvp in samePosDictionary)
        {
            string logMessage = "Position " + kvp.Key + " contains: ";
            foreach (GameObject obj in kvp.Value)
            {
                // Append the GameObject's name (or any property you want to display)
                logMessage += obj.name + ", ";
            }
            Debug.Log(logMessage);
        }
    }

    public void sameposswapping()
    {
        int nnum = currentposbasedoncolor(colornum,CurrentPosition);
        // Ensure the key exists in the dictionary
        if (!samePosDictionary.ContainsKey(nnum))
        {
            Debug.LogError("No objects found at CurrentPosition.");
            return;
        }

        List<GameObject> objectsAtPosition = samePosDictionary[nnum];

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

        movesortorder(gameObject, targetObject, true, false);

        if(currentPiece.toppositionnumber == 0)
        {
            currentPiece.ispressedbyotherpiece = false;
        }
        else
        {
            currentPiece.ispressedbyotherpiece = true;
        }
        if(targetPiece.toppositionnumber == 0)
        {
            targetPiece.ispressedbyotherpiece = false;
        }
        else
        {
            targetPiece.ispressedbyotherpiece = true;
        }

        Debug.Log($"Swapped toppositionnumber between {gameObject.name} and {targetObject.name}");
    }

    public void listmaintain()
    {
        int nnum = currentposbasedoncolor(colornum,olddictionaryposition);
        if (samePosDictionary.ContainsKey(nnum))
        {
            Debug.Log("OldDictPosition:"+olddictionaryposition+":CurrentPos:"+CurrentPosition);
            if (olddictionaryposition != beforecurrrentposition)
            {
            Debug.Log("Listmaintain2");
                samePosDictionary[nnum].Remove(gameObject);
            
                foreach(GameObject obj in samePosDictionary[nnum])
                {
                    movesortorder(gameObject, gameObject, false, false);
                    //movesortorder(obj, obj, false, false);

                    Piece objpiece  = obj.GetComponent<Piece>();
                    if(objpiece.toppositionnumber >= 1)
                    objpiece.toppositionnumber = objpiece.toppositionnumber - 1 ;

                    if(objpiece.toppositionnumber == 0)
                    {
                        objpiece.ispressedbyotherpiece = false;
                    }
                }

                olddictionaryposition = beforecurrrentposition;
            }

        }
    }

    private void Start()
    {

        
        checkplayerenable();
        IsInBase = true;
        lastposition = CurrentPosition;
        olddictionaryposition = CurrentPosition;
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


        //if(CurrentPosition != olddictionaryposition)
        //{
        //    listmaintain();
        //}

        if(PieceManager.lastmovepiece!= null)
        {
            if(PieceManager.lastmovepiece.gameObject != this.gameObject )
            {
                wasjustmoving = false;
                lastposition = CurrentPosition;
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
    public void movesortorder(GameObject gm, GameObject gm1, bool swap, bool add)
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
            if(add == false)
            {
                childRenderer1.sortingOrder = childRenderer1.sortingOrder - 1;
                childRenderer2.sortingOrder = childRenderer2.sortingOrder - 1;

            }
            else
            {

                childRenderer1.sortingOrder = childRenderer1.sortingOrder + 1;
                childRenderer2.sortingOrder = childRenderer2.sortingOrder + 1;
            }
        }

        

        
    }


    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if(other.gameObject.CompareTag("Piece"))
    //    {
    //        if(other.gameObject.GetComponent<Piece>().colornum != colornum)
    //             listmaintain();

    //    }
    //}


   public bool pendingListUpdate = false;
    public bool newpendingUpdate = false;
    public static bool endtriggeronetime = false;
    void OnTriggerExit2D(Collider2D collision)
    {
        if(beforecurrrentposition != lastposition)
        {
            if(endtriggeronetime == false)
            {
                int newnum = currentposbasedoncolor(colornum, lastposition);
                if (collision.gameObject.CompareTag("Piece"))
                {
                    //if (collision.gameObject.GetComponent<Piece>().colornum != colornum)

                    Piece ps1 = collision.gameObject.GetComponent<Piece>();
                    int newnum1 = currentposbasedoncolor(ps1.colornum, ps1.CurrentPosition);
                    Debug.Log(CurrentPosition + "CurrPos:" + olddictionaryposition + ":dictpos"+beforecurrrentposition+":BeforeCurrentpos:"+newnum+":newnum:"+newnum1+"newnum1"+gameObject.name+":Name:"+ps1.CurrentPosition);
                    //if(CurrentPosition !=  olddictionaryposition)
                    {
                        //if (wasjustmoving)
                        {
                            if (ps1.CurrentPosition == 0 || ps1.CurrentPosition == 8 || ps1.CurrentPosition == 13
        || ps1.CurrentPosition == 21 || ps1.CurrentPosition == 26 || ps1.CurrentPosition == 34
        || ps1.CurrentPosition == 39 || ps1.CurrentPosition == 47)
                            {
                                if (CurrentPosition == 0 || CurrentPosition == 8 || CurrentPosition == 13
            || CurrentPosition == 21 || CurrentPosition == 26 || CurrentPosition == 34
            || CurrentPosition == 39 || CurrentPosition == 47)
                                //if (ismoving == false)
                                //if(newnum1 == newnum)
                                {

                                Debug.Log("Inside");
                                //if(CurrentPosition != lastposition)
                                //pendingListUpdate = true;
                                newpendingUpdate = true;

                                }
                            }
                        }

                    }
                }
                endtriggeronetime = true;
            }

        }
    }

    void FixedUpdate()
    {
        if(newpendingUpdate)
        {
            if(ismoving == false)
            {
                pendingListUpdate = true;
            }
        }
        //listmaintain();
        if (pendingListUpdate)
        {
            Debug.Log("InsideUpdate:"+CurrentPosition + "CurrPos:" + olddictionaryposition + ":dictpos:" + beforecurrrentposition + ":BeforeCurrentpos:");
            if (beforecurrrentposition != olddictionaryposition)
            {
                if(ismoving==false)
                {
                    listmaintain();
                    pendingListUpdate = false;
                    newpendingUpdate = false;

                }

            }
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
                    //if (colornum != ps.colornum)
                    {
                        if (ps.ismoving == false)
                        {
                            if (CurrentPosition == 0 || CurrentPosition == 8 || CurrentPosition == 13
                    || CurrentPosition == 21 || CurrentPosition == 26 || CurrentPosition == 34
                    || CurrentPosition == 39 || CurrentPosition == 47)
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
                                        //childRenderer1.sortingOrder = pieceChildRenderers[0].sortingOrder + 1;
                                        //childRenderer2.sortingOrder = pieceChildRenderers[1].sortingOrder + 1;
                                        //ps.gameObject.GetComponent<Collider2D>().enabled = false;
                                        ps.ispressedbyotherpiece = true;
                                        //ps.toppositionnumber++;

                               
                                        samepositionmaintainlist(ps.toppositionnumber, ps.gameObject, ps);
                                        Debug.Log("PieceColornum:" + colornum+":++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                                        Debug.Log("Lastpos:" + lastposition + ":CurrentPostion:" + CurrentPosition+":LastPieceMove:"+PieceManager.lastmovepiece);
                                        lastposition = CurrentPosition;
                                        

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
                                if (colornum != ps.colornum)
                                {
                                    cutpiece(ps.colornum, ps.piecenumber, ps.gameObject);

                                    ps.IsInBase = true;
                                    ps.CurrentPosition = -1;
                                    ps.lastposition = ps.CurrentPosition;

                                    homeorcutturn = true;

                                }

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