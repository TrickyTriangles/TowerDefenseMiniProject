using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroExperienceManager : MonoBehaviour
{
    [SerializeField] private int[] experience_per_level;

    public int EvaluateLevel(int current_experience)
    {
        for (int i = 0; i < experience_per_level.Length; i++)
        {
            if (current_experience < experience_per_level[i])
            {
                return i;
            }
        }

        return experience_per_level.Length;
    }

    public int GetTargetExperienceForCurrentLevel(int current_level)
    {
        return experience_per_level[current_level - 1];
    }

    public int GetTargetExperienceForNextLevel(int current_level)
    {
        if (current_level < experience_per_level.Length)
        {
            return experience_per_level[current_level];
        }
        else
        {
            return experience_per_level[current_level - 1];
        }
    }
}
