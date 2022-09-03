using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    MoveByGrid moveByGrid;

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check hight of jump        
        if (transform.position.x == (float) moveByGrid.x)
        {
            if ((transform.position.y - SystemStatic.level - moveByGrid.y + 1) > 0.6 && moveByGrid.isMove)
            {
                collision.gameObject.SendMessage("DestroyCube");
            } else
            {
                EventsController.GameOverEvent.Invoke();
            }
        } else
        {
            EventsController.GameOverEvent.Invoke();
        }

    }

    
}
