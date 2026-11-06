using UnityEngine;

public class BateriaColeccionable : MonoBehaviour
{
private void OnTriggerEnter(Collider other)
    {
        // 1. Busca si el objeto que chocó tiene el script de inventario
        Inventario inventario = other.GetComponent<Inventario>();

        if (inventario != null)
        {
            // Suma la batería al inventario interno
            inventario.SumarBateria();

            // 2. Actualiza el HUD de tu compañero automáticamente usando el Singleton
            if (HUD.Instancia != null)
            {
                HUD.Instancia.SumarBateriaHUD();
            }

            // 3. Destruye el objeto de la batería de la escena
            Destroy(gameObject);
        }
    }
}
