using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// ORBITA - Construye las pantallas automáticamente desde el diseño de Stitch.
// Menú de Unity:  ORBITA > 1. Construir Menú Principal   /   ORBITA > 2. Construir UI del Nivel
public static class OrbitaUIBuilder
{
    const string RAIZ = "Assets/ORBITA_UI/";

    struct E
    {
        public string pantalla, id, archivo, archivoOff; public float x, y, w, h;
        public E(string p, string i, string a, string off, float x, float y, float w, float h)
        { pantalla = p; id = i; archivo = a; archivoOff = off; this.x = x; this.y = y; this.w = w; this.h = h; }
    }

    // Posiciones exactas (en píxeles de 1920x1080) sacadas del diseño de Stitch
    static readonly E[] ELEMENTOS =
    {
        new E("menu","btn-settings","menu_btn0.png",null,18.0f,18.0f,92.0f,92.0f),
        new E("menu","btn-credits","menu_btn1.png",null,1746.0f,18.0f,156.0f,80.0f),
        new E("menu","btn-play","menu_btn2.png",null,796.0f,874.0f,328.0f,104.0f),
        new E("comojugar","idx0","comojugar_btn0.png",null,715.0f,876.0f,180.0f,84.0f),
        new E("comojugar","idx1","comojugar_btn1.png",null,890.0f,872.0f,314.0f,92.0f),
        new E("pausa","btn-resume","pausa_btn0.png",null,752.0f,363.0f,416.0f,90.0f),
        new E("pausa","btn-restart","pausa_btn1.png",null,752.0f,439.0f,416.0f,80.0f),
        new E("pausa","btn-howtoplay","pausa_btn2.png",null,752.0f,505.0f,416.0f,80.0f),
        new E("pausa","btn-settings","pausa_btn3.png",null,752.0f,571.0f,416.0f,80.0f),
        new E("pausa","btn-quit","pausa_btn4.png",null,752.0f,637.0f,416.0f,80.0f),
        new E("gameover","retry-btn","gameover_btn0.png",null,672.0f,930.0f,308.0f,92.0f),
        new E("gameover","menu-btn","gameover_btn1.png",null,975.0f,934.0f,273.0f,84.0f),
        new E("creditos","btn-back","creditos_btn0.png",null,852.0f,958.0f,216.0f,80.0f),
        new E("ajustes","idx2","ajustes_btn2.png",null,806.0f,707.0f,308.0f,86.0f),
        new E("ajustes","toggle-sound","ajustes_toggle-sound_on.png","ajustes_toggle-sound_off.png",1087.0f,518.0f,88.0f,54.0f),
        new E("ajustes","toggle-music","ajustes_toggle-music_on.png","ajustes_toggle-music_off.png",1087.0f,614.0f,88.0f,54.0f),
    };

    // ---------------- MENÚ PRINCIPAL ----------------
    [MenuItem("ORBITA/1. Construir Menú Principal (escena actual)")]
    static void ConstruirMenu()
    {
        if (!EscenaGuardada()) return;
        PrepararSprites();
        BorrarSiExiste("Canvas_Menu");
        LimpiarViejos();

        var am = AsegurarAudioManager();
        AsegurarEventSystem();

        var canvas = CrearCanvas("Canvas_Menu", 0);
        var mp = canvas.AddComponent<MenuPrincipal>();
        canvas.AddComponent<MusicaDeEscena>().cancion = Audio("musica_menu");

        var principal = Panel(canvas.transform, "PanelPrincipal", "menu_fondo.png");
        Boton(principal, "menu", "btn-play", mp.AbrirComoJugar);
        Boton(principal, "menu", "btn-settings", mp.AbrirOpciones);
        Boton(principal, "menu", "btn-credits", mp.AbrirCreditos);

        var como = Panel(canvas.transform, "PanelComoJugar", "comojugar_fondo.png");
        Boton(como, "comojugar", "idx0", mp.VolverAlPrincipal);
        Boton(como, "comojugar", "idx1", mp.Jugar);

        var opc = PanelAjustes(canvas.transform, mp.VolverAlPrincipal);

        var cred = Panel(canvas.transform, "PanelCreditos", "creditos_fondo.png");
        Boton(cred, "creditos", "btn-back", mp.VolverAlPrincipal);

        mp.panelPrincipal = principal.gameObject;
        mp.panelComoJugar = como.gameObject;
        mp.panelOpciones = opc;
        mp.panelCreditos = cred.gameObject;
        como.gameObject.SetActive(false); opc.SetActive(false); cred.gameObject.SetActive(false);

        Terminar(0);
        EditorUtility.DisplayDialog("ORBITA", "Menú principal listo.\n\nDale Play para probarlo.", "OK");
    }

