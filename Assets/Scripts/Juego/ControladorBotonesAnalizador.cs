using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorBotonesAnalizador : MonoBehaviour
{
    public GameObject botonAnalisis;
    public GameObject botonNegacion;
    public AudioSource audio;
    public ControladorPantallaAnalisis analizador;

    private Animator animatorAnalisis;
    private Animator animatorNegacion;
    private string btnName;

    private void Start()
    {
        if (botonAnalisis == null)
        {
            //Si no hay un objeto asignado a botonAnalisis el videojuego no hara nada
            Debug.LogWarning("There is no object assigned to: botonAnalisis");
            return;
        }

        if (botonNegacion == null)
        {
            //Si no hay un objeto asignado a botonNegacion el videojuego no hara nada
            Debug.LogWarning("There is no object assigned to: botonNegacion");
            return;
        }

        animatorAnalisis = botonAnalisis.GetComponent<Animator>();
        animatorNegacion = botonNegacion.GetComponent<Animator>();
    }

    //Se busca constantemente el input del usuario y se extrae una referencia del boton presionado por el usuario
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Boton Analizador")
            {
                //Cambiar a solo objetos dentro de capa de interactuables de clasificacion
                btnName = hit.collider.transform.parent.name;
                Debug.Log(btnName);
                Debug.Log(hit.collider.gameObject.tag);
                ButtonUsed(btnName);
            }
        }
    }

    //CREAR FUNCION DE ANALISIS CON MODELOS 3D
    private void ButtonUsed(string btnName)
    {
        if (btnName == "Boton de Analisis P")
        {
            animatorAnalisis.SetTrigger("Presionado");
            analizador.CorrutineReceiver();
            audio.Play();
        }
        else if (btnName == "Boton de Negacion P")
        {
            animatorNegacion.SetTrigger("Presionado");
            audio.Play();
        }
    }
}
