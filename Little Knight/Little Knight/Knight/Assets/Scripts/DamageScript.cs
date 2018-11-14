using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {

    public GameObject hitFX;
    public GameObject hitFXBlock;
    public float damage = 30;
    public GameObject GC;

    private void Start()
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (!other.GetComponent<Stats>().iframes)
            {
                Stats plStats = other.GetComponent<Stats>();

                if (!plStats.blocking)
                {
                    plStats.health -= damage;
                    hitFX = Instantiate(hitFX.gameObject, transform.position, transform.rotation);

                }
                else if (plStats.blocking)
                {
                    hitFXBlock = Instantiate(hitFXBlock.gameObject, transform.position, transform.rotation);
                    plStats.health -= damage / 2;
                    plStats.stamina -= damage * 2;
                    Animator anim = GetComponentInParent<Animator>();
                    anim.SetTrigger("HitShield");
                }


                plStats.iframes = true;
                other.GetComponent<PlayerController>().canMove = false;
                other.GetComponent<Animator>().SetTrigger("Hit");
            }

        }

        if (other.gameObject.CompareTag("ShieldPlayer"))
        {
            // Do Stuff
        }

    }
}
