using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
    public GameObject credits;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    public void PlayGame ()
	{
		SceneManager.LoadScene ("Main");
	}

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Credits()
    {
        credits.SetActive(true);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
    }


    public void QuitGame()
	{
		Debug.Log ("Quit");
		Application.Quit ();
	}
}
