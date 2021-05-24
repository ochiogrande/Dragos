using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeper : MonoBehaviour
{
    private bool canOpen;

    //items for sale
    public string[] ItemsForSale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            print(canOpen);
            print(Player.instance.canMove);
            print(!Shop.instance.shopMenu.activeInHierarchy);
        }

        if (canOpen && Input.GetKeyDown(KeyCode.K) && Player.instance.canMove && !Shop.instance.shopMenu.activeInHierarchy) 
        {
            Shop.instance.itemsForSale = ItemsForSale;
            Shop.instance.OpenShop();
            print("seting item for sale");
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpen = false;
        }
    }
}
