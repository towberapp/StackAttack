using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCubeGenerator : MonoBehaviour
{

    [Header("Settings")]

    [SerializeField] [Range(0, 10)] private int specialCubePercent;
    [SerializeField][Range(5f, 15f)] private float timerForTimeBonus = 7f;

    [Header("Cubes")]

    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject cubeFolder;

    [SerializeField] private Sprite[] spriteBlock;
    [SerializeField] private GameObject[] specialBlock;

    private float time = 0.0f;
    private int lastRand = -1;

    [SerializeField] private bool stopCran = false;

     
    private void Update()
    {
        if (!SystemStatic.isGameOver && SystemStatic.isStartGame) 
        { 
            time += Time.deltaTime;
            if (time >= MainConfig.intervalCube && !stopCran)
            {
                time = 0.0f;
                NewBlock();
            }
        }
    }



    private void Awake()
    {
        EventsController.StartEvent.AddListener(OnStartGame);
        EventsController.DropCran.AddListener(DropCranEvent);

        BonusController.bonusTimeEvent.AddListener(OnTimerStop);
    }

    private void OnTimerStop()
    {
        //Debug.Log("ON TIMER STOP PRESTRT");

        StartCoroutine(WaitAndStopTimer(timerForTimeBonus));
    }

    private IEnumerator WaitAndStopTimer(float seconds)
    {
        //Debug.Log("BONUS -> TIMER START");
        stopCran = true;
        yield return new WaitForSeconds(seconds);
        stopCran = false;
        //Debug.Log("BONUS -> TIMER USED");
        BonusController.onUseBonus.Invoke();
    }


    private void OnStartGame()
    {
/*        TestGenerator(2, 0);
        TestGenerator(3, 1);
        TestGenerator(4, 1);*/
    }


    private void TestGenerator(int x, int count) 
    {
        int y = GridController.yPole - 1;
        Vector2Int pos = new Vector2Int(x, y + SystemStatic.level);
        Vector2Int arrayPos = new(x, y);
        
        GameObject obj = GenerateSpecialObj(pos, count);
        SetCoords(arrayPos, obj);
    }


    private void NewBlock()
    {
        int x = GetEmptyCoord();        

        if (x >= 0)
        {
            GetNewBlock(x);
        } else 
        {
            EventsController.GameOverEvent.Invoke();
            SystemStatic.isGameOver = true;
            SystemStatic.isStartGame = false;
            Debug.LogWarning("GAME OVER");
        }
    }


    private void GetNewBlock(int x)
    {
        int y = GridController.yPole - 1;

        Vector2Int pos = new Vector2Int(x, y + SystemStatic.level);
        Vector2Int arrayPos = new(x, y);

        //random simple special
        int getRandom = Random.Range(1,11);
        
        MainConfig.countCubeSet++;

        GameObject obj;
        if (getRandom > specialCubePercent)
        {
            obj = GeneratSimpleBlock(pos);
        } else
        {
            int blockInt = Random.Range(0, specialBlock.Length);
            obj = GenerateSpecialObj(pos, blockInt);
        }

        //obj.SetActive(false);
        EventsController.RunCran.Invoke(obj, x);

        //Debug.Log("SetCoords:" + arrayPos);
            
        SetCoords(arrayPos, obj);
    }


    private void DropCranEvent(GameObject block, int xPos)
    {        
        block.transform.SetParent(cubeFolder.transform);
        block.GetComponent<IndividualBlockControl>().DropCran();
    }


    private void SetCoords(Vector2Int pos, GameObject obj)
    {
        //if (MainConfig.countCubeSet == 10000) MainConfig.countCubeSet = 10;
        //obj.GetComponent<SpriteRenderer>().sortingOrder = MainConfig.countCubeSet + 10;

        MoveByGrid moveByGrid = obj.GetComponent<MoveByGrid>();
        moveByGrid.x = pos.x;
        moveByGrid.y = pos.y;

        GridController.blockGrid[pos.x, pos.y] = obj;
        GridController.mainGrid[pos.x, pos.y] = 1;
    }


    private GameObject GenerateSpecialObj(Vector2Int pos, int blockInt)
    {             
        GameObject obj = Instantiate(specialBlock[blockInt], ((Vector3Int)pos), Quaternion.identity, cubeFolder.transform);

        return obj;
    }

    private GameObject GeneratSimpleBlock (Vector2Int pos)
    {
        GameObject obj = Instantiate(cube, ((Vector3Int)pos), Quaternion.identity, cubeFolder.transform);
        int spriteInt = Random.Range(0, spriteBlock.Length);
        obj.GetComponent<SpriteRenderer>().sprite = spriteBlock[spriteInt];

        return obj;
    }
    




    private int GetEmptyCoord()
    {
        int count = 0;
        List<int> termsList = new List<int>();

        for (int x = 0; x < GridController.xPole; x++)
        {
            if (GridController.mainGrid[x, GridController.yPole - 2] == 1)
            {
                count++;
            } else
            {
                termsList.Add(x);
            }                
        }


        if (count == GridController.xPole)
        {           
            return -1;
        }
        int[] terms = termsList.ToArray();
        
        int randItem = Random.Range(0, terms.Length);

        while (randItem == lastRand)
        {
            randItem = Random.Range(0, terms.Length);
        }

        lastRand = randItem;

        return terms[randItem];
    }


}
