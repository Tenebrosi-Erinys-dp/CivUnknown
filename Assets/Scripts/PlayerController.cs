using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = this;
        Defaults.player = this;
    }

    // Update is called once per frame
    void Update()
    {
        MovementController();
    }

    void MovementController()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rb.MovePosition((Vector2)transform.position + new Vector2(x, y).normalized * speed * Time.deltaTime);
        rb.MoveRotation(Mathf.Rad2Deg * Vector2Angle(MouseAsWorldPos()));
    }

    public override void OnHit(int damage)
    {
        Debug.Log("Ow that hurt me for " + damage + " damage!");
    }
}