using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTNTControl : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject fx;
    MoveByGrid moveByGrid;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        moveByGrid = GetComponent<MoveByGrid>();
    }

    public void StartAnim()
    {
        anim.Play("tnt");
        StartCoroutine(fxStart());
    }

    IEnumerator fxStart()
    {
        yield return new WaitForSeconds(7f);

        if (moveByGrid.y >= 0)
        {

            GameObject obj = Instantiate(fx, transform.position, Quaternion.identity, transform.parent);
            SpriteRenderer render = GetComponent<SpriteRenderer>();
            render.enabled = false;
            BoomCobe();
            yield return new WaitForSeconds(2f);
            Destroy(obj);
        }
    }

    private void BoomCobe()
    {
        Vector2Int myPos = new(moveByGrid.x, moveByGrid.y);
        Vector2Int left = myPos + Vector2Int.left;
        Vector2Int right = myPos + Vector2Int.right;
        Vector2Int up = myPos + Vector2Int.up;
        Vector2Int leftUp = myPos + Vector2Int.left + Vector2Int.up;
        Vector2Int rightUp = myPos + Vector2Int.right + Vector2Int.up;

        Vector2Int leftDown = myPos - Vector2Int.left + Vector2Int.up;
        Vector2Int rightDown = myPos - Vector2Int.right + Vector2Int.up;
        Vector2Int down = myPos - Vector2Int.up;

        DeleteCube(left);
        DeleteCube(right);
        DeleteCube(up);
        DeleteCube(leftUp);
        DeleteCube(rightUp);

        if (down.y >= 0) 
        { 
            DeleteCube(down);
            DeleteCube(leftDown);
            DeleteCube(rightDown);
        }

        DeleteCube(myPos);

    }

    private void DeleteCube(Vector2Int pos) 
    {

        if (GridController.IsOutOfRange(pos)) return;
        
        if (pos.x == MainConfig.playerX && pos.y == MainConfig.playerY)
        {
            //gameOver
            Debug.Log("GameOver 1");
            EventsController.GameOverEvent.Invoke();
            return;            
        }
 
        if (!GridController.CheckCubeInArray(pos)) return;

        GameObject obj = GridController.blockGrid[pos.x, pos.y];
        Destroy(obj);
        GridController.DeleteCube(pos.x, pos.y);
    }


    private void GameOver()
    {
        Debug.Log("GameOver 2");        
    }

}
