using UnityEngine;
//using UnityEngine.UI;
using System.Collections;
using NUnit.Framework.Interfaces;

public class LudoDice2D : MonoBehaviour
{
    [Header("Dice Settings")]
    [SerializeField] private Sprite[] greendiceSprites; // 6 sprites for dice faces (1-6)
    [SerializeField] private Sprite[] greendiceAnim; // 6 sprites for dice faces (1-6)

    [SerializeField] private Sprite[] bluediceSprites; // 6 sprites for dice faces (1-6)
    [SerializeField] private Sprite[] bluediceAnim; // 6 sprites for dice faces (1-6)

    [SerializeField] private Sprite[] reddiceSprites; // 6 sprites for dice faces (1-6)
    [SerializeField] private Sprite[] reddiceAnim; // 6 sprites for dice faces (1-6)

    [SerializeField] private Sprite[] yellowdiceSprites; // 6 sprites for dice faces (1-6)
    [SerializeField] private Sprite[] yellowdiceAnim; // 6 sprites for dice faces (1-6)

    [SerializeField] private SpriteRenderer diceRenderer;
    [SerializeField] private float rollDuration = 1.5f;
    [SerializeField] private int rollAnimFrames = 15;

    [Header("Player Turn Settings")]
    [SerializeField] private Color[] playerColors = new Color[4];
    //[SerializeField] private Text turnText;
    public int currentPlayerIndex = 0;
    //private Quaternion initialRotation;
    private bool isRolling = false;
    private int finalValue;
    private bool allowInteraction = true;
    public float rotationSpeed = 360f;

    public int CurrentPlayer => currentPlayerIndex + 1;
    public static event System.Action<int, int> OnDiceRollCompleted;

    public bool oneappeardice = false;

    public int dicecount = 0;
    public int oneorsix = 1;

    public int oneorsixcounter = 0;

    public static bool threehome = false;

    public static bool threeoneorsix = false;

    //public int playernum = 4, player1 = 0 , player2 = 1, player3 = 2, player4 = 3;
    public static int playernum = 4, player1 = 0, player2 = 1, player3 = 2, player4 = 3;
    public  int playernumber = 4, playernumber1 = 0, playernumber2 = 1, playernumber3 = 2, playernumber4 = 3;
    public int AIplayernum = 0, AIplayernum1 = 0, AIplayernum2 = 1, AIplayernum3 = 2;
    public static int AIstaticnum = 0, AIstaticnum1 = 0, AIstaticnum2 = 1, AIstaticnum3 = 2;
    public static int AIjustnumber = 0;
    public static int AInumber;
    [SerializeField] private float throwForceMultiplier = 50f;
    public int probabilityplayer =0;

    public float dragamount = 1f;
    int prevdicenumber = 0;

    public Rigidbody2D rb;

    public bool isAI = false;
    public static bool isAIturn = false;

    private bool isDragging = false;
    private Vector2 dragStartPos;
    private Vector2 lastTouchPos;

    public bool AIagain = false;

    public static bool forAIdiceroll = false;

    public int noofAI = 0;

    public static int numberofAI = 0;

    public bool thiscodecomplete = false;
    public bool piecemanagercodecomplete = false;

    public int tempdicevalue = 0, tempplayerindex = 0;

    public int allowinteractionnumber = 0;
    //private Rigidbody2D rb;
    int firstime = 0;
    // Tweak this value in the Inspector to adjust throw strength.
    [SerializeField]
    private float dragForceMultiplier = 5f;
    public bool touchbool = false;

    bool oneofai = false;

