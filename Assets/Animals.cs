using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Animals : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField] private GameObject headPart;
    [SerializeField] private GameObject forelegPart;
    [SerializeField] private GameObject backlegPart;
    [SerializeField] private GameObject torsoPart;
    private PartPrefabsData headPartData;
    private PartPrefabsData forelegPartData;
    private PartPrefabsData backlegPartData;
    private PartPrefabsData torsoPartData;
    bool validBody = false;

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
        headPartData = headPart.GetComponent<PartPrefabsData>();
        forelegPartData = forelegPart.GetComponent<PartPrefabsData>();
        backlegPartData = backlegPart.GetComponent<PartPrefabsData>();
        torsoPartData = torsoPart.GetComponent<PartPrefabsData>();
    }

    private void Start() {
        CheckValidity();
        GatherStat();
    }

    void CheckValidity(){
        if(headPartData.GetBodyPartType() != BodyPartType.head) return;
        if(forelegPartData.GetBodyPartType() != BodyPartType.foreleg) return;
        if(backlegPartData.GetBodyPartType() != BodyPartType.backleg) return;
        if(torsoPartData.GetBodyPartType() != BodyPartType.torso) return;
        validBody = true;
    }
    void GatherStat(){
        if(validBody){
            GatherStatBonuses(headPartData);
            GatherStatBonuses(forelegPartData);
            GatherStatBonuses(backlegPartData);
            GatherStatBonuses(torsoPartData);
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
            forelegPartData.PlayAttack();
            currentAttackType = AttackType.slash;
            currentAttackBonus = slashBonus;
        }
    }

    public void PierceAttack(){
        if(validBody){
            forelegPartData.PlayAttack();
            currentAttackType = AttackType.pierce;
            currentAttackBonus = pierceBonus;
        }
    }

    public void BluntAttack(){
        if(validBody){
            forelegPartData.PlayAttack();
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
        if (hitAnimals.GetHitPoint()<=0)
        {
            WinGame();
            Destroy(other.gameObject);
        }
    }

    public float GetHitPoint(){
        return hitPoint;
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
    
    void WinGame(){
        if(this.gameObject != null) Debug.Log("You win");
    }

    public void ReduceHitpoint(float damage){
        hitPoint -= damage;
    }

    public void TakeDamage(float attack, float attackBonus, AttackType attackType, PartPrefabsData hitBodyPart){
        float resistance = 0;
        if (hitBodyPart.GetAttackTypeResistace() == attackType){
            resistance = hitBodyPart.GetResistanceValue();
        }
        ReduceHitpoint((attack + attackBonus) - (defense + resistance));
    }
}
