using UnityEngine;
using UnityEngine.SceneManagement;

// ORBITA - Pantalla "¡Inténtalo de nuevo!" (Game Over) y Victoria.
// Tus compañeros llaman:  PantallaFin.Instancia.MostrarGameOver();
//                         PantallaFin.Instancia.MostrarVictoria();
public class PantallaFin : MonoBehaviour
{
    public static PantallaFin Instancia;
    public static bool JuegoTerminado;

    public GameObject panelGameOver;
    public GameObject panelVictoria;   // opcional (aún no hay diseño)
    public string escenaMenu = "MenuPrincipal";

    [Header("Sonidos")]
    public AudioClip sonidoGameOver;
    public AudioClip sonidoVictoria;

    void Awake()
    {
        Instancia = this;
        JuegoTerminado = false;
        if (panelGameOver) panelGameOver.SetActive(false);
        if (panelVictoria) panelVictoria.SetActive(false);
    }

    public void MostrarGameOver() => Mostrar(panelGameOver, sonidoGameOver);
    public void MostrarVictoria() => Mostrar(panelVictoria != null ? panelVictoria : null, sonidoVictoria);

    void Mostrar(GameObject panel, AudioClip sonido)
    {
        if (JuegoTerminado) return;
        JuegoTerminado = true;
        Time.timeScale = 0f;
        if (panel) panel.SetActive(true);
        if (AudioManager.Instancia != null)
        {
            AudioManager.Instancia.DetenerMusica();
            AudioManager.Instancia.ReproducirEfecto(sonido);
        }
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenu);
    }
}
