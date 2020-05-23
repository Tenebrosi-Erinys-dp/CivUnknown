using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class Melee : MonoBehaviour
{
    //box collider is trigger moves with player swing
    //rotate swing box collider

    GameObject Swing = GameObject.Find("Swing");
    public double swingTime = 0;
    public Vector2 BoxColliderSize = Vector2.zero;
    public Vector3 BoxColliderPosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If press button to melee attack
        if (Input.GetButton("Fire1")) {
            //Instantiate the collider on the player's arm at BoxColliderPosition
            GameObject.Instantiate(Swing, BoxColliderPosition, Quaternion.identity, GameObject.Find("Player").transform);
            //rotate the box collider in a semicircle within the total time

            // Destroy once done
            GameObject.Destroy(Swing);
        } 
    }
    //Damage enemies hit by swing (collider)
    private void OnTriggerEnter2D(Collider2D collider) { 
    
    } 
}