    // ---------------- UI DEL NIVEL ----------------
    [MenuItem("ORBITA/2. Construir UI del Nivel (escena actual)")]
    static void ConstruirNivel()
    {
        if (!EscenaGuardada()) return;
        PrepararSprites();
        BorrarSiExiste("UI_Nivel");

        AsegurarAudioManager();
        AsegurarEventSystem();

        var canvas = CrearCanvas("UI_Nivel", 10);
        canvas.AddComponent<MusicaDeEscena>().cancion = Audio("musica_nivel");

        // HUD
        var hudGo = Hijo(canvas.transform, "HUD"); Estirar(hudGo);
        var hud = canvas.AddComponent<HUD>();
        var overlay = Imagen(hudGo.transform, "HUD_Diseño", "hud_overlay.png"); Estirar(overlay.gameObject); overlay.raycastTarget = false;
        var barra = Imagen(hudGo.transform, "BarraOxigeno", "hud_barra_oxigeno.png");
        Colocar(barra.rectTransform, 98.0f,54.0f,220.0f,16.0f);
        barra.type = Image.Type.Filled; barra.fillMethod = Image.FillMethod.Horizontal; barra.fillOrigin = 0; barra.raycastTarget = false;
        hud.barraOxigeno = barra;
        hud.textoPorcentaje = Texto(hudGo.transform, "TextoPorcentaje", "100%", new Color(0.502f,0.965f,0.925f), 12.8f, 268.8f,28.0f,67.2f,24.0f, TextAlignmentOptions.Right);
        hud.textoTiempo = Texto(hudGo.transform, "TextoTiempo", "00:00", new Color(0.914f,0.867f,1.000f), 18.7f, 1798.1f,52.0f,79.9f,30.0f, TextAlignmentOptions.Left);
        var alerta = Imagen(hudGo.transform, "AlertaOxigenoBajo", "oxigeno_bajo_overlay.png"); Estirar(alerta.gameObject); alerta.raycastTarget = false;
        alerta.transform.SetSiblingIndex(0);
        hud.alertaOxigenoBajo = alerta.gameObject; alerta.gameObject.SetActive(false);
        hud.sonidoAlarma = Audio("alarma_oxigeno");
        hud.sonidoRecarga = Audio("recoger");

        // Pausa
        var pausa = canvas.AddComponent<MenuPausa>();
        var pPausa = Panel(canvas.transform, "PanelPausa", "pausa_fondo.png");
        Boton(pPausa, "pausa", "btn-resume", pausa.Reanudar);
        Boton(pPausa, "pausa", "btn-restart", pausa.Reiniciar);
        Boton(pPausa, "pausa", "btn-howtoplay", pausa.AbrirComoJugar);
        Boton(pPausa, "pausa", "btn-settings", pausa.AbrirOpciones);
        Boton(pPausa, "pausa", "btn-quit", pausa.VolverAlMenu);

        var pComo = Panel(canvas.transform, "PanelComoJugar", "comojugar_fondo.png");
        Boton(pComo, "comojugar", "idx0", pausa.VolverAPausa);
        Boton(pComo, "comojugar", "idx1", pausa.Reanudar);

        var pOpc = PanelAjustes(canvas.transform, pausa.VolverAPausa);

        pausa.panelPausa = pPausa.gameObject; pausa.panelComoJugar = pComo.gameObject; pausa.panelOpciones = pOpc;

        // Game Over
        var fin = canvas.AddComponent<PantallaFin>();
        var pGO = Panel(canvas.transform, "PanelGameOver", "gameover_fondo.png");
        Boton(pGO, "gameover", "retry-btn", fin.Reintentar);
        Boton(pGO, "gameover", "menu-btn", fin.VolverAlMenu);
        fin.panelGameOver = pGO.gameObject;
        fin.sonidoGameOver = Audio("game_over");
        fin.sonidoVictoria = Audio("victoria");

        foreach (var g in new[] { pPausa.gameObject, pComo.gameObject, pOpc, pGO.gameObject }) g.SetActive(false);

        // Guardar como prefab para que el equipo lo arrastre a su nivel
        PrefabUtility.SaveAsPrefabAssetAndConnect(canvas, RAIZ + "UI_Nivel.prefab", InteractionMode.AutomatedAction);

        Terminar(-1);
        EditorUtility.DisplayDialog("ORBITA", "UI del nivel lista.\n\nTambién quedó guardada como prefab en ORBITA_UI/UI_Nivel para que tus compañeros la arrastren a su escena.", "OK");
    }

