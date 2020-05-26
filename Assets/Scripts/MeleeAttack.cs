using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class MeleeAttack : MonoBehaviour
{
    //box collider is trigger moves with player swing
    //rotate swing box collider

    GameObject Swing;
    public double swingTime = 0;
    public Vector2 BoxColliderSize = Vector2.zero;
    public Vector3 BoxColliderPosition = Vector3.zero;
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Swing = GameObject.Find("Swing");
        GameObject Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //If press button to melee attack
        if (Input.GetButton("Fire1")) {
            //Instantiate the collider on the player's arm at BoxColliderPosition
            GameObject.Instantiate(Swing, BoxColliderPosition, Quaternion.identity, GameObject.Find("Player").transform);
            //rotate the box collider in a semicircle within the total time
            Swing.transform.RotateAround(new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z), new Vector3(Player.transform.position.x, BoxColliderPosition.y, Player.transform.position.z), 180 * Time.deltaTime);
            // Destroy once done
            GameObject.Destroy(Swing);
        } 
    }
    
}
