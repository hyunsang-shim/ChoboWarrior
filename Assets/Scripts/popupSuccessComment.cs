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
                successComment.text = $"전투 대승리!!!\n\n {points} 포인트를 얻었습니다!";
            else
                successComment.text = $"전투 승리!\n\n {points} 포인트를 얻었습니다!";
        }
        else
        {
            if (isGreatSuccess)
                successComment.text = $"수련 대성공!!!\n\n {points} 포인트를 얻었습니다!";
            else
                successComment.text = $"수련 성공!\n\n {points} 포인트를 얻었습니다!";
        }
    }
}
