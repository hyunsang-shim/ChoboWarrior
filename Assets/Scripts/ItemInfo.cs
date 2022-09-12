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
    public int itemId;
    public ShopAttendant attendant;
    public bool isSet = false;
    public Text txtItemName;
    public Text txtItemPrice;

    private void Awake()
    {
        isSet = false;
        isSelected.SetActive(false);
    }

    private void Start()
    {
        if (attendant == null)
        {
            ShopAttendant tmp_SA = FindObjectOfType<ShopAttendant>().GetComponent<ShopAttendant>();
            attendant = tmp_SA;
        }
        
        GetComponent<Button>().onClick.AddListener(() => attendant.SetItemPriceInfo(itemPrice, itemId));


        txtItemName.text = itemName;
        txtItemPrice.text = itemPrice.ToString() + " pt";
    }


    private void LateUpdate()
    {
        if (isSet)
            isSelected.SetActive(true);
        else
            isSelected.SetActive(false);
    }

    public void SetShopAttendant(ShopAttendant _ref)
    {
        attendant = _ref;
    }

}
