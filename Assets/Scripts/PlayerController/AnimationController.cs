using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationController : MonoBehaviour
{

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer playerSprite;

    private void Awake()
    {
        EventsController.playerRunAnimationEvent.AddListener(OnRun);
        EventsController.playerPushAnimationEvent.AddListener(OnPush);
        EventsController.playerIdleAnimationEvent.AddListener(OnIdle);
    }

    private void OnIdle()
    {
        throw new NotImplementedException();
    }

    private void OnPush()
    {
        throw new NotImplementedException();
    }

    private void OnRun()
    {
        throw new NotImplementedException();
    }

    private void FlipPlayer(int x)
    {
        if (x > 0)
        {
            playerSprite.flipX = true;
        }
        else
        {
            playerSprite.flipX = false;
        }
    }

    private void OnDestroy()
    {
        EventsController.playerRunAnimationEvent.RemoveAllListeners();
        EventsController.playerPushAnimationEvent.RemoveAllListeners();
        EventsController.playerIdleAnimationEvent.RemoveAllListeners();
    }

 

 
}
