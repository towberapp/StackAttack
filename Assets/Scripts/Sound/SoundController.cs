using System;
using UnityEngine;
using UnityEngine.Events;

public class SoundController : MonoBehaviour
{
    [Header("Robot")]
    [SerializeField] private AudioSource robot;
    [SerializeField] private AudioClip idlleClip;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip boomClip;

    [Header("Box")]
    [SerializeField] private AudioSource boxAudioSourse;    

    [Header("Specisal Box")]
    [SerializeField] private AudioSource tntAudioSource;
    
    [Header("Coin")]
    [SerializeField] private AudioSource coin;

    [Header("NextLevel")]
    [SerializeField] private AudioSource nextlevel;




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


    private void Awake()
    {
        EventsController.playerRunAnimationEvent.AddListener(OnRun);
        EventsController.blockMoove.AddListener(OnPush);
        EventsController.playerIdleAnimationEvent.AddListener(OnIdle);
        EventsController.playerJumpAnimationEvent.AddListener(OnJump);
        EventsController.playerDestroyAnimationEvent.AddListener(OnDestroyPers);

        //box
        EventsController.boxDownFloor.AddListener(BoxOnFloor);

        //box special
        EventsController.tntBlowUp.AddListener(TntBlowUp);

        //
        EventsController.NextLevelEvent.AddListener(OnNextLevel);
        EventsController.onTakeChip.AddListener(OnTakeChip);

    }

    private void OnNextLevel()
    {
        nextlevel.Play();
    }

    private void OnTakeChip()
    {
        coin.Play();
    }

    private void TntBlowUp()
    {
        tntAudioSource.Play();
    }

    private void BoxOnFloor()
    {
        boxAudioSourse.Play(); 
    }


    private void OnDestroyPers()
    {
        robot.Stop();
        robot.loop = false;
        robot.clip = boomClip;
        robot.Play();
    }

    private void OnJump(int arg0)
    {
      
    }

    private void OnIdle()
    {
        if (currentStatus == Status.Idle) return;

        robot.Stop();
        robot.loop = false;
        robot.clip = idlleClip;
        robot.Play();

        currentStatus = Status.Idle;
    }

    private void OnPush(bool arg0)
    {
        
    }

    private void OnRun()
    {
        if (currentStatus == Status.Run) return;

        robot.Stop();
        robot.loop = true;
        robot.clip = runClip;
        robot.Play();

        currentStatus = Status.Run;
    }
}
