using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorAudio : MonoBehaviour
{
    public AudioSource source;
    public Button botonMute;
    public Sprite sonido;
    public Sprite mute;

    private bool isMute;

    void Start()
    {
        isMute = false;
        botonMute.onClick.AddListener(Mute);
    }

    private void Mute()
    {
        if(!isMute)
        {
            source.mute = true;
            botonMute.image.sprite = mute;
            isMute = true;
        }
        else
        {
            source.mute = false;
            botonMute.image.sprite = sonido;
            isMute = false;
        }
    }
}
