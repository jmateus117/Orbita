using UnityEngine;
using UnityEngine.UI;

// ORBITA - Panel de Ajustes: interruptores de Sonido (efectos) y Música.
public class MenuOpciones : MonoBehaviour
{
    public Toggle toggleSonido;
    public Toggle toggleMusica;

    void OnEnable()
    {
        var am = AudioManager.Instancia;
        if (am == null) return;

        if (toggleSonido)
        {
            toggleSonido.SetIsOnWithoutNotify(am.EfectosActivos);
            toggleSonido.onValueChanged.RemoveListener(am.SetEfectosActivos);
            toggleSonido.onValueChanged.AddListener(am.SetEfectosActivos);
        }
        if (toggleMusica)
        {
            toggleMusica.SetIsOnWithoutNotify(am.MusicaActiva);
            toggleMusica.onValueChanged.RemoveListener(am.SetMusicaActiva);
            toggleMusica.onValueChanged.AddListener(am.SetMusicaActiva);
        }
    }
}