    public static int AIforPieceManagerNumber = 0;
    public bool coroutinecompletes = false;
    void Awake()
    {
        noofAI = 0;
        coroutinecompletes = false;
        touchbool = false;
        oneofai = false;
        thiscodecomplete = true;
        piecemanagercodecomplete = true;
        allowinteractionnumber = 0;
        firstime = 0;
        //playernum = 4;
        //if(noofAI > 0)
        //{
        //    playernumber = 4 - noofAI;
        //}
        playernum = playernumber;
        player1 = playernumber1;
        player2 = playernumber2;
        player3 = playernumber3;
        player4 = playernumber4;



        //initialRotation = transform.rotation;
        if (diceRenderer == null)
            diceRenderer = GetComponent<SpriteRenderer>();


        playerAIorhumandecide();

        Playerpreferenceupdate();
        currentPlayerIndex = playernumber1;
        changedicecoloronturn(1);
        AInumber = AIplayernum;
        allowInteraction = true;
        thiscodecomplete = true;
        piecemanagercodecomplete = true;
        AIjustnumber = AIplayernum1;
        //UpdateTurnDisplay();
    }
    void playernumberupdating()
    {
        playernumber = playernum;
        playernumber1 = player1;
        playernumber2 = player2;
        playernumber3 = player3;
        playernumber4 = player4;

        AIplayernum = AIstaticnum;
        noofAI = AIstaticnum;
        numberofAI = AIstaticnum;
        AIplayernum1 = AIstaticnum1;
        AIplayernum2 = AIstaticnum2;
        AIplayernum3 = AIstaticnum3;

    }
    public int secondoneorsix;
    private void Start()
    {
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

        AIstaticnum = noofAI;
        AIstaticnum1 = AIplayernum1;
        AIstaticnum2 = AIplayernum2;
        AIstaticnum3 = AIplayernum3;
    }
    void Playerpreferenceupdate()
    {

        if (PlayerPrefs.HasKey("Fling"))
        {
            int fli = PlayerPrefs.GetInt("Fling");

            if (fli == 0)
            {
                optionscript.flingenable = false;
            }
            else if (fli == 1)
            {
                optionscript.flingenable = true;
            }
        }

        if (PlayerPrefs.HasKey("Difflevel"))
        {
            optionscript.difficultylevel = PlayerPrefs.GetInt("Difflevel");
        }

        if(PlayerPrefs.HasKey("CoinNumber"))
        {
            optionscript.coinnumber = PlayerPrefs.GetInt("CoinNumber");
        }
    }
    void playerAIorhumandecide()
    {
        if(PlayerPrefs.HasKey("Player1") && PlayerPrefs.HasKey("Player2") && PlayerPrefs.HasKey("Player3") && PlayerPrefs.HasKey("Player4") && PlayerPrefs.HasKey("PlayerNumber"))
        {
            int p1, p2, p3, p4, pnum, Ai1, Ai2, Ai3, Ai4, Ainum;
             p1 = PlayerPrefs.GetInt("Player1");
             p2 = PlayerPrefs.GetInt("Player2");
             p3 = PlayerPrefs.GetInt("Player3");
             p4 = PlayerPrefs.GetInt("Player4");
             pnum = PlayerPrefs.GetInt("PlayerNumber");

            Ai1 = PlayerPrefs.GetInt("AI1");
            Ai2 = PlayerPrefs.GetInt("AI2");
            Ai3 = PlayerPrefs.GetInt("AI3");
            Ai4 = PlayerPrefs.GetInt("AI4");
            Ainum = PlayerPrefs.GetInt("AINumber");



            if(p1 == 1)
            {
                player1 = 0;
                if(p2 == 1)
                {
                    player2 = 1;

                    if(p3 == 1)
                    {
                        player3 = 2;
                        if(p4 == 1)
                        {
                            player4 = 3;
                        }
                    }
                }
                else if(p3 == 1)
                {
                    player2 = 2;
                    if(p4 == 1)
                    {
                        player3 = 3;
                    }
                }
                else if(p4 == 1)
                {
                    player2 = 3;
                }
            }
            else if(p2 == 1)
            {
                player1 = 1;
                if(p3 == 1)
                {
                    player2 = 2;
                    if(p4 == 1)
                    {
                        player3 = 3;
                    }
                }
                else if(p4 == 1)
                {
                    player2 = 3;
                }
            }
            else if(p3 == 1)
            {
                player1 = 2;
                if(p4 == 1)
                {
                    player2 = 3;
                }
            }
            AIplayernum = Ainum;
            noofAI = Ainum;
            numberofAI = Ainum;
            //if(Ainum == 1)
            {
                if (Ai1 == 1)
                {
                    AIplayernum1 = 0;

                    if (Ai2 == 1)
                    {
                        AIplayernum2 = 1;
                        if (Ai3 == 1)
                        {
                            AIplayernum3 = 2;
                        }
                    }
                    else if (Ai3 == 1)
                    {
                        AIplayernum2 = 2;
                    }
                }
                else if (Ai2 == 1)
                {
                    AIplayernum1 = 1;
                    if (Ai3 == 1)
                    {
                        AIplayernum2 = 2;

                        if (Ai4 == 1)
                        {
                            AIplayernum3 = 3;
                        }
                    }
                    else if (Ai4 == 1)
                    {
                        AIplayernum2 = 3;
                    }
                }
                else if(Ai3 == 1)
                {
                    AIplayernum1 = 2;
                    if(Ai4 == 1)
                    {
                        AIplayernum2 = 3;
                    }
                }
                else if(Ai4 == 1)
                {
                    AIplayernum1 = 3;
                }
            }
            //if(pnum == 2)
            
                //player1 = p1;
                //player2 = p2;
                //player3 = p3;
                //player4 = p4;
                playernum = pnum;

                playernumber1 = player1;
                playernumber2 = player2;
                playernumber3 = player3;
                playernumber4 = player4;
                playernumber = pnum;

                Debug.Log(player1+player2 +player3 +player4 +playernum);    
                
            

            
        }
    }

