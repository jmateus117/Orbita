using UnityEngine;
public class Movimiento : MonoBehaviour
{

    [Header("Movimiento:")]
    public float Speed = 8.0f;
    
    public Rigidbody rb;

    [Header("Salto y Gravedad:")]

    public float JumpForce = 7.0f;

    public float lowJumpMultiplier = 2.0f;

    public float fallMultiplier = 2.5f;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //movimiento:

        float x = Input.GetAxis("Horizontal");

        float z = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(x, 0.0f , z ) * Time.deltaTime * Speed);

        //salto:

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);

        }
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1 ) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }

    public void SetExternalVelocity(Vector3 platformVelocity)
    {

    }


}
    
