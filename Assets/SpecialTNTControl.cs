using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTNTControl : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject fx;
    MoveByGrid moveByGrid;
    AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        moveByGrid = GetComponent<MoveByGrid>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartAnim()
    {
        anim.Play("tnt");
        StartCoroutine(fxStart());
    }

    public void PlayAudio()
    {
        audioSource?.Play();
    }

    IEnumerator fxStart()
    {
        yield return new WaitForSeconds(7f);

        if (moveByGrid.y >= 0)
        {

            GameObject obj = Instantiate(fx, transform.position, Quaternion.identity, transform.parent);
            SpriteRenderer render = GetComponent<SpriteRenderer>();
            render.enabled = false;
            BoomCobe();

            EventsController.tntBlowUp.Invoke();
        }
    }

    private void BoomCobe()
    {
        Debug.Log("BOOM FUNCTION");

        List<Vector2Int> listGO = new List<Vector2Int>();
        Vector2Int myPos = new(moveByGrid.x, moveByGrid.y);

        for (int xOffset = -1; xOffset <= 1; xOffset++)
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                Vector2Int offset = new(myPos.x+xOffset, myPos.y+yOffset);

                if (GridController.IsOutOfRange(offset)) continue;

                int type = GridController.mainGrid[offset.x, offset.y];

                Debug.Log("TYPE: " + type);

                if (type >0)
                {
                    Debug.Log("Add POS: " + offset);
                    listGO.Add(offset);                    
                }                             
            }

        Debug.Log("ADD GAME OBJ");

        List<GameObject> listObject = new();
        for (int i = 0; i < listGO.Count; i++)
        {
            if (GridController.IsOutOfRange(listGO[i])) continue;
            if (GridController.mainGrid[listGO[i].x, listGO[i].y] == 0) continue;
            if (GridController.blockGrid[listGO[i].x, listGO[i].y] == null) continue;

            Debug.Log("ADD OBJ: " + GridController.blockGrid[listGO[i].x, listGO[i].y].name);

            listObject.Add(GridController.blockGrid[listGO[i].x, listGO[i].y]);
        }

        Debug.Log("TRY DESTROY");

        for (int i = 0; i < listObject.Count; i++)
        {
            if (listObject[i].TryGetComponent<IndividualBlockControl>(out var individualBlockControl))
            {
                Debug.Log("START DESTRY CUBE: " + listObject[i].name);
                individualBlockControl.DestroyCube();
            }
                

            if (listObject[i].TryGetComponent<MoveController>(out _))
            {
                    Debug.Log("START DESTRY PLAYER");
                    GridController.StartDethPlayer();                  
            }
                
        }
    }

}
