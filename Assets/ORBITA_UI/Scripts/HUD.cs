using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ORBITA - HUD en partida: barra de oxígeno, %, tiempo y alerta "OXÍGENO BAJO".
//
// MODO DEMO (activado por defecto): el oxígeno baja solo y al llegar a 0 sale Game Over.
// Sirve para el video aunque el resto del juego no esté conectado.
//
// Cuando tus compañeros conecten el oxígeno real, llaman:
//   HUD.Instancia.ActualizarOxigeno(oxigenoActual, oxigenoMaximo);   // apaga el demo solo
//   HUD.Instancia.RecargarOxigeno();                                  // al recoger un tanque O2
public class HUD : MonoBehaviour
{
    public static HUD Instancia;

    public Image barraOxigeno;            // Image tipo Filled
    public TMP_Text textoPorcentaje;
    public TMP_Text textoTiempo;
    public GameObject alertaOxigenoBajo;  // overlay rojo

    [Range(0f, 1f)] public float umbralBajo = 0.25f;
    public AudioClip sonidoAlarma;
    public AudioClip sonidoRecarga;

    [Header("Modo demo")]
    public bool oxigenoDemo = false;
    public float segundosDeOxigeno = 60f;

    float oxigeno = 1f;
    float esperaAlarma;

    void Awake()
    {
        Instancia = this;
        Mostrar(1f);
    }

    void Update()
    {
        if (textoTiempo)
        {
            int s = Mathf.FloorToInt(Time.timeSinceLevelLoad);
            textoTiempo.text = $"{s / 60:00}:{s % 60:00}";
        }

        if (PantallaFin.JuegoTerminado || Time.timeScale == 0f) return;

        if (oxigenoDemo)
        {
            oxigeno -= Time.deltaTime / Mathf.Max(1f, segundosDeOxigeno);
            Mostrar(oxigeno);
            if (oxigeno <= 0f && PantallaFin.Instancia != null) PantallaFin.Instancia.MostrarGameOver();
        }

        if (oxigeno <= umbralBajo && oxigeno > 0f)
        {
            esperaAlarma -= Time.deltaTime;
            if (esperaAlarma <= 0f)
            {
                esperaAlarma = 1f;
                if (AudioManager.Instancia != null) AudioManager.Instancia.ReproducirEfecto(sonidoAlarma);
            }
        }
    }

    void Mostrar(float valor)
    {
        oxigeno = Mathf.Clamp01(valor);
        if (barraOxigeno) barraOxigeno.fillAmount = oxigeno;
        if (textoPorcentaje) textoPorcentaje.text = Mathf.CeilToInt(oxigeno * 100) + "%";
        if (alertaOxigenoBajo) alertaOxigenoBajo.SetActive(oxigeno <= umbralBajo);
    }

    public void ActualizarOxigeno(float actual, float maximo)
    {
        oxigenoDemo = false;
        if (maximo > 0) Mostrar(actual / maximo);
    }

    public void RecargarOxigeno()
    {
        Mostrar(1f);
        if (AudioManager.Instancia != null) AudioManager.Instancia.ReproducirEfecto(sonidoRecarga);
    }
    public void SumarBateriaHUD()
    {
        // Coloca aquí la lógica de lo que debe pasar en la interfaz cuando recojas una batería
        // (por ejemplo, sumar a un contador de baterías o actualizar un texto en pantalla).
        Debug.Log("¡Batería sumada al HUD!");
    }
}
