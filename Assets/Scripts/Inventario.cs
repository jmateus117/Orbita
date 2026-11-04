using UnityEngine;
using TMPro;

public class Inventario : MonoBehaviour
{
[Header("Contador")]
    public int bateriasRecolectadas = 0;

    [Header("UI")]
    public TextMeshProUGUI textoBaterias; // Cambia a TextMeshProUGUI si usas TMP

    void Start()
    {
        ActualizarUI();
    }

    public void SumarBateria()
    {
        bateriasRecolectadas++;
        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (textoBaterias != null)
        {
            textoBaterias.text = "Baterías: " + bateriasRecolectadas;
        }
    }
}
