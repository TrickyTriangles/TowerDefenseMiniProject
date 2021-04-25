using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private TowerBuildManager tower_build_manager;
    [SerializeField] private Reticle reticle;
    private Coroutine level_up_routine;

    [Header("Level Up Routine Values")]
    [SerializeField] [Range(0.1f, 2f)] private float confirmation_time;

    [Header("Confirmation Meter Elements")]
    [SerializeField] private Image level_info_panel;
    [SerializeField] private Image confirmation_bar_fill;
    [SerializeField] private Image confirmation_bar_back;
    [SerializeField] private TextMeshProUGUI tower_level_up_text;

    [Header("Money Requirement Elements")]
    [SerializeField] private Image money_icon;
    [SerializeField] private TextMeshProUGUI money_readout;

    [Header("Power Upgrade Elements")]
    [SerializeField] private Image power_icon;
    [SerializeField] private TextMeshProUGUI power_readout;

    [Header("Range Upgrade Elements")]
    [SerializeField] private Image range_icon;
    [SerializeField] private TextMeshProUGUI range_readout;

    [Header("Delay Upgrade Elements")]
    [SerializeField] private Image delay_icon;
    [SerializeField] private TextMeshProUGUI delay_readout;

    private void Start()
    {
        if (tower_build_manager != null)
        {
            tower_build_manager.SubscribeToInitiateTowerLevelUpEvent(TowerBuildManager_InitiateTowerLevelUp);
            tower_build_manager.SubscribeShowTowerData(TowerBuildManager_ShowTileData);
            tower_build_manager.SubscribeHideTowerData(TowerBuildManager_HideTileData);
        }
    }

    private void TowerBuildManager_InitiateTowerLevelUp(Tower tower)
    {
        if (GameManager.Instance.Gold >= tower.UpgradeCost)
        {
            if (level_up_routine == null && !tower.IsMaxLevel)
            {
                level_up_routine = StartCoroutine(LevelUpRoutine(tower));
            }
        }
    }

    private void TowerBuildManager_ShowTileData(TileData tile)
    {
        Tower tower = tile.GetTower();
        transform.position = _camera.WorldToScreenPoint(tile.transform.position);

        if (level_info_panel != null)
        {
            level_info_panel.gameObject.SetActive(true);

            confirmation_bar_fill.fillAmount = tower.IsMaxLevel ? 1f : 0f;
            tower_level_up_text.text = tower.IsMaxLevel ? "MAX" : tower.Level.ToString() + " - " + (tower.Level + 1).ToString();
            money_readout.text = tower.IsMaxLevel ? "-- G" : tower.UpgradeCost.ToString() + " G";
            money_readout.color = GameManager.Instance.Gold >= tower.UpgradeCost ? Color.white : Color.red;
            power_readout.text = tower.IsMaxLevel ? tower.CurrentPower.ToString() : tower.CurrentPower.ToString() + " - " + tower.NextPower.ToString();
            range_readout.text = tower.IsMaxLevel ? tower.CurrentRange.ToString("F") : tower.CurrentRange.ToString("F") + " - " + tower.NextRange.ToString("F");
            delay_readout.text = tower.IsMaxLevel ? tower.CurrentDelay.ToString("F") : tower.CurrentDelay.ToString("F") + " - " + tower.NextDelay.ToString("F");
        }
    }

    private void TowerBuildManager_HideTileData()
    {
        if (level_info_panel != null)
        {
            level_info_panel.gameObject.SetActive(false);
        }
    }

    private IEnumerator LevelUpRoutine(Tower tower)
    {
        float timer = 0f;

        reticle.LockReticle();

        while (Input.GetMouseButton(1))
        {
            timer += Time.deltaTime;
            confirmation_bar_fill.fillAmount = MathUtils.SmootherStep(0f, confirmation_time, timer);

            if (timer >= confirmation_time)
            {
                GameManager.Instance.RemoveGold(tower.UpgradeCost);
                tower.LevelUp();
                break;
            }

            yield return null;
        }

        reticle.UnlockReticle();
        level_up_routine = null;
    }
}
