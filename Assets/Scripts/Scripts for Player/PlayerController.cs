using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using GoogleMobileAds.Api;

public class PlayerController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public static PlayerController Instance;

    float BallPos;
    int n;
    int oldn;

    [SerializeField]Transform Ball;  // prefab

    [SerializeField]
    Transform Parent;  // empty parent obj 

    //for particle on ball destroy
    [SerializeField]
    Transform PSysObj;
    ParticleSystem.MainModule PSys;

    [SerializeField]
    Transform MainCam;  // Main Cam

    [HideInInspector]
    public List<Transform> Balls = new List<Transform>();  // List of Balls
    CollisionDetect FirstBallCol;

    [SerializeField] public Material[] MatPrefabs;  // colors for Balls

    [HideInInspector]
    public bool Pressed = false; // Press on panel(touch)
    Touch touch;

    int count;

    ///         Block ::::: Blocks :::::
    [SerializeField]
    Transform BlockParent;

    Transform Block;

    [SerializeField]
    TextMeshProUGUI LevelText;  // level text

    [SerializeField]
    GameObject Instruction1,Instruction2; // touch (arrow, hand) instructions at start

    bool Instr1=true; 
    bool Instr2;

    int BallSpawnNumber; // Number of Balls to spawn

    [SerializeField]
    Transform TextBlock;     // TEXT BLOCK

    // Small Interval block               [   for level 19   ]
    [SerializeField]
    Transform SmallIntervalBlock;

    [SerializeField]
    Transform SmallBlockParents;

    float TimeAddEasy = 0.7f;
    float TimeAdd = 0.6f;
    float ActualTime = 0f;

    [HideInInspector]
    public int PlayerLevel=0;

    [HideInInspector]
    public bool StartGame = false;
    [HideInInspector]
    public int totalNoOfLevels = 21;
    [HideInInspector]
    public bool UseVib = true;

    GameObject TunnelObj;

    BannerView bannerView;// = new BannerView("ca-app-pub-9285045534177890/3505990696", AdSize.Banner, 0, 50);

    [SerializeField] ObstacleHandler obstacleHandler;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Destroy(this);
        }

        //PlayerLevel =0;
        //PlayerPrefs.SetInt("Level", PlayerLevel);
        PlayerLevel = PlayerPrefs.GetInt("Level");


        /*
        if (Debug.isDebugBuild)
        {
            Debug.Log("This is a debug build!");
        }
        */

    }
    
    void Start()
    {
        // Initialize Ads
        MobileAds.Initialize(initStatus => { });
        this.RequestBanner();

        // set ball spwn count & no of lvls
        BallSpawnNumber = LevelManager.Instance.LevelSettings[PlayerLevel].NoOfBalls; // number of balls to spawn
        totalNoOfLevels = LevelManager.Instance.LevelSettings.Length;
        // end..

        //
        TunnelObj = TunnelScript.Instance.gameObject;
        TunnelScript.Instance?.ChangeTunnelMat();
        BallPos = 0;
        for (int i = 0; i < BallSpawnNumber; i++)
        {
            Vector3 Pos = new Vector3(Parent.transform.position.x, Parent.transform.position.y, Parent.transform.position.z + BallPos);

            Transform BallInit = Instantiate(Ball, Pos, Quaternion.identity, Parent.transform);

            //BallCount++;
            //System.Array.Resize(ref Balls, BallCount);

            MeshRenderer bMR = BallInit.GetComponent<MeshRenderer>();   //ball mesh renderer for asigning mat

            // For Random color Balls =>
            do
            {
                n = (int)Random.Range(0, MatPrefabs.Length);
            } while (oldn == n && i > 0);

            oldn = n;
            bMR.material = MatPrefabs[n];  // random material
            
            Balls.Add(BallInit);

            BallPos-=1.1f;
        }

        PSys = PSysObj.transform.GetComponent<ParticleSystem>().main;

        MeshRenderer MR = Balls[0].GetComponent<MeshRenderer>();
        PSys.startColor = new Color(MR.material.color.r, MR.material.color.g, MR.material.color.b);
        PSysObj.parent = Balls[0];
        PSysObj.position = Balls[0].position;

        // adding rigidbody to first ball
        Balls[0].gameObject.AddComponent<Rigidbody>().useGravity = false;
        //Debug.Log(Balls.Length);
        FirstBallCol = Balls[0].gameObject.AddComponent<CollisionDetect>();
        FirstBallCol.PC = transform.GetComponent<PlayerController>();

        //Play button text
        if (PlayerLevel == 0)
        {
            LevelText.text = "Start";
        }
        else
        {
            LevelText.text = "Level " + PlayerLevel.ToString();
        }
        // for diabling tunnel
        if (PlayerLevel % 5 == 0 && PlayerLevel > 0)
        { TunnelObj.gameObject.SetActive(false); }
        else { TunnelObj.gameObject.SetActive(true); }
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-9285045534177890/3505990696";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-9285045534177890/7577931790";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
    }


    /// <summary>
    ///
    ///                     Update
    /// 
    /// </summary>
    void Update()
    {
        if (StartGame == false || Instruction1.activeInHierarchy==true || Instruction2.activeInHierarchy == true)
        {  
            return;
        }

        if (Balls.Count == 0)
        {
            
            TunnelScript.Instance?.ChangeTunnelMat();

            obstacleHandler.DestroyLastLevelObstacles();

            if (PlayerLevel <= totalNoOfLevels)
            {
                PlayerLevel++;
            }
            //Debug.Log(PlayerLevel);
            LevelN();

            LevelText.text = "Level " + PlayerLevel.ToString();

            
            if (Parent.childCount > 0)
            {
                for (int i = 0; i < Parent.childCount; i++)
                {
                    Destroy(Parent.GetChild(i).gameObject);
                }
            }

            // for diabling tunnel
            if (PlayerLevel % 5 == 0 && PlayerLevel > 0)
            { TunnelObj.gameObject.SetActive(false); }
            else
            {
                TunnelObj.gameObject.SetActive(true);
                if (SmallBlockParents.childCount > 0)
                {
                    foreach (Transform t in SmallBlockParents)
                    {
                        Destroy(t.gameObject);
                    }
                }
            }
            //...

            //ChangeRotSide=true;

            // call awake and start again to instantiate balls
            Awake();Start();

            // ADS

            if(PlayerLevel>0 && PlayerLevel %2==0)
            {
                //Advertisement.Show();
            }
            return;
        }

        ///<summary>
        ///
        ///             SMALL INTERVAL BLOCKS
        ///
        /// </summary>

        if (PlayerLevel % 5 == 0 && PlayerLevel>0)
        {
            if (Time.time > ActualTime)
            {
                ActualTime = Time.time + TimeAddEasy;

                StartCoroutine(SmallIntervalBlockInstantiate());
               
            }
            return;
        }
        

        ///<summary>
        ///
        ///           Instruction screen
        /// 
        /// </summary>
        switch (PlayerLevel)
        {
            case 0:
                if (Instr1)
                {
                    StartGame = false;
                    Instruction1.gameObject.SetActive(true);
                    Instr1 = false;
                    StartCoroutine(Instr2Active());
                }
                if (Instr2 && !Instr1)
                {
                    StartGame = false;
                    Instruction2.gameObject.SetActive(true);
                    Instr2 = false;
                }

                break;
        }
        
    }
    #region Done with

    void Movement()
    {
        ///<summary>
        /// 
        ///         Keyboard Movement 
        /// 
        /// </summary>
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Balls[0].transform.Translate(-0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Balls[0].transform.Translate(0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Balls[0].transform.Translate(0, 0.1f, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Balls[0].transform.Translate(0, -0.1f, 0);
        }
        ///<summary>
        /// 
        ///       Touch Control
        /// 
        /// </summary>
        if (Pressed)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    Balls[0].transform.Translate(touch.deltaPosition.x * 0.02f, touch.deltaPosition.y * 0.02f, 0);
                }
            }
        }

        ///<summary>
        /// 
        ///       CLAMP POSITION FOR FIRST BALLs in Tunnel
        /// 
        /// </summary>

        Vector3 v = Vector3.ClampMagnitude(Balls[0].transform.position, 7f);
        Balls[0].transform.position = v;

        //Balls[0].transform.position = new Vector3(Mathf.Clamp(Balls[0].transform.position.x, -4.5f, 4.5f), Mathf.Clamp(Balls[0].transform.position.y, -4.5f, 4.5f), 0);

       
        ///<summary>
        /// 
        ///      snake type movement of rest of the balls          
        /// 
        /// </summary>
        if (Balls.Count > 1)
        {
            if (System.Math.Abs(Balls[Balls.Count - 1].position.x - Balls[0].position.x) > 0 ||
                System.Math.Abs(Balls[Balls.Count - 1].position.y - Balls[0].position.y) > 0 ||
                System.Math.Abs(Balls[1].position.z - Balls[0].position.z - 1) > 0)
            {
                for (int i = 1; i < (Balls.Count>15?15:Balls.Count); i++)
                {
                    Vector3 ToPos = new Vector3(Balls[i - 1].transform.position.x, Balls[i - 1].transform.position.y, Balls[i - 1].transform.position.z - 1.1f);
                    //Vector3 ToPosz = new Vector3(Balls[i].transform.position.x, Balls[i].transform.position.y, Balls[i - 1].transform.position.z - 1.1f);

                    Balls[i].transform.position = Vector3.Slerp(Balls[i].transform.position, ToPos, 1f / Mathf.Abs(7 - i));
                    //Balls[i].transform.position = Vector3.Slerp(Balls[i].transform.position, ToPos, 1f);
                }
            }
        }

        ///<summary>
        ///
        ///        Camera Position Slerp to Ball Position
        /// 
        ///</summary>
        float yPos = Balls[0].transform.position.y > 3 ? Balls[0].transform.position.y : Balls[0].transform.position.y + 2;
        Vector3 BallToCam = Vector3.ClampMagnitude( new Vector3(Balls[0].transform.position.x, yPos , -8), 10f);

        MainCam.transform.position = Vector3.Slerp(MainCam.transform.position, BallToCam, 0.05f);

        //Vector3 vCam = Vector3.ClampMagnitude(BallToCam, 6f);
        //MainCam.transform.position = vCam;

    }

    private void FixedUpdate()
    {
        if (Balls.Count == 0 || StartGame == false)
        {
            return;
        }
        Movement();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        //Debug.Log("clicked");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        //Debug.Log("Exit");
    }


    

    ///<summary>
    /// 
    ///        Remove Balls from List    (after collision)     CollisionDetect script => on First Ball
    ///  
    /// </summary>
    public void RemoveBalls()
    {
        if (Balls.Count > 0)
        {
            StartCoroutine(SetParticleColor());

            Balls[0].position = new Vector3(Balls[0].position.x, Balls[0].position.y, 0);

            //FirstBallCol.remove = false;
            if (Balls[0].GetComponent<CollisionDetect>() == null)
            {
                FirstBallCol = Balls[0].gameObject.AddComponent<CollisionDetect>();
            }
            else
            {
                FirstBallCol = Balls[0].GetComponent<CollisionDetect>();
            }

            FirstBallCol.PC = this;
        }
    }

    IEnumerator SetParticleColor()
    {
        yield return new WaitForSeconds(0.3f);
        MeshRenderer MR = Balls[0].GetComponent<MeshRenderer>();
        PSys.startColor = new Color(MR.material.color.r, MR.material.color.g, MR.material.color.b);
        PSysObj.parent = Balls[0]; PSysObj.position = Balls[0].position;
    }

    ///<summary>
    /// 
    ///      Insert Ball at first Position   (after collision)         CollisionDetect script => on First Ball
    /// 
    /// </summary>
    public void InsertBalls(string mat)
    {
        Transform BallInit = Instantiate(Ball,new Vector3(Balls[0].position.x, Balls[0].position.y, 0), Quaternion.identity, Parent.transform);
        MeshRenderer BallInitMat = BallInit.gameObject.GetComponent<MeshRenderer>();

        // Material String
        if(mat == "b")
        {
            BallInitMat.material = MatPrefabs[0];
        }
        else if (mat == "g")
        {
            BallInitMat.material = MatPrefabs[1];
        }
        else if (mat == "o")
        {
            BallInitMat.material = MatPrefabs[2];
        }
        else if (mat == "r")
        {
            BallInitMat.material = MatPrefabs[3];
        }
        else if (mat == "y")
        {
            BallInitMat.material = MatPrefabs[4];
        }

        Destroy(FirstBallCol); // Destroy old collison detect at Balls[0]     Imp.

        // Insert

        Balls.Insert(0, BallInit);

        FirstBallCol = Balls[0].gameObject.AddComponent<CollisionDetect>();
        FirstBallCol.PC = transform.GetComponent<PlayerController>();

        for (int i = 1; i < Balls.Count; i++)
        {
            if (Balls[0].GetComponent<MeshRenderer>().material.name == Balls[i].GetComponent<MeshRenderer>().material.name)
            {
                count++;
            }
            else
            {
                if (count >= 2)
                {
                    for (int j = 0; j < count+1 ; j++)
                    {
                        Animator BallAM = Balls[j].GetComponent<Animator>();
                        BallAM.SetBool("Destroy", true);
                    }

                }
                count = 0;
                break;
            }
            if(i==Balls.Count-1)
            {
                if (count >= 2)
                {
                    for (int j = 0; j < count + 1; j++)
                    {
                        Animator BallAM = Balls[j].GetComponent<Animator>();
                        BallAM.SetBool("Destroy", true);
                    }

                }
                count = 0;
                break;
            }
        }

        

    }

    #endregion 

    /// <summary>
    ///
    ///             Text Block Instantiate   [ LEVEL 16 ..] 
    /// 
    /// </summary>

    void TextBlockInstantiate()
    {
        Block = Instantiate(TextBlock, BlockParent);

        int childWithSameColor = Random.Range(0, Block.childCount);

        int iter = 0;
        foreach (Transform MainTextBlock in Block)
        {
            Transform Canvas_ = MainTextBlock.GetChild(0);

            Text BlockTextField = Canvas_.GetChild(0).GetComponent<Text>();

            int RandMat = Random.Range(0, MatPrefabs.Length);

            if (iter != childWithSameColor)
            {
                int RandMatName = Random.Range(0, MatPrefabs.Length);
                

                BlockTextField.text = MatPrefabs[RandMatName].name;

                BlockTextField.color = new Color(MatPrefabs[RandMat].color.r, MatPrefabs[RandMat].color.g, MatPrefabs[RandMat].color.b);
            }
            else
            {
                string BallMatName = Balls[0].GetComponent<MeshRenderer>().material.name.Substring(0, 1);

                if (BallMatName == "b")
                {
                    BlockTextField.text = MatPrefabs[0].name;
                }
                else if (BallMatName == "g")
                {
                    BlockTextField.text = MatPrefabs[1].name;
                }
                else if (BallMatName == "o")
                {
                    BlockTextField.text = MatPrefabs[2].name;
                }
                else if (BallMatName == "r")
                {
                    BlockTextField.text = MatPrefabs[3].name;
                }
                else if (BallMatName == "y")
                {
                    BlockTextField.text = MatPrefabs[4].name;
                }
                BlockTextField.color = new Color(MatPrefabs[RandMat].color.r, MatPrefabs[RandMat].color.g, MatPrefabs[RandMat].color.b);
            }
            iter++;
        }
    }
    bool isStartGame()
    {
        return StartGame;
    }

    /// <summary>
    ///
    ///                Small Interval Blocksssss
    ///                
    ///                NOTE:these blocks are getting destroyed when ball collides FIX THAT --- Fixed i think
    /// 
    /// </summary>
    IEnumerator SmallIntervalBlockInstantiate()
    {
        if(SmallBlockParents.childCount == 0)
        {
            for (int i = 0; i < 25; i++)
            {
                yield return new WaitUntil(isStartGame);

                Transform SmallIntBlock_ = Instantiate(SmallIntervalBlock, new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 20f), Quaternion.identity, SmallBlockParents);

                MeshRenderer SIMeshRend = SmallIntBlock_.GetChild(0).GetComponent<MeshRenderer>();

                int RandColor = Random.Range(0, MatPrefabs.Length);

                SIMeshRend.material = MatPrefabs[RandColor];
            }
            // we can also make levels without initializing for asthetic look
            StartCoroutine(InitSmallIntervalBlocks());
        }
        else
        {
            foreach(Transform t in SmallBlockParents)
            {
                yield return new WaitUntil(isStartGame);

                if (t.position.z < MainCam.position.z)
                {
                    StartCoroutine(SmallBlocksToBack(t));
                }
            }
        }
    }



    IEnumerator SmallBlocksToBack(Transform t)
    {
        yield return new WaitUntil(isStartGame);

        t.position = new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 20f);
        MeshRenderer SIMeshRend = t.GetChild(0).GetComponent<MeshRenderer>();

        int RandColor = Random.Range(0, MatPrefabs.Length);

        SIMeshRend.material = MatPrefabs[RandColor];
    }



    IEnumerator InitSmallIntervalBlocks()
    {
        foreach (Transform t in SmallBlockParents)
        {
            yield return new WaitUntil(isStartGame);
            yield return new WaitForSeconds(1f);
            if (t != null)
            {
                t.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    ///
    ///             Instruction 2 top activate at level 0
    ///  
    /// </summary>
   
    IEnumerator Instr2Active()  
    {
        yield return new WaitUntil(isStartGame);
        yield return new WaitForSeconds(3);

        Instr2 = true;     
    }

    void LevelN()
    {
        PlayerPrefs.SetInt("Level",PlayerLevel);
        //Debug.Log(clevel);
    }
}

