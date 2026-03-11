using System.Collections.Generic;
using Sirenix.OdinInspector;

using UnityEngine;

public class BarbaroNPC : MonoBehaviour
{
    public BarbaroData data;
    public Animator animator;
    private bool IsPlayerNearby = false;

    [ReadOnly] public List<string> bancoDialogos = new List<string>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) GenerarBanco();

        if (IsPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (bancoDialogos.Count > 0) Hablar();
            else GameManager.Instance.OnBarbarianTalk?.Invoke("¡Aún no tengo qué decir! (Presiona P)");
        }
    }

    private void GenerarBanco()
    {
        GameManager.Instance.OnBarbarianThink?.Invoke();

        string prompt = $"Eres {data.nombre}. {data.descripcion}. Estás en {data.lugar}. " +
                        $"El jugador es: {data.PlayerDescription}. " +
                        "Genera 10 frases cortas chistosas dirigidas al jugador. " +
                        "Regla: Si es guerrero, admíralo. Si es mago o lleva pijama, búrlate. " +
                        "Usa rimas graciosas. Separa frases solo con '|'. NO uses comillas.";

        GameManager.Instance.askIa(prompt, (respuesta) => {
            respuesta = respuesta.Replace("\\n", "").Replace("\n", "");
            string[] frases = respuesta.Split('|');
            bancoDialogos.Clear();
            foreach (var f in frases) if (!string.IsNullOrWhiteSpace(f)) bancoDialogos.Add(f.Trim());
        });
    }

    private void Hablar()
    {
        string frase = bancoDialogos[Random.Range(0, bancoDialogos.Count)];
        GameManager.Instance.OnBarbarianTalk?.Invoke(frase);
        if (animator) animator.SetTrigger("IsTalking");
    }

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) { IsPlayerNearby = true; GameManager.Instance.OnPlayerEntered?.Invoke(true); } }
    private void OnTriggerExit(Collider other) { IsPlayerNearby = false; GameManager.Instance.OnPlayerEntered?.Invoke(false); }
}
