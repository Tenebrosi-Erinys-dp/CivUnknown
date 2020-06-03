using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : EntityController
{
    public Slider healthSlider;
    public Slider chargeSlider;
    Animator myAnim;

    GameObject head;

    AudioSource walkAudio;
    AudioSource laserAudio;
    AudioSource hitAudio;
    AudioSource chargeUpAudio;
    AudioSource DingAudio;

    public AudioClip walk;
    public AudioClip laser;
    public AudioClip hit;
    public AudioClip chargeUp;
    public AudioClip ding;

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
    public float maxSpellCharge = 3.5f;

    public float spellRange = 10f;
    public float spellWidth = 2f;
    public float spellDuration = .5f;
    public float spellRecovery = .25f;

    public float currentSpeed;

    public float spellChargeSpeedMult = 0.8f;
    public float spellFireSpeedMult = 0.5f;

    public int spellDamage = 3;
    public float dingTimer = 4;

    int lastDirection;
    Animator anim;

    // Start is called before the first frame update
    new void Start()
    {
        myAnim = GetComponent<Animator>();
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
        hitAudio.volume = 0.7f;

        chargeUpAudio = gameObject.AddComponent<AudioSource>();
        chargeUpAudio.clip = chargeUp;
        chargeUpAudio.volume = 3;

        DingAudio = gameObject.AddComponent<AudioSource>();
        DingAudio.clip = ding;
        DingAudio.volume = 2;
        
        timer = timeToStep;
        currentSpeed = speed;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CooldownController();
        SpellController();
        AttackController();
    }

    private void FixedUpdate()
    {
        MovementController();
    }

    void AttackController()
    {
        if (Input.GetButton("Fire1") && !attacking && !charging)
        {
            StartCoroutine(MeleeAttack());
        }
    }

        //sample code. Have a conditional to call each animation like this
        //if(direction > Whatever value)
        //{
             //myAnim.SetInteger("State", transition number);
        //}
        //Note: transition number is the number I assigned for each animation in the controller

    protected override void CooldownController()
    {
        timeSinceLastInvinc += Time.deltaTime;
        base.CooldownController();
        if (spellCD > 0)
        {
            spellCD -= Time.deltaTime;
        }
        if (Mathf.Approximately(spellCurrentCharge, 0))
        {
            chargeSlider.value = 1 - spellCD / maxSpellCD;
        }
    }

    void SpellController()
    {
        if(spellCD <= 0 && Input.GetButton("Fire2") && !attacking)
        {
            if (!charging)
            {
                charging = true;
                Debug.Log("Starting to play");
                chargeUpAudio.Play();
                dingTimer -= 2;
            }

            else if((charging) && (!chargeUpAudio.isPlaying) && (!laserAudio.isPlaying) && (dingTimer == 0))
            {
                DingAudio.Play();
                Debug.Log("Playing");
            }

            else
            {
                dingTimer = 2;
            }

            currentSpeed = speed * spellChargeSpeedMult;
            Debug.Log(spellCurrentCharge);
            if (spellCurrentCharge >= maxSpellCharge)
            {
                //Fire strongest spell
                Debug.Log("Fire");
                StartCoroutine(LaserSpell());
                spellCD = maxSpellCD;
                spellCurrentCharge = 0;
            }
            spellCurrentCharge += Time.deltaTime;
        }
        else
        {
            spellCurrentCharge = 0;
            currentSpeed = speed;
            chargeUpAudio.Stop();
            charging = false;
        }
        if (!Mathf.Approximately(spellCurrentCharge, 0))
        {
            chargeSlider.value = 1 - spellCurrentCharge / maxSpellCharge;
        }
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

    void MovementController()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        MoveInDirection(new Vector2(x, y));
        //rotRB.MoveRotation(Mathf.Rad2Deg * Vector2Angle(MouseAsWorldPos()));
        head.transform.rotation = Quaternion.LookRotation(Vector3.forward, MouseAsWorldPos() - head.transform.position);

        //this is to play sound effect
        timer += Time.fixedDeltaTime;
        bool isMoving = !(Mathf.Approximately(x, 0) && Mathf.Approximately(y, 0));

        if (timer > timeToStep && isMoving)
        {
            walkAudio.Play();
            timer = 0;
        }

        int direction = Mathf.Abs(x) >= Mathf.Abs(y) && x > 0 ? 0 :
            Mathf.Abs(x) >= Mathf.Abs(y) && x < 0 ? 2 :
            Mathf.Abs(x) < Mathf.Abs(y) && y > 0 ? 1 :
            Mathf.Abs(x) < Mathf.Abs(y) && y < 0 ? 3 : -1;
        //Direction represents the unit circle: 0 is right, 1 is up, 2 is left, and 3 is down

        if(lastDirection != direction)
        {
            myAnim.SetInteger("Mummy", 1);
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
        hitboxInstance = Instantiate(meleeHitbox, transform);
        hitboxInstance.transform.position = transform.position;
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
            hitboxInstance.transform.position = transform.position;
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