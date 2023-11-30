using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionScript : MonoBehaviour
{
    public EviromentChecker enviromentChecker;
    bool playerInAction;
    public Animator animator;
    [Header("Parkour Action Area")]
    public List<NewAction> newAction;
    public PlayerMovement playerscript;
    void Update()
    {
        if (Input.GetButton("Jump") && !playerInAction)
        {
            var hitData = enviromentChecker.CheckObstacle();
            if (hitData.hitFound)
            {
                foreach (var action in newAction)
                {
                    if (action.CheckIfAvailable(hitData,transform))
                    {
                        StartCoroutine(PerformParkourAction(action));
                        break;
                    }
                }
            }
        }
    }
    IEnumerator PerformParkourAction(NewAction action)
    {
        playerInAction = true;
        playerscript.SetControl(false);
        animator.CrossFade(action.AnimationName, 0.2f);
        yield return null;
        var animationState = animator.GetNextAnimatorStateInfo(0);
        if (!animationState.IsName(action.AnimationName))
            Debug.Log("Animation Name is Incorrect");
        float timeCounter = 0f;
        while (timeCounter <= animationState.length)
        {
            timeCounter += Time.deltaTime;
            if (action.LookAtObstacle)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,action.RequireRotation,playerscript.rotSpeed*Time.deltaTime);
            }
            if (action.AllowTargetMatching)
            {
                CompareTarget(action);
            }
            yield return null;
        }
        playerscript.SetControl(true);
        playerInAction = false;
    }
    void CompareTarget(NewAction action)
    {
        animator.MatchTarget(action.ComparePosition, transform.rotation, action.CompareBodyPart, new MatchTargetWeightMask(action.ComparePositionWeight,0),action.CompareStartTime,action.CompareEndTime);
    }
}
