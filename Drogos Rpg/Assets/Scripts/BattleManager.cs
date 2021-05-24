using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{

    //add players and enemies
    public static BattleManager instance;

    private bool battleActive;

    public GameObject battleScene;

    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    //add turns
    public int currentTurn;
    public bool turnWaiting;

    public GameObject uiButtonsHolder;

    //calling attack effects
    public BattleMoves[] movesList;
    public GameObject enemyAttackEffect;

    //caling Damage Number
    public DamageNumber theDamageNumber;

    //updating stats
    public Text[] playerNames, playerHP, playerMP;


    //player select enemy
    public GameObject targetMenu;
    public BattleTargetButton[] targetButton;

    //magic menu
    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    //no mp
    public BattleNotification battleNotice;

    //flee function
    public int chanceToFlee = 45;

    //this is not part from procent succes(no reward, skype turn)
    private bool fleeing;

    //making item button
    public GameObject itemMenu;

    //game over
    
    public string gameOverScene;

    public int rewardXP;
    public string[] rewardItems;

    public GameObject battleEnemy;


    //create a non excape from dragon fight
    public bool cannotFlee;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            BattleStarts(new string[] { "Eye Ball", "Spider", "Scheleton", "Scheleton" } , false);
        }

        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                }
                else
                {
                    uiButtonsHolder.SetActive(false);
                    //enemy should attack
                    StartCoroutine(EnemyMoveCo());
                }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }
    }

    public void BattleStarts(string[] enemiesToSpawn , bool setCannotFlee)
    {
        if (!battleActive)
        {
            cannotFlee = setCannotFlee;

            battleActive = true;

            GameManager.instance.battleActive = true;
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);

            AudioManager.instance.PlayBGM(0);

            for (int i = 0; i < playerPositions.Length; i++)
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);

                            CharStats thePlayer = GameManager.instance.playerStats[i];
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strenght = thePlayer.strenght;
                            activeBattlers[i].defence = thePlayer.defence;
                            activeBattlers[i].wpnPower = thePlayer.wpnpwr;
                            activeBattlers[i].armrPower = thePlayer.armorpwr;

                        }
                    }
                }
            }

            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {

                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }

                    }

                }

            }

            turnWaiting = true;
            currentTurn = 0;
            UpdateUIStats();


        }

    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;

        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHP < 0)
            {
                activeBattlers[i].currentHP = 0;
            }

            if (activeBattlers[i].currentHP == 0)
            {
                //handle dead battler
                if(activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                    allPlayersDead = false;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                StartCoroutine(EndBattleCo());
            }
            else
            {
                StartCoroutine(GameOverCo());
            }

           /* battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false; */
        }
        else
        {
            while (activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();



    }

    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }

        int selectedTarget = players[Random.Range(0, players.Count)];

        //activeBattlers[selectedTarget].currentHP -= 20;
        //selecting the moves
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].movesName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float attackPwr = activeBattlers[currentTurn].strenght + activeBattlers[currentTurn].wpnPower;
        float defencePwr = activeBattlers[target].defence + activeBattlers[target].armrPower;

        float damageCalc = (attackPwr / defencePwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].charName + "is dealing" + damageCalc + "(" + damageToGive + ") damage to" + activeBattlers[target].charName);

        activeBattlers[target].currentHP -= damageToGive;

        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

        UpdateUIStats();
    }


    public void UpdateUIStats()
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

                    playerNames[i].gameObject.SetActive(true);
                    playerNames[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;
                }
                else
                {
                    playerNames[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerNames[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {

        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].movesName == moveName)
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);
        uiButtonsHolder.SetActive(false);

        targetMenu.SetActive(false);

        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> Enemies = new List<int>();

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }

        for (int i = 0; i < targetButton.Length; i++)
        {
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)
            {
                targetButton[i].gameObject.SetActive(true);
                targetButton[i].moveName = moveName;
                targetButton[i].activeBattlerTarget = Enemies[i];
                targetButton[i].targetName.text = activeBattlers[Enemies[i]].charName;
            }
            else
            {
                targetButton[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);

        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].movesName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].cost.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }

    }

    public void Flee()
    {
        if (cannotFlee)
        {
            battleNotice.theText.text = "Cannot Flee this battle!";
            battleNotice.Activate();
        }
        else
        {
            int fleeSucces = Random.Range(0, 100);
            if (fleeSucces < chanceToFlee)
            {
                //battleActive = false;
                //battleScene.SetActive(false);
                fleeing = true; //is not part from procent succes(no reward)

                StartCoroutine(EndBattleCo());
            }
            else
            {
                NextTurn();
                battleNotice.theText.text = "Couldn`t escape! ";
                battleNotice.Activate();
            }
        }
    }

    public void OpenItemMenu()
    {

        itemMenu.SetActive(true);


    }

    public void ShowItems(Items items)
    {
        
    }

    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);

        yield return new WaitForSeconds(1f);
        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i <activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer)
            {
                for(int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if(activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    {
                        // update after the battle(verry important to add all modified variables like XP and others bonus we get after battle).
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHP;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        //GameManager.instance.battleActive = false;

        if(fleeing)//this is a part to skype reward if skype game , or press flee button.
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            BattleReward.instance.OpenRewardScreen(rewardXP, rewardItems);
        }

        AudioManager.instance.PlayBGM(FindObjectOfType<CameraControler>().musicToPlay);

        
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(gameOverScene);
    }
}

