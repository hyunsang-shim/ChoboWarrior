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
    public GameObject btnBuy, btnClose;

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
                itemList.transform.GetChild(i).GetComponent<ItemInfo>().itemId = i;
            }

        btnClose.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ClosePopUpButton(gameObject));

    }

    public void SetItemPriceInfo(int _price, int _activeItemId)
    {
        DeactivateOtherItems(_activeItemId);

        isItemSelected = true;
        txtItemPrice.text = _price.ToString() + " pt";

        itemList.transform.GetChild(_activeItemId).GetComponent<ItemInfo>().isSet = true;
        

        int afterpurchase = 0;
        afterpurchase = GameManager.Instance.GetCurrentPoint() - _price;

        txtAfterPurchasePoint.text = afterpurchase.ToString() + " pt";

        if (afterpurchase > 0)
        {
            txtAfterPurchasePoint.color = new Color(0.85f,0.85f,0.85f);
        }
        else
            txtAfterPurchasePoint.color = new Color(1f,0.028f,0.028f);
    }

    void DeactivateOtherItems(int _activeItemID)
    {
        if (itemList.transform.childCount > 0)
            for (int i = 0; i < itemList.transform.childCount; i++)
            {
                if (i != _activeItemID)
                {
                    itemList.transform.GetChild(i).GetComponent<ItemInfo>().isSet = false;
                }
            }
    }
    public void ClearItemPriceInfo()
    {
        isItemSelected = false;
    }
}
