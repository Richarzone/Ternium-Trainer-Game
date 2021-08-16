using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

//using System.Runtime.InteropServices;
//using MySql.Data.MySqlClient;

public class ControladorPanelCamara : MonoBehaviour
{
    public FetchData datos = new FetchData();
    public DataPublisher dataPublisher;
    public List<BotonCamara> botones;
    public RawImage pantalla;
    public Texture pleaseWait;
    public TextMeshProUGUI intentos;
    public AudioSource audio;
    public AudioClip correcto;
    public AudioClip incorrecto;

    private List<FetchData.Question> preguntas = new List<FetchData.Question>();
    private List<FetchData.Question> preguntasEnUso = new List<FetchData.Question>();
    private List<FetchData.Question> preguntasUsadas = new List<FetchData.Question>();
    private List<Texture> fotografiasEnUso = new List<Texture>();
    private Texture imagen;
    private int intento;
    private int currentCamID;
    private int cont;
    private string idTipoChatarra;
    private string respuesta;
    private string gameStartTime;
    private string gameEndTime;
    private string questionStartTime;
    private string questionEndTime;
    private string idJuego;

    SortedDictionary<string, string> tiposChatarra = new SortedDictionary<string, string>()
    { {"Chatarra Nacional Primera", "3"},
      {"Mixto Cizallado", "5"},
      {"Mixto Para Procesar", "6"},
      {"Placa y Estructura Nacional", "7"},
      {"Rebaba de Acero", "8"},
      {"Regreso Industrial Galvanizado Nacional", "9"},
      {"Chicharron Nacional", "19"}
    };
    //Hash fotografiasEnUso x preguntasEnUso

    private void Start()
    {
        cont = 0;
        intento = 5;
        intentos.text = "Intentos: " + intento;

        gameStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        dataPublisher.CreateJuegoReceiver(ref gameStartTime, ref gameStartTime);

        Invoke("SetUp", 3f);
    }

    //SetUp: Realiza los ajustes necesarios para comenzar el juego
    private void SetUp()
    {
        Debug.Log("ID JUEGO: " + idJuego);

        //Se asigna la funcion de cambio de imagen a los botones del panel del camaras
        foreach (BotonCamara bc in botones)
        {
            bc.boton.onClick.AddListener(delegate { CameraHandler(bc.ID); });
        }

        //Se aleatoriza la lista de preguntas
        int n = preguntas.Count;
        System.Random rnd = new System.Random();

        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            FetchData.Question value = preguntas[k];
            preguntas[k] = preguntas[n];
            preguntas[n] = value;
        }

