using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defaults : MonoBehaviour
{
    public static Defaults instance;
    public ProjectileController arrow;
    public EnemyController hunter;
    public static PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
}
