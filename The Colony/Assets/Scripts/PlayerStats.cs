using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float Health = 100f;
    public float Hunger = 100f;
    public float HungerRate = 0.5f;

    private PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        InvokeRepeating("DecreaseHunger", 5f, 5f);
    }

    public void Damage(float _damage)
    {
        Health -= _damage;
        if (Health < 0 || Health == 0)
        {
            Health = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("You died :'(");
    }

    public void Heal(float _extraHealth)
    {
        Health += _extraHealth;
        if (Health > 100)
        {
            Health = 100f;
        }
    }

    private void DecreaseHunger()
    {
        Hunger -= HungerRate;
        if (Hunger < 0 || Hunger == 0)
        {
            Hunger = 0;
            Damage(5f);
        }
        /*if (Hunger < 80f)
        {
            controller.walkSpeed = 2f;
        }*/
    }
}
