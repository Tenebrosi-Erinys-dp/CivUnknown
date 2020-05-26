using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : EntityController
{
    [SerializeField]
    Slider slider;
    public AudioSource As;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        pc = this;
        Defaults.player = this;
        As = GetComponent<AudioSource>();
       
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
        As.Play();
    }

    public override void OnHit(int damage)
    {
        Debug.Log("Ow that hurt me for " + damage + " damage!");
        base.OnHit(damage);
        slider.value = GetHealthPercent();
    }

    public override void Die()
    {
        //Game Over

    }
}