        //Se descargan tres imagenes de la lista aleatorea y se asignan las primeras tres imagenes para comenzar
        for (int i = 0; i < botones.Count; i++)
        {
            StartCoroutine(PhotoDownloader(preguntas[i].imageURL));
        }
    }

    //CameraHandler: Controla que fotografia se desplegara en la pantalla y guarda una referencia de la camara activa
    private void CameraHandler(int id)
    {
        pantalla.texture = fotografiasEnUso[id - 1];
        currentCamID = id;
        questionStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        Debug.Log("-----------------------------------");
        Debug.Log("Tipo: " + preguntasEnUso[id - 1].classification);
        Debug.Log("URL: " + preguntasEnUso[id - 1].imageURL);
        Debug.Log("-----------------------------------");
    }

    //ListReciver: Recibe una lista con todos los datos de la API solicitados. (Ver FetchData para mas informacion)
    public void ListReceiver(List<FetchData.Question> lista)
    {
        preguntas = lista;
    }

    // IdReceiver: Recive el ID del juego actual
    public void IdReceiver(string id)
    {
        idJuego = id;
    }

    //PhotoHandler: Administra las fotografias que se estan utilizando en los botones de las camaras, las fotografias que estan en espera para ser asignadas a un boton y las fotografias ya utilizadas
    public void PhotoHandler(ref List<Texture> imgList, int numFoto, ref Texture img, ref List<FetchData.Question> prgList)
    {
        if (preguntas.Count == 0)
        {
            preguntas = preguntasUsadas;
        }

        imgList[numFoto] = img;
        prgList[numFoto] = preguntas[0];

        //Debug.Log(prgList.Count);

        preguntasUsadas.Add(preguntas[0]);
        preguntas.RemoveAt(0);
    }

    //CorrutineReciver: Funcion de apoyo que da la señal de cuando un boton de clasificacion a sido presionado
    public void CorrutineReceiver(ref string type)
    {
        StartCoroutine(ClassificationButtonPressed(type));
    }

    //ClassificationButtonPressed: Funcion de bucle de juego. Llama a las funciones de administracion del juego y procesa la respuesta recibida
    private IEnumerator ClassificationButtonPressed(string tipoChatarra)
    {
        Debug.Log("Respuesta: " + tipoChatarra);
        //Debug.Log("CamID:" + (currentCamID - 1));
        Debug.Log("Boton precionado: " + preguntasEnUso[currentCamID - 1].classification);

        pantalla.texture = pleaseWait;
        questionEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        idTipoChatarra = tiposChatarra[tipoChatarra];

        switch (currentCamID)
        {
            case 1:
                StartCoroutine(PhotoDownloader(preguntas[0].imageURL));

                if (preguntasEnUso[currentCamID - 1].classification == tipoChatarra)
                {
                    respuesta = "1";
                    dataPublisher.CreatePreguntaReceiver(ref idTipoChatarra, ref questionStartTime, ref questionEndTime, ref respuesta, ref idJuego);

                    audio.PlayOneShot(correcto);
                }
                else
                {
                    respuesta = "0";
                    dataPublisher.CreatePreguntaReceiver(ref idTipoChatarra, ref questionStartTime, ref questionEndTime, ref respuesta, ref idJuego);

                    audio.PlayOneShot(incorrecto);
                    intento--;
                    intentos.text = "Intentos: " + intento;
                }

                if (intento == 1)
                {
                    intentos.color = Color.red;
                }

                if (intento == 0)
                {
                    gameEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dataPublisher.UpdateJuegoReceiver(ref idJuego, ref gameEndTime);
                    dataPublisher.UpdateLogroReceiver();

                    ManejoDeEscenas.ChangeScene("Game Over");
                }

                yield return new WaitForSeconds(3);
                CameraHandler(currentCamID);
                break;
            case 2:
                StartCoroutine(PhotoDownloader(preguntas[0].imageURL));

                if (preguntasEnUso[currentCamID - 1].classification == tipoChatarra)
                {
                    respuesta = "1";
                    dataPublisher.CreatePreguntaReceiver(ref idTipoChatarra, ref questionStartTime, ref questionEndTime, ref respuesta, ref idJuego);

                    audio.PlayOneShot(correcto);
                }
                else
                {
                    respuesta = "0";
                    dataPublisher.CreatePreguntaReceiver(ref idTipoChatarra, ref questionStartTime, ref questionEndTime, ref respuesta, ref idJuego);

                    audio.PlayOneShot(incorrecto);
                    intento--;
                    intentos.text = "Intentos: " + intento;
                }

                if (intento == 1)
                {
                    intentos.color = Color.red;
                }

                if (intento == 0)
                {
                    gameEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dataPublisher.UpdateJuegoReceiver(ref idJuego, ref gameEndTime);
                    dataPublisher.UpdateLogroReceiver();

                    ManejoDeEscenas.ChangeScene("Game Over");
                }

                yield return new WaitForSeconds(3);
                CameraHandler(currentCamID);
                break;
            case 3:
                StartCoroutine(PhotoDownloader(preguntas[0].imageURL));

                if (preguntasEnUso[currentCamID - 1].classification == tipoChatarra)
                {
                    respuesta = "1";
                    dataPublisher.CreatePreguntaReceiver(ref idTipoChatarra, ref questionStartTime, ref questionEndTime, ref respuesta, ref idJuego);

                    audio.PlayOneShot(correcto);
                }
                else
                {
                    respuesta = "0";
                    dataPublisher.CreatePreguntaReceiver(ref idTipoChatarra, ref questionStartTime, ref questionEndTime, ref respuesta, ref idJuego);

                    audio.PlayOneShot(incorrecto);
                    intento--;
                    intentos.text = "Intentos: " + intento;
                }

                if (intento == 1)
                {
                    intentos.color = Color.red;
                }

                if (intento == 0)
                {
                    gameEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dataPublisher.UpdateJuegoReceiver(ref idJuego, ref gameEndTime);
                    dataPublisher.UpdateLogroReceiver();

                    ManejoDeEscenas.ChangeScene("Game Over");
                }

                yield return new WaitForSeconds(3);
                CameraHandler(currentCamID);
                break;
        }
    }

    //PhotoDownloader: Descarga las fotografias utilizando el URL de la imagen.
    private IEnumerator PhotoDownloader(string url)
    {
        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(url);

        yield return imageRequest.SendWebRequest();

        if (imageRequest.isDone)
        {
            imagen = DownloadHandlerTexture.GetContent(imageRequest);

            if (cont < 3)
            {
                fotografiasEnUso.Add(imagen);
                preguntasEnUso.Add(preguntas[0]);

                PhotoHandler(ref fotografiasEnUso, fotografiasEnUso.Count - 1, ref imagen, ref preguntasEnUso);
                cont++;
            }
            else
            {
                PhotoHandler(ref fotografiasEnUso, currentCamID - 1, ref imagen, ref preguntasEnUso);
            }
        }
    }
}