    // ---------------- Ajustes (compartido) ----------------
    static GameObject PanelAjustes(Transform padre, UnityAction atras)
    {
        var p = Panel(padre, "PanelOpciones", "ajustes_fondo.png");
        Boton(p, "ajustes", "idx2", atras);
        var mo = p.gameObject.AddComponent<MenuOpciones>();
        mo.toggleSonido = Interruptor(p, "ajustes", "toggle-sound");
        mo.toggleMusica = Interruptor(p, "ajustes", "toggle-music");
        return p.gameObject;
    }

    // ---------------- Ayudantes ----------------
    static bool EscenaGuardada()
    {
        var esc = EditorSceneManager.GetActiveScene();
        if (!string.IsNullOrEmpty(esc.path)) return true;
        EditorUtility.DisplayDialog("ORBITA", "Primero guarda la escena (File > Save As) y vuelve a intentarlo.", "OK");
        return false;
    }

    static void Terminar(int posicionBuild)
    {
        var esc = EditorSceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(esc);
        EditorSceneManager.SaveScene(esc);
        var lista = EditorBuildSettings.scenes.ToList();
        if (!lista.Any(s => s.path == esc.path))
        {
            var nueva = new EditorBuildSettingsScene(esc.path, true);
            if (posicionBuild == 0) lista.Insert(0, nueva); else lista.Add(nueva);
            EditorBuildSettings.scenes = lista.ToArray();
        }
    }

    static void PrepararSprites()
    {
        foreach (var guid in AssetDatabase.FindAssets("t:Texture2D", new[] { RAIZ + "Sprites" }))
        {
            var ruta = AssetDatabase.GUIDToAssetPath(guid);
            var imp = (TextureImporter)AssetImporter.GetAtPath(ruta);
            if (imp.textureType == TextureImporterType.Sprite && imp.isReadable && imp.maxTextureSize >= 2048) continue;
            imp.textureType = TextureImporterType.Sprite;
            imp.spriteImportMode = SpriteImportMode.Single;
            imp.mipmapEnabled = false;
            imp.alphaIsTransparency = true;
            imp.maxTextureSize = 2048;
            imp.isReadable = true; // para que solo la parte visible del botón reciba clics
            imp.SaveAndReimport();
        }
    }

    static Sprite Spr(string archivo)
    {
        var s = AssetDatabase.LoadAssetAtPath<Sprite>(RAIZ + "Sprites/" + archivo);
        if (s == null) Debug.LogWarning("ORBITA: no encontré el sprite " + archivo);
        return s;
    }

    static AudioClip Audio(string nombre) => AssetDatabase.LoadAssetAtPath<AudioClip>(RAIZ + "Audio/" + nombre + ".wav");

    static void BorrarSiExiste(string nombre)
    {
        foreach (var go in EditorSceneManager.GetActiveScene().GetRootGameObjects())
            if (go.name == nombre) Object.DestroyImmediate(go);
    }

    // Quita el Canvas vacío y el AudioManager roto que se hicieron a mano antes
    static void LimpiarViejos()
    {
        foreach (var go in EditorSceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (go.name == "Canvas" && go.transform.childCount == 0) Object.DestroyImmediate(go);
            else if (go.name == "AudioManager" && go.GetComponents<Component>().Any(c => c == null)) Object.DestroyImmediate(go);
        }
    }

