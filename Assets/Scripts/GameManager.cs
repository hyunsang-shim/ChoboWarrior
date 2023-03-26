using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class GameManager : MonoBehaviour
{
    public DataManager DM;
    public DataManager.PlayerData player;
    public UIPlayerGearDisplay[] uiGearUIs;



    private static GameManager sInstance;
    public static GameManager Instance
    {
        get
        {
            if (sInstance == null)
                sInstance = new GameObject("GameManager").AddComponent<GameManager>();

            return sInstance;
        }
    }

    private void Awake()
    {
        DM = DataManager.Instance;
    }

    private void Start()
    {
        player = DM.ReadPlayerData();

        
        player.currentArmoridx = 1;
        player.currentArmor = DM.Weapons[player.currentArmoridx];

        player.currentWeaponidx = 2;
        player.currentWeapon = DM.Weapons[player.currentWeaponidx];

        player.currentShieldidx = 3;
        player.currentShield= DM.Weapons[player.currentShieldidx];



        DM.SetPlayerData(player);

        foreach (UIPlayerGearDisplay o in uiGearUIs)
            o.Refresh();
    }



}