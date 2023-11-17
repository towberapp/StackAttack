using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileCube : MonoBehaviour
{
    
    [SerializeField] private List<Sprite> spriteAnim;
    [SerializeField] private float spriteDelay = 0.2f;

    IndividualBlockControl individualBlockControl;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        individualBlockControl = GetComponent<IndividualBlockControl>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() => individualBlockControl.touchGround.AddListener(OnTouchGround);


    private void OnTouchGround() => StartCoroutine(AnimateSprites());


    private IEnumerator AnimateSprites()
    {
        for (int i = 0; i < spriteAnim.Count; i++)
        {
            spriteRenderer.sprite = spriteAnim[i];
            yield return new WaitForSeconds(spriteDelay);
        }
    }
}