    public void AIrollfunc()
    {
        if (noofAI == 0)
        {
            //StartCoroutine(touchroll());
            touchrollbool = true;
        }
        else if (noofAI == 1)
        {
            StartCoroutine(AIfunc(noofAI));

        }
        else if (noofAI == 2)
        {

            //AIfunc(noofAI);
            StartCoroutine(AIfunc(noofAI));
        }

        else if (noofAI == 3)
        {
            StartCoroutine(AIfunc(noofAI));
        }
    }
    public void AIsubfunc(int numberofai, bool forward)
    {
        Debug.Log("AIAGAIN:" + AIagain);
        //Debug.Log("AInumberchange");

        if (forward)
        {
            if(numberofai == 1)
            {
                return;
            }

            else if (numberofai == 2)
            {

                if (AIagain == false)
                {
                    if (AIjustnumber == AIplayernum1)
                    {
                        AIjustnumber = AIplayernum2;
                    }
                    else if (AIjustnumber == AIplayernum2)
                    {
                        AIjustnumber = AIplayernum1;
                    }
                }

            }
            else if (numberofai == 3)
            {
                if (AIagain == false)
                {
                    if (AIjustnumber == AIplayernum1)
                    {
                        AIjustnumber = AIplayernum2;
                    }
                    else if (AIjustnumber == AIplayernum2)
                    {
                        AIjustnumber = AIplayernum3;
                    }
                    else if (AIjustnumber == AIplayernum3)
                    {
                        AIjustnumber = AIplayernum1;
                    }
                }
            }

        }
        else
        {
            if (numberofai == 1)
            {
                return;
            }

            else if (numberofai == 2)
            {

                if (AIagain == false)
                {
                    if (AIjustnumber == AIplayernum1)
                    {
                        AIjustnumber = AIplayernum2;
                    }
                    else if (AIjustnumber == AIplayernum2)
                    {
                        AIjustnumber = AIplayernum1;
                    }
                }

            }
            else if (numberofai == 3)
            {
                if (AIagain == false)
                {
                    if (AIjustnumber == AIplayernum1)
                    {
                        AIjustnumber = AIplayernum3;
                    }
                    else if (AIjustnumber == AIplayernum2)
                    {
                        AIjustnumber = AIplayernum1;
                    }
                    else if (AIjustnumber == AIplayernum3)
                    {
                        AIjustnumber = AIplayernum2;
                    }
                }
            }
        }

    }

