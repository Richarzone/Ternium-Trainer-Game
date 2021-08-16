using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using SimpleJSON;

public class FetchData : MonoBehaviour
{
    public ControladorPanelCamara panelCamara;

    private List<Question> questionData = new List<Question>();
    private static List<Question> preguntasJuego = new List<Question>();

    private void Start()
    {
        StartCoroutine(GetData());
    }

    //GetData: Funcion de conexion con API. Se descarga una copia de un archivo JSON con la informacion del area de chatarra.
    public IEnumerator GetData()
    {
        FetchData preguntas = new FetchData();

        string questionsURL = "https://chatarrap-api.herokuapp.com/images";

        UnityWebRequest infoRequest = UnityWebRequest.Get(questionsURL);
        infoRequest.SetRequestHeader("auth_key", "eyJhbGciOiJIUzI1NiJ9.VGVjRXF1aXBvNA.mcsN6gZZIGrggkL_i2lNgaUPo5JdInNC7_SDsLv6Fek");

        infoRequest.SetRequestHeader("Origin", "https://localhost:5001");
        infoRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");
        infoRequest.SetRequestHeader("Access-Control-Allow-Credentials", "true");
        infoRequest.SetRequestHeader("Content-Type", "image/jpeg");

        yield return infoRequest.SendWebRequest();

        Debug.Log(infoRequest.isHttpError);
        Debug.Log(infoRequest.isNetworkError);
            
        if (infoRequest.isDone)
        {
            JSONNode jsonData = JSON.Parse(infoRequest.downloadHandler.text);

            for (int i = 0; i < jsonData.Count; i++)
            {
                if(jsonData[i]["area"] == "Chatarra")
                {
                    Question pregunta = new Question();
                    pregunta.imageURL = jsonData[i]["imageURL"];
                    pregunta.id = jsonData[i]["_id"];
                    pregunta.classification = jsonData[i]["classification"];
                    preguntas.questionData.Add(pregunta);
                }
            }
        }
        preguntasJuego = preguntas.questionData;
        panelCamara.ListReceiver(preguntasJuego);
    }

    public struct Question
    {
        public string imageURL { get; set; }
        public string id { get; set; }
        public string classification { get; set; }
    }
}