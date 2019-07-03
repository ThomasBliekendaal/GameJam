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

    public GameObject optionsPanel;

    private void Start()
    {
        foreach(Button b in mainMenu)
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
        
    }

    private void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
