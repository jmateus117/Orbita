using UnityEngine;
using UnityEngine.UI;

// ORBITA - Hace sonar el "clic" al presionar un botón o un interruptor.
public class BotonConSonido : MonoBehaviour
{
    void Awake()
    {
        var b = GetComponent<Button>();
        if (b != null) b.onClick.AddListener(Sonar);
        var t = GetComponent<Toggle>();
        if (t != null) t.onValueChanged.AddListener(_ => Sonar());
    }

    void Sonar()
    {
        if (AudioManager.Instancia != null) AudioManager.Instancia.Clic();
    }
}
