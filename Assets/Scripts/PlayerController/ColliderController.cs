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
                    Invoke("GameOver", 1f);
                    SystemStatic.isStartGame = false;

                    GridController.DeleteCube(moveByGrid.x, moveByGrid.y);
                    EventsController.playerDestroyAnimationEvent.Invoke();
                    //EventsController.GameOverEvent.Invoke();
                }
            } else
            {
                Invoke("GameOver", 1f);
                SystemStatic.isStartGame = false;

                GridController.DeleteCube(moveByGrid.x, moveByGrid.y);
                EventsController.playerDestroyAnimationEvent.Invoke();
            }
    }

    private void GameOver()
    {
        //EventsController.GameOverEvent.Invoke();

        EventsController.BeforeGameOverEvent.Invoke();
    }

    
}
