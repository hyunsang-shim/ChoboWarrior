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

    public GameObject go_popupTraining;
    public GameObject go_popupBattle;
    public GameObject go_popupResult;


    float trainingSuccessRate;
    float battleSuccessRate;


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
        sInstance = this;
        DM = DataManager.Instance;
    }

    private void Start()
    {
        player = DM.ReadPlayerData();
        UpdateCurrentGear(player.currentWeaponidx, player.currentArmoridx, player.currentShieldidx);       
        UpdateCurrentSuccessProbs();

        DM.SetPlayerData(player);

        foreach (UIPlayerGearDisplay o in uiGearUIs)
            o.Refresh();
    }

    public int GetCurrentGold()
    {
        return player.currentGold;
    }


    /// <summary>
    /// make sure to do UpdateCurrentSuccessProbs() after UpdateCurrentGear()
    /// </summary>
    /// <param name="_WeaponID"></param>
    /// <param name="_ArmorID"></param>
    /// <param name="_ShieldID"></param>
    void UpdateCurrentGear(int _WeaponID, int _ArmorID, int _ShieldID)
    {
        player.currentArmoridx = _ArmorID;
        player.currentWeaponidx = _WeaponID;
        player.currentShieldidx = _ShieldID;

        player.currentArmor = DM.GetItemData("Armor", _ArmorID);
        player.currentWeapon = DM.GetItemData("Weapon", _WeaponID);
        player.currentShield = DM.GetItemData("Shield", _ShieldID);

        DM.SavePlayerData();

        foreach (UIPlayerGearDisplay o in uiGearUIs)
        {
            Debug.Log($"o = {o.name}");
            o.Refresh();
        }
    }

    void UpdateCurrentSuccessProbs()
    {
        trainingSuccessRate = player.currentArmor.TrainingSuccessProb + player.currentWeapon.TrainingSuccessProb + player.currentShield.TrainingSuccessProb;
        battleSuccessRate = player.currentArmor.battleSuccessProb + player.currentWeapon.battleSuccessProb + player.currentShield.battleSuccessProb;
    }

    public int CheckSuccess(string _checkTarget)
    {
        int r1 = 0;
        int p = Random.Range(0, 100000);

        if (_checkTarget.Equals("Training"))
        {
            Debug.Log($"p = {p}, / {trainingSuccessRate}");
            if (p <= trainingSuccessRate)
                r1 = 1;                
        }
        else if (_checkTarget.Equals("Battle"))
        {
            if (p <= battleSuccessRate)
                r1 = 1;
        }

        // Debug always success
        r1 = 1;
        
        // r1 += (Random.Range(0, 100000) <= 10000) ? 1 : 0;       // noremal code

        if (r1 >= 1)
            player.currentTrainingPoints++;

        if (player.currentTrainingPoints > 5)
            player.currentTrainingPoints = 5;


        Debug.Log($"r1 = {r1}");

        return r1;

    }

    public void StartTraining()
    {
        if (go_popupTraining == null)
        {
            Debug.Log("Training popup is null");
            go_popupTraining = Instantiate(Resources.Load("Prefabs/UI/popupTraining") as GameObject);
            go_popupTraining.transform.parent = GameObject.Find("UIRoot").transform;
            go_popupTraining.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            go_popupTraining.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }
        else if (!go_popupTraining.activeSelf)
        {
            Debug.Log("Training popup is not null and not active");
            go_popupTraining.SetActive(true);
        }
        else
        { go_popupTraining.SetActive(true); }
    }

    public void DisableObject(GameObject _o)
    {
        _o.gameObject.SetActive(false);
    }



}