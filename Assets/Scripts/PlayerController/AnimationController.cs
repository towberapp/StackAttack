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
        EventsController.playerDirectionEvent.AddListener(OnChangeDirection);
    }

    private void OnChangeDirection(int arg0)
    {
        FlipPlayer(arg0);
    }

    private void OnIdle()
    {
        anim.SetBool("Run", false);
        //print("ANIM IDLE");
    }

    private void OnPush()
    {
        //print("ANIM PUSH");
        anim.Play("Player_push");
        anim.SetBool("Run", true);
    }

    private void OnRun()
    {
        //print("ANIM RUN");
        anim.Play("Player_run");
        anim.SetBool("Run", true);
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
        EventsController.playerDirectionEvent.RemoveAllListeners();
    }

 

 
}
