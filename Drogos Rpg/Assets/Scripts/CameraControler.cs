using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraControler : MonoBehaviour
{
    public Transform target;
    //limit to tileMap
    public Tilemap theMap;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;
    //limiting in the Tilemap
    private float halfHight;
    private float halfWidth;


    //audio is caled here
    public int musicToPlay;
    public bool musicStarted;

    // Start is called before the first frame update
    void Start()
    {
        //target = Player.instance.transform;
        target = FindObjectOfType<Player>().transform;

        halfHight = Camera.main.orthographicSize;
        halfWidth = halfHight * Camera.main.aspect;

        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHight, 0f);
        topRightLimit = theMap.localBounds.max + new Vector3(-halfWidth, -halfHight, 0f);

        //Here we send data to Player , to use and move inside TileMap
        Player.instance.setBounds(theMap.localBounds.min, theMap.localBounds.max);
    }

    // LateUpdate is called once per frame ,after update.
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);

        if(!musicStarted)
        {
            musicStarted = true;

            AudioManager.instance.PlayBGM(musicToPlay);
        }
    }
}
