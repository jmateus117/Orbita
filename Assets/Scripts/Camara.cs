using UnityEngine;

public class Camara : MonoBehaviour
{
[Header("Objetivo")]
    public Transform target; // Arrastra aquí a tu Player (la cápsula)

    [Header("Configuración de Movimiento")]
    public float smoothSpeed = 5f; // Qué tan rápido sigue la cámara (más alto = más rígido, más bajo = más flotante)
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Distancia de la cámara respecto al jugador

    void LateUpdate()
    {
        if (target == null) return;

        // Posición a la que queremos que vaya la cámara
        Vector3 targetPosition = target.position + offset;

        // Movimiento suave utilizando Lerp (crea el efecto flotante estilo Hollow Knight)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Actualizamos la posición de la cámara
        transform.position = smoothedPosition;
    }
}
