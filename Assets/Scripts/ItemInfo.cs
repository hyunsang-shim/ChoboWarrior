using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public Image itemSprite;
    public GameObject isSelected;
    public string itemName;
    public int itemPrice;
    public ShopAttendant attendant;
    public bool isSet = true;
    public Text txtItemName;
    public Text txtItemPrice;

    private void Awake()
    {
        isSelected.SetActive(false);
    }

    private void LateUpdate()
    {
        txtItemName.text = itemName;
        txtItemPrice.text = itemPrice.ToString() + " pt";
    }

    public void SetItemPrice()
    {
        if (isSet)
        {
            isSet = false;
            isSelected.SetActive(true);
            attendant.SetItemPriceInfo(itemPrice);
        }
        else
        {
            isSet = true;
            isSelected.SetActive(false);
            attendant.ClearItemPriceInfo();
        }
    }

    public void SetShopAttendant(ShopAttendant _ref)
    {
        attendant = _ref;
    }

}
