using UnityEngine;

public class BateriaColeccionable : MonoBehaviour
{
private void OnTriggerEnter(Collider other)
    {
        // Busca si el objeto que chocó tiene el script de inventario
        Inventario inventario = other.GetComponent<Inventario>();

        if (inventario != null)
        {
            // Suma la batería al contador
            inventario.SumarBateria();

            // Destruye el objeto de la batería de la escena
            Destroy(gameObject);
        }
    }
}
