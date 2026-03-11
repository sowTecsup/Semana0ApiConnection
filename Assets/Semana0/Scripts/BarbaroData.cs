using UnityEngine;

[CreateAssetMenu(fileName = "BarbaroData", menuName = "Scriptable Objects/BarbaroData")]
public class BarbaroData : ScriptableObject
{
    public string nombre = "Rudy el Doma-Osos";
    [TextArea(3, 10)]
    public string descripcion = "Un bárbaro de 2 metros con el torso desnudo y una cicatriz en el pecho. Lleva una capa de piel de oso amigable. Está desorientado en una cocina moderna y habla siempre con rimas graciosas.";
    [TextArea(3, 5)]
    public string lugar = "Taberna del Dragón Ciego (que parece un apartamento común y corriente con microondas).";
    [TextArea(3, 10)]
    public string PlayerDescription = "Un guerrero colosal nivel máximo con armadura de huesos y un hacha gigante.";
}
