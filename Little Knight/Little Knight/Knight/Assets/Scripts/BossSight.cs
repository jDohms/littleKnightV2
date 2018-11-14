using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSight : MonoBehaviour {

    BossAI bossAI;
    public GameObject BossUI;
    public GameObject GC;


    void Start ()
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        bossAI = GetComponentInParent<BossAI>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (bossAI.curTarget == null)
        {
            if (other.GetComponent<PlayerController>())
            {
                BossUI.SetActive(true);
                bossAI.curTarget = other.transform;
                GC.SendMessage("PlayBossMusic");
            }
        }
    }
}
