using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGoldReadout : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        StartCoroutine(StartupRoutine());
    }

    private void GameManager_UpdateUIOnCoinCollected(int new_value)
    {
        if (text != null)
        {
            text.text = new_value + "G";
        }
    }

    private IEnumerator StartupRoutine()
    {
        while (!GameManager.IsInitialized)
        {
            yield return null;
        }

        GameManager.Instance.Subscribe_OnCoinCollected(GameManager_UpdateUIOnCoinCollected);
    }
}
