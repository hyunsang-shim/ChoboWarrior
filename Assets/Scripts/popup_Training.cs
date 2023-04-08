using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popup_Training : MonoBehaviour
{
    public Slider Progress;
    public Button btnAbort;
    public GameObject Result_Success;
    public GameObject Result_GreatSuccess;
    public GameObject Result_Fail;
    public GameObject ResultPopup_fail;
    public GameObject ResultPopup_success;
    public GameObject ResultPopup_great;
    public GameObject UIRoot;

    public float TrainingTime;

    bool aborted = false;

    private void Awake()
    {
        UIRoot = GameObject.Find("UIRoot");

    }

    private void Start()
    {
        Progress.maxValue = TrainingTime;
        Progress.value = 0f;


        GetComponent<RectTransform>().offsetMax = Vector2.zero;
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
    }

    private void LateUpdate()
    {
        Progress.value += Time.fixedDeltaTime;
        if (aborted == false)
        {
            if (Progress.value >= TrainingTime)
            {
                switch (GameManager.Instance.CheckSuccess("Training"))
                {
                    case 0:
                        CallFailPopup();
                        break;
                    case 1:
                        CallSuccessPopup();
                        break;
                    case 2:
                        CallGreatSuccessPopup();
                        break;
                    default:
                        CallFailPopup();
                        break;
                }


                Progress.maxValue = TrainingTime;
                Progress.value = 0f;
            }
        }
    }

    

    void CallFailPopup()    {       
        Debug.Log("Training Fail");

        if (ResultPopup_fail == null)
            ResultPopup_fail = Instantiate(Result_Fail);
        else if (!ResultPopup_fail.activeSelf)
            ResultPopup_fail.SetActive(true);
        
        ResultPopup_fail.GetComponentInChildren<Button>().onClick.AddListener(() => GameManager.Instance.DisableObject(ResultPopup_fail));
        ResetPopupTransformAndSize(ResultPopup_fail);

        if (Random.Range(0, 100000) <= 50000)
            Debug.Log("Durability Dropped");

        gameObject.SetActive(false);
    }

    void CallSuccessPopup() {       
        Debug.Log("Training Success");
        if (ResultPopup_success == null)
            ResultPopup_success = Instantiate(Result_Success);
        else if (!ResultPopup_success.activeSelf)
            ResultPopup_success.SetActive(true); 

        ResultPopup_success.GetComponentInChildren<Button>().onClick.AddListener(() => GameManager.Instance.DisableObject(ResultPopup_success));
        ResetPopupTransformAndSize(ResultPopup_success);

        if (Random.Range(0, 100000) <= 25000)
            Debug.Log("Durability Not Dropped");

        gameObject.SetActive(false);
    }
    void CallGreatSuccessPopup() {     
        Debug.Log("Training Greatly Success");
        if (Random.Range(0, 1000000) <= 50000)
            Debug.Log("Durability Dropped");
    }

    void ResetPopupTransformAndSize(GameObject _o)
    {
        _o.transform.parent = UIRoot.transform;
        _o.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        _o.GetComponent<RectTransform>().offsetMin = Vector2.zero;
    }
}
