using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharStats[] playerStats;

    public bool gameMenuopen , dialogActive , fadingBetweenAreas , shopActive , battleActive;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Items[] referanceItem;

    //gold implemented
    public int currentGold;

    // Start is called before the first frame update
    void Start()
    {        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }

        }
        DontDestroyOnLoad(gameObject);

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameMenuopen || dialogActive || fadingBetweenAreas || shopActive || battleActive)
        {
            Player.instance.canMove = false;
        }
        else
        {
            Player.instance.canMove = true;
        }
       

        //test save&load data
        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
        }

    }

    public Items GetItemDetails(string ItemToGrab)
    {
        for(int i = 0; i < referanceItem.Length; i++)
        {
            if(referanceItem[i].itemName == ItemToGrab)
            {
                return referanceItem[i];
            }

        }

        return null;
    }

    public void SortItems()
    {
        //check if next itemButton is empty
        bool itemAfterSpace = true;
        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            //cheking if the itemButtons are empty or whit item.
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    //this keep the loop on
                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }


    //add or remove items from inventory
    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemsHeld.Length;
                foundSpace = true;
            }
        }

        if(foundSpace)
        {
            bool itemExist = false;
            for(int i = 0; i < referanceItem.Length; i++)
            {
                if(referanceItem[i].itemName == itemToAdd)
                {
                    itemExist = true;

                    i = referanceItem.Length;
                }
            }

            if(itemExist)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + "Does not exist!!");
            }

        }

        GameMenu.instance.ShowButtons();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;

                i = itemsHeld.Length;
            }
        }

        if(foundItem)
        {
            numberOfItems[itemPosition]--;

            if(numberOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }

            GameMenu.instance.ShowButtons();

        }
        else
        {
            Debug.LogError("Couldnt find " + itemToRemove);
        }

    }

    public void SaveData()
    {
        //save position char
        PlayerPrefs.SetString("Current_scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", Player.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", Player.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", Player.instance.transform.position.z);


        //save char info
        for(int i = 0; i < playerStats.Length; i++)
        {
            if(playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0);
            }

            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentEXP", playerStats[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strenght", playerStats[i].strenght);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defence", playerStats[i].defence);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WpnPwr", playerStats[i].wpnpwr);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armorpwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWpn", playerStats[i].equipedWpn);
            PlayerPrefs.SetString("Player-" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equipedArmor);
        }

        //save inventory info
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }

    }

    
    public void LoadData()
    {
        
        //load position player
        Player.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));


        // load player info
        for(int i = 0; i < playerStats.Length; i++)
        {
            if(PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
            playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentEXP");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP");
            playerStats[i].strenght = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strenght");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defence");
            playerStats[i].wpnpwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WpnPwr");
            playerStats[i].armorpwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmrPwr");
            playerStats[i].equipedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWpn");
            playerStats[i].equipedArmor = PlayerPrefs.GetString("Player-" + playerStats[i].charName + "_EquippedArmr");
        }

        //load inventory info
        for( int i= 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }
}
