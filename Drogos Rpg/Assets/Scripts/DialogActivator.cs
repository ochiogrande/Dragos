using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public string[] lines;

    private bool CanActivate;

    public bool isPerson = true;

    //quest activator
    public bool shouldActivateQuest;
    public string questToMArk;
    public bool markComplete;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CanActivate && Input.GetButtonDown("npc") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            DialogManager.instance.showDialog(lines , isPerson);
            DialogManager.instance.ShouldActivateQuestAtEnd(questToMArk , markComplete);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            CanActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag =="Player")
        {
            CanActivate = false;
        }
    }
}
