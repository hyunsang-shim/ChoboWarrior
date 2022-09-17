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
    public Text txtMyPoints;
    public GameObject btnBuy, btnClose;
    int pointsAfterPurchase = 0, myPoints;
    int itemPrice, selectedItemId;

    private void LateUpdate()
    {

        txtMyPoints.text = string.Format("{0:N0}", myPoints) + " pt";


        if (!isItemSelected)
        {
            txtItemName.text = "선택된 아이템 없음";
            txtItemPrice.text = "0";
            pointsAfterPurchase = myPoints;
            txtAfterPurchasePoint.color = new Color(0.85f, 0.85f, 0.85f);
        }


        txtMyPoints.text = string.Format("{0:N0}", myPoints) + " pt";


        txtAfterPurchasePoint.text = string.Format("{0:N0}", pointsAfterPurchase) + " pt";
    }

    private void Start()
    {
        myPoints = GameManager.Instance.GetCurrentPoint();

        if (itemList.transform.childCount > 0)
        {
            UpdateItemList();
        }
        else
        {
            InitItemList();
        }

        txtMyPoints.text = string.Format("{0:N0}", myPoints) + " pt"; 
        itemPriceInfo.SetActive(true);
        calcInfo.SetActive(true);
        pointsAfterPurchase = myPoints;


        btnClose.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ClosePopUpButton(gameObject));
        btnBuy.GetComponent<Button>().onClick.AddListener(() => BuyItem());

    }

    public void SetItemPriceInfo(int _price, int _activeItemId)
    {
        DeactivateOtherItems(_activeItemId);

        isItemSelected = true;
        txtItemPrice.text = _price.ToString() + " pt";
        itemPrice = _price;
        selectedItemId = _activeItemId;


        itemList.transform.GetChild(_activeItemId).GetComponent<ItemInfo>().isSet = true;
        txtItemName.text = itemList.transform.GetChild(_activeItemId).GetComponent<ItemInfo>().txtItemName.text;
        txtItemPrice.text = string.Format("{0:N0}", itemPrice) + " pt";


        pointsAfterPurchase = GameManager.Instance.GetCurrentPoint() - _price;

        txtAfterPurchasePoint.text = string.Format("{0:N0}", pointsAfterPurchase) + " pt";

        if (pointsAfterPurchase > 0)
        {
            txtAfterPurchasePoint.color = new Color(0.85f,0.85f,0.85f);
        }
        else
            txtAfterPurchasePoint.color = new Color(1f,0.028f,0.028f);

        GameManager.Instance.SM.PlaySFX("Select");

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

    public void BuyItem()
    {
        if(GameManager.Instance.CheckBuyItem(selectedItemId, itemPrice) == true)
        {
            itemList.transform.GetChild(selectedItemId).GetComponent<ItemInfo>().isSold = true;

            GameManager.Instance.SaveData();
            Debug.Log($"Item Bought!!! {selectedItemId}({itemPrice}pt)");
            isItemSelected = false;
        }


        UpdateItemList();

    }

    void UpdateItemList()
    {
        myPoints = GameManager.Instance.GetCurrentPoint();
        int _count = GameManager.Instance.itemCount;
        Transform trans_Child;
        for (int i = 0; i < itemList.transform.childCount; i++)
        {
            trans_Child = itemList.transform.GetChild(i);
            trans_Child.GetComponent<ItemInfo>().itemId = i;
            trans_Child.GetComponent<ItemInfo>().isSet = false;
            trans_Child.GetComponent<ItemInfo>().isSold = false;
            trans_Child.transform.SetParent(itemList.transform);


            if (((i == GameManager.Instance.weaponIdx) && i < 3)
                || ((i == GameManager.Instance.shieldIdx + 3) && i < 6)
                || ((i == GameManager.Instance.armorIdx + 6) && i < 9))
            {
                itemList.transform.GetChild(i).GetComponent<ItemInfo>().isSold = true;
                trans_Child.GetComponent<ItemInfo>().isSet = false;
                isItemSelected = false;
            }
        }

    }


    void InitItemList()
    {
        myPoints = GameManager.Instance.GetCurrentPoint();


        int _count = GameManager.Instance.itemCount;

        for (int i = 0; i < _count; i++)
        {
            GameObject o = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ItemInfo"));
            o.GetComponent<ItemInfo>().SetShopAttendant(gameObject.GetComponent<ShopAttendant>());
            o.GetComponent<ItemInfo>().itemId = i;
            o.GetComponent<ItemInfo>().isSet = false;
            o.GetComponent<ItemInfo>().isSold = false;
            o.transform.SetParent(itemList.transform);
            o.GetComponent<ItemInfo>().SetItemInfo(i);

            if ((i == GameManager.Instance.weaponIdx) && i < 3)
                itemList.transform.GetChild(i).GetComponent<ItemInfo>().isSold = true;

            if ((i == GameManager.Instance.shieldIdx + 3) && i < 6)
                itemList.transform.GetChild(i).GetComponent<ItemInfo>().isSold = true;

            if ((i == GameManager.Instance.armorIdx + 6) && i < 9)
                itemList.transform.GetChild(i).GetComponent<ItemInfo>().isSold = true;




        }
    }

}
