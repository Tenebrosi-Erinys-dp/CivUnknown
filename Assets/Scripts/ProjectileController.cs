using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Hitbox
{
    public Vector2 startPosition;

    // Start is called before the first frame update
    new protected void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        MovementController();
    }

    protected void MovementController()
    {
        if (Vector2.Distance(transform.position, startPosition) > maxRange)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnemy && other.gameObject.CompareTag("Player") || !isEnemy && other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EntityController>().OnHit(attackDamage);
            Destroy(gameObject);
        }
    }
}
