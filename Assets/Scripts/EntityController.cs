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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    }
}
