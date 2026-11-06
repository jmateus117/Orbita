using UnityEngine;

// ORBITA - Pon este script en un objeto de la escena y arrástrale la canción de esa escena.
public class MusicaDeEscena : MonoBehaviour
{
    public AudioClip cancion;

    void Start()
    {
        if (AudioManager.Instancia != null)
            AudioManager.Instancia.ReproducirMusica(cancion);
    }
}
