using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    //Can detect the mummy within 10 units
    float detectionRadius = 10f;

    //Can shoot player from 8 units away
    float maxRange = 8f;

    //Will avoid being less than 7 units away from player
    float minRange = 6f;

    //Can fire once every 2 seconds
    float maxFiringCooldown = 2f;
    float firingCooldown = 0f;

    //Can dash once every 2 seconds
    float maxDashCooldown = 2f;
    float dashCooldown = 0f;

    //Strafe reset time
    float strafeResetSpeed;
    float strafeMax = 5f;
    float strafeMin = 1f;
    float currentStrafeTime = 0;

    Vector2 player;
    bool detected;

    bool strafing = false;
    int dir;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        strafeResetSpeed = Random.Range(strafeMin, strafeMax);
        healthBar = Instantiate(Defaults.instance.health);
        healthBar.entity = this;
        healthBar.bar = healthBar.transform.Find("Bar").gameObject;
        maxHp = 10;
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        CooldownController();
        MovementController();
    }

    float DetectionCheck()
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

    void MovementController()
    {
        float distance = DetectionCheck();

        if (detected)
        {
            //Look at player
            rb.MoveRotation(Mathf.Rad2Deg * Vector2Angle(player));

            Vector2 direction = player;
            if (distance > maxRange)
            {
                strafing = false;

                //Move towards player
                direction -= (Vector2)transform.position;
                direction = Vector2.ClampMagnitude(direction, speed * Time.deltaTime);
                direction += (Vector2)transform.position;
                rb.MovePosition(direction);
            }
            else if (distance < minRange)
            {
                strafing = false;

                //Move away from player
                direction = FlipAroundSelf(player);
                direction -= (Vector2)transform.position;
                direction = Vector2.ClampMagnitude(direction, speed * Time.deltaTime);
                direction += (Vector2)transform.position;
                rb.MovePosition(direction);
            }
            else
            {
                FireController();
                if (!strafing)
                {
                    //Begin strafing
                    strafing = true;
                    //0 = counterclockwise, 1 = clockwise, 2 = NO STRAFE
                    dir = Random.Range(0, 3);
                    Strafe();
                }
                else if (strafing)
                {
                    Strafe();
                    if (currentStrafeTime < strafeResetSpeed)
                    {
                        currentStrafeTime += Time.deltaTime;
                    }
                    else
                    {
                        currentStrafeTime = 0;
                        strafing = false;
                        strafeResetSpeed = Random.Range(strafeMin, strafeMax);
                    }
                }
            }
        }
    }

    void CooldownController()
    {
        if (firingCooldown > 0)
        {
            firingCooldown -= Time.deltaTime;
        }
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }
    }

    void FireController()
    {
        if (firingCooldown <= 0)
        {
            ProjectileController arrow = Instantiate(Defaults.instance.arrow, transform);
            arrow.attackDamage = attackDamage;
            arrow.transform.position = transform.position;
            arrow.transform.rotation = transform.rotation;
            firingCooldown = maxFiringCooldown;
        }
    }

    void Strafe()
    {
        float movementDirection = Mathf.Rad2Deg * Vector2Angle(player);
        if (dir == 0)
        {
            movementDirection += 90;
        }
        else if (dir == 1)
        {
            movementDirection -= 90;
        }
        else
        {
            return;
        }
        //Get unit vector from angle
        Vector2 newPos = new Vector2(Mathf.Cos(Mathf.Deg2Rad * movementDirection), Mathf.Sin(Mathf.Deg2Rad * movementDirection));
        newPos *= speed * Time.deltaTime;
        rb.MovePosition(newPos + (Vector2)transform.position);
    }
}
