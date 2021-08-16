using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TipoDeChatarra : MonoBehaviour
{
    public ControladorBotonesClasificacion botonesAnalizador;
    public Image fondoTexto;
    public TextMeshProUGUI tipoChatarra;
    public int idBoton;

    private Color tempColorText;

    void Start()
    {
        tempColorText = fondoTexto.color;
        tempColorText.a = 0f;
        fondoTexto.color = tempColorText;
    }

    public void OnMouseOver()
    {
        tempColorText.a = 1f;
        fondoTexto.color = tempColorText;
        tipoChatarra.text = botonesAnalizador.GetTipoChatarra(idBoton);
    }

    public void OnMouseExit()
    {
        tempColorText.a = 0f;
        fondoTexto.color = tempColorText;
        tipoChatarra.text = "";
    }
}
