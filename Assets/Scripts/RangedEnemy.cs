using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyController
{
    AudioSource LaserArrowAudio;
    public AudioClip LaserArrow;
    // Start is called before the first frame update
    //Can shoot player from 8 units away
    float maxRange = 8f;

    //Will avoid being less than 7 units away from player
    float minRange = 6f;

    //Can dash once every 2 seconds
    float maxDashCooldown = 2f;
    float dashCooldown = 0f;

    //Strafe reset time
    float strafeResetSpeed;
    float strafeMax = 5f;
    float strafeMin = 1f;
    float currentStrafeTime = 0;

    public float arrowRange = 10f;

    bool strafing = false;
    int dir;


    new void Start()
    {
        base.Start();
        strafeResetSpeed = Random.Range(strafeMin, strafeMax);

        LaserArrowAudio = gameObject.AddComponent<AudioSource>();
        LaserArrowAudio.clip = LaserArrow;
        LaserArrowAudio.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        CooldownController();
    }

    private void FixedUpdate()
    {
        MovementController();
    }

    override protected void CooldownController()
    {
        base.CooldownController();
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }
    }

    void AttackController()
    {
        if (attackCD <= 0)
        {
            ProjectileController arrow = Instantiate(Defaults.instance.arrow);
            arrow.isEnemy = true;
            arrow.attackDamage = attackDamage;
            arrow.transform.position = transform.position;
            arrow.startPosition = arrow.transform.position;
            arrow.transform.rotation = rotator.transform.rotation;
            arrow.transform.eulerAngles = arrow.transform.eulerAngles + Vector3.forward * 90;
            arrow.parent = gameObject;
            attackCD = maxAttackCD;
            LaserArrowAudio.Play();
            Debug.Log("Is Playing");
        }
    }

    protected void MovementController()
    {
        float distance = DetectionCheck();
        if (detected)
        {
            //Look at player
            //rb.MoveRotation(Mathf.Rad2Deg * Vector2Angle(player));
            rotator.transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector3)player - transform.position);

            Vector2 direction = player;
            if (distance > maxRange)
            {
                strafing = false;

                //Move towards player
                direction -= (Vector2)transform.position;
                MoveInDirection(direction);
            }
            else if (distance < minRange)
            {
                strafing = false;

                //Move away from player
                direction = FlipAroundSelf(player);
                direction -= (Vector2)transform.position;
                MoveInDirection(direction);
            }
            else
            {
                AttackController();
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
        Vector2 newPos = new Vector2(Mathf.Cos(Mathf.Deg2Rad * movementDirection), Mathf.Sin(Mathf.Deg2Rad * movementDirection)).normalized;
        rb.MovePosition(transform.position + (Vector3)newPos.normalized * speed * Time.deltaTime);
    }
}
