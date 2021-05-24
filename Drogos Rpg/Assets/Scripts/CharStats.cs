using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public static CharStats instance;

    public string charName;
    public int playerLevel = 1;
    public int currentEXP;    
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 1000;


    public int currentHP;
    public int maxHP= 100;
    public int currentMP;
    public int maxMP = 30;
    public int[] mpLvlBonus; 
    public int strenght;
    public int defence;
    public int wpnpwr;
    public int armorpwr;
    public string equipedWpn;
    public string equipedArmor;
    public Sprite charImage;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;
        for(int i = 2;  i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            addExp(1000);
        }
    }

    public void addExp(int expToAdd)
    {
        currentEXP += expToAdd;
        
        if(playerLevel < maxLevel)
        {

            if (currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];
                playerLevel++;

                //here is code to add , str or def to our player. Automatic , when is leveling up.

                if (playerLevel % 2 == 0)
                {
                    strenght++;
                }
                else
                {
                    defence++;
                }

                maxHP = Mathf.FloorToInt(maxHP * 1.05F);
                currentHP = maxHP;

                maxMP += mpLvlBonus[playerLevel];
                currentMP = maxMP;
            }

        }

        if(playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
