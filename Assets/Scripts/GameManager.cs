using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Main UI 요소들")]
    public Image weaponImage, shieldImage, armorImage;
    public Text weaponName, shieldName, armorName, textMyPoints;
    public GameObject MainScreen;
    public Sprite[] ItemSprites;
    public Sprite[] weaponSprites, shieldSprites, armorSprites;
    public GameObject[] trainingStackIcons;
    public int itemCount = 0;
    public int weaponDur_Max, weaponDur_Cur;
    public int armorDur_Max, armorDur_Cur;
    public int shieldDur_Max, shieldDur_Cur;
    public Slider weaponDurSlider, armorDurSlider, shieldDurSlider;

    [Header("저장 데이터")]
    public int n_myPoint;
    public int trainingStack; 
    public int weaponIdx, shieldIdx, armorIdx;

    [Header("데이터베이스")]   
    string[] weaponNames = { "없음", "싸구려 검", "좋은 검", "최강의 검" };
    string[] shieldNames = { "없음", "싸구려 방패", "좋은 방패", "최강의 방패" };
    string[] armorNames = { "없음", "싸구려 갑옷", "좋은 갑옷", "최강의 갑옷" };
    int[] gearDurablities = { 0, 5, 8, 15 };
    
    [Header("설정값들")]
    public int battleTimerMax;
    public int trainingTimerMax;
    public int baseTrainingSuccessRate;
    public int trainingSuccessRate;
    public int baseBattleSuccessRate;
    public int battleSuccessRate;


    [Header("밸런스 설정값")]
    public int battleSuccessRatePerGear = 350;
    public int trainingSuccessRatePerGear = 500;
    public int battleSuccessRatePerTrainingStack = 500;
    public int battleReward = 100;
    public int trainingReward = 10;

    [Header("UI팝업창들")]
    public GameObject popupTraining;
    public GameObject popupBattle;
    public GameObject popupShop;
    public GameObject trainingSuccess, trainingFail, trainingGreatSuccess;
    public GameObject battleSuccess, battleFail, battleGreatSuccess;

    [Header("기타 연관 요소들과 매니저")]
    public SoundManager SM;

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
        sInstance = GetComponent<GameManager>();
        InitData();
    }


    void InitData()
    {   
        ItemSprites.Initialize();

        /// 전체 아이템 스프라이트를 각각의 장비 스프라이트로 나눠서 넣는다.
        ItemSprites = Resources.LoadAll<Sprite>("Icons/choboWarrior_Icons");

        itemCount = ItemSprites.Length;


        weaponSprites = new Sprite[4];
        shieldSprites = new Sprite[4];
        armorSprites = new Sprite[4];

        // 무기 스프라이트 나눠넣기
        for (int i = 0; i < (itemCount/4); i++)
        {
            weaponSprites[i] = ItemSprites[i];
        }

        // 방패 스프라이트 나눠넣기
        for(int i = 0; i < (itemCount/4); i++)
        {
            shieldSprites[i] = ItemSprites[i + 4];
        }

        // 갑옷 스프라이트 나눠넣기
        for (int i = 0; i < (itemCount / 4); i++)
        {
            armorSprites[i] = ItemSprites[i + 8];
        }


        /// 저장된 데이터에서 장비 정보를 가져온다.
        weaponIdx = PlayerPrefs.GetInt("Weapon", 0);
        shieldIdx = PlayerPrefs.GetInt("Shield", 0);
        armorIdx = PlayerPrefs.GetInt("Armor", 0);


        /// 데이터가 잘못되었을 때의 에러 처리. 0번 아이템으로 돌려버린다.
        /// 
        if (weaponIdx > 4)
            weaponIdx = 0;

        if (shieldIdx > 4)
            shieldIdx = 0;

        if (armorIdx > 4)
            armorIdx = 0;
        /// ============================================================


        weaponImage.sprite = weaponSprites[weaponIdx];
        shieldImage.sprite = shieldSprites[shieldIdx];
        armorImage.sprite = armorSprites[armorIdx];

        weaponName.text = weaponNames[weaponIdx];
        shieldName.text = shieldNames[shieldIdx];
        armorName.text = armorNames[armorIdx];


        /// 저장된 데이터에서 포인트 정보와 누적된 훈련 스택을 가져온다.
        n_myPoint = PlayerPrefs.GetInt("Point", 100);
        textMyPoints.text = string.Format("{0:N0}", n_myPoint) + " pt";

        trainingStack = PlayerPrefs.GetInt("TrainingStack", 1);

        weaponDur_Cur = PlayerPrefs.GetInt("WeaponDur_Cur", 1);
        armorDur_Cur = PlayerPrefs.GetInt("ArmorDur_Cur", 1);
        shieldDur_Cur = PlayerPrefs.GetInt("ShieldDur_Cur", 1);

        weaponDur_Max = PlayerPrefs.GetInt("WeaponDur_Max", 1);
        armorDur_Max= PlayerPrefs.GetInt("ArmorDur_Max", 1);
        shieldDur_Max = PlayerPrefs.GetInt("ShieldDur_Max", 1);

        weaponDurSlider.maxValue = weaponDur_Max;
        shieldDurSlider.maxValue = weaponDur_Max;
        armorDurSlider.maxValue = weaponDur_Max;

        weaponDurSlider.value = weaponDur_Cur;
        shieldDurSlider.value = shieldDur_Cur;
        armorDurSlider.value = armorDur_Cur;


    }
    public float GetBattleSuccessRate()
    {
        return battleSuccessRate;
    }

    public float GetTrainingSuccessRate()
    {
        return trainingSuccessRate;
    }

    public float GetBaseBattleSuccessRate()
    {
        return baseBattleSuccessRate;
    }

    public float GetBaseTrainingSuccessRate()
    {
        return baseTrainingSuccessRate;
    }


    public int GetTrainingStack()
    {
        return trainingStack;
    }
    public void AddPoint(int _n)
    {
        n_myPoint += _n;

        UpdatePointText();
    }

    void UpdatePointText()
    {
        textMyPoints.text = string.Format("{0:N0}", n_myPoint) + " pt";
    }

    public void ShowTrainingPopup()
    {
        SaveData();
        GameObject k = Instantiate(popupTraining);
        k.transform.SetParent(MainScreen.transform);

        SM.PlaySFX("Click");
    }

    public void ShowBattlePopup()
    {
        SaveData();
        GameObject k = Instantiate(popupBattle);
        k.transform.SetParent(MainScreen.transform);

        SM.PlaySFX("Click");

    }

    public void ShowTrainingFail()
    {
        GameObject k = Instantiate(trainingFail);
        k.transform.SetParent(MainScreen.transform); 
        
        RectTransform rt = k.GetComponent<RectTransform>();
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;       // right, top
        rt.localScale = Vector3.one;

        k.GetComponentInChildren<Button>().onClick.AddListener(()=>ClosePopUpButton(k));

        SM.PlaySFX("Fail");
    }

    public void ShowTrainingSuccess()
    {
        GameObject k = Instantiate(trainingSuccess);
        k.transform.SetParent(MainScreen.transform);

        RectTransform rt = k.GetComponent<RectTransform>();
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;       // right, top
        rt.localScale = Vector3.one;

        k.GetComponentInChildren<Button>().onClick.AddListener(() => ClosePopUpButton(k));
        
        trainingStack++;
        if (trainingStack >= 5)
            trainingStack = 5;

        SM.PlaySFX("Success");
        AddPoint(trainingReward);
    }

    public void ShowTrainingGreatSuccess()
    {
        GameObject k = Instantiate(trainingGreatSuccess);
        k.transform.SetParent(MainScreen.transform);

        RectTransform rt = k.GetComponent<RectTransform>();
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;       // right, top
        rt.localScale = Vector3.one;

        k.GetComponentInChildren<Button>().onClick.AddListener(() => ClosePopUpButton(k));

        trainingStack++;
        if (trainingStack >= 5)
            trainingStack = 4;

        SM.PlaySFX("GreatSuccess");
        AddPoint(trainingReward*2);
    }

    public void ShowBattleFail()
    {
        GameObject k = Instantiate(battleFail);
        k.transform.SetParent(MainScreen.transform);

        RectTransform rt = k.GetComponent<RectTransform>();
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;       // right, top
        rt.localScale = Vector3.one;

        k.GetComponentInChildren<Button>().onClick.AddListener(() => ClosePopUpButton(k));
        trainingStack = 0;
        SM.PlaySFX("Fail");

        int p = Random.Range(0, 10000);
        if(p > 3500)
        { 
            switch(p%3)
            {
                case 0:
                    weaponDur_Cur--;
                    if (weaponDur_Cur < 1)
                    {
                        weaponName.text = "없음";
                        weaponImage.sprite = null;
                        weaponDur_Cur = 0;
                        weaponDur_Max = 1;
                        weaponIdx = 0;
                    }
                    break;
                case 1:
                    shieldDur_Cur--;
                    if (shieldDur_Cur < 1)
                    {
                        shieldName.text = "없음";
                        shieldImage.sprite = null;
                        shieldDur_Cur = 0;
                        shieldDur_Max = 1;
                        shieldIdx = 0;
                    }
                    break;
                case 2:
                    armorDur_Cur--;
                    if(armorDur_Cur < 1)
                    {
                        armorName.text = "없음";
                        armorImage.sprite = null;
                        armorDur_Cur = 0;
                        armorDur_Max = 1;
                        armorIdx = 0;
                    }
                    break;
            }
                }
    }

    public void ShowBattleSuccess()
    {
        GameObject k = Instantiate(battleSuccess);
        k.transform.SetParent(MainScreen.transform);

        RectTransform rt = k.GetComponent<RectTransform>();
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;       // right, top
        rt.localScale = Vector3.one;

        k.GetComponentInChildren<Button>().onClick.AddListener(() => ClosePopUpButton(k));

        trainingStack = 0;
        SM.PlaySFX("Success");
        AddPoint(battleReward);
    }

    public void ShowBattleGreatSuccess()
    {
        GameObject k = Instantiate(battleGreatSuccess);
        k.transform.SetParent(MainScreen.transform);

        RectTransform rt = k.GetComponent<RectTransform>();
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;       // right, top
        rt.localScale = Vector3.one;

        k.GetComponentInChildren<Button>().onClick.AddListener(() => ClosePopUpButton(k));

        trainingStack = 0;
        SM.PlaySFX("GreatSuccess");
        AddPoint(battleReward*2);
    }

    public void ShowShop()
    {
        GameObject sh = Instantiate(popupShop);
        sh.transform.SetParent(MainScreen.transform);


        RectTransform rt = sh.GetComponent<RectTransform>();
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;       // right, top
        rt.localScale = Vector3.one;


        SM.PlaySFX("Shop");

    }

    public void ClosePopUpButton(GameObject _o)
    {
        SaveData();
        Destroy(_o);
    }
   
    public void AbortTraining(GameObject _o)
    {
        Destroy(_o);
    }
    
    public void AbortBattle(GameObject _o)
    {
        Destroy(_o);
    }

    public int GetBattleTimerMax()
    {
        return battleTimerMax;
    }
    public int GetTrainingTimerMax()
    {
        return trainingTimerMax;
    }


    public void UpdateRates()
    {
        // 수련 성공률 = 기본 성공률 + 장비 단계 * 5%
        // 1000 = 10%
        trainingSuccessRate = baseTrainingSuccessRate 
            + (weaponIdx + shieldIdx + armorIdx + 3) * trainingSuccessRatePerGear;
        



        // 전투 성공률 = 기본 성공률 25% + 수련 스택 당 5% + 장비 당 3.5%
        // 100 = 1%
        battleSuccessRate = baseBattleSuccessRate 
            + trainingStack * battleSuccessRatePerTrainingStack 
            + (weaponIdx + shieldIdx + armorIdx + 3) * battleSuccessRatePerGear;
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }


    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Point", n_myPoint);
        PlayerPrefs.SetInt("TrainingStack", trainingStack);
        PlayerPrefs.SetInt("Weapon", weaponIdx > 3 ? 0 : weaponIdx);
        PlayerPrefs.SetInt("WeaponDur_Cur", weaponDur_Cur);
        PlayerPrefs.SetInt("WeaponDur_Max", weaponDur_Max);
        PlayerPrefs.SetInt("Shield", shieldIdx > 3 ? 0 : shieldIdx);
        PlayerPrefs.SetInt("ShieldDur_Cur",shieldDur_Cur);
        PlayerPrefs.SetInt("ShieldDur_Max",shieldDur_Max);
        PlayerPrefs.SetInt("Armor", armorIdx > 3 ? 0 : armorIdx);
        PlayerPrefs.SetInt("ArmorDur_Cur", shieldDur_Cur);
        PlayerPrefs.SetInt("ArmorDur_Max", shieldDur_Max);

    }


    public int GetCurrentPoint()
    {
        return n_myPoint;
    }

    
    private void LateUpdate()
    {
        for (int i = 0; i < trainingStackIcons.Length; i++)
        {
            if (i < trainingStack)
                trainingStackIcons[i].GetComponent<Image>().color = new Color(trainingStackIcons[i].GetComponent<Image>().color.r, trainingStackIcons[i].GetComponent<Image>().color.g, trainingStackIcons[i].GetComponent<Image>().color.b, 1);
            else
                trainingStackIcons[i].GetComponent<Image>().color = new Color(trainingStackIcons[i].GetComponent<Image>().color.r, trainingStackIcons[i].GetComponent<Image>().color.g, trainingStackIcons[i].GetComponent<Image>().color.b, 0);
        }

        weaponImage.sprite = weaponSprites[weaponIdx];
        shieldImage.sprite = shieldSprites[shieldIdx];
        armorImage.sprite = armorSprites[armorIdx];

        weaponName.text = weaponNames[weaponIdx];
        shieldName.text = shieldNames[shieldIdx];
        armorName.text = armorNames[armorIdx];

        weaponDurSlider.value = weaponDur_Cur;
        weaponDurSlider.maxValue = weaponDur_Max;

        shieldDurSlider.value = shieldDur_Cur;
        shieldDurSlider.maxValue = shieldDur_Max;

        armorDurSlider.value = armorDur_Cur;
        armorDurSlider.maxValue = armorDur_Max;

        UpdatePointText();
        UpdateRates();
    }

    public bool CheckBuyItem(int _id, int _price)
    {
        Debug.Log($"CheckBuyItem: Args- _price == {_price}, _id == {_id}");

        if (_price <= n_myPoint)
        {
            if (_id < 3) // 무기
            {
                weaponIdx = _id;
                weaponImage.sprite = weaponSprites[_id];
                weaponName.text = weaponNames[_id];
            }
            else if (_id < 6) // 방패
            {
                shieldIdx = _id-3;
                shieldImage.sprite = shieldSprites[_id - 3];
                shieldName.text = shieldNames[_id - 3];
            }
            else // 갑옷
            {
                armorIdx = _id-6;
                armorImage.sprite = armorSprites[_id - 6];
                armorName.text = armorNames[_id - 6];
            }

            n_myPoint -= _price;

            Debug.Log($"Item Buy Check Successful!");

            SM.PlaySFX("BuySuccess");
            return true;
        }
        else
        {
            Debug.Log("Item Buy Check Failed! - Not enough Points: price {_price} / on hand {n_myPoint}");

            SM.PlaySFX("BuyFail");
            return false;
        }

    }


    public string GetItemName(int _i)
    {
        if (_i < 3)
            return weaponNames[_i];
        else if (_i < 6)
            return shieldNames[_i-3];
        else
            return armorNames[_i-6];
    }

    public Sprite GetItemSprite(int _i)
    {
        return ItemSprites[_i];
    }

    public int GetItemPrice(int _i)
    {
        return 10 * (_i % 3 == 0 ? 1 : (_i % 3 == 1 ? 10 : 100));
    }
    
    public int GetRewardPoint(bool _isBattle, bool _isGreatSuccess)
    {
        if (_isBattle)
        {
            if (_isGreatSuccess)
                return battleReward * 2;
            else
                return battleReward;

        }
        else
        {
            if (_isGreatSuccess)
                return trainingReward * 2;
            else
                return trainingReward;
        }
    }
}
