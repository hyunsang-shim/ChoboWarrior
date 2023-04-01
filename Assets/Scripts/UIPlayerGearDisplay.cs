using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerGearDisplay : MonoBehaviour
{
    public enum GearType{
        Weapon = 0,
        Armor = 1,
        Shield = 2,
        Point = 100,
        TrainingPoints = 200

    }

    public int gearIdx;
    public int currentDur;
    public int maxDur;
    public GearType UIType;
    public string type;
    DataManager.PlayerData _p;
    DataManager.ItemData _i;
    GameManager _g;
    public Image gearImage;
    public Text gearName;
    public Slider slider;


    private void Awake()
    {

        

    }
    private void Start()
    {
        _g = GameManager.Instance;

    }

    public void Refresh()
    {

        _p = DataManager.Instance.GetPlayerData();

        switch (UIType)
        {
            case GearType.Weapon:
                gearIdx = _p.currentWeaponidx;
                type = "Weapon";
                slider.maxValue = gearIdx == 0 ? 1 : _p.currentWeapon.maxDur;
                slider.value = gearIdx == 0 ? 1 : _p.currentWeaponDur;
                gearName.text = _p.currentWeapon.itemName;
                gearImage.sprite = DataManager.Instance.GetItemImage(type, gearIdx);
                break;
            case GearType.Armor:
                gearIdx = _p.currentArmoridx;
                type = "Armor";
                slider.maxValue = gearIdx == 0 ? 1 : _p.currentArmor.maxDur;
                slider.value = gearIdx == 0 ? 1 : _p.currentArmorDur;
                gearName.text = _p.currentArmor.itemName;
                gearImage.sprite = DataManager.Instance.GetItemImage(type, gearIdx);
                break;
            case GearType.Shield:
                gearIdx = _p.currentShieldidx;
                type = "Shield";
                slider.maxValue = gearIdx == 0 ? 1 : _p.currentShield.maxDur;
                slider.value = gearIdx == 0 ? 1 : _p.currentShieldDur;
                gearName.text = _p.currentShield.itemName;
                gearImage.sprite = DataManager.Instance.GetItemImage(type, gearIdx);
                break;
            case GearType.Point:
                gearName.text = _p.currentGold.ToString() + " pt";
                break;
        }

        DataManager.Instance.SetPlayerData(_p);
    }

}
