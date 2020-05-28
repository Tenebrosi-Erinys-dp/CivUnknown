using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{
    protected Rigidbody2D rb;
    public float speed;
    public int attackDamage;
    public float maxRange;
    public bool isEnemy;
    public GameObject parent;

    // Start is called before the first frame update
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (parent == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = parent.transform.position;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnemy && other.gameObject.CompareTag("Player") || !isEnemy && other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EntityController>().OnHit(attackDamage);
        }
    }
}
