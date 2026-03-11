using DamageNumbersPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public DamageNumber damageNumberPrefab;
    public Transform textAnchor;
    public AudioSource audio;

    void Start()
    {
        GameManager.Instance.OnBarbarianTalk += (txt) => {
            audio.Stop();
            DamageNumber dn = damageNumberPrefab.Spawn(textAnchor.position);
            dn.bottomText = txt;
            audio.Play();
        };

        GameManager.Instance.OnBarbarianThink += () => {
            DamageNumber dn = damageNumberPrefab.Spawn(textAnchor.position);
            dn.bottomText = "Mmmm... (Rudy está pensando)";
            dn.lifetime = 1.5f;
        };
    }
}
