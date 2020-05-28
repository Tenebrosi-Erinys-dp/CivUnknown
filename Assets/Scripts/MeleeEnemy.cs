using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController
{
    public Hitbox hitbox;
    Hitbox hitboxInstance;
    public float attackRange = 2f;
    public float attackRadius = 45;
    public float attackDuration = 1f;
    public float attackRecovery = 0.5f;
    public float runAwayRange = 10f;
    bool attacking = false;
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        MovementController();
        CooldownController();
    }

    void MovementController()
    {
        float distance = DetectionCheck();
        if (detected)
        {
            //IF player is too close AND cooldown is off, attack
            if(!attacking && distance < attackRange && attackCD <= 0)
            {
                attackCD = maxAttackCD;
                attacking = true;
                StartCoroutine(MeleeAttack());
            }
            //If player is too far AND cooldown is off, attack
            else if (!attacking && distance >= attackRange && attackCD <= 0.5f)
            {
                //Go closer
                Debug.Log("Closing In");
                MoveInDirection(player - (Vector2)transform.position);
                rb.MoveRotation(Vector2Angle(player));
            }
            //If the cooldown is too long and player is too close, run
            else if(!attacking && distance < runAwayRange)
            {
                Vector2 direction = FlipAroundSelf(player);
                direction -= (Vector2)transform.position;

                //ADD BACK AFTER TESTING
                MoveInDirection(direction);
                rb.MoveRotation(Vector2Angle(player));
            }
            //Otherwise, wait
        }
    }

    IEnumerator RotateTo(Vector2 position, float seconds)
    {
        float timer = 0;
        float initialRot = rb.rotation;
        float newRot = Vector2Angle(position);
        while(timer < seconds)
        {
            timer += Time.deltaTime;
            rb.MoveRotation(Mathf.Lerp(initialRot, newRot, timer / seconds));
            yield return null;
        }
    }

    IEnumerator MeleeAttack()
    {
        //Generate attack hitbox
        float timer = 0f;
        float rotation = rb.rotation - attackRadius / 2f;
        float finalRotation = rb.rotation + attackRadius / 2f;
        hitboxInstance = Instantiate(hitbox);
        hitboxInstance.parent = gameObject;
        hitboxInstance.transform.position = transform.position;
        hitboxInstance.transform.eulerAngles = new Vector3(0, 0, rotation);
        hitboxInstance.attackDamage = attackDamage;
        hitboxInstance.isEnemy = true;

        //Rotate attack hitbox
        while (timer < attackDuration)
        {
            timer += Time.deltaTime;
            float z = Mathf.Lerp(rotation, finalRotation, timer / attackDuration);
            hitboxInstance.transform.eulerAngles = new Vector3(0, 0, z);
            yield return null;
        }
        Destroy(hitboxInstance.gameObject);

        //Rotate back around
        timer = 0f;
        while(timer < attackRecovery)
        {
            timer += Time.deltaTime;
            rb.MoveRotation(Mathf.Lerp(rb.rotation, Mathf.Rad2Deg * Vector2Angle(FlipAroundSelf(player)), timer / attackRecovery));
            yield return null;
        }
        attacking = false;
    }

    //Damage Enemies on collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Defaults.player.OnHit(attackDamage);
        }
    }
}
