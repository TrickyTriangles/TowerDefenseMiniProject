using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevelUpAnimation : MonoBehaviour
{
    [SerializeField] private Tower tower;

    public void AdjustTowerLevel_AnimationEvent()
    {
        if (tower != null)
        {
            tower.AdjustLevelDetails();
        }
    }
}
