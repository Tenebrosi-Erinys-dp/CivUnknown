using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : EntityController
{
    public Slider healthSlider;
    public Slider cdSlider;
    public Slider chargeSlider;

    GameObject head;

    AudioSource walkAudio;
    AudioSource laserAudio;
    AudioSource hitAudio;

    public AudioClip walk;
    public AudioClip laser;
    public AudioClip hit;

    public float timer;
    public float timeToStep = .6f;
    public Hitbox meleeHitbox;
    public GameObject rangedHitbox;
    Hitbox hitboxInstance;
    public float attackRange = 2f;
    public float attackRadius = 45;
    public float attackDuration = 1f;
    bool attacking = false;
    bool charging = false;

    public float maxSpellCD = 5f;
    public float spellCD = 0;
    public float spellCurrentCharge = 0;
    public float maxSpellCharge = 3;

    public float spellRange = 10f;
    public float spellWidth = 2f;
    public float spellDuration = .5f;
    public float spellRecovery = .25f;

    public float currentSpeed;

    public float spellChargeSpeedMult = 0.8f;
    public float spellFireSpeedMult = 0.5f;

    public int spellDamage = 3;

    // Start is called before the first frame update
    new void Start()
    {
        head = GameObject.Find("HeadObject");
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        pc = this;
        Defaults.player = this;

        walkAudio = gameObject.AddComponent<AudioSource>();
        walkAudio.clip = walk;
        walkAudio.volume = 0.5f;

        laserAudio = gameObject.AddComponent<AudioSource>();
        laserAudio.clip = laser;
        laserAudio.volume = 1;

        hitAudio = gameObject.AddComponent<AudioSource>();
        hitAudio.clip = hit;
        hitAudio.volume = 1;


        timer = timeToStep;
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        MovementController();
        CooldownController();
        SpellController();
        AttackController();
    }

    void AttackController()
    {
        if (Input.GetButton("Fire1") && !attacking && !charging)
        {
            StartCoroutine(MeleeAttack());
        }
    }

    protected override void CooldownController()
    {
        timeSinceLastInvinc += Time.deltaTime;
        base.CooldownController();
        if (spellCD > 0)
        {
            spellCD -= Time.deltaTime;
        }
        cdSlider.value = 1 - (spellCD / maxSpellCD);
    }

    void SpellController()
    {
        if(spellCD <= 0 && Input.GetButton("Fire2") && !attacking)
        {
            currentSpeed = speed * spellChargeSpeedMult;
            spellCurrentCharge += Time.deltaTime;
            if(spellCurrentCharge >= maxSpellCharge)
            {
                //Fire strongest spell
                Debug.Log("Fire");
                StartCoroutine(LaserSpell());
                spellCD = maxSpellCD;
                spellCurrentCharge = 0;
            }
        }
        else
        {
            spellCurrentCharge = 0;
            currentSpeed = speed;
        }
        chargeSlider.value = spellCurrentCharge / maxSpellCharge;
    }

    IEnumerator LaserSpell()
    {
        float timer = 0;
        currentSpeed = speed * spellFireSpeedMult;
        hitboxInstance = Instantiate(rangedHitbox, head.transform.position, head.transform.rotation).GetComponent<Hitbox>();
        hitboxInstance.parent = head;
        hitboxInstance.transform.eulerAngles = new Vector3(0, 0, hitboxInstance.transform.eulerAngles.z + 90);
        hitboxInstance.attackDamage = spellDamage;
        laserAudio.Play();
        while(timer < spellDuration)
        {
            timer += Time.deltaTime;
            hitboxInstance.transform.localScale = new Vector3(spellRange, Mathf.Lerp(0, spellWidth, timer / spellDuration), 1);
            hitboxInstance.transform.rotation = head.transform.rotation;
            hitboxInstance.transform.eulerAngles = new Vector3(0, 0, hitboxInstance.transform.eulerAngles.z + 90);
            hitboxInstance.transform.position = head.transform.position;
            yield return null;
        }
        Destroy(hitboxInstance.gameObject);
        yield return new WaitForSeconds(spellRecovery);
        currentSpeed = speed;
    }

    protected override void MoveInDirection(Vector2 pos)
    {
        Vector2 mPos = pos.normalized * currentSpeed * Time.deltaTime;
        rb.MovePosition((Vector2)transform.position + mPos);
    }

    void MovementController()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        MoveInDirection(new Vector2(x, y));
        //rotRB.MoveRotation(Mathf.Rad2Deg * Vector2Angle(MouseAsWorldPos()));
        head.transform.rotation = Quaternion.LookRotation(Vector3.forward, MouseAsWorldPos() - head.transform.position);

        //this is to play sound effect
        timer += Time.deltaTime;
        bool isMoving = !(Mathf.Approximately(x, 0) && Mathf.Approximately(y, 0));

        if (timer > timeToStep && isMoving)
        {
            walkAudio.Play();
            timer = 0;
        }
    }

    public override void OnHit(int damage)
    {
        Debug.Log("Ow that hurt me for " + damage + " damage!");
        base.OnHit(damage);
        timeSinceLastInvinc = 0;
        healthSlider.value = GetHealthPercent();
        hitAudio.Play();
    }

    IEnumerator MeleeAttack()
    {
        attacking = true;
        //Generate attack hitbox
        float timer = 0f;
        float rotation = head.transform.eulerAngles.z - attackRadius / 2f + 90;
        float finalRotation = head.transform.eulerAngles.z + attackRadius / 2f + 90;
        hitboxInstance = Instantiate(meleeHitbox, head.transform);
        hitboxInstance.parent = head;
        hitboxInstance.attackDamage = attackDamage;
        hitboxInstance.isEnemy = false;
        //Rotate attack hitbox
        while (timer < attackDuration)
        {
            Debug.Log(timer);
            timer += Time.deltaTime;
            float z = Mathf.Lerp(rotation, finalRotation, timer / attackDuration);
            hitboxInstance.transform.eulerAngles = new Vector3(0, 0, z);
            yield return null;
        }
        Destroy(hitboxInstance.gameObject);
        attacking = false;
    }

    protected override void Die()
    {
        //Game Over
        //Debug.Break();
        GameManager.instance.GameOver();
    }
}