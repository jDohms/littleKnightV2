using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public GameObject winner;
    public GameObject gameOver;
    public GameObject lore;
    public GameObject tutorialBasic;
    public GameObject tutorialBars;
    public GameObject tutorialAttack;
    public GameObject tutorialHeal;
    public bool skip;


    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    public PlayerController Player;



    public void IvokeGameOver()
    {
        gameOver.SetActive(true);
        Invoke("GameOver", 5);
    }

    public void IvokeWinner()
    {
        winner.SetActive(true);
        Invoke("Winner", 5);
    }

    public void CloseLore()
    {
        lore.SetActive(false);
        if (!skip)
        {
            tutorialBasic.SetActive(true);
        }
    }

    public void CloseTutorial1()
    {
        tutorialBasic.SetActive(false);
        if (!skip)
        {
            tutorialBars.SetActive(true);
        }
    }

    public void CloseTutorial2()
    {
        tutorialBars.SetActive(false);
        if (!skip)
        { 
            tutorialAttack.SetActive(true);
    }
    }

    public void CloseTutorial3()
    {
        tutorialAttack.SetActive(false);
        if (!skip)
        {
            tutorialHeal.SetActive(true);
        }
    }

    public void CloseTutorial4()
    {
        tutorialHeal.SetActive(false);
    }


    public void GameOver()
    {
        Time.timeScale = 0f;

        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
        skip = true;
    }

    public void YoureWinner()
    {
        Time.timeScale = 0f;
    }
}
