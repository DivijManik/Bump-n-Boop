using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public static PlayerController Instance;

    float BallPos;
    int n;
    int oldn;

    [SerializeField]
    Transform Ball;  // prefab

    [SerializeField]
    Transform Parent;  // empty parent obj 

    //for particle on ball destroy
    [SerializeField]
    Transform PSysObj;
    ParticleSystem.MainModule PSys;

    [SerializeField]
    Transform MainCam;  // Main Cam
    float DistBallToCam;

    // Transform[] Balls;
    int BallCount;

    public List<Transform> Balls = new List<Transform>();  // List of Balls
    CollisionDetect FirstBallCol;

    [SerializeField] Material[] MatPrefabs;  // colors for Balls

    public bool Pressed = false; // Press on panel(touch)
    Touch touch;

    int count;
    
    /// <summary>
    ///
    ///         Block ::::: Blocks :::::
    /// 
    /// </summary>

    [SerializeField]
    Transform BlockParent;

    [SerializeField]
    Transform [] Blocks;

    Transform Block;

    [SerializeField]
    TextMeshProUGUI LevelText;  // level text

    [SerializeField]
    GameObject Instruction1,Instruction2; // touch (arrow, hand) instructions at start

    bool Instr1=true; 
    bool Instr2;

    int BallSpawnNumber; // Number of Balls to spawn
    int BlockNum; // Range from where to start for spawning blocks

    int CurrentBlockNum; // current index of the block     [ FOR LEVEL ] 
    int RandomPosToRot;  // Rotate to the left or right   [ For Level ]
    bool ChangeRotSide=true; // LEVEL 14-15

    [SerializeField]
    Transform TextBlock;     // TEXT BLOCK

    //Random pos to move to
    int[] RandomPosToMoveTo = new int[5];

    // Small Interval block               [   for level 19   ]
    [SerializeField]
    Transform SmallIntervalBlock;

    float TimeAddEasy = 0.7f;
    float TimeAdd = 0.6f;
    float ActualTime = 0f;

    int PlayerLevel=0;

    [HideInInspector]
    public bool StartGame = false;
    [HideInInspector]
    public int totalNoOfLevels = 21;
    [HideInInspector]
    public bool UseVib = true;

    private void Awake()
    {
        if(Instance == null)
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

        if (PlayerLevel < 5)
        {
            BallSpawnNumber = 4; // number of balls to spawn
            if (PlayerLevel <= 1)
            {
                BlockNum = 3; // only cube blocks
            }
            else
            {
                BlockNum = 2; //  (also instantiates capsule blocks)
            }
        }
        else if(PlayerLevel<10)
        {
            if(PlayerLevel<=7)
            {
                BallSpawnNumber = 6;
            }
            else
            {
                BallSpawnNumber = 7;
            }
            BlockNum = 1;
        }
        else if(PlayerLevel<=15)
        {
            BallSpawnNumber = 8;
            BlockNum = 0;
        }
        else
        {
            BallSpawnNumber = 10;
            BlockNum = 0;
        }

        /*
        if (Debug.isDebugBuild)
        {
            Debug.Log("This is a debug build!");
        }
        */
        
    }
    
    void Start()
    {
        foreach (Transform t in BlockParent)
        {
            Destroy(t.gameObject);
        }

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
    }

    void Update()
    {
        
        //    <<<<      working on this   >>>
        if (StartGame == false || Instruction1.activeInHierarchy==true || Instruction2.activeInHierarchy == true)
        {  
            return;
        }

        if (Balls.Count == 0)
        {
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
            ChangeRotSide=true;

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

        if (PlayerLevel == 6)
        {
            if (Time.time > ActualTime)
            {
                ActualTime = Time.time + TimeAddEasy;

                SmallIntervalBlockInstantiate();
               
            }
            return;
        }

        if (PlayerLevel == 19)
        {
            if (Time.time > ActualTime)                             //    [   for level  19    ]
            {
                ActualTime = Time.time + TimeAdd;

                SmallIntervalBlockInstantiate();
                
            }
            return;
        }

        if (PlayerLevel == 22 && Time.time > ActualTime)
        {

            ActualTime = Time.time + TimeAddEasy;

            SmallIntervalBlockInstantiate();
            return;
        }
        ///<summary>
        ///
        ///           Block Script;    Switch statement for Levels 
        /// 
        /// </summary>
        if (Block != null || PlayerLevel==19)
        {
            switch (PlayerLevel)
            {
                case 0:
                    if (Instr1)
                    {
                        Instruction1.gameObject.SetActive(true);
                        Instr1 = false;
                        StartCoroutine(Instr2Active());
                    }
                    if (Instr2 && !Instr1)
                    {
                        Instruction2.gameObject.SetActive(true);
                        Instr2 = false;
                    }

                    Block.transform.Translate(0, 0, -0.1f, Space.World);

                    break;
                case 1: case 2: 

                    Block.transform.Translate(0, 0, -0.14f, Space.World);

                    break;
                case 3:
                case 4:
                

                    if (RandomPosToRot == 0)
                    {
                        Block.transform.Translate(0, 0, -0.2f, Space.World);
                    }
                    else
                    {
                        Block.transform.Translate(0, 0, -0.1f, Space.World);
                    }

                    if (ChangeRotSide)                                          // [    change rotation pos    ]
                    {
                        RandomPosToRot = Random.Range(0, 2);
                        ChangeRotSide = false;
                        StartCoroutine(ChangeRotationSide());
                    }

                    break;
                case 5:

                    Block.transform.Translate(0, 0, -0.1f, Space.World);

                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:

                    if (CurrentBlockNum >= 2)     // for rotation  
                    {
                        foreach (Transform block in Block)
                        {
                            block.transform.Rotate(0, 0, 1f, Space.World);
                        }
                    }
                    else
                    {
                        Block.transform.Rotate(0, 0, 1f, Space.World);
                    }

                    Block.transform.Translate(0, 0, -0.14f, Space.World);

                    break;

                case 11:
                case 12:

                    if (RandomPosToRot == 0)
                    {
                        Block.transform.Rotate(0, 0, 1f, Space.World);
                    }
                    else
                    {
                        Block.transform.Rotate(0, 0, -1f, Space.World);
                    }

                    Block.transform.Translate(0, 0, -0.1f, Space.World);
                    break;

                case 13:
                case 14:
                case 15:


                    if (RandomPosToRot == 0)
                    {
                        Block.transform.Rotate(0, 0, 1f, Space.World);
                    }
                    else
                    {
                        Block.transform.Rotate(0, 0, -1f, Space.World);
                    }

                    Block.transform.Translate(0, 0, -0.1f, Space.World);

                    if (ChangeRotSide)                                          // [    change rotation pos    ]
                    {
                        RandomPosToRot = Random.Range(0, 2);
                        ChangeRotSide = false;
                        StartCoroutine(ChangeRotationSide());
                    }


                    break;
                case 16:

                    Block.transform.Translate(0, 0, -0.1f, Space.World);

                    break;
                case 17:
                    if (CurrentBlockNum >= 2)
                    {
                        int ite = 0;

                        foreach (Transform block in Block)
                        {
       
                            if(RandomPosToMoveTo[ite]==0 && block.position.y >= -3f) 
                            {
                                block.transform.Translate(0, -0.02f, 0, Space.World);
                            }
                            else if (block.position.y <= 3f)
                            {
                                block.transform.Translate(0, 0.02f, 0, Space.World);
                            }

                            ite++;
                        }
                        
                    }
                   
                    Block.transform.Translate(0, 0, -0.14f, Space.World);

                    break;

                case 18:
                    if (CurrentBlockNum >= 2)
                    {
                        int ina = 0;

                        foreach (Transform block in Block)
                        {

                            if (RandomPosToMoveTo[ina] == 0 && block.position.y >= -3f)
                            {
                                block.transform.Translate(0, -0.02f, 0, Space.World);
                            }
                            else if (block.position.y <= 3f)
                            {
                                block.transform.Translate(0, 0.02f, 0, Space.World);
                            }

                            ina++;
                        }

                    }
                    else if (RandomPosToRot == 0)
                    {
                        Block.transform.Rotate(0, 0, 1f, Space.World);
                    }
                    else
                    {
                        Block.transform.Rotate(0, 0, -1f, Space.World);
                    }

                    if (ChangeRotSide)                                          
                    {
                        RandomPosToRot = Random.Range(0, 2);
                        ChangeRotSide = false;
                        StartCoroutine(ChangeRotationSide());
                    }

                    Block.transform.Translate(0, 0, -0.14f, Space.World);
                    break;

                case 19:
                    //Debug.Log("HIOO");
                    break;
                case 20:

                    int iter = 0;

                    foreach (Transform block in Block)
                    {

                        if (RandomPosToMoveTo[iter] == 0 && block.position.y >= -3f)
                        {
                            block.transform.Translate(0, -0.02f, 0, Space.World);
                        }
                        else if (block.position.y <= 3f)
                        {
                            block.transform.Translate(0, 0.02f, 0, Space.World);
                        }

                        iter++;
                    }

                    Block.transform.Translate(0, 0, -0.1f, Space.World);
                    break;

                case 21:
                case 22:

                    if (CurrentBlockNum >= 2)
                    {
                        int it = 0;

                        foreach (Transform block in Block)
                        {

                            if (RandomPosToMoveTo[it] == 0 && block.position.y >= -3f)
                            {
                                block.transform.Translate(0, -0.02f, 0, Space.World);
                            }
                            else if (block.position.y <= 3f)
                            {
                                block.transform.Translate(0, 0.02f, 0, Space.World);
                            }

                            it++;
                        }

                    }
                    else if (RandomPosToRot == 0)
                    {
                        Block.transform.Rotate(0, 0, 1f, Space.World);
                    }
                    else
                    {
                        Block.transform.Rotate(0, 0, -1f, Space.World);
                    }

                    if (ChangeRotSide)
                    {
                        RandomPosToRot = Random.Range(0, 2);
                        ChangeRotSide = false;
                        StartCoroutine(ChangeRotationSide());
                    }

                    Block.transform.Translate(0, 0, -0.14f, Space.World);

                    break;

            }
        }
        else
        {  
            if (PlayerLevel != 19)                                        //      for normal Block Instantiate other then level 19    ;;;
            {
                if (PlayerLevel == 16 || PlayerLevel == 20 || PlayerLevel == 5)
                {
                    TextBlockInstantiate();
                }
                else
                {
                    BlocksInstantiate(BlockNum);
                }

                if (PlayerLevel > 10)      // [ FOR LEVEL ] 
                {
                    RandomPosToRot = Random.Range(0, 2);
                }
                if (PlayerLevel >= 17)
                {
                    for (int i = 0; i < 5; i++)
                    {

                        int randPos = Random.Range(0, 2);

                        RandomPosToMoveTo[i] = randPos;
                    }
                }
            }
        }
        
        if ( PlayerLevel != 19 && Block.transform.position.z <= Balls[0].transform.position.z-1f )               //       for other levels   
        {
            Destroy(Block.gameObject);
        }
       

        // working on this

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
        ///       CLAMP POSITION FOR FIRST BALLs
        /// 
        /// </summary>

        Balls[0].transform.position = new Vector3(Mathf.Clamp(Balls[0].transform.position.x, -4.5f, 4.5f), Mathf.Clamp(Balls[0].transform.position.y, -4.5f, 4.5f), 0);

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

                    Balls[i].transform.position = Vector3.Slerp(Balls[i].transform.position, ToPos, 1f / Mathf.Abs(Balls.Count - i));
                    //Balls[i].transform.position = Vector3.Slerp(Balls[i].transform.position, ToPos, 1f);
                }
            }
        }

        ///<summary>
        ///
        ///        Camera Position Slerp to Ball Position
        /// 
        ///</summary>

        DistBallToCam = Vector3.SqrMagnitude(MainCam.position - Balls[(Balls.Count > 15 ? 14 : Balls.Count-1)].transform.position);

        Vector3 BallToCam = new Vector3(Balls[(Balls.Count > 15 ? 14 : Balls.Count - 1)].transform.position.x, Balls[(Balls.Count > 15 ? 14 : Balls.Count - 1)].transform.position.y + 2, -10);
        //Debug.Log(Balls[Balls.Count - 1].transform.position.y - MainCam.transform.position.y);
        if (Mathf.Abs(Balls[0].transform.position.y - MainCam.transform.position.y) < -1)
        {
            MainCam.transform.position = Vector3.Slerp(MainCam.transform.position, BallToCam, 0.01f * Balls.Count > 15? 14 : Balls.Count * (DistBallToCam * 0.05f));
        }
        else
        {
            MainCam.transform.position = Vector3.Slerp(MainCam.transform.position, BallToCam, 0.08f);
        }
        // Edit multiply value for better result

    }

    private void FixedUpdate()
    {
        if(Balls.Count == 0)
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
    ///             Blocks Instatiate === Random Color === Block color to Ball color
    ///
    /// </summary>

    void BlocksInstantiate(int Block_n)
    {
        int RandBlock = Random.Range(Block_n, Blocks.Length);

        Block = Instantiate(Blocks[RandBlock], BlockParent);

        CurrentBlockNum = RandBlock;

        int childWithSameColor = Random.Range(0, Blocks[RandBlock].childCount); // Get a random child object from blockParent
        int iter=0;

        foreach (Transform childBlock in Block)
        {
            MeshRenderer BlockMR = childBlock.GetComponent<MeshRenderer>();

            if (iter != childWithSameColor)
            {
                int RandMat = Random.Range(0, MatPrefabs.Length);

                BlockMR.material = MatPrefabs[RandMat];  
            }
            else
            {
                string BallMatName = Balls[0].GetComponent<MeshRenderer>().material.name.Substring(0,1);

                if (BallMatName == "b")
                {
                    BlockMR.material = MatPrefabs[0];
                }
                else if (BallMatName == "g")
                {
                    BlockMR.material = MatPrefabs[1];
                }
                else if (BallMatName == "o")
                {
                    BlockMR.material = MatPrefabs[2];
                }
                else if (BallMatName == "r")
                {
                    BlockMR.material = MatPrefabs[3];
                }
                else if (BallMatName == "y")
                {
                    BlockMR.material = MatPrefabs[4];
                }
            }

            iter++;

        }
    }

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

    /// <summary>
    ///
    ///                Small Interval Blocksssss
    ///                
    ///                NOTE:these blocks are getting destroyed when ball collides FIX THAT
    /// 
    /// </summary>
    void SmallIntervalBlockInstantiate()
    {
        if(BlockParent.childCount == 0)
        {
            for (int i = 0; i < 25; i++)
            {
                Transform SmallIntBlock_ = Instantiate(SmallIntervalBlock, new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 20f), Quaternion.identity, BlockParent);

                MeshRenderer SIMeshRend = SmallIntBlock_.GetChild(0).GetComponent<MeshRenderer>();

                int RandColor = Random.Range(0, MatPrefabs.Length);

                SIMeshRend.material = MatPrefabs[RandColor];
            }
            // we can also make levels without initializing for asthetic look
            StartCoroutine(InitSmallIntervalBlocks());
        }
        else
        {
            foreach(Transform t in BlockParent)
            {
                if (t.position.z < MainCam.position.z)
                {
                    SmallBlocksToBack(t);
                }
            }
        }
    }

    public void SmallBlocksToBack(Transform t)
    {
        t.position = new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 20f);
        MeshRenderer SIMeshRend = t.GetChild(0).GetComponent<MeshRenderer>();

        int RandColor = Random.Range(0, MatPrefabs.Length);

        SIMeshRend.material = MatPrefabs[RandColor];
    }

    IEnumerator InitSmallIntervalBlocks()
    {
        foreach (Transform t in BlockParent)
        {
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
        yield return new WaitForSeconds(3);

        Instr2 = true;     
    }
    /// <summary>
    ///
    ///         Change Rotation Side used At LEVEL 14-15    ;;;
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeRotationSide()
    {
        yield return new WaitForSeconds(3);

        ChangeRotSide = true;
    }


    void LevelN()
    {
        PlayerPrefs.SetInt("Level",PlayerLevel);
        //Debug.Log(clevel);
    }
}

