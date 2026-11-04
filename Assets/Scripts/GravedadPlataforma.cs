using UnityEngine;

public class GravedadPlataforma : MonoBehaviour
{
public float gravedad = 1.5f;
    public bool esMorada = false; // SI, LO LEISTE, NO LO MARQUES

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            Movimiento movimiento = other.GetComponent<Movimiento>();

            if (movimiento != null)
            {
                if (esMorada)
                {
                    // Gravedad hacia el techo
                    movimiento.gravedadActual = Vector3.up * (9.81f * gravedad);
                    rb.AddForce(Vector3.up * 15f * gravedad, ForceMode.Acceleration);
                }
                else
                {
                    // Gravedad normal (roja o verde)
                    movimiento.gravedadActual = Physics.gravity * gravedad;
                    rb.AddForce(Physics.gravity * gravedad, ForceMode.Acceleration);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            Movimiento movimiento = other.GetComponent<Movimiento>();

            if (movimiento != null)
            {
                // Regresa a la normalidad al salir
                movimiento.gravedadActual = Vector3.down * 9.81f;
            }
        }
    }
}
