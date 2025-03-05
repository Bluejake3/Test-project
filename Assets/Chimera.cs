using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField] private List<PartPrefabsData> bodyParts;
    PartPrefabsData firstForelegPart;
    [SerializeField] private bool validBody = false;

    [Header("Basic Stats")]
    [SerializeField] private float hitPoint = 0; 
    [SerializeField] private float attack = 0;
    [SerializeField] private float defense = 0; 
    [SerializeField] private float agility = 0;

    [Header("Attack Bonus")]
    [SerializeField] private float slashBonus = 0;
    [SerializeField] private float pierceBonus = 0;
    [SerializeField] private float bluntBonus = 0;
    float currentAttackBonus = 0;
    AttackType currentAttackType;


    private void Awake() {
        CheckValidity();
        GatherStat();
        SearchFirstForelegPart();
    }
    void CheckValidity(){
        bool headExist = false, forelegExist = false, backlegExist = false, torsoExist = false;
        foreach(PartPrefabsData currentParts in bodyParts){
            
            switch(currentParts.GetBodyPartType()){
                case BodyPartType.head:
                {
                    if(!headExist) headExist = true;
                    break;
                }
                case BodyPartType.foreleg:
                {
                    if(!forelegExist) forelegExist = true;
                    break;
                }
                case BodyPartType.backleg:
                {
                    if(!backlegExist) backlegExist = true;
                    break;
        
                }
                case BodyPartType.torso:
                {
                    if(!torsoExist) torsoExist = true;
                    break;
                }
            }
            if(headExist && forelegExist && backlegExist && torsoExist){
                Debug.Log("Body verified");
                validBody = true;
                break;
            }
        }
        if(!validBody) Debug.LogWarning("Body Invalid");
    }
    void GatherStat(){
        if (validBody){
            foreach(PartPrefabsData currentParts in bodyParts){
                GatherStatBonuses(currentParts);
            }
        }
    }

    void SearchFirstForelegPart(){
        foreach(PartPrefabsData currentParts in bodyParts){
            if(currentParts.GetBodyPartType() != BodyPartType.foreleg){
                continue;
            }
            else{
                firstForelegPart = currentParts;
                break;
            }

        }
    }

    void GatherStatBonuses(PartPrefabsData bodyPart){
        hitPoint += bodyPart.GetHitPoint();
        switch (bodyPart.GetStatType())
        {
            case StatType.attack:
            {
                attack += bodyPart.GetStatBoostAmount();
                break;
            }
            case StatType.defense:
            {
                defense += bodyPart.GetStatBoostAmount();
                break;
            }
            case StatType.agility:
            {
                agility += bodyPart.GetStatBoostAmount();
                break;
            }
            default:
            { 
                Debug.Log("failed"); 
                break;
            }
        }

        switch (bodyPart.GetAttackTypeBonus()){
            case AttackType.slash:
            {
                slashBonus += bodyPart.GetAttackBonusValue();
                break;
            }
            case AttackType.blunt:
            {
                bluntBonus += bodyPart.GetAttackBonusValue();
                break;
            }
            case AttackType.pierce:
            {
                pierceBonus += bodyPart.GetAttackBonusValue();
                break;
            }
            default :
            {
                Debug.Log("failed");
                break;
            }
        }
    }

    public void SlashAttack(){
        if(validBody){
            firstForelegPart.PlayAttack();
            currentAttackType = AttackType.slash;
            currentAttackBonus = slashBonus;
        }
    }

    public void PierceAttack(){
        if(validBody){
            firstForelegPart.PlayAttack();
            currentAttackType = AttackType.pierce;
            currentAttackBonus = pierceBonus;
        }
    }

    public void BluntAttack(){
        if(validBody){
            firstForelegPart.PlayAttack();
            currentAttackType = AttackType.blunt;
            currentAttackBonus = bluntBonus;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag != "Animals") return;
        GameObject hitObject = other.gameObject;
        PartPrefabsData hitBodyPart = hitObject.GetComponent<PartPrefabsData>();
        Animals hitAnimals = hitObject.GetComponent<Animals>();
        hitAnimals.TakeDamage(attack, currentAttackBonus, currentAttackType, hitBodyPart);
    }

    public float GetAttack(){
        return attack;
    }
    public float GetDefense(){
        return defense;
    }
    public float GetAgility(){
        return agility;
    }

    public void ReduceHitpoint(float damage){
        hitPoint -= damage;
        if(hitPoint <= 0){
            Destroy(this);
        }
    }

    public void TakeDamage(float attack, float attackBonus, AttackType attackType, PartPrefabsData hitBodyPart){
        float resistance = 0;
        if (hitBodyPart.GetAttackTypeResistace() == attackType){
            resistance = hitBodyPart.GetResistanceValue();
        }
        ReduceHitpoint((attack + attackBonus) - (defense + resistance));
    }
}
