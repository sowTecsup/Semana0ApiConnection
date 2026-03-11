using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configuración API")]
    [SerializeField] private string apiKey = "TU_API_KEY_AQUI";
    private string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite-preview:generateContent?key=";

    [Header("Eventos")]
    public Action<string> OnBarbarianTalk;
    public Action OnBarbarianThink;
    public Action<bool> OnPlayerEntered;

    public GameObject interactHint;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        OnPlayerEntered += (isInside) => interactHint.SetActive(isInside);
    }

    public void askIa(string prompt, Action<string> alTerminar)
    {
        StartCoroutine(PostRequest(prompt, alTerminar));
    }

    IEnumerator PostRequest(string prompt, Action<string> callback)
    {
        // Limpieza de seguridad para el JSON
        string safePrompt = prompt.Replace("\"", "\\\"").Replace("\n", " ");
        string jsonData = "{\"contents\":[{\"parts\":[{\"text\":\"" + safePrompt + "\"}]}]}";

        using (UnityWebRequest request = new UnityWebRequest(url + apiKey, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string rawResponse = request.downloadHandler.text;
                try
                {
                    int inicio = rawResponse.IndexOf("\"text\": \"") + 9;
                    int fin = rawResponse.IndexOf("\"", inicio);
                    string limpio = rawResponse.Substring(inicio, fin - inicio);
                    callback(limpio);
                }
                catch
                {
                    callback("¡Grog se confundió con las palabras!");
                }
            }
            else
            {
                callback("¡El bárbaro tiene dolor de cabeza! (Error API)");
            }
        }
    }
}
