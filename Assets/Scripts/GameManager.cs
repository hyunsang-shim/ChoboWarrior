using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Main UI ��ҵ�")]
    public Image weaponImage, shieldImage, armorImage;
    public Text weaponName, shieldName, armorName, myPoint;
    public GameObject MainScreen;
    public Sprite[] ItemSprites;
    public GameObject[] trainingStackIcons;

    [Header("���� ������")]
    public int n_myPoint;
    public int trainingStack;

    [Header("�����ͺ��̽�")]
    public int weaponIdx, shieldIdx, armorIdx;
    string[] weaponNames = { "�α��� ��", "���� ��", "�ְ��� ��" };
    string[] shieldNames = { "�α��� ����", "���� ����", "�ְ��� ����" };
    string[] armorNames = { "�α��� ����", "���� ����", "�ְ��� ����" };

    [Header("��������")]
    public int battleTimerMax;
    public int trainingTimerMax;
    public float trainingSuccessRate;

    [Header("UI�˾�â��")]
    public GameObject popupTraining;
    public GameObject popupBattle;
    public GameObject popupShop;
    public GameObject trainingSuccess, trainingFail, trainingGreatSuccess;
    public GameObject battleSuccess, battleFail, battleGreatSuccess;    


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

        ItemSprites.Initialize();
        ItemSprites = Resources.LoadAll<Sprite>("Icons/choboWarrior_Icons");


        InitData();

    }


    void InitData()
    {
        weaponIdx = PlayerPrefs.GetInt("Weapon", 0);
        shieldIdx = PlayerPrefs.GetInt("Shield", 0);
        armorIdx = PlayerPrefs.GetInt("Armor", 0);

        weaponImage.sprite = ItemSprites[weaponIdx];
        shieldImage.sprite = ItemSprites[shieldIdx + 3];
        armorImage.sprite = ItemSprites[armorIdx + 6];

        weaponName.text = weaponNames[weaponIdx];
        shieldName.text = shieldNames[shieldIdx];
        armorName.text = armorNames[armorIdx];

        n_myPoint = PlayerPrefs.GetInt("Point", 0);
        myPoint.text = string.Format("{0:N0}", n_myPoint) + " pt";

        trainingStack = PlayerPrefs.GetInt("TrainingStack", 0);

    }
    public float GetBattleSuccesssRate()
    {
        // ���� ����ġ�� ���� ���� ������ (�ִ� 5ȸ)
        // ���� 1ȸ�� 4% ����. (�ִ� 20%)
        // ���� ����ġ�� ���� �� ����.

        // ����� ���� ������
        // �ϱ޼� = 0 + 3 + 6 = 9
        // �߱޼� = 1 + 4 + 7 = 12
        // ��޼� = 2 + 5 + 8 = 15

        // ���� ������ ����
        // ���� ����ġ + ���� ������ ���� * ���� ��� = ���� ������ (MAX 80%)
        // �¸� �� ��¸��� 10% Ȯ��
        float winRate = 0f;
        winRate += trainingStack * 400;
        winRate += (weaponIdx) * 400;
        winRate += (shieldIdx + 3) * 400;
        winRate += (armorIdx + 6) * 400;

        Debug.Log($"GetBattleSuccesssRate: {winRate}");
        return winRate;
    }

    public float GetTrainingSuccessRate()
    {
        return trainingSuccessRate;
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
        myPoint.text = string.Format("{0:N0}", n_myPoint) + " pt";
    }

    public void ShowTrainingPopup()
    {
        SaveData();
        GameObject k = Instantiate(popupTraining);
        k.transform.SetParent(MainScreen.transform);
    }

    public void ShowBattlePopup()
    {
        SaveData();
        GameObject k = Instantiate(popupBattle);
        k.transform.SetParent(MainScreen.transform);
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
            trainingStack = 4;

        AddPoint(10);
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

        AddPoint(20);
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
        AddPoint(10);
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
        AddPoint(20);
    }

    public void ShowShop()
    {
        GameObject sh = Instantiate(popupShop);
        sh.transform.SetParent(MainScreen.transform);


        RectTransform rt = sh.GetComponent<RectTransform>();
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;       // right, top
        rt.localScale = Vector3.one;

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
        PlayerPrefs.SetInt("Weapon", weaponIdx);
        PlayerPrefs.SetInt("Shield", shieldIdx);
        PlayerPrefs.SetInt("Armor", armorIdx);

    }


    public int GetCurrentPoint()
    {
        return n_myPoint;
    }

    
    private void LateUpdate()
    {
        for (int i = 0; i <= trainingStackIcons.Length-1; i++)
        {
            if (i < trainingStack)
                trainingStackIcons[i].GetComponent<Image>().color = new Color(trainingStackIcons[i].GetComponent<Image>().color.r, trainingStackIcons[i].GetComponent<Image>().color.g, trainingStackIcons[i].GetComponent<Image>().color.b, 1);
            else
                trainingStackIcons[i].GetComponent<Image>().color = new Color(trainingStackIcons[i].GetComponent<Image>().color.r, trainingStackIcons[i].GetComponent<Image>().color.g, trainingStackIcons[i].GetComponent<Image>().color.b, 0);
        }
    }

}
