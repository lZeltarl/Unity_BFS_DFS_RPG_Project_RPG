using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GraphManager: Gera os nós do mapa (missões) e desenha as conexões.
/// Controla espaçamento e nomes de missão.
/// </summary>
public class GraphManager : MonoBehaviour
{
    [Header("Prefabs e referências (arraste no Inspector)")]
    public GameObject nodePrefab; // Prefab do nó (com script Node)
    public GameObject playerPrefab;
    public Transform nodesParent;
    public int nodeCount = 25;
    public Vector2 areaSize = new Vector2(22f, 12f); // 🔹 AUMENTEI a área para espaçar mais os nós

    [HideInInspector] public List<Node> nodes = new List<Node>();
    [HideInInspector] public List<List<int>> adjacency = new List<List<int>>();

    void Awake()
    {
        if (nodesParent == null)
        {
            GameObject np = new GameObject("NodesParent");
            np.transform.parent = this.transform;
            nodesParent = np.transform;
        }

        GenerateNodes();
        GenerateAdjacency();
        DrawEdges();
        InstantiatePlayer();
    }

    // 🔹 Gera nós em grade espaçada e dá nomes às missões
    void GenerateNodes()
    {
        int cols = 5;
        int rows = Mathf.CeilToInt(nodeCount / (float)cols);
        float startX = -areaSize.x / 2f;
        float startY = -areaSize.y / 2f;
        float stepX = (cols > 1) ? areaSize.x / (cols - 1) : 0.0f;
        float stepY = (rows > 1) ? areaSize.y / (rows - 1) : 0.0f;

        int id = 0;
        for (int r = 0; r < rows && id < nodeCount; r++)
        {
            for (int c = 0; c < cols && id < nodeCount; c++)
            {
                Vector3 pos = new Vector3(
                    startX + c * stepX,
                    startY + r * stepY,
                    0f
                );
                // 🔹 Pequeno deslocamento aleatório para parecer mais natural
                pos += new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.6f, 0.6f), 0f);

                GameObject go;
                if (nodePrefab != null)
                    go = Instantiate(nodePrefab, pos, Quaternion.identity, nodesParent);
                else
                {
                    go = new GameObject("Node_Dyn_" + id);
                    go.transform.parent = nodesParent;
                    go.transform.position = pos;
                    var sr = go.AddComponent<SpriteRenderer>();
                    Sprite s = Resources.Load<Sprite>("node_circle");
                    if (s != null) sr.sprite = s;
                    sr.sortingOrder = 0;
                    go.AddComponent<BoxCollider2D>();
                    go.AddComponent<Node>();
                }

                go.name = "Node_" + id;
                Node nodeComp = go.GetComponent<Node>();
                nodeComp.id = id;
                nodes.Add(nodeComp);
                adjacency.Add(new List<int>());
                id++;
            }
        }

        // 🔹 Lista de nomes de missão (usada para mostrar ao visitar)
        string[] names = new string[] {
            "Chegue na cidade principal","Converse com o ancião","Vá à floresta norte","Cace lobos selvagens","Retorne ao ancião",
            "Vá ao templo antigo","Derrote o guardião das ruínas","Traga o artefato sagrado","Vá ao vilarejo leste","Proteja os aldeões",
            "Encontre o mapa perdido","Descubra o esconderijo dos bandidos","Elimine o líder dos bandidos","Retorne ao ferreiro","Repare sua espada",
            "Vá à montanha congelada","Encontre o dragão adormecido","Derrote o dragão","Volte à cidade principal","Fale com o rei",
            "Parta em missão final","Entre no portal das sombras","Derrote o demônio ancestral","Restaure a paz no reino","Finalize a jornada do herói"
        };

        // 🔹 Define o nome da missão e cria o texto flutuante no prefab
        for (int i = 0; i < nodes.Count && i < names.Length; i++)
        {
            nodes[i].SetLabel(names[i]);
        }
    }

    // 🔹 Conecta os nós próximos entre si
    void GenerateAdjacency()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            List<KeyValuePair<int, float>> dists = new List<KeyValuePair<int, float>>();
            for (int j = 0; j < nodes.Count; j++)
            {
                if (i == j) continue;
                float d = Vector3.Distance(nodes[i].transform.position, nodes[j].transform.position);
                dists.Add(new KeyValuePair<int, float>(j, d));
            }
            dists.Sort((a, b) => a.Value.CompareTo(b.Value));

            int connections = Random.Range(2, 4);
            for (int k = 0; k < connections && k < dists.Count; k++)
            {
                int neighbor = dists[k].Key;
                if (!adjacency[i].Contains(neighbor)) adjacency[i].Add(neighbor);
                if (!adjacency[neighbor].Contains(i)) adjacency[neighbor].Add(i);
            }
        }
    }

    // 🔹 Desenha as linhas entre os nós
    void DrawEdges()
    {
        GameObject edgesParent = new GameObject("Edges");
        edgesParent.transform.parent = this.transform;

        for (int i = 0; i < adjacency.Count; i++)
        {
            foreach (int j in adjacency[i])
            {
                if (j <= i) continue;
                GameObject lineGO = new GameObject($"Edge_{i}_{j}");
                lineGO.transform.parent = edgesParent.transform;
                LineRenderer lr = lineGO.AddComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.useWorldSpace = true;
                lr.SetPosition(0, nodes[i].transform.position);
                lr.SetPosition(1, nodes[j].transform.position);
                lr.startWidth = 0.05f;
                lr.endWidth = 0.05f;
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = Color.gray;
                lr.endColor = Color.gray;
            }
        }
    }

    // 🔹 Cria o jogador na primeira missão
    void InstantiatePlayer()
    {
        if (playerPrefab != null && nodes.Count > 0)
        {
            GameObject p = Instantiate(playerPrefab, nodes[0].transform.position, Quaternion.identity);
            p.name = "Player";
        }
    }
}
