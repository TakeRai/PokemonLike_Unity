using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : MonoBehaviour
{
    private Animator animator;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Animator Animator
    {
        get
        {
            return animator;
        }
    }

    public void playMoveEffect(string moveName)
    {
        animator.Play(moveName, 0, 0.0f);
        
    }


    public void loopCircle()
    {
        animator.Play( "Circle", 0, 0.0f);
    }

    public void stopCircle()
    {
        animator.Play("Idle", 0, 0.0f);
    }
}
