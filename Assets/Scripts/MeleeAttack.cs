using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Melee : MonoBehaviour
{
    //box collider is trigger moves with player swing
    //rotate swing box collider
    //Input class fire1 Input.Getbutton("Fire1");

    GameObject Swing = GameObject.Find("Swing");
    public double swingTime = 0;
    public Vector2 BoxColliderSize = Vector2.Zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Damage enemies hit by swing (collider)
    private void OnTriggerEnter2D(Collider2D collider) { 
    
    } 
}
