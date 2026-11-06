using UnityEngine;
using UnityEngine.SceneManagement;

// ORBITA - Menú principal: Jugar -> Cómo jugar -> ¡A jugar! (carga el nivel).
// ENTER avanza, ESC abre Ajustes o vuelve.
public class MenuPrincipal : MonoBehaviour
{
    [Tooltip("Nombre EXACTO de la escena del nivel")]
    public string escenaJuego = "Nivel1";

    public GameObject panelPrincipal;
    public GameObject panelComoJugar;
    public GameObject panelOpciones;
    public GameObject panelCreditos;

    void Start()
    {
        Time.timeScale = 1f;
        VolverAlPrincipal();
    }

    void Update()
    {
        bool enPrincipal = panelPrincipal != null && panelPrincipal.activeSelf;
        if (Teclas.Escape())
        {
            if (enPrincipal) AbrirOpciones(); else VolverAlPrincipal();
        }
        else if (Teclas.Enter())
        {
            if (enPrincipal) AbrirComoJugar();
            else if (panelComoJugar != null && panelComoJugar.activeSelf) Jugar();
        }
    }

    void Mostrar(GameObject panel)
    {
        foreach (var p in new[] { panelPrincipal, panelComoJugar, panelOpciones, panelCreditos })
            if (p != null) p.SetActive(p == panel);
    }

    public void VolverAlPrincipal() => Mostrar(panelPrincipal);
    public void AbrirComoJugar() => Mostrar(panelComoJugar);
    public void AbrirOpciones() => Mostrar(panelOpciones);
    public void AbrirCreditos() => Mostrar(panelCreditos);

    public void Jugar() => SceneManager.LoadScene(escenaJuego);

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
