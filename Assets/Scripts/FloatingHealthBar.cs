using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingHealthBar : MonoBehaviour
{
    public EntityController entity;
    public GameObject bar;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        UpdateHP();
    }
    public virtual void UpdateHP()
    {
        if (entity == null) Destroy(gameObject);
        else transform.position = entity.transform.position + offset;
        float health = entity.GetHealthPercent();
        bar.transform.localScale = new Vector3(health, bar.transform.localScale.y, 1);
    }
}
