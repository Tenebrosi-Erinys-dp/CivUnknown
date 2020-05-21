using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityController : MonoBehaviour
{

    protected Rigidbody2D rb;
    public float speed = 5f;
    protected static PlayerController pc;
    public int attackDamage;

    public int maxHp;
    public int currentHp;

    public GameObject healthBar;

    //This is for a health bar that exists under the character. Leave null if the health bar is part of UI
    public Transform child;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] children = GetComponentsInChildren<GameObject>();
        foreach(GameObject child in children)
        {
            if(child.CompareTag("Rotation Fixer"))
            {
                this.child = child.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(child != null) child.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
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

    protected Vector2 FlipAroundSelf(Vector2 pos)
    {
        pos -= (Vector2)transform.position;
        pos *= -1;
        pos += (Vector2)transform.position;
        return pos;
    }

    public virtual void OnHit(int damage)
    {
        currentHp -= damage;
        if(healthBar == null)
        {
            GameObject[] children = GetComponentsInChildren<GameObject>();
            foreach (GameObject child in children)
            {
                if(child.CompareTag("Health Bar"))
                {
                    healthBar = child;
                }
            }
        }
        healthBar.transform.localScale = new Vector3(maxHp / (float)currentHp, transform.localScale.y, 1);

        if(currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {

    }
}
