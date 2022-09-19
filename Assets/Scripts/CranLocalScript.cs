using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CranLocalScript : MonoBehaviour
{
    [SerializeField] Animator m_Animator;
     
    public void SetTrigger()
    {
        //m_Animator.SetTrigger("Drop");
        m_Animator.Play("roga");
    }

}
