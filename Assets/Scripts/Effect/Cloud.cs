using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float moveSpeed = 0.5f; // Скорость движения облака
    public float destroyXPosition = 15.0f; // Координата X, при которой облако будет уничтожено

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    void Update()
    {
        // Плавно двигаем объект вправо
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // Проверяем, достигло ли облако заданной координаты X для уничтожения
        if (transform.position.x >= destroyXPosition)
        {
            Destroy(gameObject);
        }
    }
}
