using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected float startedTime;
    protected bool isAnimationFinished;
    protected bool isExitingState;
    private string animationName;
    public PlayerState (Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationName)
    {
        this.player = player;
        this.playerData = playerData;
        this.stateMachine = stateMachine;
        this.animationName = animationName;
    }
    public virtual void Enter() {
        DoChecks();

        startedTime = Time.time;
        Debug.Log(stateMachine.CurrentState.ToString());
        player.Anim.SetBool(animationName, true);
        isAnimationFinished = false;
        isExitingState = false;
    }
    public virtual void Exit() {
        isExitingState = true;
        player.Anim.SetBool(animationName, false);
    }
    public virtual void LogicalUpdate() { }
    public virtual void PhysicalUpdate() {
        DoChecks();
    }
    public virtual void DoChecks() { }

    public virtual void AnimationTriggers() { }

    // trigger this function if you want to wait the animation to end before moving to next state 
    // you can trigger this animation by animation event
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
