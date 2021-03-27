using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    public delegate void UpdateUIOnCoinCollected(int new_value);
    public delegate void UpdateUIOnExperienceChange(int new_value, int target_value, int level, int current_level_target_xp);
    public delegate void HeroKilledMobEvent(int mob_exp_value, Hero hero);

    [SerializeField] private HeroExperienceManager hero_experience_manager;
    private int gold_total;
    private int experience_total;

    private CoinCollectDelegate coin_collect_delegate;
    public CoinCollectDelegate CoinCollectDelegate
    {
        get { return coin_collect_delegate; }
    }

    private HeroKilledMobEvent hero_killed_mob_delegate;
    public HeroKilledMobEvent HeroKilledMob
    {
        get { return hero_killed_mob_delegate; }
    }

    private Action<Hero, int> hero_gained_experience_event;
    public Action<Hero, int> HeroGainedExperienceEvent
    {
        get { return hero_gained_experience_event; }
    }

    private UpdateUIOnCoinCollected update_ui_coin_collected_delegate;
    private UpdateUIOnExperienceChange update_ui_on_experience_change_delegate;

    public void Subscribe_OnCoinCollected(UpdateUIOnCoinCollected del) { update_ui_coin_collected_delegate += del; }
    public void Unsubscribe_OnCoinCollected(UpdateUIOnCoinCollected del) { update_ui_coin_collected_delegate -= del; }

    public void Subscribe_OnExperienceChange(UpdateUIOnExperienceChange del) { update_ui_on_experience_change_delegate += del; }
    public void Unsubscribe_OnExperienceChange(UpdateUIOnExperienceChange del) { update_ui_on_experience_change_delegate -= del; }

    private void Start()
    {
        coin_collect_delegate = Coin_OnCollected;
        hero_killed_mob_delegate = Hero_KilledMob;
        hero_gained_experience_event = Hero_GainedExperience;
    }

    private void Hero_KilledMob(int mob_experience_value, Hero hero)
    {
        Hero_GainedExperience(hero, mob_experience_value);
    }

    private void Hero_GainedExperience(Hero hero, int value)
    {
        experience_total += value;

        if (EnvironmentText.IsInitialized)
        {
            EnvironmentText.Instance.DrawText("+" + value + "XP", EnvironmentText.TextTypes.EXPERIENCE, hero.transform.position);
        }

        int current_level = hero_experience_manager.EvaluateLevel(experience_total);
        int target_xp = hero_experience_manager.GetTargetExperienceForNextLevel(current_level);
        int current_level_target_xp = hero_experience_manager.GetTargetExperienceForCurrentLevel(current_level);

        if (current_level != hero.Level) { hero.Level = current_level; }

        update_ui_on_experience_change_delegate?.Invoke(experience_total, target_xp, current_level, current_level_target_xp);
    }

    private void Coin_OnCollected(object sender, int value)
    {
        gold_total += value;
        update_ui_coin_collected_delegate?.Invoke(gold_total);
    }
}
