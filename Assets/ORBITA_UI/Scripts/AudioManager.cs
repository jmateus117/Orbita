using UnityEngine;

// ORBITA - Controla TODA la música y los efectos. Sobrevive entre escenas (solo hay uno).
// Desde cualquier script:  AudioManager.Instancia.ReproducirEfecto(miClip);
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instancia;

    public AudioSource fuenteMusica;
    public AudioSource fuenteEfectos;

    [Header("Sonidos de interfaz")]
    public AudioClip clicBoton;

    void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
        DontDestroyOnLoad(gameObject);

        if (fuenteMusica == null) fuenteMusica = gameObject.AddComponent<AudioSource>();
        if (fuenteEfectos == null) fuenteEfectos = gameObject.AddComponent<AudioSource>();
        fuenteMusica.loop = true;
        fuenteMusica.playOnAwake = false;
        fuenteEfectos.playOnAwake = false;

        fuenteMusica.volume = PlayerPrefs.GetFloat("volMusica", 0.6f);
        fuenteEfectos.volume = PlayerPrefs.GetFloat("volEfectos", 1f);
        fuenteMusica.mute = PlayerPrefs.GetInt("musicaOn", 1) == 0;
        fuenteEfectos.mute = PlayerPrefs.GetInt("efectosOn", 1) == 0;
    }

    public void ReproducirMusica(AudioClip clip)
    {
        if (clip == null) return;
        if (fuenteMusica.clip == clip && fuenteMusica.isPlaying) return;
        fuenteMusica.clip = clip;
        fuenteMusica.Play();
    }

    public void DetenerMusica() => fuenteMusica.Stop();

    public void ReproducirEfecto(AudioClip clip)
    {
        if (clip != null) fuenteEfectos.PlayOneShot(clip);
    }

    public void Clic() => ReproducirEfecto(clicBoton);

    // --- Interruptores de Ajustes ---
    public bool MusicaActiva => !fuenteMusica.mute;
    public bool EfectosActivos => !fuenteEfectos.mute;

    public void SetMusicaActiva(bool activa)
    {
        fuenteMusica.mute = !activa;
        PlayerPrefs.SetInt("musicaOn", activa ? 1 : 0);
    }

    public void SetEfectosActivos(bool activos)
    {
        fuenteEfectos.mute = !activos;
        PlayerPrefs.SetInt("efectosOn", activos ? 1 : 0);
    }

    // --- Volumen (por si luego agregan sliders) ---
    public float VolumenMusica => fuenteMusica.volume;
    public float VolumenEfectos => fuenteEfectos.volume;
    public void CambiarVolumenMusica(float v) { fuenteMusica.volume = v; PlayerPrefs.SetFloat("volMusica", v); }
    public void CambiarVolumenEfectos(float v) { fuenteEfectos.volume = v; PlayerPrefs.SetFloat("volEfectos", v); }
}
