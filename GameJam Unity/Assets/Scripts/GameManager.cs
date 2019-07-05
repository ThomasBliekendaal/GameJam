using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject victory;
    public GameObject gameOver;
    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip levelMusic;

    public List<GameObject> endGoals = new List<GameObject>();
    public List<GameObject> meleeUnits = new List<GameObject>();
    public List<GameObject> rangedUnits = new List<GameObject>();
    public List<GameObject> supportUnits = new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();
    private int playerUnitCount;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Melee"))
        {
            meleeUnits.Add(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Ranged"))
        {
            rangedUnits.Add(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Support"))
        {
            supportUnits.Add(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyUnits.Add(g);
        }

        if(SceneManager.GetActiveScene().buildIndex < 1)
        {
            audioSource.PlayOneShot(menuMusic);
            audioSource.loop = true;
        }
        else
        {
            audioSource.PlayOneShot(levelMusic);
            audioSource.loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerUnitCount = meleeUnits.Count + rangedUnits.Count + supportUnits.Count;
        if(playerUnitCount <= 0)
        {
            gameOver.SetActive(true);
        }
        else if (enemyUnits.Count <= 0)
        {
            if (SceneManager.GetActiveScene().buildIndex < 5)
            {
                if (PlayerPrefs.HasKey("Points"))
                {
                    PlayerPrefs.SetInt("Points", gameObject.GetComponent<UIManager>().maxPoints);
                }
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                victory.SetActive(true);
            }
        }
    }
}
