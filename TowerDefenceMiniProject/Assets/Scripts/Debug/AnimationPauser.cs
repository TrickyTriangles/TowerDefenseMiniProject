using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AnimationPauser : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animation_state_name;

    [SerializeField] [Range(0, 1f)] private float normalized_time;
    public float Normalized_time
    {
        get { return normalized_time; }
        set { normalized_time = value; MoveToFrame(); }
    }

    private void Start()
    {
        MoveToFrame();
    }

    private void OnValidate()
    {
        Normalized_time = normalized_time;
    }

    private void MoveToFrame()
    {
        if (animator != null)
        {
            animator.speed = 0f;
            animator.Play(animation_state_name, 0, normalized_time);
        }
    }
}
