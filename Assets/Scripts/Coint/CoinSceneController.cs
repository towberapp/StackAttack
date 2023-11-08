
using System.Collections.Generic;
using UnityEngine;

public class CoinSceneController : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprite;
    
    [SerializeField] private GameObject piupiu;
    [SerializeField] private GameObject piupiuRed;


    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    private void Start()
    {
        spriteRenderer.sprite = GetRandomSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Coin Trigger: Player");

            EventsController.onTakeChip.Invoke();
            Instantiate(piupiu, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        // если столкнулс€ с кубом - уничтожить
        if (collision.CompareTag("Cube")) 
        {            
            Debug.Log("Coin Trigger: Cube");

            Instantiate(piupiuRed, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }


    Sprite GetRandomSprite()
    {
        if (sprite.Count == 0)
        {
            Debug.LogWarning("—писок sprite пуст.");
            return null;
        }

        int randomIndex = Random.Range(0, sprite.Count);
        return sprite[randomIndex];
    }

}
