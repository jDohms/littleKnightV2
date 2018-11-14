using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    Stats plStats;

    public Text healthTxt;
    public Text StaminaTxt;

    public Slider healthBar;
    public Slider staminaBar;

    void Start ()
    {
        plStats = GetComponent<Stats>();
	}
	
	void FixedUpdate ()
    {
        healthBar.value = plStats.health;
        staminaBar.value = plStats.stamina;

        healthTxt.text = "" + plStats.health;
        StaminaTxt.text = "" + plStats.stamina;
	}
}
