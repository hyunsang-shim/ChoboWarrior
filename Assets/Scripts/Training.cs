using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Training : MonoBehaviour
{
    Slider slider;
    Button btnAbort;
    public Text txtTraining;
    string defaultString = "ผ๖ทรม฿";
    public float progress = 0;
    public float trainingSuccessRate = 0;
    
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
        slider.maxValue = GameManager.Instance.GetTrainingTimerMax();
        trainingSuccessRate= GameManager.Instance.GetTrainingSuccessRate();

        btnAbort.onClick.AddListener(() => GameManager.Instance.AbortTraining(gameObject));
    }

    private void FixedUpdate()
    {
        progress += Time.fixedDeltaTime;

        slider.value = progress;

        if (progress >= slider.maxValue)
        {
            int k = Random.Range(0, 10000);

            Debug.Log($"Training esult: {k} / {trainingSuccessRate}");
            if (k <= trainingSuccessRate)
            {
                if (Random.Range(0,10000) < 2000)
                    GameManager.Instance.ShowTrainingGreatSuccess();
                else
                    GameManager.Instance.ShowTrainingSuccess();
            }
            else
                GameManager.Instance.ShowTrainingFail();

            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {   
        string tmp = defaultString;

        for (int i=0; i < (progress % 4); i++)
        {
            if (i % 4 == 0) continue;

            tmp += ".";
        }

        txtTraining.text = tmp;
    }

}
