using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControladorAnalizador : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject analizador;
    public AudioSource audio;

    private Image tabImage;
    private Animator animatorPanel;
    private Color tempColor;

    private void Start()
    {
        tabImage = GetComponent<Image>();
        animatorPanel = analizador.GetComponent<Animator>();
        tempColor = tabImage.color;
        animatorPanel.SetBool("MouseOn", false);
        animatorPanel.Play("Analizador Inactivo");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tempColor.a = 0f;
        tabImage.color = tempColor;
        animatorPanel.Play("Desplegar Analizador");
        animatorPanel.SetBool("MouseOn", true);
        audio.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tempColor.a = 1f;
        tabImage.color = tempColor;
        animatorPanel.SetBool("MouseOn", false);
        audio.Play();
    }
}