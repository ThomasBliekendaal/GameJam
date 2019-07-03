using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUp : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public string text;
    private GameObject parent;
    private GameObject cam;

    private void Awake()
    {
        parent = GameObject.FindGameObjectWithTag("WorldUI");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        transform.SetParent(parent.transform);
    }

    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = text;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform);

        transform.Translate(new Vector3(0, 1 * speed, 0));

        lifeTime -= Time.deltaTime;
        if(lifeTime<= 0)
        {
            Destroy(gameObject);
        }
    }
}
