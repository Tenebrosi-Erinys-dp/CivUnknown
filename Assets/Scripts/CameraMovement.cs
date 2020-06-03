using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

//Kaijie Zhou
//Last edited by: Nick Erb 05/28
public class CameraMovement : MonoBehaviour
{
    public int roomWidthInstance;
    public int roomHeightInstance;
    public static int roomWidth = 0;
    public static int roomHeight = 0;
    float camZ;
    static GameObject Player;
    Vector2 lastRoom;
    public float timeToChangeRooms = .25f;
    public GameObject color;
    // Start is called before the first frame update
    void Start()
    {
        roomWidth = roomWidthInstance;
        roomHeight = roomHeightInstance;
        camZ = transform.position.z;
        Player = GameObject.Find("Player");
    }

    // Update is called once per framee
    void Update()
    {
        //Basically, take the player's position
        Vector3 playerPos = Player.transform.position;
        Vector2Int room = GetRoomFromPosition(playerPos);
        //And derive the room value from that
        if (lastRoom != room)
        {
            lastRoom = room;
            //Change rooms
            StartCoroutine(SmoothCameraMove(room, timeToChangeRooms));
        }
        //Then, set the camera into the center of the derived room
    }

    public static Vector2Int GetRoomFromPosition(Vector3 pos)
    {
        Vector2Int room = Vector2Int.zero;
        room.x = (int)(pos.x + roomWidth / 2) / roomWidth;
        room.y = (int)(pos.y + roomHeight / 2) / roomHeight;
        return room;
    }

    IEnumerator SmoothCameraMove(Vector2Int room, float time)
    {
        //Randomize color
        Vector3 startPos = transform.position;
        float timer = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, new Vector3(room.x * roomWidth, room.y * roomHeight, camZ), timer / time);
            yield return null;
        }
    }
}
