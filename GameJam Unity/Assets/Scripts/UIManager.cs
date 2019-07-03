using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public List<Button> mainMenu = new List<Button>();
    public List<Button> start = new List<Button>();
    public List<Button> options = new List<Button>();
    public List<Button> optionsClose = new List<Button>();
    public List<Button> closeGame = new List<Button>();

    public List<PlayerUnit> units = new List<PlayerUnit>();
    public List<GameObject> upgrades = new List<GameObject>();
    public Text pointCounter;

    public GameObject optionsPanel;
    public GameObject pausePanel;

    public int points;

    private void Start()
    {
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
            b.onClick.AddListener(delegate { LoadScene(1); });
        }
        foreach (Button b in options)
        {
            b.onClick.AddListener(delegate { optionsPanel.SetActive(true); });
        }
        foreach (Button b in optionsClose)
        {
            b.onClick.AddListener(delegate { optionsPanel.SetActive(false); });
        }
        foreach (Button b in closeGame)
        {
            b.onClick.AddListener(delegate { Application.Quit(); });
        }
    }

    private void Update()
    {
        pointCounter.text = points.ToString();
        if (points > 0)
        {
            foreach(GameObject g in upgrades)
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
        if(pausePanel != null && Input.GetButtonDown("Cancel"))
        {
            pausePanel.SetActive(!pausePanel.active);
            if(pausePanel.active == true)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    private void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
