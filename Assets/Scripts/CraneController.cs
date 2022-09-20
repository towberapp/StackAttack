using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CraneController : MonoBehaviour
{
    [SerializeField] private GameObject cranGroup;
    [SerializeField] private GameObject cranPrefab;     

    private void Awake()
    {
        EventsController.StartEvent.AddListener(OnStartGame);
        EventsController.RunCran.AddListener(NewCube);
    }


    private void NewCube(GameObject block, int xPos)
    {
        Vector3 pos = cranGroup.transform.position;
        GameObject obj = Instantiate(cranPrefab, pos, Quaternion.identity, cranGroup.transform);        

        StartCoroutine(MoveObject(new Vector2Int(GridController.xPole + 4, 0), obj, xPos, block));
    }

    private void OnStartGame()
    {
        Vector3 pos = cranGroup.transform.position;
        cranGroup.transform.position = pos + new Vector3(0, GridController.yPole-0.33f-1, 10);
    }


    public IEnumerator MoveObject(Vector2 vector, GameObject obj, int stopPos, GameObject block)
    {
        //int rnd = stopPos > GridController.xPole / 2 ? -1 : 1;

        int rnd =  UnityEngine.Random.Range(0, 2) * 2 - 1;

        float startpos = rnd > 0 ? -2f : GridController.xPole + 2f;        

        obj.transform.position = new(startpos, obj.transform.position.y, obj.transform.position.z);


        block.transform.SetParent(obj.transform);
        block.transform.localPosition = new Vector3(0, -0.72f, 0);

        bool drop = false;
        float elapseTime = 0;
        float realSpeed = MainConfig.speedMove * vector.x / 2;

        Vector2 originPos = obj.transform.localPosition;
        Vector2 targetPos = originPos + vector * rnd;   
       

        while (elapseTime < realSpeed && !SystemStatic.isGameOver)
        {
            bool right = obj.transform.position.x <= stopPos;
            bool left = obj.transform.position.x >= stopPos;
            bool check = rnd > 0 ? left : right;
            
            if (check && !drop)
            {
                drop = true;
                EventsController.DropCran.Invoke(block, stopPos); 
                obj.GetComponent<CranLocalScript>().SetTrigger();
                yield return new WaitForSeconds(0.5f);                     
            }

            obj.transform.localPosition = Vector3.Lerp(originPos, targetPos, (elapseTime / realSpeed));

            elapseTime += Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }    

}
