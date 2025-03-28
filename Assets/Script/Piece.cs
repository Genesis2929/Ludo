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


    public GameObject barriergameobject;


    public bool ispressedbyotherpiece = false;

    public static bool redhavecut = false, greenhavecut = false, bluehavecut = false, yellowhavecut = false;
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
    public static int redhomenum, greenhomenum, yellowhomenum, bluehomenum;

    public static Dictionary<int, List<GameObject>> samePosDictionary = new Dictionary<int, List<GameObject>>();
    void havecutupdate()
    {
        if(colornum == 0)
        {
            if(greenhavecut == false)
            {
                foreach(Piece psss in PieceManager.p1piece)
                {
                    if(psss.CurrentPosition == 51)
                    {
                        psss.CurrentPosition = -1;
                    }

                }
            }
            greenhavecut = true;
        }
        else if(colornum == 1)
        {
            if (yellowhavecut == false)
            {
                foreach (Piece psss in PieceManager.p2piece)
                {
                    if (psss.CurrentPosition == 51)
                    {
                        psss.CurrentPosition = -1;
                    }

                }
            }
            yellowhavecut = true;
        }
        else if(colornum == 2)
        {
            if (bluehavecut == false)
            {
                foreach (Piece psss in PieceManager.p3piece)
                {
                    if (psss.CurrentPosition == 51)
                    {
                        psss.CurrentPosition = -1;
                    }

                }
            }
            bluehavecut = true;
        }
        else
        {
            if (redhavecut == false)
            {
                foreach (Piece psss in PieceManager.p4piece)
                {
                    if (psss.CurrentPosition == 51)
                    {
                        psss.CurrentPosition = -1;
                    }

                }
            }
            redhavecut = true;  
        }
    }
    public int currentposbasedoncolor(int cnum, int currentposnumber)
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

    public bool gamewin(int nummm)
    {
        if(colornum == 0)
        {
            if((greenhomenum + nummm) == optionscript.coinnumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(colornum == 1)
        {
            if ((yellowhomenum + nummm)== optionscript.coinnumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (colornum == 2)
        {
            if ((bluehomenum+nummm) == optionscript.coinnumber)
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
            if ((redhomenum + nummm)== optionscript.coinnumber)
            {
                return true;
            }
            else
            {
                return false;
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
        redhomenum = 0; 
        greenhomenum = 0;
        yellowhomenum = 0;
        bluehomenum = 0;

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
        //this.gameObject.GetComponent<Collider2D>().enabled = false;
        IsInHome = true;

        if(colornum == 0)
        {
            greenhomenum++;

            //if(greenhomenum == optionscript.coinnumber)
            //{
            //    PieceManager.onpiecehomecomplete(PieceManager.p1piece);
            //}
        }
        if (colornum == 1)
        {
            yellowhomenum++;

            //if (yellowhomenum == optionscript.coinnumber)
            //{
            //    PieceManager.onpiecehomecomplete(PieceManager.p2piece);
            //}
        }
        if (colornum == 2)
        {
            bluehomenum++;

            //if (bluehomenum == optionscript.coinnumber)
            //{
            //    PieceManager.onpiecehomecomplete(PieceManager.p3piece);
            //}
        }
        if (colornum == 3)
        {
            redhomenum++;

            //if (redhomenum == optionscript.coinnumber)
            //{
            //    PieceManager.onpiecehomecomplete(PieceManager.p4piece);
            //}
        }
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


        if(optionscript.barrier)
        {
            if(barrieron)
            {
                if(ismoving == false)
                {
                    if(forshowstar(true, CurrentPosition))
                    {
                        barrierbreak(false, null);
                    }
                    else
                    {
                        if (optionscript.showstar)
                        {
                            if (forshowstar(false, CurrentPosition))
                            {
                                barrierbreak(false, null);
                            }
                        }
                    }

                }
            }
        }

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
    //public void ToggleHighlight(bool state)
    //{
    //    highlightEffect.SetActive(state);
    //}

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

    public bool piecehavecut()
    {
        if(colornum == 0)
        {
            return greenhavecut;
        }
        else if (colornum == 1)
        {
            return yellowhavecut;
        }
        else if (colornum == 2)
        {
            return bluehavecut;
        }
        else
        {
            return redhavecut;
        }
    }
    public void touchclick()
    {
        Debug.Log("OnMouseDOwn");
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
                Debug.Log("OnMouseDOwn2");
                if (PieceManager.selectedpiecemove)
                {
                Debug.Log("OnMouseDOwn3");
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


    public void movesortorder(GameObject gm, GameObject gm1, bool swap, bool add)
    {

        // Here we assume that "certainpiece" (this gameObject) has exactly two children.
        SpriteRenderer childRenderer1 = gm.transform.GetChild(0).GetComponent<SpriteRenderer>();
        //SpriteRenderer childRenderer2 = gm.transform.GetChild(1).GetComponent<SpriteRenderer>();

        if(swap)
        {
            SpriteRenderer childRenderer11 = gm1.transform.GetChild(0).GetComponent<SpriteRenderer>();
            //SpriteRenderer childRenderer22 = gm1.transform.GetChild(1).GetComponent<SpriteRenderer>();

            int tempsortingorder;
            // Safety check in case one of the children is missing a SpriteRenderer.
            if (childRenderer1 != null && childRenderer11 != null)
            {
                tempsortingorder = childRenderer1.sortingOrder;
                childRenderer1.sortingOrder = childRenderer11.sortingOrder;
                childRenderer11.sortingOrder = tempsortingorder;

                //tempsortingorder = childRenderer2.sortingOrder;
                //childRenderer2.sortingOrder = childRenderer22.sortingOrder;
                //childRenderer22.sortingOrder = tempsortingorder;
            }

        }
        else
        {
            if(add == false)
            {
                childRenderer1.sortingOrder = childRenderer1.sortingOrder - 1;
                //childRenderer2.sortingOrder = childRenderer2.sortingOrder - 1;

            }
            else
            {

                childRenderer1.sortingOrder = childRenderer1.sortingOrder + 1;
                //childRenderer2.sortingOrder = childRenderer2.sortingOrder + 1;
            }
        }

        

        
    }




   public bool pendingListUpdate = false;
    public bool newpendingUpdate = false;
    public static bool endtriggeronetime = false;

    void forcollision(GameObject gm, Piece ps)
    {
        SpriteRenderer[] pieceChildRenderers = gm.GetComponentsInChildren<SpriteRenderer>();

        Debug.Log("::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");

        // Ensure that we have at least 2 child SpriteRenderers in the Piece object.
        if (pieceChildRenderers.Length >= 1)
        {
            // Get the child SpriteRenderer components from "certainpiece"
            // Here we assume that "certainpiece" (this gameObject) has exactly two children.
            SpriteRenderer childRenderer1 = transform.GetChild(0).GetComponent<SpriteRenderer>();
            //SpriteRenderer childRenderer2 = transform.GetChild(1).GetComponent<SpriteRenderer>();

            // Safety check in case one of the children is missing a SpriteRenderer.
            if (childRenderer1 != null)
            {
                // Assign the sorting order from the Piece's children to the corresponding children in "certainpiece".
                //childRenderer1.sortingOrder = pieceChildRenderers[0].sortingOrder + 1;
                //childRenderer2.sortingOrder = pieceChildRenderers[1].sortingOrder + 1;
                //ps.gameObject.GetComponent<Collider2D>().enabled = false;
                //ps.ispressedbyotherpiece = true;
                //ps.toppositionnumber++;


                //samepositionmaintainlist(ps.toppositionnumber, ps.gameObject, ps);
                Debug.Log("PieceColornum:" + colornum + ":++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Debug.Log("Lastpos:" + lastposition + ":CurrentPostion:" + CurrentPosition + ":LastPieceMove:" + PieceManager.lastmovepiece);
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

   public bool forshowstar(bool mainstar, int curpos)
    {
        if(mainstar)
        {
            if (curpos == 0 || curpos == 13
|| curpos == 26 || curpos == 39)
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
            if (curpos == 8 || curpos == 21
|| curpos == 34 || curpos == 47)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public LudoDice2D dicesystem;
    public int collidednum = 0;
    
    void sidesit(Piece gm)
    {
        Vector3 prepos = gm.gameObject.transform.position;
        for(int i=0; i< gm.collidednum; i++)
        {
            int jn = -1;
            int kn = 1;

            if(i %2 != 0)
            {
                kn = 2 * kn;
            }
            gm.gameObject.transform.position = prepos + Mathf.Pow(jn, i+1) * new Vector3(0.02f, 0.02f, 0) *kn;
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


                    //if(ps.CurrentPosition == CurrentPosition)
                    if (currentposbasedoncolor(colornum, CurrentPosition) == currentposbasedoncolor(ps.colornum, ps.CurrentPosition))
                    //if (colornum != ps.colornum)
                    {
                        if (ps.ismoving == false)
                        {
                    //        if (CurrentPosition == 0 || CurrentPosition == 8 || CurrentPosition == 13
                    //|| CurrentPosition == 21 || CurrentPosition == 26 || CurrentPosition == 34
                    //|| CurrentPosition == 39 || CurrentPosition == 47)
                    if(forshowstar(true, CurrentPosition) == true)
                            {
                                // Get all child SpriteRenderer components from the Piece object.
                                forcollision(collision.gameObject, ps);
                                collidednum++;
                                ps.collidednum++;
                                sidesit(ps);
                            }
                            //    }
                            //if (collision.gameObject.CompareTag("Piece"))
                            //{
                            else
                            {
                                if (optionscript.showstar)
                                {
                                    if (forshowstar(false, CurrentPosition) == true)
                                    {
                                        forcollision(collision.gameObject, ps);
                                        collidednum++;
                                        ps.collidednum++;
                                        sidesit(ps);
                                    }
                                    else
                                    {
                                        if (colornum != ps.colornum)
                                        {
                                            cutpiece(ps.colornum, ps.piecenumber, ps.gameObject);

                                            if (optionscript.barrier && ps.barrieron)
                                            {
                                                barrierbreak(true, ps);
                                            }

                                            ps.IsInBase = true;
                                            ps.CurrentPosition = -1;
                                            ps.lastposition = ps.CurrentPosition;

                                            havecutupdate();

                                            if (optionscript.cutgainturn)
                                                homeorcutturn = true;


                                            if (homeorcutturn)
                                            {
                                                dicesystem.homecutturning();
                                            }

                                        }
                                        else
                                        {
                                            formbarrier(ps);
                                        }
                                    }
                                }
                                else
                                {
                                    if (colornum != ps.colornum)
                                    {
                                        cutpiece(ps.colornum, ps.piecenumber, ps.gameObject);

                                        if(optionscript.barrier && ps.barrieron)
                                        {
                                            barrierbreak(true,ps);
                                        }

                                        ps.IsInBase = true;
                                        ps.CurrentPosition = -1;
                                        ps.lastposition = ps.CurrentPosition;

                                        havecutupdate();

                                        if (optionscript.cutgainturn)
                                            homeorcutturn = true;

                                        if(homeorcutturn)
                                        {
                                            dicesystem.homecutturning();
                                        }

                                        

                                    }
                                    else
                                    {
                                        formbarrier(ps);
                                    }
                                }

                                //GameObject gobj = ps.gameObject;


                            }

                        }
                        Debug.Log("CurrentPosition:" + CurrentPosition);
                        //Debug.Log("Collided with an object tagged as 'Piece'");
                        // Add your logic here for when the object with the tag "Piece" collides.
                    }

                }
                else if (collision.gameObject.CompareTag("Home"))
                {
                    if(optionscript.homegainturn)
                    {
                         homeorcutturn = true;

                    }
                    if (homeorcutturn)
                    {
                        if(gamewin(1))
                        {
                            homeorcutturn = false;
                        }
                        else
                        {
                            dicesystem.homecutturning();
                        }
                    }
                    homereached();
                }

                wasjustmoving = false;
            }

        }
    }

    public bool barrieron = false;
    public bool pieceinsidepiece = false;
    public static List<Piece> barrierpos = new List<Piece>();
    public List<Piece> barrierposition = new List<Piece>();
    public void formbarrier(Piece ps)
    {
        if(optionscript.barrier)
        {
            if(barrieron == false)
            {
                barrierpos.Add(gameObject.GetComponent<Piece>());
                barrierposition = barrierpos;
                barrieron = true;
                ps.barrieron = true;
                //GameObject g1 = Instantiate(barriergameobject);
                //g1.transform.position = transform.position;
                ps.gameObject.transform.position = transform.position + new Vector3(0.05f , 0.05f ,0);

                //gameObject.transform.SetParent(g1.transform);
                ps.gameObject.transform.SetParent(gameObject.transform);
                ps.pieceinsidepiece = true;
                ps.gameObject.GetComponent<CircleCollider2D>().enabled = false;
           

                //ps.gameObject.GetComponent<Piece>().enabled = false;
                //gameObject.GetComponent<Piece>().enabled = false;

            }

        }
    }

    public void barrierbreak(bool childcut, Piece pie)
    {
        GameObject childgm = null;
        //barrierpos.Remove(gameObject.transform);
        if (childcut)
        {
            barrierpos.Remove(pie);
            pie.barrieron = false;
            childgm = pie.gameObject.transform.GetChild(2).gameObject;
        }
        else
        {
            barrierpos.Remove(gameObject.GetComponent<Piece>());
            barrieron = false;
            childgm = gameObject.transform.GetChild(2).gameObject;
            //gameObject.transform.SetParent(null);
        }

        GameObject parentobj =gameObject.transform.parent.gameObject;
        Piece ps = childgm.GetComponent<Piece>();
        ps.barrieron = false;

        ps.gameObject.transform.position = transform.position;
        ps.CurrentPosition = CurrentPosition;
        ps.lastposition = ps.CurrentPosition;
        ps.gameObject.transform.SetParent(parentobj.transform);
        ps.pieceinsidepiece=false;
        ps.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        barrierposition = barrierpos;

        if (childcut)
        {
            cutpiece(ps.colornum, ps.piecenumber, childgm);
            ps.IsInBase = true;
            ps.CurrentPosition = -1;
            ps.lastposition = ps.CurrentPosition;
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