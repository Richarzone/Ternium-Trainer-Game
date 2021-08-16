using System.Collections;
using UnityEngine;
using System;
using System.Data;
using System.Text;
using UnityEngine.Networking;
using SimpleJSON;
using System.Runtime.InteropServices;

public class DataPublisher : MonoBehaviour
{
    public ControladorPanelCamara panelCamara;
    public static string idJuego;

    public void CreateJuegoReceiver(ref string sT, ref string eT)
    {
        StartCoroutine(CreateJuego(sT, eT));
    }

    public void UpdateJuegoReceiver(ref string ID, ref string eTUpdate)
    {
        StartCoroutine(UpdateJuego(ID, eTUpdate));
    }

    public void CreatePreguntaReceiver(ref string type, ref string sT, ref string eT, ref string resp, ref string gameId)
    {
        StartCoroutine(CreatePregunta(type, sT, eT, resp, gameId));
    }

    public void UpdateLogroReceiver()
    {
        StartCoroutine(UpdateLogro());
    }


    //CreateJuego: Crea un nuevo juego con el mismo tiempo inicial y final
    public IEnumerator CreateJuego(string startTime, string endTime)
    {
        string uri = "https://localhost:5001/juego?user=" + UserData.username + "&startTime=" + startTime + "&endTime=" + endTime;
        WWWForm form = new WWWForm();

        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                JSONNode jsonIdGame = JSON.Parse(request.downloadHandler.text);
                idJuego = jsonIdGame[0]["user"];
                panelCamara.IdReceiver(idJuego);
            }
        }
    }

    //UpdateJuego: Actualiza el el tiempo final del juego que acaba de terminar
    public IEnumerator UpdateJuego(string id, string endTime)
    {
        string uri = "https://localhost:5001/juegoupdate?id=" + id + "&endTime=" + endTime;
        WWWForm form = new WWWForm();

        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    //CreatePregunta: Publica en la base de datos la informacion de una pregunta
    public IEnumerator CreatePregunta(string tipo, string startTime, string endTime, string respuesta, string gameId)
    {
        string uri = "https://localhost:5001/pregunta?user=" + UserData.username + "&tipo=" + tipo + "&startTime=" + startTime + "&endTime=" + endTime + "&isCorrecta=" + respuesta + "&idJuego=" + gameId;
        WWWForm form = new WWWForm();

        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    public IEnumerator UpdateLogro()
    {
        string uri = "https://localhost:5001/logrosprocedure";
        WWWForm form = new WWWForm();

        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}