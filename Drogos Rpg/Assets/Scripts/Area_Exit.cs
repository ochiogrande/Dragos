using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Area_Exit : MonoBehaviour
{
    public string areaToLoad;

    public string areaTransitionName;

    public AreaEntrance theEntrance;

    //Fading between the scenes
    public float fadeWaitToLoad = 1f;
    private bool shouldLoadAfterFade;

    // Start is called before the first frame update
    void Start()
    {       
       theEntrance.transitionName = areaTransitionName;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldLoadAfterFade)
        {
            fadeWaitToLoad -= Time.deltaTime;
            if(fadeWaitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //SceneManager.LoadScene(areaToLoad);
            shouldLoadAfterFade = true;

            GameManager.instance.fadingBetweenAreas = true;

            UIFade.instance.FadeToBlack();

            Player.instance.areaTransitionName = areaTransitionName;
        }
    }
}
