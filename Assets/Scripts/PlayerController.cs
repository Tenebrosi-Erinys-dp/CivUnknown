﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : EntityController
{
    [SerializeField]
    Slider slider;
    public AudioSource Audio;
    public float timer = 0.5f;
    public float TimeToStep = 6f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        pc = this;
        Defaults.player = this;
        Audio = GetComponent<AudioSource>();
        timer = TimeToStep;
       
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

        //this is to play sound effect
        timer += Time.deltaTime;
        if(timer > TimeToStep)
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