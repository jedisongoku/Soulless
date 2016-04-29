using UnityEngine;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class CatalogRune
{
    public static CatalogRune rune;

    public string itemId;
    public string itemClass;
    public string displayName;
    public string description;
    public string skillSlot;
    public int resourceGeneration;
    public int resourceUsage;
    public int attackRange;
    public int attackRadius;
    public int attackPercentage;
    public int increasedDamage;
    public int increasedCrit;
    public int increasedSpeed;
    public float effectTime;
    public float cooldown;
    public string damageType;

    void Awake()
    {
        rune = this;
    }

    public CatalogRune(string _itemId, string _itemClass, string _displayName, string _description, string _skillSlot, int _resourceGeneration, int _resourceUsage, int _attackRange,
        int _attackRadius, int _attackPercentage, int _increasedDamage, int _increasedCrit, int _increasedSpeed, float _effecTime, float _cooldown, string _damageType)
    {
        itemId = _itemId;
        itemClass = _itemClass;
        displayName = _displayName;
        description = _description;
        skillSlot = _skillSlot;
        resourceGeneration = _resourceGeneration;
        resourceUsage = _resourceUsage;
        attackRange = _attackRange;
        attackRadius = _attackRadius;
        attackPercentage = _attackPercentage;
        increasedDamage = _increasedDamage;
        increasedCrit = _increasedCrit;
        increasedSpeed = _increasedSpeed;
        effectTime = _effecTime;
        cooldown = _cooldown;
        damageType = _damageType;
    }
}
