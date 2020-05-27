using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : EntityController
{
    public Slider slider;
    public AudioSource Audio;
    public float timer;
    public float timeToStep = .6f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        pc = this;
        Defaults.player = this;
        Audio = GetComponent<AudioSource>();
        timer = timeToStep;
    }

    // Update is called once per frame
    void Update()
    {
        MovementController();
    }

    void MovementController()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        MoveInDirection(new Vector2(x, y));
        rb.MoveRotation(Mathf.Rad2Deg * Vector2Angle(MouseAsWorldPos()));

        //this is to play sound effect
        timer += Time.deltaTime;
        bool isMoving = !(Mathf.Approximately(x, 0) && Mathf.Approximately(y, 0));

        if (timer > timeToStep && isMoving)
        {
            Audio.Play();
            timer = 0;
        }
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