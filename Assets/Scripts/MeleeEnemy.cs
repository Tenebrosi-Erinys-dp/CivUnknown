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
        CooldownController();
    }

    private void FixedUpdate()
    {
        MovementController();
    }

    void MovementController()
    {
        float distance = DetectionCheck();
        if (detected)
        {
            if (!attacking && attackCD <= 0 && distance > attackRange)
            {
                //Go towards player, begin rotating
                //rb.MoveRotation(Mathf.Rad2Deg * Vector2Angle(player));
                rotator.transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector3)player - transform.position);
                MoveInDirection(player - (Vector2)transform.position);
            }
            else if (!attacking && attackCD <= 0 && distance <= attackRange)
            {
                //Attack player
                attackCD = maxAttackCD;
                StartCoroutine(MeleeAttack());
            
            }
            else if(!attacking && distance < runAwayRange)
            {
                //Run away and rotate away from player
                //rb.MoveRotation(Mathf.Rad2Deg * Vector2Angle(FlipAroundSelf(player)));
                rotator.transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector3)FlipAroundSelf(player) - transform.position);
                MoveInDirection(FlipAroundSelf(player) - (Vector2)transform.position);
            }
            else
            {
                //Do nothing
            }
        }
    }

    IEnumerator MeleeAttack()
    {
        //Generate attack hitbox
        attacking = true;
        float timer = 0f;
        float rotation = rotator.transform.eulerAngles.z + 90 - attackRadius / 2f;
        float finalRotation = rotator.transform.eulerAngles.z + 90 + attackRadius / 2f;
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
