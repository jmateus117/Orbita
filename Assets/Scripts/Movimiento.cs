using UnityEngine;
using System.Collections;
public class Movimiento : MonoBehaviour
{
[Header("Movimiento")]
    public float Speed = 8.0f;
    public Rigidbody rb;
    public float rotationSpeed = 15f;

    [Header("Salto y Suelo")]
    public float JumpForce = 7.0f;
    public float lowJumpMultiplier = 2.0f;
    public float fallMultiplier = 2.5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Wall Jump / Impulso de Muro")]
    public LayerMask wallLayer;
    private bool isTouchingWall;
    private bool isWallSliding;
    public float wallSlideSpeed = 1.0f;
    public float wallJumpForce = 8.0f; 
    private Vector3 lastWallNormal;
    private float wallHoldTimer = 0f;          
    private float wallJumpCooldown = 0f;    

    public Vector3 gravedadActual = Vector3.down;
    private bool isRotatingGravity = false;
    private bool gravedadInvertidaActual = false;
    private Collider capsuleCollider;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        capsuleCollider = GetComponent<Collider>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveInput = new Vector3(x, 0f, z);

        // Rotación suave hacia donde se mueve
        if (moveInput.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput, -gravedadActual.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Auto-detección de gravedad invertida
        bool debeEstarInvertida = gravedadActual.y > 0;
        if (debeEstarInvertida != gravedadInvertidaActual && !isRotatingGravity)
        {
            StartCoroutine(RotarSuavizado(debeEstarInvertida));
        }

        // Detección de muros ajustada para el nuevo modelo 3D
        isTouchingWall = false;
        
        // Ajustamos la altura al torso (0.5f) y aumentamos la distancia a 0.8f para asegurar el impacto
        Vector3 rayOrigin = transform.position + (Vector3.up * 0.5f); 
        float rayDistance = 1.8f;
        Vector3 dirToCheck = moveInput.magnitude > 0.1f ? moveInput.normalized : transform.forward;

        if (moveInput.magnitude > 0.1f)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, moveInput.normalized, out hit, rayDistance, wallLayer))
            {
                isTouchingWall = true;
                lastWallNormal = hit.normal; 
            }
        }

        // Dibuja la línea en la pestaña Scene (Verde si detecta el muro, Azul si estás quieto)
        Debug.DrawRay(rayOrigin, dirToCheck * rayDistance, moveInput.magnitude > 0.1f ? (isTouchingWall ? Color.green : Color.red) : Color.blue);

        // Reducir temporizadores
        if (wallJumpCooldown > 0f)
        {
            wallJumpCooldown -= Time.deltaTime;
            isWallSliding = false;
        }
        else
        {
            if (isTouchingWall && !isGrounded && Vector3.Dot(rb.linearVelocity, gravedadActual.normalized) > 0)
            {
                if (wallHoldTimer <= 0f && !isWallSliding)
                {
                    wallHoldTimer = 0.6f; 
                }

                if (wallHoldTimer > 0f)
                {
                    wallHoldTimer -= Time.deltaTime;
                    isWallSliding = false; 
                }
                else
                {
                    isWallSliding = true; 
                }
            }
            else
            {
                isWallSliding = false;
                wallHoldTimer = 0f;
            }
        }

        // Lógica de Salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Vector3 direccionSalto = -gravedadActual.normalized;
                Vector3 vel = rb.linearVelocity;
                rb.linearVelocity = new Vector3(vel.x, 0f, vel.z);
                rb.AddForce(direccionSalto * JumpForce, ForceMode.Impulse);
            }
            else if (isTouchingWall || isWallSliding || wallHoldTimer > 0f)
            {
                // IMPULSO LIMPIO DESDE EL MURO
                Vector3 pushDirection = (lastWallNormal + (-gravedadActual.normalized * 0.8f)).normalized;
                
                rb.linearVelocity = Vector3.zero; 
                
                // Desplazamos ligeramente al jugador hacia afuera para que no colisione con el muro al instante
                transform.position += lastWallNormal * 0.15f;

                // Aplicamos el impulso de salida
                rb.AddForce(pushDirection * wallJumpForce, ForceMode.Impulse);
                
                wallJumpCooldown = 0.3f;
                isWallSliding = false;
                wallHoldTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 gravedadDir = gravedadActual.normalized;
        float verticalVel = Vector3.Dot(rb.linearVelocity, gravedadDir);

        if (wallHoldTimer > 0f)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        if (isWallSliding)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallSlideSpeed, rb.linearVelocity.z);
            return;
        }

        rb.linearVelocity = new Vector3(x * Speed, rb.linearVelocity.y, z * Speed);

        if (verticalVel > 0 && !isGrounded)
        {
            rb.AddForce(gravedadDir * Physics.gravity.magnitude * (fallMultiplier - 1f), ForceMode.Acceleration);
        }
        else if (verticalVel < 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(gravedadDir * Physics.gravity.magnitude * (lowJumpMultiplier - 1f), ForceMode.Acceleration);
        }
    }

    IEnumerator RotarSuavizado(bool invertida)
    {
        isRotatingGravity = true;
        gravedadInvertidaActual = invertida;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, 180f);

        float elapsed = 0f;
        float duration = 0.4f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
        isRotatingGravity = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    
}
    
