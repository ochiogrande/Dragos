using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialLoaders : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject player;
    public GameObject gameMan;
    public GameObject audio;
    public GameObject battleManager;
   

    // Start is called before the first frame update
    void Start()
    {
        if(UIFade.instance == null)
        {
            //Instantiate(UIScreen);
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }

        if(Player.instance == null)
        {
            Player clone = Instantiate(player).GetComponent<Player>();
            Player.instance = clone;
        }

        if(GameManager.instance == null)
        {
            Instantiate(gameMan);
        }

        if(AudioManager.instance == null)
        {
            Instantiate(audio);
        }

        if(BattleManager.instance == null)
        {
            Instantiate(battleManager);
        }

      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
