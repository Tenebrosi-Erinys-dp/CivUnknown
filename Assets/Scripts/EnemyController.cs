using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    //Can detect the mummy within 10 units
    float detectionRadius = 10f;

    protected Vector2 player;
    protected bool detected;


    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        healthBar = Instantiate(Defaults.instance.health);
        healthBar.entity = this;
        healthBar.bar = healthBar.transform.Find("Bar").gameObject;
        currentHp = maxHp;
    }

    // Update is called once per frame

    protected float DetectionCheck()
    {
        float distance = Vector2.Distance(pc.transform.position, transform.position);
        if (distance < detectionRadius)
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
        Destroy(gameObject);
    }
}
