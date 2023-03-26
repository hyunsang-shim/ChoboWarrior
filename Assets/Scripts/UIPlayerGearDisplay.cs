using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerGearDisplay : MonoBehaviour
{
    public enum GearType{
        Weapon = 0,
        Armor = 1,
        Shield = 2
    }

    public int gearIdx;
    public int currentDur;
    public int maxDur;
    public GearType gearType;
    public string type;
    DataManager.PlayerData _p;
    DataManager.ItemData _i;
    GameManager _g;
    public Image gearImage;
    public Text gearName;
    public Slider slider;


    private void Awake()
    {
        _g = GameManager.Instance;

        switch (gearType)
        {
            case GearType.Weapon:
                gearIdx = _p.currentWeaponidx;
                type = "Weapon";
                slider.maxValue = _p.currentWeapon.maxDur;
                slider.value = _p.currentWeaponDur;
                gearName.text = _p.currentWeapon.itemName;
                gearImage.sprite = DataManager.Instance.GetItemImage(type, gearIdx);
                break;
            case GearType.Armor:
                gearIdx = _p.currentArmoridx;
                type = "Armor";
                slider.maxValue = _p.currentArmor.maxDur;
                slider.value = _p.currentArmorDur;
                gearName.text = _p.currentArmor.itemName;
                gearImage.sprite = DataManager.Instance.GetItemImage(type, gearIdx);
                break;
            case GearType.Shield:
                gearIdx = _p.currentShieldidx;
                type = "Shield";
                slider.maxValue = _p.currentShield.maxDur;
                slider.value = _p.currentShieldDur;
                gearName.text = _p.currentShield.itemName;
                gearImage.sprite = DataManager.Instance.GetItemImage(type, gearIdx);
                break;
        }

    }
    private void Start()
    {
        // _p = GameManager.Instance.DM.GetPlayerData();
       

    }

    public void Refresh()
    {

        _p = DataManager.Instance.GetPlayerData();

        switch (gearType)
        {
            case GearType.Weapon:
                gearIdx = _p.currentWeaponidx;
                type = "Weapon";
                slider.maxValue = _p.currentWeapon.maxDur;
                slider.value = _p.currentWeaponDur;
                gearName.text = _p.currentWeapon.itemName;
                gearImage.sprite = DataManager.Instance.GetItemImage(type, gearIdx);
                break;
            case GearType.Armor:
                gearIdx = _p.currentArmoridx;
                type = "Armor";
                slider.maxValue = _p.currentArmor.maxDur;
                slider.value = _p.currentArmorDur;
                gearName.text = _p.currentArmor.itemName;
                gearImage.sprite = DataManager.Instance.GetItemImage(type, gearIdx);
                break;
            case GearType.Shield:
                gearIdx = _p.currentShieldidx;
                type = "Shield";
                slider.maxValue = _p.currentShield.maxDur;
                slider.value = _p.currentShieldDur;
                gearName.text = _p.currentShield.itemName;
                gearImage.sprite = DataManager.Instance.GetItemImage(type, gearIdx);
                break;
        }
    }

}
