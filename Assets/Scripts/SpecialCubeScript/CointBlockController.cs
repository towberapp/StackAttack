using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(IndividualBlockControl))]
public class CointBlockController : MonoBehaviour
{
    [SerializeField] private GameObject coinGroup;

    private Animator animator;
    private IndividualBlockControl individualBlockControl;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        individualBlockControl = GetComponent<IndividualBlockControl>();

        individualBlockControl.touchGround.AddListener(OnTouchGround);
    }

    private void OnTouchGround()
    {
        //Debug.Log("ON TOUCH GROUND");

        animator.SetTrigger("Break");
        Instantiate(coinGroup, transform.position, transform.rotation);
        Invoke("DestroyCube", 0.6f);

    }

    private void DestroyCube()
    {
        individualBlockControl.DestroyCube();
    }


    private void OnDestroy()
    {
        individualBlockControl.touchGround.RemoveListener(OnTouchGround);
    }



}
