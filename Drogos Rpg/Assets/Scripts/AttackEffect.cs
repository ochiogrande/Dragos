using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float attackLenght;
    public int soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySFX(soundEffect);   
    }

    // Update is called once per frame
    void Update()
    {
        DestroyObject(gameObject, attackLenght);
    }
}
