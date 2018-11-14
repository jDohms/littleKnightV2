using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScriptPlayer : MonoBehaviour
{

    public GameObject hitFX;
    public float damage = 30;
    AttackControl attCtrl;

    void Start()
    {
        attCtrl = GetComponentInParent<AttackControl>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (attCtrl.stab == true)
        {
            if (other.GetComponentInParent<Stats>())
            {
                if (!other.GetComponentInParent<Stats>().dead)
                {
                    Stats enStats = other.GetComponentInParent<Stats>();
                    EnemyAI eAI = other.GetComponentInParent<EnemyAI>();

                    eAI.agent.isStopped = true;


                    enStats.health -= damage * 2;
                    enStats.iframes = true;

                    other.GetComponentInParent<Animator>().SetTrigger("Stab");

                    Instantiate(hitFX.gameObject, hitFX.transform.position, transform.rotation);
                }
            }
        }
        else
        if (other.GetComponent<Stats>())
        {
            if (!other.GetComponent<Stats>().iframes)
            {

                Stats enStats = other.GetComponent<Stats>();
                EnemyAI eAI = other.GetComponentInParent<EnemyAI>();

                if (!enStats.blocking)
                    enStats.health -= damage;

                enStats.iframes = true;

                other.GetComponent<Animator>().SetTrigger("Hit");
                Instantiate(hitFX.gameObject, transform.position, transform.rotation);
            }
        }
    }
}
