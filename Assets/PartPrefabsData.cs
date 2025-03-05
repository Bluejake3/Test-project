using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPrefabsData : MonoBehaviour
{
    [Header("Part Type")]
    [SerializeField] private BodyPartType bodyPartType;

    [Header("Basic Stats")]
    [SerializeField] private float hitPoint;
    [SerializeField] private StatType statType;
    [SerializeField] private float statBoostAmount;

    [Header("Attack Bonus Atrribute")]
    [SerializeField] private AttackType attackBonusType;
    [SerializeField] private float AttackBonusValue;

    [Header("Attack Resistance Attribute")]
    [SerializeField] private AttackType attackTypeResistance;
    [SerializeField] private float resistanceValue;

    [Header("Arts")]
    [SerializeField] private AudioSource attackSFX;
    [SerializeField] private Animation animations;

    public BodyPartType GetBodyPartType() {
        return bodyPartType;
    }

    public float GetHitPoint() {
        return hitPoint;
    }

    public StatType GetStatType(){
        return statType;
    }
    public float GetStatBoostAmount(){
        return statBoostAmount;
    }

    public AttackType GetAttackTypeBonus(){
        return attackBonusType;
    }

    public float GetAttackBonusValue()
    {
        return AttackBonusValue;
    }

    public AttackType GetAttackTypeResistace(){
        return attackTypeResistance;
    }

    public float GetResistanceValue(){
        return resistanceValue;
    }

    public void PlayAttack(){
        animations.Play();
        attackSFX.Play();
    }
}
