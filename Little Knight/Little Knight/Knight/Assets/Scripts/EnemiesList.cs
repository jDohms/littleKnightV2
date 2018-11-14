using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesList : MonoBehaviour {

    Stats enStats;
    EnemyAI enAI;

	void Start ()
    {
        enStats = GetComponentInParent<Stats>();
        enAI = GetComponentInParent<EnemyAI>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Stats>() && !other.GetComponent<PlayerController>())
        {
            Stats stats = other.GetComponent<Stats>();
            if (stats.transform != enAI.transform && !stats.dead)
            {
                if (!enAI.Enemies.Contains(stats.transform))
                {
                    enAI.Enemies.Add(stats.transform);
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Stats>())
        {
            Stats stats = other.GetComponent<Stats>();
        }


    }


    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Stats>())
        {
            if (enAI.Enemies.Contains(other.transform))
            {
                enAI.Enemies.Remove(other.transform);
            }
        }
    }
}
