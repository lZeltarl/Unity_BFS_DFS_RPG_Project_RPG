🧭 Explicação do Funcionamento na Unity

Quando o projeto é iniciado (ao apertar Play na Unity), o sistema cria automaticamente um mapa de missões interligadas em um plano 2D.
Cada nó (representado por um círculo colorido) simboliza uma missão do jogador dentro do mundo do jogo.

🟢 Etapa 1: Geração do Mapa

Assim que o jogo começa:

O GraphManager instancia vários Nodes (nós) no espaço, usando o NodePrefab.

Esses nós são posicionados de forma distribuída no mapa e conectados entre si por linhas que representam os caminhos possíveis.

O fundo do mapa é exibido atrás (um sprite que representa o ambiente, como um mapa ou diagrama).

O resultado é uma estrutura de pontos conectados, semelhante a um mapa de rotas.

🟣 Etapa 2: Início da Busca (BFS ou DFS)

Após o mapa ser criado, o sistema inicia um dos algoritmos de busca:

BFS (Busca em Largura): percorre os nós camada por camada, começando pelo nó inicial e explorando todos os vizinhos antes de avançar.

DFS (Busca em Profundidade): segue um caminho até o fim antes de voltar e explorar os próximos.

Durante a execução:

O nó atual sendo visitado muda de vermelho para verde.

Isso indica que ele já foi descoberto ou completado.

🟡 Etapa 3: Exibição de Nomes das Missões

Cada vez que um nó é visitado:

O nome da missão aparece logo acima dele em texto branco.

Esse nome é gerado dinamicamente pelo script Node.cs, que cria um objeto de texto (TextMeshPro) posicionado acima do sprite.

O texto fica visível por alguns segundos e depois desaparece, simulando um “popup de missão completada”.

Visualmente, isso ajuda a acompanhar o progresso da busca — mostrando quais missões estão sendo visitadas e em qual ordem.

🔵 Etapa 4: Atualização Visual Contínua

Enquanto o algoritmo percorre o mapa:

Os nós visitados permanecem verdes.

Os não visitados continuam vermelhos.

As conexões entre eles permanecem visíveis, indicando o caminho que a busca está explorando.

Isso cria um efeito visual dinâmico, mostrando o funcionamento interno do algoritmo passo a passo, como se fosse um mapa de progresso de missões em tempo real.

🧠 Interpretação Visual (para o professor)

Em resumo:

Cada círculo vermelho = missão ainda não explorada.

Cada círculo verde = missão já visitada.

Texto branco acima do nó = nome da missão que acabou de ser alcançada.

Linhas entre nós = caminhos disponíveis entre missões.

O sistema mostra como a IA percorre o mapa, simulando o comportamento de exploração automática.
