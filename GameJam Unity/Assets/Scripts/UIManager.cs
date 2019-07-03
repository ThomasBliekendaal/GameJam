using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public List<Button> mainMenu = new List<Button>();

    private void Start()
    {
        foreach(Button b in mainMenu)
        {
            b.onClick.AddListener(delegate { LoadScene(0); });
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
