using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuSelect : MonoBehaviour
{
    public GameObject itemMenu;
    

    private CharStats[] playerStats;

    public Text[] nameText, HPText, MPText, LevText, ExpText;
    public Slider[] expSlider;
    public Image[] charImage;
    public GameObject[] charStatHolder;

    public GameObject[] windows;

    //Menu info for Game stats
    public GameObject[] statusButtons;

    //status infos
    public Text statusName, statusHP, statusMP, statusStr, statusDef, statusWpnEqpd, statusWpnPwr, statusArmrEqpd, statusArmrPwr, statusExp;
    public Image statusImg;

    //items button is called here
    public ItemButton[] itemButtons;

    //using items from inventory
    public static ItemMenuSelect instance;
    public string selectedItem;
    public Items activeItem;
    public Text itemName, itemDescription, useButtonText;


    //calling the character to use or equip
    public GameObject itemCharChoiseMenu;
    public Text[] itemsCharChoiseNames;

    //gold show in the menu
    public Text goldText;

    //close game whit quit button
    public string mainMenuName;

    

    public int buttonValue;
    public Image buttonImage;
    public Text amountText;

    

    void Start()
    {
        // calling the game menu
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {       
        


    }

    public void Press()
    {
        
        
               
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);
                nameText[i].text = playerStats[i].charName;
                HPText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                MPText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                LevText[i].text = "Level: " + playerStats[i].playerLevel;
                ExpText[i].text = "" + playerStats[i].currentEXP + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentEXP;
                charImage[i].sprite = playerStats[i].charImage;
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void ShowButtons()
    {
        //caling the function Sort items , from game manager
        GameManager.instance.SortItems();


        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;
            //cheking if is item in the item button
            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }
}
