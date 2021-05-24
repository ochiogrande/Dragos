using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    //open and close the shop
    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    //gold acces text
    public Text goldText;

    //items for sale
    public string[] itemsForSale;

    //items to buy
    public ItemButton[] buyItemsButtons;
    public ItemButton[] sellItemsButtons;

    public Items selectedItem;
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemsName, sellItemDescription, sellItemValue;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();
        GameManager.instance.shopActive = true;

        goldText.text = GameManager.instance.currentGold.ToString() + "g";

       
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);
        GameManager.instance.shopActive = false;
        
        
        
    }

    public void OpenBuyMenu()
    {
        buyItemsButtons[0].Press();

        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        for(int i = 0; i < buyItemsButtons.Length; i++)
        {
            buyItemsButtons[i].buttonValue = i;
            
            if (itemsForSale[i] != "")
            {
                buyItemsButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemsButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemsButtons[i].amountText.text = "";
            }
            else
            {
                buyItemsButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemsButtons[i].amountText.text = "";
            }
        }
    }

    public void OpenSellMenu()
    {
        sellItemsButtons[0].Press();

        buyMenu.SetActive(false);
        sellMenu.SetActive(true);

        ShowSellItems();
    }

    //sorting function , items in the inventory
    private void ShowSellItems()
    {
        GameManager.instance.SortItems();
        for (int i = 0; i < sellItemsButtons.Length; i++)
        {

            sellItemsButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                sellItemsButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemsButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                sellItemsButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                sellItemsButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemsButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectBuyItem(Items buyItem)
    {
        selectedItem = buyItem;
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        buyItemValue.text = "Value: " + selectedItem.value + "g";
        //buyItemValue.color = new Color(sellItemValue.color.r,1f, sellItemValue.color.g,1f , sellItemValue.color.b, 0f, sellItemValue.color.a, 1f);
    }

    public void SelectSellItem(Items sellItem)
    {
        selectedItem = sellItem;
        sellItemsName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.description;
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * 0.2f).ToString() + "g";
    }


    //sell and buy
    public void BuyItem()
    {
        if (selectedItem != null)
        {            
            if (GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value;

                GameManager.instance.AddItem(selectedItem.itemName);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void SellItem()
    {
        if(selectedItem != null)
        {
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * 0.2f);

            GameManager.instance.RemoveItem(selectedItem.itemName);
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";

        ShowSellItems();
    }
}
