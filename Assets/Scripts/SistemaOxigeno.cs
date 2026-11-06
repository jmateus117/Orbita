using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SistemaOxigeno : MonoBehaviour
{
    [Header("Configuración de Oxígeno")]
    public float tiempoMaximoOxigeno = 180f; // 3 minutos
    private float oxigenoActual;

    [Header("Respawn")]
    public Transform puntoRespawn; // <--- Aquí arrastrarás tu plataforma de respawn
    private Rigidbody rb;
    private Vector3 posicionFallback; // Por si acaso olvidas asignar la plataforma

    void Start()
    {
        oxigenoActual = tiempoMaximoOxigeno;
        rb = GetComponent<Rigidbody>();

        // Guardamos una posición de respaldo inicial por seguridad
        posicionFallback = transform.position;

        // Sincronizamos con el HUD de tu compañero al iniciar
        if (HUD.Instancia != null)
        {
            HUD.Instancia.ActualizarOxigeno(oxigenoActual, tiempoMaximoOxigeno);
        }
    }

    void Update()
    {
        if (oxigenoActual > 0)
        {
            oxigenoActual -= Time.deltaTime;

            // Actualizamos el HUD de tu compañero en cada frame
            if (HUD.Instancia != null)
            {
                HUD.Instancia.ActualizarOxigeno(oxigenoActual, tiempoMaximoOxigeno);
            }

            if (oxigenoActual <= 0)
            {
                ReiniciarEnRespawn();
            }
        }
    }

    public void RecargarOxigeno(float cantidad)
    {
        oxigenoActual += cantidad;
        oxigenoActual = Mathf.Clamp(oxigenoActual, 0, tiempoMaximoOxigeno);

        // Si tu compañero tiene una función para recargar con sonido y efectos, la llamamos:
        if (HUD.Instancia != null)
        {
            HUD.Instancia.ActualizarOxigeno(oxigenoActual, tiempoMaximoOxigeno);
            
            // Si quieres que reproduzca el sonido de recarga de tu amigo al llenarse o recoger tanque:
            if (oxigenoActual >= tiempoMaximoOxigeno)
            {
                HUD.Instancia.RecargarOxigeno();
            }
        }
    }

    void ReiniciarEnRespawn()
    {
        Debug.Log("¡Te has quedado sin oxígeno! Teletransportando al respawn...");

        // 1. Rellenamos el oxígeno al máximo y actualizamos el HUD
        oxigenoActual = tiempoMaximoOxigeno;
        if (HUD.Instancia != null)
        {
            HUD.Instancia.ActualizarOxigeno(oxigenoActual, tiempoMaximoOxigeno);
        }

        // 2. Frenamos las físicas de la cápsula para que no salga volando
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 3. Teletransportamos a la plataforma de respawn (si está asignada)
        if (puntoRespawn != null)
        {
            transform.position = puntoRespawn.position;
        }
        else
        {
            // Si olvidaste asignarla, vuelve al punto donde inició el juego
            transform.position = posicionFallback;
        }
    }
}