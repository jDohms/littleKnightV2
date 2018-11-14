using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTargets : MonoBehaviour {

    Stats plStats;
    PlayerController plControl;

	void Start ()
    {
        plStats = GetComponentInParent<Stats>();
        plControl = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Stats>())
        {
            Stats stats = other.GetComponent<Stats>();
            if (stats.transform != plControl.transform && !stats.dead)
            {
                if (!plControl.Enemies.Contains(stats.transform))
                {
                    plControl.Enemies.Add(stats.transform);
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Stats>())
        {
            Stats stats = other.GetComponent<Stats>();
            if (stats.dead)
            {
                if (!plControl.Enemies.Contains(stats.transform))
                {
                    plControl.lockTarget = false;
                    plControl.Enemies.Remove(other.transform);
                    plControl.cameraTarget.position = Vector3.Lerp(plControl.cameraTarget.position, plControl.targetPos,
                     Time.deltaTime * plControl.camTargetSpeed);

                }
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Stats>())
        {
            if (plControl.Enemies.Contains(other.transform))
            {
                plControl.Enemies.Remove(other.transform);
            }
        }
    }
}
