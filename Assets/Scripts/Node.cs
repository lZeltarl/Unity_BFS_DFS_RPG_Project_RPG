using UnityEngine;
using TMPro;

/// <summary>
/// Node: representa uma missão no mapa.
/// Controla cor (vermelho = não visitado / verde = visitado)
/// e exibe o nome da missão quando for visitado.
/// </summary>
[RequireComponent(typeof(Transform))]
public class Node : MonoBehaviour
{
    public int id;
    public string nodeName = "Missão";
    public bool visited = false;

    private SpriteRenderer sr;
    private TextMeshPro textPopup;
    private GameObject textBackground; // 🔹 Fundo do texto

    void Awake()
    {
        // 🔹 Garante que o SpriteRenderer existe
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>("circle"); // opcional: sprite padrão
            sr.color = Color.white;
        }

        SetColorRed();

        // 🔹 Cria o fundo do texto (um retângulo preto semi-transparente)
        textBackground = new GameObject("TextBackground");
        textBackground.transform.SetParent(this.transform);
        textBackground.transform.localPosition = new Vector3(0, 0.8f, 0.1f); // ligeiramente à frente
        var bgRenderer = textBackground.AddComponent<SpriteRenderer>();
        bgRenderer.sprite = Resources.Load<Sprite>("UI/Square"); // use qualquer sprite quadrado
        bgRenderer.color = new Color(0, 0, 0, 0.6f); // preto 60% transparente
        bgRenderer.sortingOrder = 1;
        textBackground.SetActive(false);

        // 🔹 Cria o texto
        GameObject go = new GameObject("MissionName");
        go.transform.SetParent(this.transform);
        go.transform.localPosition = new Vector3(0, 0.8f, 0);
        textPopup = go.AddComponent<TextMeshPro>();
        textPopup.alignment = TextAlignmentOptions.Center;
        textPopup.fontSize = 5f;
        textPopup.color = Color.white;
        textPopup.sortingOrder = 2;
        textPopup.text = nodeName;
        textPopup.gameObject.SetActive(false);
    }

    /// <summary>
    /// Define o nome da missão e atualiza o texto associado.
    /// </summary>
    public void SetLabel(string txt)
    {
        nodeName = txt;
        if (textPopup != null)
            textPopup.text = txt;
    }

    /// <summary>
    /// Marca o nó como visitado (fica verde e mostra o nome da missão)
    /// </summary>
    public void MarkVisited()
    {
        visited = true;
        SetColorGreen();
        ShowMissionName();
    }

    /// <summary>
    /// Marca o nó como não visitado (fica vermelho e esconde o nome)
    /// </summary>
    public void MarkUnvisited()
    {
        visited = false;
        SetColorRed();
        if (textPopup != null)
            textPopup.gameObject.SetActive(false);
        if (textBackground != null)
            textBackground.SetActive(false);
    }

    /// <summary>
    /// Exibe o nome da missão por alguns segundos
    /// </summary>
    void ShowMissionName()
    {
        if (textPopup == null) return;

        textPopup.gameObject.SetActive(true);
        textBackground.SetActive(true);

        // Ajusta o tamanho do fundo conforme o comprimento do texto
        var bgRenderer = textBackground.GetComponent<SpriteRenderer>();
        if (bgRenderer != null)
        {
            float width = textPopup.textBounds.size.x * 0.6f;
            float height = textPopup.textBounds.size.y * 1.2f;
            textBackground.transform.localScale = new Vector3(width, height, 1);
        }

        StopAllCoroutines();
        StartCoroutine(HideMissionNameAfterDelay(2.5f));
    }

    System.Collections.IEnumerator HideMissionNameAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        if (textPopup != null)
            textPopup.gameObject.SetActive(false);
        if (textBackground != null)
            textBackground.SetActive(false);
    }

    // -------------------
    // Métodos de cor
    // -------------------
    void SetColorGreen()
    {
        if (sr != null) sr.color = Color.green;
    }

    void SetColorRed()
    {
        if (sr != null) sr.color = Color.red;
    }
}
