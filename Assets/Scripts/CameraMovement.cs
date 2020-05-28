using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraMovement : MonoBehaviour
{
    public int roomWidth = 0;
    public int roomHeight = 0;
    float camZ;
    Transform currentPosition;
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        camZ = transform.position.z;
        currentPosition = gameObject.transform;
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    Vector2Int room = Vector2Int.zero;
    void Update()
    {
        //Basically, take the player's position
        Vector3 player = Player.transform.position;
        //And derive the room value from that
        room.x = (int)player.x / roomWidth;
        room.y = (int)player.y / roomHeight;
        //Then, set the camera into the center of the derived room
        Vector3 camPos = new Vector3(room.x * roomWidth, room.y * roomHeight, camZ);
        gameObject.GetComponent<Camera>().transform.position = camPos;
    }

    //FixedUpdate, use FixedDeltaTime
    void FixedUpdate() {
        //If player went up, down, right or left to the next room
        /*if (PlayerPosition.position.y -= currentPosition.position.y == roomWidth) { 
        
        }*/
    }
}
