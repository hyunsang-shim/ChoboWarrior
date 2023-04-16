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

    public int trainingSuccessReward = 2;
    public int battleSuccessReward = 15;


    public int defaultTrainingSuccessRate = 1000;
    public int defaultBattleSuccessRate = 1000;

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
        Debug.Log($"current Goild at startup: {player.currentGold}");
        DM.SetPlayerData(player);

        UpdateUIs();
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


        Debug.Log($"UpdateCurrentGear");

        DM.SavePlayerData();
        Debug.Log($" DM.SavePlayerData();");

        UpdateUIs();
        Debug.Log($" UpdateUIs();");
    }

    void UpdateCurrentSuccessProbs()
    {
        trainingSuccessRate = defaultTrainingSuccessRate + player.currentArmor.TrainingSuccessProb + player.currentWeapon.TrainingSuccessProb + player.currentShield.TrainingSuccessProb;
        battleSuccessRate = defaultBattleSuccessRate + player.currentArmor.battleSuccessProb + player.currentWeapon.battleSuccessProb + player.currentShield.battleSuccessProb;
    }

    public int CheckSuccess(string _checkTarget)
    {
        int r1 = 0;
        int p = Random.Range(0, 100000);
        int reward = 0;

        if (_checkTarget.Equals("Training"))
        {
            Debug.Log($"p = {p} / {trainingSuccessRate}");
            if (p <= trainingSuccessRate)
            {
                r1 = 1;
                reward = trainingSuccessReward;
            }
        }
        else if (_checkTarget.Equals("Battle"))
        {
            if (p <= battleSuccessRate)
            {
                r1 = 1;
                reward = battleSuccessReward;
            }
        }

        // Debug always success
        r1 = 1;
        reward = trainingSuccessReward;
        // r1 += (Random.Range(0, 100000) <= 10000) ? 1 : 0;       // noremal code

        if (r1 >= 1)
        {
            player.currentTrainingPoints++;
            player.currentGold += reward;
            DM.SetPlayerData(player);
            Debug.Log($"DM.SetPlayerData(player) / currentTrainingPoints:{player.currentTrainingPoints} / currentReward:{reward}");

        }

        if (player.currentTrainingPoints >= 5)
            player.currentTrainingPoints = 4;


        Debug.Log($"Success? (1=yes/0=no) = {r1}");
        Debug.Log($"gold = {player.currentGold}");
        Debug.Log($"training buff = {player.currentTrainingPoints}");

        UpdateUIs();
        return r1;

    }

    public void StartTraining()
    {
        if (go_popupTraining == null)
        {
            Debug.Log("Training popup is null");
            go_popupTraining = Instantiate(Resources.Load("Prefabs/UI/popupTraining") as GameObject);
            go_popupTraining.transform.SetParent(GameObject.Find("UIRoot").transform);
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


    public void StartBattle()
    {
        if (go_popupBattle == null)
        {
            Debug.Log("Training popup is null");
            go_popupBattle = Instantiate(Resources.Load("Prefabs/UI/popupBattle") as GameObject);
            go_popupBattle.transform.SetParent(GameObject.Find("UIRoot").transform);
            go_popupBattle.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            go_popupBattle.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }
        else if (!go_popupBattle.activeSelf)
        {
            Debug.Log("Training popup is not null and not active");
            go_popupBattle.SetActive(true);
        }
        else
        { go_popupBattle.SetActive(true); }
    }


    public void DisableObject(GameObject _o)
    {
        _o.gameObject.SetActive(false);
    }

    public void UpdateUIs()
    {
        foreach (UIPlayerGearDisplay o in uiGearUIs)
        {
            o.Refresh();
            Debug.Log($"{o.name} refreshed");
        }
    }


}