using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [SerializeField] private GameObject puff;
    
    MoveByGrid moveByGrid;
    

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Cube") || collision.CompareTag("CubeCoin"))
            // check hight of jump        
            if (transform.position.x == (float) moveByGrid.x)
            {
                if ((transform.position.y - SystemStatic.level - moveByGrid.y + 1) > 0.6 && moveByGrid.isMove)
                {
                    Instantiate(puff, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                    collision.gameObject.SendMessage("DestroyCube");

                } else
                {
                    Debug.Log("Start Deth in fly Collider");
                    GridController.StartDethPlayer();

                }
            } else
            {
                Debug.Log("Start Deth Collider");
                GridController.StartDethPlayer();
            }
    }

/*    private void DeletePlayer()
    {
        Vector2Int posPlayer = new Vector2Int(MainConfig.playerX, MainConfig.playerY);
        GameObject player = GridController.blockGrid[posPlayer.x, posPlayer.y];

        if (player == null) return;

        if (player.CompareTag("Player"))
        {
            Debug.Log($"Try Del player -> pos: {posPlayer}, obj: {player}");
            GridController.DeleteCube(posPlayer.x, posPlayer.y);
        }            
        else
        {
            Debug.Log($"Try Del player -> pos: {posPlayer}, obj: {player}");
            Debug.LogWarning("Ошибка удаления игрока");
        }           
    }*/

    /*private void StartDethPlayer()
    {
        SystemStatic.isStartGame = false;
        GridController.DeletePlayer();
        EventsController.StartBeforeGameOverEvent.Invoke();
        EventsController.playerDestroyAnimationEvent.Invoke();
    }*/



    
}
