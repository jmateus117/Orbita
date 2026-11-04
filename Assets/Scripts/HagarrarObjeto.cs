using UnityEngine;

public class HagarrarObjeto : MonoBehaviour
{
// Definimos los tipos de coleccionables que tendrás en el juego
    public enum TipoColeccionable
    {
        Bateria,
        Oxigeno
    }

    [Header("Configuración del Objeto")]
    public TipoColeccionable tipo;
    public float valorRecarga = 45f; // Usado si es Oxígeno (cuánto recarga)
    
    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si el objeto que chocó es el Player
        if (other.CompareTag("Player") || other.GetComponent<Movimiento>() != null)
        {
            // Dependiendo del tipo, actuamos de forma diferente
            switch (tipo)
            {
                case TipoColeccionable.Bateria:
                    Inventario inventario = other.GetComponent<Inventario>();
                    if (inventario != null)
                    {
                        inventario.SumarBateria();
                        Destroy(gameObject);
                    }
                    break;

                case TipoColeccionable.Oxigeno:
                    SistemaOxigeno oxigeno = other.GetComponent<SistemaOxigeno>();
                    if (oxigeno != null)
                    {
                        oxigeno.RecargarOxigeno(valorRecarga);
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }
}
