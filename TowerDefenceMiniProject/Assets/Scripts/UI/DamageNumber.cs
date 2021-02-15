using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationCurve height;
    private TextMeshProUGUI text;
    private Vector3 startPosition;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        startPosition = text.rectTransform.position;

        StartCoroutine(WaitForAnimationToFinish());
    }

    private IEnumerator WaitForAnimationToFinish()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float timer = 0f;

        while (timer < info.length)
        {
            timer += Time.deltaTime;

            Vector3 new_offset = new Vector3(0f, height.Evaluate(timer));
            text.rectTransform.position = startPosition + new_offset;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
