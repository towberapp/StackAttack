using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private float offWait = 5f;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer playerSprite;

    public enum Status
    {
        Idle,
        Run,
        Push,
        Jump,
        Off,
        Damage
    }

    public Status currentStatus = Status.Idle;

    private IEnumerator courutoneWhaitOff;

    private void Awake()
    {
        EventsController.playerRunAnimationEvent.AddListener(OnRun);
        EventsController.playerPushAnimationEvent.AddListener(OnPush);
        EventsController.playerIdleAnimationEvent.AddListener(OnIdle);
        EventsController.playerDirectionEvent.AddListener(OnChangeDirection);

        EventsController.playerOffAnimationEvent.AddListener(OnOff);
        EventsController.playerDestroyAnimationEvent.AddListener(OnDestroyPers);
        EventsController.playerJumpAnimationEvent.AddListener(OnJump);

        EventsController.playerStopMoove.AddListener(OnStopMoove);
        EventsController.blockMoove.AddListener(OnBlockMoove);
    }

    private void OnDestroyPers()
    {
        //Debug.Log("ANIM -> DAMAGE BOX");
        
        anim.SetBool("Off", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Run", false);
        anim.SetBool("BlockMoove", false);

        anim.SetTrigger("DamgeBox");

        StopCoroutine(courutoneWhaitOff);
    }

    private void OnBlockMoove(bool arg0)
    {
       // Debug.Log("ANIM -> BLOCK MOOVE :" + arg0);

        anim.SetBool("BlockMoove", arg0);
        
        if (arg0)
            anim.SetBool("Run", false);
    }

    private void OnStopMoove()
    {
        //Debug.Log("ANIM CONTROLLER -> STOP MOOVE");
    }


    private void OnOff()
    {
        anim.SetBool("Off", true);
    }

    private void OnChangeDirection(int arg0)
    {
        FlipPlayer(arg0);
    }

    private IEnumerator WaitForIdle()
    {
        yield return new WaitForSeconds(offWait);
        OnOff();
    }

    

    private void OnIdle()
    {
        //if (currentStatus == Status.Idle) return;

        //Debug.Log("ANIM -> IDLE");   
                
        anim.SetBool("Idle", true);
        anim.SetBool("BlockMoove", false);
        anim.SetBool("Run", false);

        FlipPlayer(0);

        currentStatus = Status.Idle;

        courutoneWhaitOff = WaitForIdle();
        StartCoroutine(courutoneWhaitOff);
    }

    private void OnRun()
    {
        //Debug.Log("ANIM -> RUN");

        if (currentStatus == Status.Run) return;
        if (currentStatus == Status.Jump) return;

        anim.SetBool("Off", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Run", true);


        //anim.SetTrigger("run");
        currentStatus = Status.Run;

        if (courutoneWhaitOff != null)
            StopCoroutine(courutoneWhaitOff);
    }


    private void OnPush()
    {
 /*       Debug.Log("ANIM -> PUSH");

        if (currentStatus == Status.Push) return;
        

        anim.SetBool("Idle", false);

        anim.SetTrigger("push");
        currentStatus = Status.Push;
 */
    }


    private void OnJump(int direct)
    {
        //Debug.Log("ANIM -> JUMP");


        if (currentStatus == Status.Jump) return;

        anim.SetBool("Off", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Run", false);

        if (courutoneWhaitOff != null)
            StopCoroutine(courutoneWhaitOff);

        if (direct != 0)
        {                        
            anim.SetTrigger("jump");
            currentStatus = Status.Jump;
        }


    }



    private void FlipPlayer(int x)
    {
        if (x > 0)
        {
            anim.SetBool("Flip", true);
            playerSprite.flipX = true;
        }
        else
        {
            anim.SetBool("Flip", false);
            playerSprite.flipX = false;
        }
    }


    private void OnDestroy()
    {
        EventsController.playerRunAnimationEvent.RemoveAllListeners();
        EventsController.playerPushAnimationEvent.RemoveAllListeners();
        EventsController.playerIdleAnimationEvent.RemoveAllListeners();
        EventsController.playerDirectionEvent.RemoveAllListeners();

        EventsController.playerOffAnimationEvent.RemoveAllListeners();
        EventsController.playerDestroyAnimationEvent.RemoveAllListeners();
        EventsController.playerJumpAnimationEvent.RemoveAllListeners();
    }

 

 
}
