using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class PlayerAnimationControllerNetwork : NetworkBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] PlayerControllerNetwork playerControllerNetwork;

    [SerializeField] float walkSpeed;
    [SerializeField] float blendSpeed;

    bool onFalling;

    private void Awake()
    {
        playerControllerNetwork = GetComponent<PlayerControllerNetwork>();
        animator = GetComponent<Animator>();


    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Assert("Threhold " + idle_walk_run.children[1].threshold);
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("changeAnimationInfo");
        //    ChangedAnimationInfo();
        //}
    }

    void ChangedAnimationInfo()
    {
        //var tmpBlendTree = movemantBlendTree.children;
        //tmpBlendTree[1].threshold = walkSpeed;
        //movemantBlendTree.children = tmpBlendTree;
    }

    public void MovemantAnimation(float currentSpeed)
    {
        //Dang sai nhung khong hieu sao no chay duoc ???

        var tmp = blendSpeed + Time.deltaTime * 10;
        blendSpeed = tmp < currentSpeed ? tmp : currentSpeed;

        //Debug.Log("moveAnim" + blendSpeed);
        animator.SetFloat("Speed", blendSpeed);
        //animator.Rebind();

    }

    public void OnFootstep()
    {
        //Debug.Log("Animation Event: OnFootstep");
    }

    public void JumpAnim()
    {
        animator.SetTrigger("Jump");
    }

    void JumpAnimCallback()
    {
        //playerControllerNetwork.JumpCallback();
    }


    void FallingAnim()
    {
        if (playerControllerNetwork.isGround)
        {
            if (onFalling)
            {
                onFalling = false;
                animator.SetTrigger("Ladding");
            }
        }
        else
        {
            if (!onFalling)
            {
                onFalling = true;
                animator.SetTrigger("Falling");
            }
        }
    }

  
    public void DrawArrowAim()
    {
        animator.SetTrigger("DrawArrow");
    }

    public void AimRecoilAim()
    {
        animator.SetTrigger("AimRecoil");
    }

    //void AimRecoilCallback()
    //{
    //    Debug.Log("Bang");
    //    bow.onDrawArrow = false;
    //    bow.AimRecoil();
    //}


}
