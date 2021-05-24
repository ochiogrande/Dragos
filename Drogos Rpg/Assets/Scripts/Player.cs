using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;

    public Animator myAnim;

    public static Player instance;

    public string areaTransitionName;

    //limit of player going outside the TileMap
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    public bool canMove;


    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
            
        }

       

        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {
        if (canMove)
        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            if (canMove)
            {
                myAnim.SetFloat("lastmoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("lastmoveY", Input.GetAxisRaw("Vertical"));
            }
        }

        //limiting the position of player inside TileMap
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y) , transform.position.z);
    }

    //calling the data from Camera , pe cose she use TileMap , to stay inside. So whit this function , we call those data from Camera.
    public void setBounds(Vector3 botLeft , Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(0.7f , 0.7f , 0f);
        topRightLimit = topRight + new Vector3(-0.7f , -0.7f , 0f);
    }

}
