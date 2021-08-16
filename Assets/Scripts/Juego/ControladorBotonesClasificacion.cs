using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorBotonesClasificacion : MonoBehaviour
{
    public List<BotonClasificacion> botones;
    public ControladorPanelCamara panelCamara;
    public AudioSource audio;

    private List<string> tiposChatarra = new List<string>();
    private List<Animator> animatorList = new List<Animator>();
    private int btnListIndex = 0;
    private string btnName;

    private void Start()
    {
        //Revisar valores NULL dentro de la lista botones
        foreach (BotonClasificacion b in botones)
        {
            if (b == null)
            {
                //Si hay valores NULL el videojuego no hara nada
                Debug.LogWarning("There are NULL elements in: botones");
                return;
            }
            else
            {
                tiposChatarra.Add(botones[btnListIndex].type);
                btnListIndex++;
            }
            
        }

        for (int i = 0; i < botones.Count; i++)
        {
            animatorList.Add(botones[i].boton.GetComponent<Animator>());
        }
    }

    //Se busca constantemente el input del usuario y se extrae una referencia del boton presionado por el usuario
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Boton Clasificacion")
            {
                //Cambiar a solo objetos dentro de capa de interactuables de clasificacion
                btnName = hit.collider.transform.parent.name;
                ButtonUsed(btnName);
            }
        }
    }

    //ButtonUsed: Busca que boton fue presionado de la lista de botones y corre la animacion de boton presionado en dicho boton. Ademas llama a la funcion ClassificationButtonPressed
    private void ButtonUsed(string btnName)
    {
        for (int i = 0; i < botones.Count; i++)
        {
            if (botones[i].boton.name == btnName)
            {
                Debug.Log("Nombre de boton: " + btnName);
                Debug.Log("Tipo de chatarra (boton precionado): " + botones[i].type);
                animatorList[i].SetTrigger("Presionado");
                panelCamara.CorrutineReceiver(ref botones[i].type);
                audio.Play();
            }
        }
    }

    public string GetTipoChatarra(int id)
    {
        return tiposChatarra[id];
    }
}