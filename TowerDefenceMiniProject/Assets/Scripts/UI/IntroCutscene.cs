using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Action CutsceneIsOver;

    public void CutsceneEnded()
    {
        CutsceneIsOver?.Invoke();
    }

    public void StopAnimation()
    {
        if (animator != null)
        {
            animator.speed = 0f;
        }
    }

    public void PlayAnimation(string name)
    {
        if (animator != null)
        {
            animator.speed = 1f;
            animator.Play(name);
        }
    }
}
