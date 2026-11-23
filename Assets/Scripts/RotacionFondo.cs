using UnityEngine;

public class RotacionFondo : MonoBehaviour
{
[Header("Configuración de Rotación")]
    [Tooltip("Eje sobre el que rotará el objeto (ej. (0, 1, 0) para girar como un trompo)")]
    [SerializeField] private Vector3 ejeDeRotacion = new Vector3(0f, 1f, 0f);

    [Tooltip("Velocidad en grados por segundo (usa valores negativos para invertir el sentido)")]
    [SerializeField] private float velocidad = 90f;

    [Tooltip("Desmarcado = propio eje (Self). Marcado = ejes del mundo (World)")]
    [SerializeField] private bool usarEspacioGlobal = false;

    void Update()
    {
        Space espacio = usarEspacioGlobal ? Space.World : Space.Self;
        transform.Rotate(ejeDeRotacion * velocidad * Time.deltaTime, espacio);
    }

    public void CambiarVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }
}
