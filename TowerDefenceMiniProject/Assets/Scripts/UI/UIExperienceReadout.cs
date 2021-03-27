using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIExperienceReadout : MonoBehaviour
{
    [SerializeField] private Image experience_bar;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        StartCoroutine(StartupRoutine());
        experience_bar.fillAmount = 0f;
    }

    private void GameManager_OnExperienceChange(int new_value, int target_xp, int level, int current_level_target_xp)
    {
        if (experience_bar != null)
        {
            float ratio = (float)(new_value - current_level_target_xp) / (target_xp - current_level_target_xp);
            experience_bar.fillAmount = ratio;
        }

        if (text != null)
        {
            text.text = level.ToString();
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