    public bool turnchangecheck = false;
    int checkturningruncount = 0;
    int prevainum = 0;
    IEnumerator AIfunc(int numberofai)
    {
        if (allowInteraction == true && thiscodecomplete == true && piecemanagercodecomplete == true)
        {

            //Debug.Log("In AIfunc at start:" + "Currentplayerindex:" + currentPlayerIndex + ":AIjustnumber:" + AIjustnumber);
            Debug.Log("CurrentPlayerAndAIjustnumber:" + currentPlayerIndex + ":" + AIjustnumber + ":::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");

            if (currentPlayerIndex == AIjustnumber)
            {
                if (firstime == 0)
                {
                    AIjustnumber = AIplayernum1;
                    firstime++;
                }
                else
                {
                    if (noofAI > 1)
                        if (currentPlayerIndex == prevainum)
                        {
                            isAI = true;
                        }

                }
                Debug.Log("A2"+prevainum);
                oneofai = false;
                thiscodecomplete = false;
                piecemanagercodecomplete = false;
                AInumber = currentPlayerIndex;
                if (isAI == false || AIagain == true)
                {
                    Debug.Log("A3");
                    bool changed = gameObject.GetComponent<Collider2D>().enabled;
                    //Debug.Log("ColliderBool:" + changed);

                    //Debug.Log("allowinteraction:" + allowInteraction + ":Changed:" + changed);
                    if (changed)
                    {
                        //Debug.Log("A4");
                        AIagain = false;

                        isAIturn = true;
                        //isAI = true;
                        dicenumberchange();
                        dicecount++;


                        prevainum = AIjustnumber;
                        //PieceManager.AIenable = false;
                        AIforPieceManagerNumber = AIjustnumber;
                        if (turnchangecheck == false)
                        {
                            //StartCoroutine(RollDice());
                            PieceManager.isitreallyAI = true;
                            yield return StartCoroutine(RollDice());
                            checkturningruncount = 0;
                            //while (coroutinecompletes == false)
                            //{
                            //    Debug.Log("wait for coroutine to  complete");
                            //}

                        }
                        else
                        {
                            piecemanagercodecomplete = true;
                        }
                        //else
                        //{
                        //    turnchangecheck = false;
                        //    isAIturn = false;
                        //}
                        //Debug.Log("AI:" + currentPlayerIndex);
                        thiscodecomplete = true;
                        //isAI = false;
                        //AIjustnumber = currentPlayerIndex;


                    }
                    else
                    {
                        thiscodecomplete = true;
                        piecemanagercodecomplete = true;
                    }

                    if (turnchangecheck)
                    {
                        //turnchangecheck = false;
                        isAIturn = false;
                        isAI = false;
                    }
                    else
                    {

                        //isAI = true;
                    }
                    //if (turnchangecheck)
                    //{
                    //    turnchangecheck = false;
                    //}
                    //else
                    //{

                    //    AIsubfunc(numberofai, true);
                    //}

                }
                else
                {
                    thiscodecomplete = true;
                    piecemanagercodecomplete = true;

                }

                isAI= false;

            }
            //else if(oneofai == false)
            //{
            //    thiscodecomplete= true;
            //    piecemanagercodecomplete= true;
            ////AIagain = true;
            //}
            else
            {
                //Debug.Log("A5");
                //Debug.Log("Not equal");
                //allowInteraction = false;
                piecemanagercodecomplete=false; 
                thiscodecomplete = false;
                //yield return StartCoroutine(touchroll());
                touchrollbool = true;
                //touchbool = true;

                //AIsubfunc(numberofai, true);
                //oneofai = true;
                //AIagain = true;
            }

        }
    }
    public bool touchrollbool = false;
    IEnumerator touchroll()
    {
        //Debug.Log("A5232332");
        //Debug.Log("Inside touchroll");
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Convert screen position to world position
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                // Check if this dice is touched using a raycast
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Dice"))
                {
                    isDragging = true;
                    dragStartPos = touchPosition;
                    lastTouchPos = touchPosition;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (isDragging)
                {
                    if (optionscript.flingenable)
                    {
                        // Option 1: Use MovePosition to let physics move the dice while dragging
                        rb.MovePosition(touchPosition);
                        // Alternatively, you could use transform.position = touchPosition;
                        lastTouchPos = touchPosition;

                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (isDragging)
                {

                    if (optionscript.flingenable)
                    {
                        // Calculate the drag delta and then compute a throw force.
                        Vector2 dragDelta = touchPosition - dragStartPos;

                        // If you want to base the throw on the overall drag distance:
                        Vector2 throwForce = dragDelta * dragForceMultiplier;

                        // Or, if you prefer to use the last movement's direction:
                        // Vector2 throwDirection = (touchPosition - lastTouchPos).normalized;
                        // Vector2 throwForce = throwDirection * dragForceMultiplier;

                        rb.AddForce(throwForce, ForceMode2D.Impulse);

                    }

                    if (!isRolling && allowInteraction)
                    {
                        dicenumberchange();
                        dicecount++;
                        //checkingturnandskip(false);

                        yield return StartCoroutine(RollDice());
                         touchrollbool = false;
                        thiscodecomplete = true;
                    }

                    isDragging = false;
                }
            }
        }
    }

    private float timer = 0f;
    public float interval = 0.5f; // Seconds

    int diffcurnum = -1;
    public void Update()
    {
        playernumberupdating();
        //collisioncheckbool = false;
        //Debug.Log("Update"+touchrollbool);
        if (touchrollbool)
        {
            StartCoroutine(touchroll());
            //AIsubfunc(noofAI, true);
        }
        if(diffcurnum != currentPlayerIndex)
        {
            threeoneorsix = false;
            seconddice3times = false;
            oneorsixcounter = 0;
            seconddicecounter = 0;
        }
        diffcurnum = currentPlayerIndex;
        numberofAI = noofAI;
        AIrollfunc();

        //timer += Time.deltaTime;

        //if (timer >= interval)
        //{

        //    timer = 0f; // Reset timer
        //}


    }

    public bool collisiontruepiece = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        //if(collisioncheckbool)
        {
            if (collision.gameObject.CompareTag("Piece"))
            {
                //Debug.Log("Colision::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
                //SetTransparency(0.5f); // Reduce transparency (0.5 means 50% transparent)
                collisiontruepiece = true;
            }

        }
        //else
        //{
        //    SetTransparency(1f);
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Piece"))
        {
            //Debug.Log("23432Colision::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
            //SetTransparency(1f); // Restore full opacity
            collisiontruepiece = false;

        }
    }

