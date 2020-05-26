using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defaults : MonoBehaviour
{
    public static Defaults instance;
    public ProjectileController arrow;
    public EnemyController hunter;
    public static PlayerController player;
    public FloatingHealthBar health;
    public AudioSource As;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
}
