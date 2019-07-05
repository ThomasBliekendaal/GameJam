﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool greybox;
    public List<Button> mainMenu = new List<Button>();
    public List<Button> start = new List<Button>();
    public List<Button> pause = new List<Button>();
    public List<Button> options = new List<Button>();
    public List<Button> optionsClose = new List<Button>();
    public List<Button> closeGame = new List<Button>();

    public AudioClip buttonPress;
    public AudioSource source;
    public AudioClip menuMusic;

    public List<PlayerUnit> units = new List<PlayerUnit>();
    public List<GameObject> upgrades = new List<GameObject>();
    public Text pointCounter;

    public GameObject optionsPanel;
    public GameObject pausePanel;

    public int points;
    public int maxPoints;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Points"))
        {
            maxPoints = PlayerPrefs.GetInt("Points");
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Melee"))
        {
            units.Add(g.GetComponent<PlayerUnit>());
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Ranged"))
        {
            units.Add(g.GetComponent<PlayerUnit>());
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Support"))
        {
            units.Add(g.GetComponent<PlayerUnit>());
        }
        foreach (Button b in mainMenu)
        {
            b.onClick.AddListener(delegate { LoadScene(0); });
        }
        foreach(Button b in start)
        {
            if (greybox == false)
            {
                b.onClick.AddListener(delegate { LoadScene(1); });
            }
            else
            {
                b.onClick.AddListener(delegate { LoadScene(6); });
            }
        }
        foreach(Button b in pause)
        {
            b.onClick.AddListener(delegate { TogglePause(); });
        }
        foreach (Button b in options)
        {
            b.onClick.AddListener(delegate { optionsPanel.SetActive(true); source.PlayOneShot(buttonPress); });
        }
        foreach (Button b in optionsClose)
        {
            b.onClick.AddListener(delegate { optionsPanel.SetActive(false); source.PlayOneShot(buttonPress); });
        }
        foreach (Button b in closeGame)
        {
            b.onClick.AddListener(delegate { source.PlayOneShot(buttonPress); Application.Quit(); });
        }
        if(menuMusic != null && SceneManager.GetActiveScene().buildIndex < 1)
        {
            source.PlayOneShot(menuMusic);
            source.loop = true;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            pointCounter.text = points.ToString();
            if (points > 0)
            {
                foreach (GameObject g in upgrades)
                {
                    g.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject g in upgrades)
                {
                    g.SetActive(false);
                    foreach (PlayerUnit p in units)
                    {
                        p.upgrades.SetActive(false);
                    }
                }
            }
            if (Input.GetButtonDown("Cancel"))
            {
                TogglePause();
            }
        }
    }

    private void LoadScene(int scene)
    {
        if (PlayerPrefs.HasKey("Points"))
        {
            PlayerPrefs.SetInt("Points", 0);
        }
        source.PlayOneShot(buttonPress);
        SceneManager.LoadScene(scene);
    }

    private void TogglePause()
    {
        source.PlayOneShot(buttonPress);
        if (pausePanel != null)
        {
            pausePanel.SetActive(!pausePanel.active);
            if (pausePanel.active == true)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}
