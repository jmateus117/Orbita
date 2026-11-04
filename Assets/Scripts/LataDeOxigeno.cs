using UnityEngine;

public class LataDeOxigeno : MonoBehaviour
{
[Header("Recarga")]
    public float cantidadRecarga = 45f; // Cuántos segundos de oxígeno recupera (45s = un 25% de la barra)

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que toca la lata es el Player
        SistemaOxigeno sistemaOxigeno = other.GetComponent<SistemaOxigeno>();

        if (sistemaOxigeno != null)
        {
            // Recarga el oxígeno
            sistemaOxigeno.RecargarOxigeno(cantidadRecarga);

            // Destruye la lata de la escena
            Destroy(gameObject);
        }
    }
}
