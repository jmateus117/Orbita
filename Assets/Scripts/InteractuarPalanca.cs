using UnityEngine;
using TMPro; // Asegúrate de tener esto si usas TextMeshPro

public class InteractuarPalanca : MonoBehaviour
{
    [Header("Configuración de la Palanca")]
    public Transform mangoPalanca; // Arrastra aquí el objeto del mango o el pivote que debe girar
    public float gradosRotacion = 90f; // Cuánto gira al activarla
    public float velocidadGiro = 5f;

    [Header("UI / Interacción")]
    public GameObject objetoTextoInteraccion; // Arrastra tu TextoInteractuar del Canvas
    private bool jugadorCerca = false;
    private bool palancaActivada = false;
    private Quaternion rotacionInicial;
    private Quaternion rotacionFinal;

    private Movimiento jugadorMovimiento; // Referencia al script de movimiento del jugador

    void Start()
    {
        if (mangoPalanca != null)
        {
            rotacionInicial = mangoPalanca.localRotation;
            rotacionFinal = rotacionInicial * Quaternion.Euler(gradosRotacion, 0f, 0f); // Ajusta el eje (X, Y o Z) según tu modelo
        }

        if (objetoTextoInteraccion != null)
        {
            objetoTextoInteraccion.SetActive(false); // Oculto al inicio
        }
    }

    void Update()
    {
        // Si el jugador está cerca y presiona la E
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            palancaActivada = !palancaActivada; // Alterna el estado (activada/desactivada)

            // Cambiar la gravedad del jugador llamando a su script
            if (jugadorMovimiento != null)
            {
                // Si estaba hacia abajo (-Vector3.up), la invertimos (Vector3.up), y viceversa
                if (jugadorMovimiento.gravedadActual.y < 0)
                {
                    jugadorMovimiento.gravedadActual = Vector3.up * Mathf.Abs(Physics.gravity.y);
                }
                else
                {
                    jugadorMovimiento.gravedadActual = Vector3.down * Mathf.Abs(Physics.gravity.y);
                }
            }
        }

        // Rotación suave del mango de la palanca
        if (mangoPalanca != null)
        {
            Quaternion objetivo = palancaActivada ? rotacionFinal : rotacionInicial;
            mangoPalanca.localRotation = Quaternion.Slerp(mangoPalanca.localRotation, objetivo, velocidadGiro * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<Movimiento>() != null)
        {
            jugadorCerca = true;
            jugadorMovimiento = other.GetComponent<Movimiento>();

            if (objetoTextoInteraccion != null)
            {
                objetoTextoInteraccion.SetActive(true); // Muestra el texto "Presiona E"
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<Movimiento>() != null)
        {
            jugadorCerca = false;
            jugadorMovimiento = null;

            if (objetoTextoInteraccion != null)
            {
                objetoTextoInteraccion.SetActive(false); // Oculta el texto al alejarte
            }
        }
    }
}