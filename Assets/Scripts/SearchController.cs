using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla as buscas BFS e DFS no mapa de missões.
/// Move o jogador entre os nós e marca cada um como visitado.
/// Versão otimizada para apresentação visual e educacional.
/// </summary>
public class SearchController : MonoBehaviour
{
    public GraphManager graphManager; // Gerencia os nós e conexões
    public GameObject player;         // Esfera do jogador
    public float moveSpeed = 3f;      // Velocidade do movimento
    private bool isRunning = false;   // Evita iniciar múltiplas buscas ao mesmo tempo

    void Start()
    {
        // Caso o player não esteja atribuído no inspetor, tenta encontrar automaticamente
        if (player == null)
        {
            GameObject p = GameObject.Find("Player");
            if (p != null) player = p;
        }
    }

    // ----------------------------
    // BOTÕES DE EXECUÇÃO
    // ----------------------------
    public void RunBFS()
    {
        if (isRunning) return;
        StartCoroutine(RunBFSCoroutine(0)); // começa a partir do nó 0
    }

    public void RunDFS()
    {
        if (isRunning) return;
        StartCoroutine(RunDFSCoroutine(0));
    }

    // Reseta o grafo (volta player pro início e limpa cores)
    public void ResetGraph()
    {
        StopAllCoroutines();
        isRunning = false;

        if (graphManager == null) return;
        foreach (var n in graphManager.nodes)
            n.MarkUnvisited();

        if (player != null && graphManager.nodes.Count > 0)
            player.transform.position = graphManager.nodes[0].transform.position;
    }

    // ----------------------------
    // ALGORITMO BFS
    // ----------------------------
    IEnumerator RunBFSCoroutine(int start)
    {
        isRunning = true;

        int n = graphManager.nodes.Count;
        bool[] visited = new bool[n];
        Queue<int> q = new Queue<int>();
        q.Enqueue(start);
        visited[start] = true;

        while (q.Count > 0)
        {
            int cur = q.Dequeue();

            // Move o player até o nó atual
            yield return StartCoroutine(MovePlayerToNode(cur));

            // Marca o nó como visitado (muda cor + mostra nome da missão)
            Node node = graphManager.nodes[cur];
            node.MarkVisited();

            // 🔊 Som opcional de "blip"
            AudioClip clip = Resources.Load<AudioClip>("Audio/blip");
            if (clip != null)
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);

            // Espera um tempinho pra visualização mais clara
            yield return new WaitForSeconds(0.3f);

            // Adiciona os vizinhos não visitados à fila
            foreach (int nei in graphManager.adjacency[cur])
            {
                if (!visited[nei])
                {
                    visited[nei] = true;
                    q.Enqueue(nei);
                }
            }
        }

        isRunning = false;
    }

    // ----------------------------
    // ALGORITMO DFS
    // ----------------------------
    IEnumerator RunDFSCoroutine(int start)
    {
        isRunning = true;

        int n = graphManager.nodes.Count;
        bool[] visited = new bool[n];
        Stack<int> st = new Stack<int>();
        st.Push(start);
        visited[start] = true;

        while (st.Count > 0)
        {
            int cur = st.Pop();

            // Move o player até o nó atual
            yield return StartCoroutine(MovePlayerToNode(cur));

            // Marca o nó como visitado (muda cor + mostra nome)
            Node node = graphManager.nodes[cur];
            node.MarkVisited();

            // 🔊 Som de "blip" opcional
            AudioClip clip = Resources.Load<AudioClip>("Audio/blip");
            if (clip != null)
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);

            yield return new WaitForSeconds(0.3f);

            // Empilha os vizinhos (de trás pra frente)
            List<int> neigh = graphManager.adjacency[cur];
            for (int i = neigh.Count - 1; i >= 0; i--)
            {
                int v = neigh[i];
                if (!visited[v])
                {
                    visited[v] = true;
                    st.Push(v);
                }
            }
        }

        isRunning = false;
    }

    // ----------------------------
    // MOVIMENTO DO PLAYER ENTRE OS NÓS
    // ----------------------------
    IEnumerator MovePlayerToNode(int nodeIndex)
    {
        if (player == null) yield break;

        Vector3 target = graphManager.nodes[nodeIndex].transform.position;
        while (Vector3.Distance(player.transform.position, target) > 0.01f)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}
