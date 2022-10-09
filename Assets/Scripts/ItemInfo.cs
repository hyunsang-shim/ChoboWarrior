using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public Image itemSprite;
    public GameObject bgSelected;
    public GameObject overlaySoldout;
    public string itemName;
    public int itemPrice;
    public int itemId;
    public ShopAttendant attendant;
    public bool isSet = false;
    public bool isSold = false;
    public Text txtItemName;
    public Text txtItemPrice;


    private void Awake()
    {
        isSet = false;
        bgSelected.SetActive(false);
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
            bgSelected.SetActive(true);
        else
            bgSelected.SetActive(false);

        if (isSold)
        {
            bgSelected.SetActive(false);
            overlaySoldout.SetActive(true);
            GetComponent<Button>().interactable = false;
        }
        else
        {            
            overlaySoldout.SetActive(false);
            GetComponent<Button>().interactable = true;
        }

        txtItemName.text = itemName;
        txtItemPrice.text = string.Format("{0:N0}", itemPrice) + " pt";
    }

    public void SetShopAttendant(ShopAttendant _ref)
    {
        attendant = _ref;
    }

    public void SetItemInfo(int _i)
    {
        Debug.Log($"Trying to get Item Info {_i}");
        

        itemSprite.sprite = GameManager.Instance.GetItemSprite(_i);
        itemName = GameManager.Instance.GetItemName(_i);
        itemPrice = GameManager.Instance.GetItemPrice(_i);

        Debug.Log($"Item Info {_i} set successfuly");
    }

}
