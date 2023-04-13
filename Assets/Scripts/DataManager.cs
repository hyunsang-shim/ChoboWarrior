using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public struct ItemData
    {
        public string itemType; 
        public string itemName;
        public int itemImage;
        public int price;
        public int currentDur;
        public int maxDur;
        public int battleSuccessProb;
        public int TrainingSuccessProb;
    }

    public struct PlayerData
    {
        public string playerName;
        public int currentWeaponidx;
        public int currentArmoridx;
        public int currentShieldidx;
        public int currentWeaponDur;
        public int currentArmorDur;
        public int currentShieldDur;
        public int currentTrainingPoints;
        public int currentGold;
        public ItemData currentArmor;
        public ItemData currentShield;
        public ItemData currentWeapon;
    }


    
    public PlayerData playerData;

    public Sprite[] weaponSprites;
    public Sprite[] armorSprites;
    public Sprite[] shieldSprites;

    public List<ItemData> Weapons = new List<ItemData>();
    public List<ItemData> Armors = new List<ItemData>();
    public List<ItemData> Shields = new List<ItemData>();

#if UNITY_EDITOR

    public int Debug_currentGold;
    public bool Debug_OverrideCurrentGold;

#endif

    private static DataManager sInstance;
    public static DataManager Instance
    {
        get
        {
            if (sInstance == null)
                sInstance = new GameObject("DataManager").AddComponent<DataManager>();

            return sInstance;
        }
    }


    private void Awake()
    {
        InitSpriteData();
        InitItemData();
    }


    public Sprite[] GetWeaponSprites()
    {
        if(weaponSprites.Length > 0)
            return weaponSprites;
        else
        {
            InitSpriteData();
            return weaponSprites;
        }    
    }

    public void InitSpriteData()
    {
        weaponSprites = Resources.LoadAll<Sprite>("Icons/WeaponIcons");
        armorSprites = Resources.LoadAll<Sprite>("Icons/ArmorIcons");
        shieldSprites = Resources.LoadAll<Sprite>("Icons/ShieldIcons");

      //  Debug.Log($"Sprite Data Initialize Successful. {weaponSprites.Length} weapon sprite(s), " +
      //      $"{armorSprites.Length} armor sprite(s) and {shieldSprites.Length} " +
      //      $"shield sprite(s) loaded.");
    }

    public void InitItemData()
    {
        List<ItemData> itemDB = ReadDB();

        for (int i = 0; i < itemDB.Count; i++)
        {
            if (itemDB[i].itemType.Equals("Weapon"))
                Weapons.Add(itemDB[i]);
            else if (itemDB[i].itemType.Equals("Armor"))
                Armors.Add(itemDB[i]);
            else if (itemDB[i].itemType.Equals("Shield"))
                Shields.Add(itemDB[i]);


/*
            Debug.Log($"Entry #{i} => ItemType: {itemDB[i].itemType} " +
               $"ItemName: {itemDB[i].itemName} " +
               $"ItemImage: {itemDB[i].itemImage} " +
               $"ItemPrice: {itemDB[i].price} " +
               $"Durablity: {itemDB[i].maxDur} " +
               $"BattleSuccessProb: {itemDB[i].battleSuccessProb} " +
               $"TrainingSuccessProb: {itemDB[i].TrainingSuccessProb}");
*/

        }

        // Debug.Log($"itemDB Initialized Successfully : {itemDB.Count} items");        
    }

    public static List<ItemData> ReadDB(string _f = "DB/Items")
    {
        var list = new List<ItemData>();
        TextAsset data = Resources.Load(_f) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            ItemData entry = new ItemData();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                if (header[j].Equals("ItemType"))
                    entry.itemType = values[j];
                else if (header[j].Equals("ItemName"))
                    entry.itemName = values[j];
                else if (header[j].Equals("ItemImage"))
                    int.TryParse(values[j], out entry.itemImage);
                else if (header[j].Equals("ItemPrice"))
                    int.TryParse(values[j], out entry.price);
                else if (header[j].Equals("Durablity"))
                    int.TryParse(values[j], out entry.maxDur);
                else if (header[j].Equals("BattleSuccessProb"))
                    int.TryParse(values[j], out entry.battleSuccessProb);
                else if (header[j].Equals("TrainingSuccessProb"))
                    int.TryParse(values[j], out entry.TrainingSuccessProb);
            }

            /*var entry = new Dictionary<string, ItemData>();
            for(var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                int n;
                float f;

                if(header[j].Equals("ItemType") || header[j].Equals("ItemName"))
                {
                    finalValue = value;
                } else
                {
                    int.TryParse(value, out n);
                    finalValue = n;
                }
                entry[header[j]] = finalValue;
            }*/
            list.Add(entry);
           /*
            Debug.Log($"DB entry#{list.Count}: ItemType: {entry.itemType} " +
                $"ItemName: {entry.itemName} " +
                $"ItemPrice: {entry.itemImage} " +
                $"ItemPrice: {entry.price} " +
                $"Durablity: {entry.maxDur} " +
                $"BattleSuccessProb: {entry.battleSuccessProb} " +
                $"TrainingSuccessProb: {entry.TrainingSuccessProb} ");
           */
        }
        return list;
    }

    public void SetPlayerData(PlayerData _p)
    {

        playerData = _p;


        //DEbug Purpose only;
#if UNITY_EDITOR
        Debug_currentGold = _p.currentGold;
#endif
    }

    public void SetPlayerData()
    {
        playerData = GameManager.Instance.player;

        //DEbug Purpose only;
#if UNITY_EDITOR
        Debug_currentGold = GameManager.Instance.player.currentGold;
#endif
    }

    public PlayerData ReadPlayerData()
    {
        PlayerData _p = new PlayerData();
        _p.playerName = PlayerPrefs.GetString("PlayerName", "½ÉÁöÈ¯");
        _p.currentWeaponidx = PlayerPrefs.GetInt("WeaponIdx", 0);
        _p.currentArmoridx = PlayerPrefs.GetInt("ArmorIdx", 0);
        _p.currentShieldidx = PlayerPrefs.GetInt("ShieldIdx", 0);
        _p.currentWeaponDur = PlayerPrefs.GetInt("CurrentWeaponDur", 1);
        _p.currentArmorDur = PlayerPrefs.GetInt("CurrentArmorDur", 1);
        _p.currentShieldDur = PlayerPrefs.GetInt("CurrentShieldDur", 1);
        _p.currentTrainingPoints = PlayerPrefs.GetInt("CurrentTrainingPoints", 0);
        _p.currentGold = PlayerPrefs.GetInt("CurrentGold", 50);
        _p.currentGold = 120;
        return _p;
    }

    public void SavePlayerData()
    {
        playerData = GameManager.Instance.player;
        PlayerPrefs.SetString("PlayerName", playerData.playerName);
        PlayerPrefs.SetInt("WeaponIdx", playerData.currentWeaponidx);
        PlayerPrefs.SetInt("ArmorIdx", playerData.currentArmoridx);
        PlayerPrefs.SetInt("ShieldIdx", playerData.currentShieldidx);
        PlayerPrefs.SetInt("CurrentWeaponDur", playerData.currentWeaponDur);
        PlayerPrefs.SetInt("CurrentArmorDur", playerData.currentArmorDur);
        PlayerPrefs.SetInt("CurrentShieldDur", playerData.currentShieldDur);
        PlayerPrefs.SetInt("CurrentTrainingPoints", playerData.currentTrainingPoints);
#if UNITY_EDITOR
        if (Debug_OverrideCurrentGold)
            playerData.currentGold = Debug_currentGold;        
#endif
        PlayerPrefs.SetInt("CurrentGold", playerData.currentGold);

    }


    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public Sprite GetItemImage(string _type, int _idx)
    {
        // Debug.Log($"Item Image Requested => Type={_type}, Index={_idx}");

        if(Weapons.Count == 0 || Armors.Count == 0 || Shields.Count == 0)
            InitItemData();

        if (_type.Equals("Weapon"))
            return weaponSprites[_idx];
        else if (_type.Equals("Armor"))
            return armorSprites[_idx];
        else if (_type.Equals("Shield"))
            return shieldSprites[_idx];
        else
            return weaponSprites[0];
    }

    public int GetItemMaxDurability(string _type, int _idx)
    {
        if (Weapons.Count == 0 || Armors.Count == 0 || Shields.Count == 0)
            InitItemData();

        if (_type.Equals("Weapon"))
            return Weapons[_idx].maxDur;
        else if (_type.Equals("Armor"))
            return Armors[_idx].maxDur;
        else if (_type.Equals("Shield"))
            return Shields[_idx].maxDur;
        else
            return 1;
    }

    public ItemData GetItemData(string _type, int _index)
    {
        ItemData r = new ItemData();

        if (_type.Equals("Weapon"))
            r = Weapons[_index];
        else if (_type.Equals("Armor"))
            r = Armors[_index];
        else if (_type.Equals("Shield"))
            r = Shields[_index];

        return r;
    }
}                        