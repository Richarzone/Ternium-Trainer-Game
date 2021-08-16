using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorPantallaAnalisis : MonoBehaviour
{
    public Camera camara;
    public Button botonCerrar;
    public Animator animatorCamara;
    public GameObject tabs;
    public RawImage pantalla;
    public Texture pleaseWait;

    private Vector3 posicionInicialCamara;
    private Texture fotografiaActual;
    private bool cerrar;

    // Start is called before the first frame update
    private void Start()
    {
        posicionInicialCamara = camara.transform.position;
        botonCerrar.onClick.AddListener(CloseAnalyser);
    }

    private void CloseAnalyser()
    {
        cerrar = true;
    }

    public void CorrutineReceiver()
    {
        StartCoroutine(AnalysisButtonPressed());
    }

    private IEnumerator AnalysisButtonPressed()
    {
        fotografiaActual = pantalla.texture;
        pantalla.texture = pleaseWait;

        animatorCamara.SetBool("isAnalizing", true);

        yield return new WaitForSeconds(1);

        animatorCamara.enabled = false;
        camara.transform.position = new Vector3(0, 7.6f, 0.45f);

        yield return WaitForButtonPress();

        camara.transform.position = posicionInicialCamara;
        animatorCamara.enabled = true;
        animatorCamara.SetBool("isAnalizing", false);

        yield return new WaitForSeconds(1);

        pantalla.texture = fotografiaActual;

        cerrar = false;
    }

    private IEnumerator WaitForButtonPress()
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (cerrar)
            {
                done = true; // breaks the loop
            }
            yield return null;
        }
    }

    //TO DO
    // - Get type of material
}
