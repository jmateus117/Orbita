using UnityEngine;
using UnityEngine.SceneManagement;

// ORBITA - Pausa con ESC: Reanudar, Reiniciar, Cómo jugar, Ajustes, Salir al menú.
public class MenuPausa : MonoBehaviour
{
    public static bool EstaPausado;

    public GameObject panelPausa;
    public GameObject panelComoJugar;
    public GameObject panelOpciones;
    public string escenaMenu = "MenuPrincipal";

    void Start()
    {
        EstaPausado = false;
        OcultarTodo();
    }

    void Update()
    {
        if (!Teclas.Escape() || PantallaFin.JuegoTerminado) return;

        if (!EstaPausado) Pausar();
        else if (panelPausa != null && !panelPausa.activeSelf) VolverAPausa();
        else Reanudar();
    }

    void OcultarTodo()
    {
        foreach (var p in new[] { panelPausa, panelComoJugar, panelOpciones })
            if (p != null) p.SetActive(false);
    }

    void Mostrar(GameObject panel)
    {
        OcultarTodo();
        if (panel != null) panel.SetActive(true);
    }

    public void Pausar()
    {
        EstaPausado = true;
        Time.timeScale = 0f;
        Mostrar(panelPausa);
    }

    public void Reanudar()
    {
        EstaPausado = false;
        Time.timeScale = 1f;
        OcultarTodo();
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        EstaPausado = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AbrirComoJugar() => Mostrar(panelComoJugar);
    public void AbrirOpciones() => Mostrar(panelOpciones);
    public void VolverAPausa() => Mostrar(panelPausa);

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        EstaPausado = false;
        SceneManager.LoadScene(escenaMenu);
    }
}
