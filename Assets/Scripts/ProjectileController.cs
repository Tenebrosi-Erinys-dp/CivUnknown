using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : EnemyController
{
    Vector2 startPosition;
    public float maxRange = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        startPosition = transform.position;
        speed = 10f;
        rb = GetComponent<Rigidbody2D>();
        attackDamage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        MovementController();
    }

    void MovementController()
    {
        if (Vector2.Distance(transform.position, startPosition) > maxRange)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Defaults.player.OnHit(attackDamage);
            Destroy(gameObject);
        }
    }
}
