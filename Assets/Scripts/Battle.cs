using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    Slider slider;
    Button btnAbort;
    public Text txtTraining;
    string defaultString = "РќХѕСп";
    public float progress = 0;
    float successRate = 0f;
    float baseSuccessRate = 0f;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        btnAbort = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        progress = 0;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        slider.maxValue = GameManager.Instance.GetBattleTimerMax();

        btnAbort.onClick.AddListener(() => GameManager.Instance.AbortBattle(gameObject));

        successRate = GameManager.Instance.GetBattleSuccesssRate();
        baseSuccessRate = GameManager.Instance.GetBaseBattleSuccesssRate();
    }

    private void FixedUpdate()
    {
        progress += Time.fixedDeltaTime;

        slider.value = progress;

        if (progress >= slider.maxValue)
        {
            int k = Random.Range(0, 10000);

            Debug.Log($"Battle Result:: {k} / {successRate}");

            if (k <= successRate)
            { 
                if (k < (baseSuccessRate/2))
                    GameManager.Instance.ShowBattleGreatSuccess();
                else
                    GameManager.Instance.ShowBattleSuccess();
            }
            else
            {
                GameManager.Instance.ShowBattleFail();
            }

            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        string tmp = defaultString;

        for (int i = 0; i < (progress % 4); i++)
        {
            if (i % 4 == 0) continue;

            tmp += ".";
        }

        txtTraining.text = tmp;
    }
}
