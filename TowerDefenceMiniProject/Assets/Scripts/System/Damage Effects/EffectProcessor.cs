using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectProcessor : MonoBehaviour
{
    public void StartNewEffect(DamageEffect new_effect)
    {
        StartCoroutine(HandleEffect(new_effect));
    }

    private IEnumerator HandleEffect(DamageEffect effect)
    {
        float timer = 0f;

        effect.BeginEffect();

        while (timer < effect.duration)
        {
            timer += Time.deltaTime;
            effect.ProcessEffect();

            yield return null;
        }

        effect.EndEffect();
    }
}
