using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopAttendant : MonoBehaviour
{
    bool isItemSelected = false;
    public GameObject itemPriceInfo;
    public GameObject calcInfo;
    public GameObject itemList;
    public Text txtItemName;
    public Text txtItemPrice;    
    public Text txtAfterPurchasePoint;

    private void LateUpdate()
    {
        if(!isItemSelected)
        {
            itemPriceInfo.SetActive(false);
            calcInfo.SetActive(false);
        }
        else
        {
            itemPriceInfo.SetActive(true);
            calcInfo.SetActive(true);
        }
    }

    private void Start()
    {
        if (itemList.transform.childCount > 0)
            for(int i = 0; i < itemList.transform.childCount; i++)
            {
                itemList.transform.GetChild(i).GetComponent<ItemInfo>().SetShopAttendant(gameObject.GetComponent<ShopAttendant>());
            }
    }

    public void SetItemPriceInfo(int _price)
    {
        isItemSelected = true;
        txtItemPrice.text = _price.ToString() + " pt";
        int afterpurchase = 0;
        afterpurchase = GameManager.Instance.GetCurrentPoint() - _price;

        txtAfterPurchasePoint.text = afterpurchase.ToString() + " pt";

        if (afterpurchase > 0)
        {
            txtAfterPurchasePoint.color = new Color(0.85f,0.85f,0.85f);
        }
        else
            txtAfterPurchasePoint.color = new Color(0.5f,0.028f,0.028f);
    }

    public void ClearItemPriceInfo()
    {
        isItemSelected = false;
    }
}
