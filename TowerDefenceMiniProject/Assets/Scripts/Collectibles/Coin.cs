using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void CoinCollectDelegate(object sender, int value);

public class Coin : MonoBehaviour
{
    private CoinCollectDelegate OnCollected;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider my_collider;
    [SerializeField] private int value = 5;

    public void Subscribe_OnCollected(CoinCollectDelegate del)
    {
        OnCollected += del;
    }

    public void Unsubscribe_OnCollected(CoinCollectDelegate del)
    {
        OnCollected -= del;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hero"))
        {
            my_collider.enabled = false;
            OnCollected?.Invoke(this, value);
            StartCoroutine(CollectRoutine());
        }
    }

    private IEnumerator CollectRoutine()
    {
        animator.Play("Collect");

        yield return new WaitForEndOfFrame();

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        while (info.normalizedTime < 1f)
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        Destroy(gameObject);
    }
}
