using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingDamageEnemy : MonoBehaviour
{
    public int damageAmount = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //Damage Enemies on collision
    private void OnTriggerEnter2D (Collider2D collision) {
       //remove comment when enemy health is implemented and change to correct varaible
        //collision.transform.parent.gameObject.health -= damageAmount;
    }
}
