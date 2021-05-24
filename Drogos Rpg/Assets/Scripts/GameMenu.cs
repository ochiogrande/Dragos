using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;

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
    public static GameMenu instance;
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

    void Start()
    {
        // calling the game menu
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("inv"))
        {
            if (theMenu.activeInHierarchy)
            {
                //theMenu.SetActive(false);
                //GameManager.instance.gameMenuopen = false;
                CloseMenu();
            }
            else
            {
                theMenu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.gameMenuopen = true;
            }

            AudioManager.instance.PlaySFX(5);
        }

        
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

    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();

        for (int i = 0; i < windows.Length; i++)
        {
            if (i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }

        itemCharChoiseMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        theMenu.SetActive(false);
        GameManager.instance.gameMenuopen = false;

        itemCharChoiseMenu.SetActive(false);

    }

    public void openStatus()
    {
        UpdateMainStats();
        //calling the all status info to update
        statusChar(0);

        for (int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            //selection of player name
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }
    //player display the status upadates in the status menu
    public void statusChar(int selected)
    {
        statusName.text = playerStats[selected].charName;
        statusHP.text = "" + playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;
        statusMP.text = "" + playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;
        statusStr.text = playerStats[selected].strenght.ToString();
        statusDef.text = playerStats[selected].defence.ToString();
        if (playerStats[selected].equipedWpn != "")
        {
            statusWpnEqpd.text = playerStats[selected].equipedWpn;
        }
        statusWpnPwr.text = playerStats[selected].wpnpwr.ToString();
        if (playerStats[selected].equipedArmor != "")
        {
            statusArmrEqpd.text = playerStats[selected].equipedArmor;
        }
        statusArmrPwr.text = playerStats[selected].armorpwr.ToString();
        statusExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();
        statusImg.sprite = playerStats[selected].charImage;
    }

    //the Items Button is called here.
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

    //call this function to know what items is to use for description and to find out the name of item
    public void SelectItem(Items newItem)
    {
        activeItem = newItem;

        if (activeItem.isItem)
        {
            useButtonText.text = "Use";

        }

        if (activeItem.isWeapon || activeItem.isArmor)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }

    }

    public void OpenItemCharChoise()
    {
        itemCharChoiseMenu.SetActive(true);

        for (int i = 0; i < itemsCharChoiseNames.Length; i++)
        {
            itemsCharChoiseNames[i].text = GameManager.instance.playerStats[i].charName;
            itemsCharChoiseNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }

    public void CloseItemCharChoise()
    {
        itemCharChoiseMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        CloseItemCharChoise();
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);
        Destroy(GameManager.instance.gameObject);
        Destroy(Player.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }

}
