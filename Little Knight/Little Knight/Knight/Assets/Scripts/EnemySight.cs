using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

    EnemyAI enAI;
    Stats stats;

    void Start ()
    {
        enAI = GetComponentInParent<EnemyAI>();
        stats = GetComponentInParent<Stats>();
    }

    private void Update()
    {
        if (stats.health <= 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enAI.curTarget == null)
        {
            if (other.GetComponent<PlayerController>())
            {
                enAI.curTarget = other.transform;
            }
        }
    }
}
