using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;

public class ControladorPanelClasificacion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;
    public AudioSource audio;

    private Image tabImage;
    private Animator animatorPanel;
    private Color tempColor;

    void Start()
    {
        tabImage = GetComponent<Image>();
        animatorPanel = panel.GetComponent<Animator>();
        tempColor = tabImage.color;

        animatorPanel.SetBool("MouseOn", false);
        animatorPanel.Play("Panel Inactivo");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tempColor.a = 0f;
        tabImage.color = tempColor;

        animatorPanel.Play("Desplegar Panel");
        animatorPanel.SetBool("MouseOn", true);
        audio.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {   
        tempColor.a = 1f;
        tabImage.color = tempColor;


        try
        {
            animatorPanel.SetBool("MouseOn",false);
            audio.Play();
        }
        catch (Exception e)
        {
            //Algo salio mal
            Debug.Log(e);
        }
    }
}
