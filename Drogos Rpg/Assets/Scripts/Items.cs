using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;
    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;
    [Header("Item Details")]
    public int amountToChange;
    public bool affectHP, affectMP, affectStr, affectDef , affectXP;
    [Header("Weapon/Armor Details")]
    public int weaponStrenght;
    public int armorStrenght;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //char use items from inventory
    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        if(isItem)
        {
            if(affectHP)
            {
                selectedChar.currentHP += amountToChange;

                if(selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            if(affectMP)
            {
                selectedChar.currentMP += amountToChange;
                if(selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }
            if(affectXP)
            {
                
                //selectedChar.currentEXP += amountToChange;               
                                   
                GameManager.instance.playerStats[charToUseOn].addExp(amountToChange);

            }

            if(affectStr)
            {
                selectedChar.strenght += amountToChange;
            }

            if(affectDef)
            {
                selectedChar.defence += amountToChange;
            }           
        }

        if(isWeapon)
        {
            if(selectedChar.equipedWpn != "")
            {
                GameManager.instance.AddItem(selectedChar.equipedWpn);
            }

            selectedChar.equipedWpn = itemName;
            selectedChar.wpnpwr = weaponStrenght;
        }

        if(isArmor)
        {
            if(selectedChar.equipedArmor != "")
            {
                GameManager.instance.AddItem(selectedChar.equipedArmor);
            }

            selectedChar.equipedArmor = itemName;
            selectedChar.armorpwr = armorStrenght;
        }

        GameManager.instance.RemoveItem(itemName);
    }
}
