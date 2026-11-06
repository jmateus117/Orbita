using UnityEngine;

// ORBITA - Cámara para recorrer el nivel mientras no hay jugador.
// Ponla en la Main Camera.  Flechas o A/D = izquierda/derecha,  W/S = subir/bajar,
// Q/E o rueda del mouse = acercar/alejar,  Shift = más rápido.
// Cuando el equipo agregue al gato y su cámara, desactiven este script.
public class CamaraRecorrido : MonoBehaviour
{
    public float velocidad = 12f;
    public float velocidadZoom = 25f;

    void Update()
    {
        float h = 0f, v = 0f, z = 0f;
        bool rapido = false;

#if ENABLE_INPUT_SYSTEM
        var k = UnityEngine.InputSystem.Keyboard.current;
        var m = UnityEngine.InputSystem.Mouse.current;
        if (k != null)
        {
            if (k.dKey.isPressed || k.rightArrowKey.isPressed) h += 1f;
            if (k.aKey.isPressed || k.leftArrowKey.isPressed) h -= 1f;
            if (k.wKey.isPressed || k.upArrowKey.isPressed) v += 1f;
            if (k.sKey.isPressed || k.downArrowKey.isPressed) v -= 1f;
            if (k.eKey.isPressed) z += 1f;
            if (k.qKey.isPressed) z -= 1f;
            rapido = k.leftShiftKey.isPressed;
        }
        if (m != null) z += m.scroll.ReadValue().y * 0.05f;
#else
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.E)) z += 1f;
        if (Input.GetKey(KeyCode.Q)) z -= 1f;
        z += Input.mouseScrollDelta.y * 2f;
        rapido = Input.GetKey(KeyCode.LeftShift);
#endif

        float vel = velocidad * (rapido ? 3f : 1f) * Time.deltaTime;
        transform.position += transform.right * h * vel
                            + Vector3.up * v * vel
                            + transform.forward * z * velocidadZoom * Time.deltaTime;
    }
}
