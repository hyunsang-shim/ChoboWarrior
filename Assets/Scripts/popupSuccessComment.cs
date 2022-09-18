using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popupSuccessComment : MonoBehaviour
{
    public UnityEngine.UI.Text successComment;
    public string comment;
    public bool isGreatSuccess = false;
    public bool isBattleSuccess = false;
    int points;


    private void Start()
    {
        GetPoint();
    }

    void GetPoint()
    {
        points = GameManager.Instance.GetRewardPoint(isBattleSuccess, isGreatSuccess);

        if (isBattleSuccess)
        {
            if (isGreatSuccess)
                successComment.text = $"���� ��¸�!!!\n\n {points} ����Ʈ�� ������ϴ�!";
            else
                successComment.text = $"���� �¸�!\n\n {points} ����Ʈ�� ������ϴ�!";
        }
        else
        {
            if (isGreatSuccess)
                successComment.text = $"���� �뼺��!!!\n\n {points} ����Ʈ�� ������ϴ�!";
            else
                successComment.text = $"���� ����!\n\n {points} ����Ʈ�� ������ϴ�!";
        }
    }
}
