using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float moveSpeed = 0.5f; // �������� �������� ������
    public float destroyXPosition = 15.0f; // ���������� X, ��� ������� ������ ����� ����������

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
        // ������ ������� ������ ������
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // ���������, �������� �� ������ �������� ���������� X ��� �����������
        if (transform.position.x >= destroyXPosition)
        {
            Destroy(gameObject);
        }
    }
}
