﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityController : MonoBehaviour
{
    protected Rigidbody2D rb;
    public float speed = 5f;
    protected static PlayerController pc;
    public int attackDamage = 1;

    //Can fire once every 2 seconds
    public float maxAttackCD = 2f;
    protected float attackCD = 0f;

    public float invincibilityTime = 0.25f;
    public float timeSinceLastInvinc = 0f;

    [SerializeField]
    protected int maxHp = 10;

    [SerializeField]
    protected int currentHp;

    protected int lastDirection;
    protected Animator myAnim;

    public FloatingHealthBar healthBar;

    protected string self;

    // Start is called before the first frame update
    protected void Start()
    {
        currentHp = maxHp;
        myAnim = GetComponent<Animator>();
    }

    protected virtual void CooldownController()
    {
        if (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }
        timeSinceLastInvinc += Time.deltaTime;
    }

    protected Vector3 MouseAsWorldPos()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = 0;
        return point;
    }

    protected float Vector2Angle(Vector2 v)
    {
        return Mathf.Atan2(v.y - transform.position.y, v.x - transform.position.x);
    }

    protected virtual void MoveInDirection(Vector2 pos)
    {
        float x = pos.x;
        float y = pos.y;
        int direction = Mathf.Abs(x) >= Mathf.Abs(y) && x > 0 ? 0 :
            Mathf.Abs(x) >= Mathf.Abs(y) && x < 0 ? 2 :
            Mathf.Abs(x) < Mathf.Abs(y) && y > 0 ? 1 :
            Mathf.Abs(x) < Mathf.Abs(y) && y < 0 ? 3 : -1;
        //Direction represents the unit circle: 0 is right, 1 is up, 2 is left, and 3 is down

        if (lastDirection != direction)
        {
            lastDirection = direction;
            myAnim.SetInteger(self, direction);
        }

        Vector2 mPos = pos.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition((Vector2)transform.position + mPos);
    }

    protected Vector2 FlipAroundSelf(Vector2 pos)
    {
        pos -= (Vector2)transform.position;
        pos *= -1;
        pos += (Vector2)transform.position;
        return pos;
    }

    public virtual void OnHit(int damage)
    {
        if (timeSinceLastInvinc > invincibilityTime)
        {

            currentHp -= damage;

            if (currentHp <= 0)
            {
                Die();
            }
            timeSinceLastInvinc = 0;
        }
    }

    protected virtual void Die()
    {

    }

    public float GetHealthPercent()
    {
        return (float)currentHp / maxHp;
    }
}
