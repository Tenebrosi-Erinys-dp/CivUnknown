using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController
{
    public Hitbox hitbox;
    Hitbox hitboxInstance;
    public float attackRange = 2f;
    public float attackRadius = 45;
    public float attackWindup = 0.5f;
    public float attackDuration = 1f;
    public float attackRecovery = 0.5f;
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
                attacking = true;
                StartCoroutine(MeleeAttack());
            }
            else if (attacking)
            {
                //Do nothing else
            }
            //Otherwise, run away
            else
            {
                Vector2 direction = FlipAroundSelf(player);
                direction -= (Vector2)transform.position;


                //ADD BACK AFTER TESTING
                //MoveInDirection(direction);
            }
        }
    }

    IEnumerator MeleeAttack()
    {
        //Rotate towards player
        float timer = 0f;
        while(timer < attackWindup)
        {
            timer += Time.deltaTime;
            rb.MoveRotation(Mathf.Lerp(rb.rotation, Mathf.Rad2Deg * Vector2Angle(player), timer / attackWindup));
            yield return null;
        }

        //Generate attack hitbox
        timer = 0f;
        float rotation = rb.rotation - attackRadius / 2f;
        float finalRotation = rb.rotation + attackRadius / 2f;
        hitboxInstance = Instantiate(hitbox);
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
            Destroy(gameObject);
        }
    }
}
