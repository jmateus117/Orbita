using UnityEngine;

public class GravedadPlataforma : MonoBehaviour
{
public float gravedad = 1f;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
        rb.useGravity = false;

        // Obtener el script de movimiento
        Movimiento movimiento = other.GetComponent<Movimiento>();

        if (movimiento != null)
        {
            movimiento.gravedadActual =
                Physics.gravity.normalized * gravedad;
        }

        // Aplicar la gravedad de esta zona
        rb.AddForce(
            Physics.gravity * gravedad,
            ForceMode.Acceleration
        );
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
            movimiento.gravedadActual = Vector3.down;
        }
        }
    }
}
