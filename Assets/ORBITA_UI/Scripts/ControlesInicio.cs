using UnityEngine;
using System.Collections;

// Muestra los controles al empezar el nivel y luego los desvanece.
public class ControlesInicio : MonoBehaviour
{
    public float segundosVisible = 6f;   // cuánto tiempo se ve
    public float duracionFade = 0.6f;    // cuánto tarda en desaparecer

    CanvasGroup grupo;

    void Awake()
    {
        grupo = GetComponent<CanvasGroup>();
        if (grupo == null) grupo = gameObject.AddComponent<CanvasGroup>();
        grupo.blocksRaycasts = false;
    }

    void OnEnable()
    {
        grupo.alpha = 1f;
        StartCoroutine(Ocultar());
    }

    IEnumerator Ocultar()
    {
        yield return new WaitForSecondsRealtime(segundosVisible);

        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.unscaledDeltaTime;
            grupo.alpha = 1f - (t / duracionFade);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}