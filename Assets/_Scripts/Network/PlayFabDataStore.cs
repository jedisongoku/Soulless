using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabDataStore : MonoBehaviour
{
    public static string sessionTicket;
    public static string playFabId;
    public static string characterId;
    public static string characterName;
    public static string friendUsername;
    public static string friendCharacterId;
    public static string currentRoomName;
    public static string friendsCurrentRoomName;
    public static int playerCurrency;
    public static int playerUnupdatedCurrency = 0;
    public static int playerInventorySlotCount = 40;

    //Dictionaries

    public static Dictionary<string, string> friendsList = new Dictionary<string, string>();
    public static Dictionary<string, CatalogRune> catalogRunes = new Dictionary<string, CatalogRune>();
    public static Dictionary<string, PlayerRune> playerAllRunes = new Dictionary<string, PlayerRune>();
    public static Dictionary<string, Sprite> catalogRuneImages = new Dictionary<string, Sprite>();
    public static Dictionary<int, Sprite> playerActiveRuneImages = new Dictionary<int, Sprite>();
    public static Dictionary<string, List<PlayerItemInfo>> playerInventoryInfo = new Dictionary<string, List<PlayerItemInfo>>();
    public static Dictionary<string, string> characters = new Dictionary<string, string>();
    public static Dictionary<int, string> playerActiveSkillRunes = new Dictionary<int, string>();
    public static Dictionary<string, int> playerActiveModifierRunes = new Dictionary<string, int>();
    public static Dictionary<string, PlayerItemInfo> playerEquippedItems = new Dictionary<string, PlayerItemInfo>();
    
    public static Dictionary<string, CatalogQuest> catalogQuests = new Dictionary<string, CatalogQuest>();
    public static Dictionary<string, UIItemInfo> catalogItems = new Dictionary<string, UIItemInfo>();

    public static List<string> playerCompletedQuests = new List<string>();
    public static List<string> playerQuestLog = new List<string>();
    public static List<string> playerInventory = new List<string>();
    public static Dictionary<string, int> statsBuilder = new Dictionary<string, int>()
    {
        {"Vitality", 0 },
        {"Strength", 0 },
        {"Intellect", 0 },
        {"Dexterity", 0 },
        {"Spirit", 0 },
        {"Critical Chance", 0 },
        {"Nature Resistance", 0 },
        {"Fire Resistance", 0 },
        {"Frost Resistance", 0 },
        {"Holy Resistance", 0 },
        {"Arcane Resistance", 0 }
    };


    //Player
    public const int playerBaseHealth = 40;
    public const int playerBaseStrength = 8;
    public const int playerBaseIntellect = 8;
    public const int playerBaseDexterity = 8;
    public const int playerBaseVitality = 9;
    public const int playerBaseSpirit = 5;
    public const int playerBaseWeaponDamage = 1;
    public const float playerBaseCriticalChance = 5;
    public const int playerBaseArmor = 200;

    public static int playerLevel = 1;
    public static int playerExperience = 0;
    public static int maxExperienceToLevel;
    
    public static int playerCurrentHealth;
    public static int playerMaxHealth = 40;
    public static int playerCurrentResource;
    public static int playerMaxResource = 100;
    public static int playerStrength;
    public static int playerIntellect;
    public static int playerDexterity;
    public static int playerVitality;
    public static int playerSpirit;
    public static float playerCriticalChance;
    public static int playerArmor;
    
    public static int playerWeaponDamage;
    public static int playerPhysicalDamage;
    public static int playerSpellDamage;
    public static int playerAttackPower;

    public static int playerStatBuilderVitality;
    public static int playerStatBuilderStrength;
    public static int playerStatBuilderIntellect;
    public static int playerStatBuilderDexterity;
    public static int playerStatBuilderSpirit;
    public static int playerStatBuilderCriticalChance;
    public static int playerStatBuilderNatureResistance;
    public static int playerStatBuilderFireResistance;
    public static int playerStatBuilderFrostResistance;
    public static int playerStatBuilderHolyResistance;

    public static int playerStatBuilderUsedPoint;
    public static int playerStatBuilderMaxPoint;


    public static Dictionary<string, string> playerInitialData = new Dictionary<string, string>()
    {
        {"Level", "1" },
        {"Experience", "0" },
        {"StatBuilderVitality", "0" },
        {"StatBuilderStrength", "0" },
        {"StatBuilderIntellect", "0" },
        {"StatBuilderDexterity", "0" },
        {"StatBuilderSpirit", "0" },
        {"StatBuilderCriticalChance", "0" },
        {"StatBuilderNatureResistance", "0" },
        {"StatBuilderFireResistance", "0" },
        {"StatBuilderFrostResistance", "0" },
        {"StatBuilderHolyResistance", "0" },
        {"StatBuilderArcaneResistance", "0" }
    };

    public static int gameMaxLevel = 40;

    
    
    
    
    
    

}