    public void SetTransparency(float alpha)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Clamp(alpha, 0f, 1f);
            spriteRenderer.color = color;
        }
    }


    private int[] weightedDice = new int[] { 1, 2, 3, 1, 4, 5, 6, 6 };

    public int probDice()
    {
        int randomIndex = Random.Range(0, weightedDice.Length);
        return weightedDice[randomIndex];
    }
    public void changedicecoloronturn(int dicevalue)
    {
        if (CurrentPlayer == 1)
        {
            diceRenderer.sprite = greendiceSprites[dicevalue - 1];
        }
        else if (CurrentPlayer == 2)
        {
            diceRenderer.sprite = yellowdiceSprites[dicevalue - 1];
        }
        else if (CurrentPlayer == 3)
        {
            diceRenderer.sprite = bluediceSprites[dicevalue - 1];
        }
        else if (CurrentPlayer == 4)
        {
            diceRenderer.sprite = reddiceSprites[dicevalue - 1];
        }

    }

    public void homecutturning()
    {

        if (Piece.homeorcutturn)
        {
            if (prevdicenumber == 2 || prevdicenumber == 3 || prevdicenumber == 4 || prevdicenumber == 5 || prevdicenumber == secondoneorsix)
            {
                if (optionscript.sixgiveanotherturn || optionscript.onealsogiveturn)
                {
                    if (prevdicenumber != secondoneorsix)
                    {
                        //currentPlayerIndex = (currentPlayerIndex - 1 + 4) % 4;
                        turnchange(false);
                        AIagain = true;
                        AIsubfunc(noofAI, false);

                        //Debug.Log("CurrentPlayerIndexafterturningchange:"+currentPlayerIndex+":::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
                        //turnchangecheck = true;
                    }
                }
                else
                {
                    turnchange(false);
                    AIagain = true;
                    AIsubfunc(noofAI, false);

                    //Debug.Log("CurrentPlayerIndexafterturningchange:"+currentPlayerIndex+":::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
                    //turnchangecheck = true;

                }
                //currentPlayerIndex = (currentPlayerIndex - 1 + 4) % 4;
                //if (aicalling)
                //AIsubfunc(noofAI, false);
            }
            else
            {
                AIturnenablefunc();
            }
            Piece.homeorcutturn = false;
            Debug.Log("CURRENTPLAYER AND IT'S INDEX" + CurrentPlayer + ":" + currentPlayerIndex);
            //changedicecoloronturn(prevdicenumber);
        }

    }
    public void checkingturnandskip()
    {
        //Debug.Log("InsideCheckturingandskip");
        if (optionscript.start3timescut)
        {
            if (threeoneorsix)
            {
                dicecount++;

                if(optionscript.threeoneskip || optionscript.threesixskip)
                {
                    turnchange(true);
                    AIagain = false;

                }

                threeoneorsix = false;
            }
        }

    }
    void dicenumberchange()
    {
        forAIdiceroll = false;
        if (dicecount > 10)
        {
            if (CurrentPlayer == 1)
            {
                if (Piece.greenoneappear == false)
                {
                    int randnum = Random.Range(0, 10);

                    if (dicecount > randnum)
                    {
                        oneappeardice = true;
                    }
                }
            }
            else if (CurrentPlayer == 2)
            {
                if (Piece.yellowoneappear == false)
                {
                    int randnum = Random.Range(0, 10);

                    if (dicecount > randnum)
                    {
                        oneappeardice = true;
                    }
                }
            }
            else if (CurrentPlayer == 3)
            {
                if (Piece.blueoneappear == false)
                {
                    int randnum = Random.Range(0, 10);

                    if (dicecount > randnum)
                    {
                        oneappeardice = true;
                    }
                }
            }
            else if (CurrentPlayer == 4)
            {
                if (Piece.redoneappear == false)
                {
                    int randnum = Random.Range(0, 10);

                    if (dicecount > randnum)
                    {
                        oneappeardice = true;
                    }
                }
            }
        }
    }

    public int seconddicecounter = 0;
    public bool seconddice3times = false;
    public static bool threeconsecappear = false;
    void consecutivecount()
    {
        if(optionscript.start3timescut || optionscript.start3timesskip)
        {
            if(finalValue == oneorsix)
            {
                oneorsixcounter++;
            }
            else
            {
                oneorsixcounter = 0;
            }

            if (oneorsixcounter == 3)
            {
                oneorsixcounter = 0;
                threeoneorsix = true;
            }
        }

        if (optionscript.threesixstart || optionscript.threeonestart)
        {
            if (finalValue == secondoneorsix)
            {
                seconddicecounter++;
            }
            else
            {
                seconddicecounter = 0;
            }

            if (seconddicecounter == 3)
            {
                seconddicecounter = 0;
                seconddice3times = true;
                threeconsecappear = true;
            }
        }
    }

    public bool collisioncheckbool = false;
    IEnumerator RollDice()
    {
        isRolling = true;
        allowInteraction = false;
        finalValue = 0;
        // float elapsedTime = 0f;

        // Dice rolling animation
        float animInterval = rollDuration / rollAnimFrames;
        //collisioncheckbool = false;
        //Debug.Log("Currentplayer" + CurrentPlayer + "CurrentPlayerIndex:" + currentPlayerIndex);
        for (int i = 0; i < rollAnimFrames; i++)
        {
            int randomrotrange = Random.Range(0, 360);
            transform.Rotate(0, 0, randomrotrange);

            int randomFace = Random.Range(0, 2);

            if (CurrentPlayer == 1)
            {
                diceRenderer.sprite = greendiceAnim[randomFace];
            }
            else if (CurrentPlayer == 2)
            {
                diceRenderer.sprite = yellowdiceAnim[randomFace];
            }
            else if (CurrentPlayer == 3)
            {
                diceRenderer.sprite = bluediceAnim[randomFace];
            }
            else if (CurrentPlayer == 4)
            {
                diceRenderer.sprite = reddiceAnim[randomFace];
            }
            yield return new WaitForSeconds(animInterval);
        }

        //collisioncheckbool = true;
        if(collisiontruepiece == true)
        {
            SetTransparency(0.7f);
        }
        // Get final result
        if (oneappeardice == false)
        {
            if (currentPlayerIndex != probabilityplayer)
                finalValue = Random.Range(1, 7);

            else
                finalValue = probDice();

        }
        else
        {
            finalValue = oneorsix;
        }
       //finalValue = 6;
        //finalValue = 1; ///////////////////////////////////////////////////////////

        consecutivecount();
        //finalValue = 6;

        //diceRenderer.sprite = greendiceSprites[finalValue - 1];
        if (CurrentPlayer == 1)
        {
            diceRenderer.sprite = greendiceSprites[finalValue - 1];
        }
        else if (CurrentPlayer == 2)
        {
            diceRenderer.sprite = yellowdiceSprites[finalValue - 1];
        }
        else if (CurrentPlayer == 3)
        {
            diceRenderer.sprite = bluediceSprites[finalValue - 1];
        }
        else if (CurrentPlayer == 4)
        {
            diceRenderer.sprite = reddiceSprites[finalValue - 1];
        }

        oneappeardice = false;
        // Handle turn logic


        if (optionscript.threeonestart && oneorsix == 6)
        {
            threehome = true;
        }
        else if (optionscript.threesixstart && oneorsix == 1)
        {
            threehome = true;
        }
        prevdicenumber = finalValue;
        HandleTurnCompletion();

        isRolling = false;

        coroutinecompletes = true;
    }

    void HandleTurnCompletion()
    {
        // Notify listeners about the dice result

        // Move to next player if not a 6 (assuming 6 gives extra turn)
        OnDiceRollCompleted?.Invoke(currentPlayerIndex, finalValue);
        //tempdicevalue = finalValue;
        //tempplayerindex = currentPlayerIndex;
        //checkformovingturn(true);
    }

    public void AIturnenablefunc()
    {
        //Debug.Log("InsideAIturnenable");
        if (currentPlayerIndex == AIjustnumber)
        {
            AIagain = true;
        }
    }
    public void checkformovingturn(bool checking2)
    {
        //Debug.Log("Checkformovingturn");
        if ((oneorsix == 1 && optionscript.sixgiveanotherturn == true) || (oneorsix == 6 && optionscript.onealsogiveturn == true))
        {
            if (finalValue != 1 && finalValue != 6)
            {
                MoveToNextPlayer(checking2);
            }
            else
            {
                // Player gets another turn
                //UpdateTurnDisplay();
                AIturnenablefunc();
                allowInteraction = true;
            }
        }
        else if (oneorsix == 1 && optionscript.sixgiveanotherturn == false)
        {
            if (finalValue != 1)
            {
                MoveToNextPlayer(checking2);
            }
            else
            {

                AIturnenablefunc();
                // Player gets another turn
                //UpdateTurnDisplay();
                allowInteraction = true;
            }
        }
        else if (oneorsix == 6 && optionscript.onealsogiveturn == false)
        {
            if (finalValue != 6)
            {
                MoveToNextPlayer(checking2);
            }
            else
            {
                AIturnenablefunc();
                // Player gets another turn
                //UpdateTurnDisplay();
                allowInteraction = true;
            }
        }

        //checkingturnandskip(true);

        //if (optionscript.start3timesskip)
        //{
        //    if (threeoneorsix)
        //    {
        //        dicecount++;
        //        //currentPlayerIndex++;
        //        //currentPlayerIndex = (currentPlayerIndex + 1) % 4;
        //        turnchange(true);
        //        //turnchangecheck = true;
        //        //if(aicalling)
        //        //{
        //        //    AIsubfunc(noofAI, true);

        //        //}

        //        //Debug.Log("CurrentPlayerIndex:" + currentPlayerIndex);
        //        threeoneorsix = false;
        //    }
        //}



        }

    void MoveToNextPlayer(bool checking)
    {
        //Debug.Log("InsideMovetoNextplayer");
        if (checking == true)
        {
            turnchange(true);
            Debug.Log("Changing turn");
        }
        //currentPlayerIndex = (currentPlayerIndex + 1) % 4;
        //UpdateTurnDisplay();
        //allowInteraction = true;

    }

    //void UpdateTurnDisplay()
    //{
    //    turnText.text = $"Player {CurrentPlayer}'s Turn";
    //    turnText.color = playerColors[currentPlayerIndex];
    //    Debug.Log(CurrentPlayer);
    //}

    public void EnableDiceInteraction()
    {
        allowInteraction = true;
    }

    public void DisableDiceInteraction()
    {
        allowInteraction = false;
    }

    // Call this when player finishes moving their piece
    public void EndTurn()
    {
        //Debug.Log("InsideEndturn");
        checkformovingturn(false);

    }


    public void turnchange(bool forward)
    {
        //turnchangecheck = true;
        //Debug.Log("hi");
        if (forward)
        {
            if (playernum == 4)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % 4;
            }
            else if (playernum == 2)
            {
                if (currentPlayerIndex == player2)
                    currentPlayerIndex = player1;
                else
                {
                    currentPlayerIndex = player2;
                }
            }

            else if (playernum == 3)
            {
                if (currentPlayerIndex == player1)
                {
                    currentPlayerIndex = player2;
                }
                else if (currentPlayerIndex == player2)
                {
                    currentPlayerIndex = player3;
                    //Debug.Log("Hi2" + currentPlayerIndex);
                }
                else if (currentPlayerIndex == player3)
                {
                    currentPlayerIndex = player1;
                }
            }
        }
        else
        {
            if (playernum == 4)
            {
                currentPlayerIndex = (currentPlayerIndex - 1 + 4) % 4;
            }
            else if (playernum == 2)
            {
                if (currentPlayerIndex == player2)
                    currentPlayerIndex = player1;
                else
                {
                    currentPlayerIndex = player2;
                }
            }

            else if (playernum == 3)
            {
                if (currentPlayerIndex == player1)
                {
                    currentPlayerIndex = player3;
                }
                else if (currentPlayerIndex == player2)
                {
                    currentPlayerIndex = player1;
                }
                else if (currentPlayerIndex == player3)
                {
                    currentPlayerIndex = player2;
                }
            }
        }
         Debug.Log("CurrentPlayerIndex:"+currentPlayerIndex+":::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
    }

}