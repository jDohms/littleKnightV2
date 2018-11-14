using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour {

    Stats bossStats;

    public Slider healthBar;

    void Start ()
    {
        bossStats = GetComponent<Stats>();
	}
	
	void FixedUpdate ()
    {
        healthBar.value = bossStats.health;
	}
}