    static AudioManager AsegurarAudioManager()
    {
        var am = Object.FindFirstObjectByType<AudioManager>();
        if (am == null) am = new GameObject("AudioManager").AddComponent<AudioManager>();
        am.clicBoton = Audio("clic");
        return am;
    }

    static void AsegurarEventSystem()
    {
        if (Object.FindFirstObjectByType<EventSystem>() != null) return;
        var es = new GameObject("EventSystem", typeof(EventSystem));
#if ENABLE_INPUT_SYSTEM
        es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
        es.AddComponent<StandaloneInputModule>();
#endif
    }

    static GameObject CrearCanvas(string nombre, int orden)
    {
        var go = new GameObject(nombre, typeof(RectTransform));
        var c = go.AddComponent<Canvas>(); c.renderMode = RenderMode.ScreenSpaceOverlay; c.sortingOrder = orden;
        var cs = go.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        cs.matchWidthOrHeight = 0.5f;
        go.AddComponent<GraphicRaycaster>();
        return go;
    }

    static GameObject Hijo(Transform padre, string nombre)
    {
        var go = new GameObject(nombre, typeof(RectTransform));
        go.transform.SetParent(padre, false);
        return go;
    }

    static void Estirar(GameObject go)
    {
        var rt = (RectTransform)go.transform;
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.offsetMin = rt.offsetMax = Vector2.zero;
    }

    static void Colocar(RectTransform rt, float x, float y, float w, float h)
    {
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(w, h);
        rt.anchoredPosition = new Vector2(x + w / 2f - 960f, 540f - (y + h / 2f));
    }

    static Image Imagen(Transform padre, string nombre, string archivo)
    {
        var img = Hijo(padre, nombre).AddComponent<Image>();
        img.sprite = Spr(archivo);
        return img;
    }

    static RectTransform Panel(Transform padre, string nombre, string fondo)
    {
        var img = Imagen(padre, nombre, fondo);
        Estirar(img.gameObject);
        return img.rectTransform;
    }

    static E Buscar(string pantalla, string id)
    {
        foreach (var e in ELEMENTOS) if (e.pantalla == pantalla && e.id == id) return e;
        throw new System.Exception("ORBITA: falta el elemento " + pantalla + "/" + id);
    }

    static Button Boton(RectTransform panel, string pantalla, string id, UnityAction accion)
    {
        var e = Buscar(pantalla, id);
        var img = Imagen(panel, "Boton_" + id, e.archivo);
        Colocar(img.rectTransform, e.x, e.y, e.w, e.h);
        img.alphaHitTestMinimumThreshold = 0.1f;
        var b = img.gameObject.AddComponent<Button>();
        var col = b.colors; col.highlightedColor = new Color(1f, 1f, 1f, 1f); col.normalColor = new Color(0.93f, 0.93f, 0.93f, 1f);
        col.pressedColor = new Color(0.75f, 0.75f, 0.75f, 1f); b.colors = col;
        img.gameObject.AddComponent<BotonConSonido>();
        UnityEventTools.AddPersistentListener(b.onClick, accion);
        return b;
    }

    static Toggle Interruptor(RectTransform panel, string pantalla, string id)
    {
        var e = Buscar(pantalla, id);
        var fondo = Imagen(panel, "Interruptor_" + id, e.archivoOff);
        Colocar(fondo.rectTransform, e.x, e.y, e.w, e.h);
        var on = Imagen(fondo.transform, "Encendido", e.archivo);
        Estirar(on.gameObject); on.raycastTarget = false;
        var t = fondo.gameObject.AddComponent<Toggle>();
        t.targetGraphic = fondo; t.graphic = on; t.isOn = true;
        t.toggleTransition = Toggle.ToggleTransition.None;
        fondo.gameObject.AddComponent<BotonConSonido>();
        return t;
    }

    static TMP_Text Texto(Transform padre, string nombre, string valor, Color color, float tam,
                          float x, float y, float w, float h, TextAlignmentOptions alin)
    {
        var t = Hijo(padre, nombre).AddComponent<TextMeshProUGUI>();
        t.text = valor; t.color = color; t.fontSize = tam; t.fontStyle = FontStyles.Bold;
        t.alignment = alin; t.raycastTarget = false;
        Colocar(t.rectTransform, x, y, w, h);
        return t;
    }
}
