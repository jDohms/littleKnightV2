using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public GameObject GC;
    public GameObject healFX;
    public float heal = 100;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            Stats plStats = other.GetComponent<Stats>();
            plStats.health += heal;
            GC.SendMessage("PlayHeal");
            healFX = Instantiate(healFX.gameObject, transform.position, transform.rotation);
        }
    }
}
