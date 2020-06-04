using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{

    protected Vector2 player;
    protected bool detected;
    protected GameObject rotator;
    public bool exists = true;

    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        exists = true;
        rb = GetComponent<Rigidbody2D>();
        healthBar = Instantiate(Defaults.instance.health);
        healthBar.entity = this;
        healthBar.bar = healthBar.transform.Find("Bar").gameObject;
        currentHp = maxHp;
        rotator = transform.Find("Rotator").gameObject;
    }

    // Update is called once per frame

    protected float DetectionCheck()
    {
        float distance = Vector2.Distance(pc.transform.position, transform.position);
        if (CameraMovement.GetRoomFromPosition(transform.position) == CameraMovement.GetRoomFromPosition(pc.transform.position))
        {
            player = pc.transform.position;
            detected = true;
        }
        else
        {
            detected = false;
        }
        return distance;
    }

    protected override void Die()
    {
        exists = false;
        GameManager.instance.CheckGameWin();
        Destroy(gameObject);
    }
}
