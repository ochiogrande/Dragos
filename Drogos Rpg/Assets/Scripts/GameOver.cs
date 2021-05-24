using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public string mainMenuScene;
    public string loadGameScene;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayBGM(4);

        Player.instance.gameObject.SetActive(false);
        GameManager.instance.gameObject.SetActive(false);
        BattleManager.instance.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void QuitToMainMenu()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(Player.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadLastSavedGame()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(Player.instance.gameObject);
        
        Destroy(GameMenu.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene(loadGameScene);
    }
}
