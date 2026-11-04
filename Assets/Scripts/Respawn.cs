using UnityEngine;

public class Respawn : MonoBehaviour
{
[Header("Referencia al punto respawn")]
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && respawnPoint != null)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // 1. Detenemos las físicas por completo antes de moverlo
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true; // Lo volvemos cinemático para que ningún viento o fuerza lo mueva

                // 2. Movemos al jugador a la posición de la zona de reaparición
                other.transform.position = respawnPoint.position;

                // 3. Volvemos a activar las físicas para que caiga por gravedad de forma estable
                rb.isKinematic = false;
                rb.linearVelocity = Vector3.zero; // Limpiamos por si acaso
            }
            else
            {
                other.transform.position = respawnPoint.position;
            }
        }
    }
}
