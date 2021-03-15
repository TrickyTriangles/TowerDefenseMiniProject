using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIExperienceReadout : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        StartCoroutine(StartupRoutine());
    }

    private void GameManager_OnExperienceChange(int new_value, int target_xp, int level)
    {
        if (text != null)
        {
            text.text = "LV " + level + ": " + new_value + "XP / " + target_xp + "XP";
        }
    }

    private IEnumerator StartupRoutine()
    {
        while (!GameManager.IsInitialized)
        {
            yield return null;
        }

        GameManager.Instance.Subscribe_OnExperienceChange(GameManager_OnExperienceChange);
    }
}
