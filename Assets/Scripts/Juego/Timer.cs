using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public Image tabImageClasificador;
    public Image tabImageAnalizador;
    public List<GameObject> botones;
    public GameObject pantallaTablet;
    public Material pantallaApagada;
    public Material pantallaEncendida;

    private float currentTime;
    private float startingTime = 5f;

    void Start()
    {
        /*foreach (GameObject b in botones)
        {
            b.SetActive(false);
        }*/

        pantallaTablet.GetComponent<MeshRenderer>().material = pantallaApagada;
        tabImageClasificador.enabled = false;
        tabImageAnalizador.enabled = false;
        countdownText.enabled = true;
        currentTime = startingTime;
    }
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;

        countdownText.text = currentTime.ToString("0");

        if (currentTime <= 0.5)
        {
            countdownText.text = "GO!";
            
        }

        if (currentTime <= 0)
        {
            /*foreach (GameObject b in botones)
            {
                b.SetActive(true);
            }*/

            pantallaTablet.GetComponent<MeshRenderer>().material = pantallaEncendida;
            tabImageClasificador.enabled = true;
            tabImageAnalizador.enabled = true;
            countdownText.enabled = false;
        }
    }
}