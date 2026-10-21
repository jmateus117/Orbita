using UnityEngine;
public class Movimiento : MonoBehaviour
{

[Header("Movimiento")]
    public float Speed = 8.0f;
    public Rigidbody rb;

    [Header("Salto y Gravedad")]
    public float JumpForce = 7.0f;
    public float lowJumpMultiplier = 2.0f;
    public float fallMultiplier = 2.5f;

    // Dirección de la gravedad actual.
    // Por defecto apunta hacia abajo.
    public Vector3 gravedadActual = Vector3.down;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movimiento horizontal
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.Translate(
            new Vector3(x, 0f, z) * Time.deltaTime * Speed
        );

        // Saltar en dirección contraria a la gravedad
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 direccionSalto = -gravedadActual.normalized;

            rb.AddForce(
                direccionSalto * JumpForce,
                ForceMode.Impulse
            );
        }
    }

    void FixedUpdate()
    {
        Vector3 gravedad = gravedadActual.normalized;

    float velocidadEnGravedad =
        Vector3.Dot(rb.linearVelocity, gravedad);

    // El personaje está cayendo
    if (velocidadEnGravedad > 0)
    {
        rb.AddForce(
            gravedad * Physics.gravity.magnitude
            * (fallMultiplier - 1f),
            ForceMode.Acceleration
        );
    }
    // El personaje está subiendo y soltaste Espacio
    else if (
        velocidadEnGravedad < 0 &&
        !Input.GetKey(KeyCode.Space)
    )
    {
        rb.AddForce(
            gravedad * Physics.gravity.magnitude
            * (lowJumpMultiplier - 1f),
            ForceMode.Acceleration
        );
    }
    }

    public void SetExternalVelocity(Vector3 platformVelocity)
    {

    }


}
    
