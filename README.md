# SmartScene - Gerenciamento de Cenas na Unity

A **SmartScene** é uma biblioteca para Unity que facilita o gerenciamento de cenas, permitindo agrupar cenas em "Scene Groups" e carregá-las de forma assíncrona. Ela é ideal para projetos que exigem carregamento dinâmico de cenas, como jogos com múltiplas fases, interfaces de usuário complexas ou sistemas de carregamento progressivo.

---

## Instalação

### Passo a Passo para Instalar via Package Manager

1. Abra o Unity e navegue até o **Package Manager**:
  - No menu superior, clique em `Window > Package Manager`.

2. No Package Manager, clique no botão **"+"** no canto superior esquerdo e selecione **"Add package from git URL..."**.

3. Insira o seguinte Git URL:
   ```
   https://github.com/Louiixx-h/smart-scene-unity-library.git
   ```

4. Clique em **"Add"**. O Unity baixará e importará automaticamente a biblioteca **SmartScene** para o seu projeto.

5. Aguarde o processo de importação ser concluído. A biblioteca estará pronta para uso!

---

## Como Usar

### 1. Criando um Scene Group

Um `Scene Group` é uma lista de cenas que podem ser carregadas juntas. Para criar um, use o `SceneGroupDataSo` (ScriptableObject).

#### Passos:

1. No Unity, clique com o botão direito no **Project Window** e selecione `Create > SmartScene > SceneGroupDataSo`.
2. Nomeie o arquivo (por exemplo, `InitialSceneGroup`).
3. No Inspector, defina o nome do grupo e a lista de cenas.

---

### 2. Usando `SceneGroupDataSO` no `SceneConfig`

Para usar os dados de um `SceneGroupDataSO` no `SceneConfig`, você pode acessar o método `GetData()` do `SceneGroupDataSO`. Aqui está um exemplo:

```csharp
// Suponha que você tenha uma referência ao SceneGroupDataSO
[SerializeField] private SceneGroupDataSo sceneGroupDataSo;

// Crie um SceneConfig usando o SceneGroupDataSO
var sceneConfig = new SceneConfig.SceneConfigBuilder()
    .SetSceneGroup(sceneGroupDataSo.GetData()) // Obtém os dados do SceneGroupDataSO
    .SetProgress(new Progress<float>(progress => Debug.Log($"Progress: {progress * 100}%")))
    .SetIgnoreIfAlreadyLoaded(true)
    .Build();
```

#### Explicação:

- **`sceneGroupDataSo.GetData()`**: Retorna o `SceneGroupData` (struct) armazenado no `SceneGroupDataSO`.
- **`SetSceneGroup`**: Define o grupo de cenas a ser carregado.
- **`SetProgress`**: Define um objeto para reportar o progresso do carregamento.
- **`SetIgnoreIfAlreadyLoaded`**: Ignora cenas já carregadas, se necessário.

---

### 3. Configurando o `GameManager`

O `GameManager` é responsável por iniciar o carregamento das cenas e gerenciar os eventos de carregamento. Aqui está um exemplo de implementação:

```csharp
using Com.LuisLabs.SmartScene;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private SceneGroupDataSo initialSceneGroupData; // Referência ao ScriptableObject no Inspector
    
    private ISmartSceneManagement _smartSceneManagement;

    protected override void Awake()
    {
        base.Awake();
        _smartSceneManagement = GetComponent<SmartSceneManagement>(); // Obtém o gerenciador de cenas
    }

    private void Start()
    {
        // Carrega o grupo de cenas inicial
        SwitchSceneGroup(initialSceneGroupData);
    }

    private void OnEnable()
    {
        // Inscreve-se nos eventos de carregamento
        _smartSceneManagement.OnLoadingStart += OnLoadingStart;
        _smartSceneManagement.OnLoadingEnd += OnLoadingEnd;
    }
    
    private void OnDisable()
    {
        // Remove a inscrição nos eventos de carregamento
        _smartSceneManagement.OnLoadingStart -= OnLoadingStart;
        _smartSceneManagement.OnLoadingEnd -= OnLoadingEnd;
    }

    /// <summary>
    /// Troca para um novo grupo de cenas.
    /// </summary>
    /// <param name="sceneGroupData">O ScriptableObject contendo o grupo de cenas.</param>
    public void SwitchSceneGroup(SceneGroupDataSo sceneGroupData)
    {
        var sceneConfig = new SceneConfig.SceneConfigBuilder()
            .SetSceneGroup(sceneGroupData.GetData()) // Obtém os dados do grupo de cenas
            .Build();

        // Inicia o carregamento assíncrono
        StartCoroutine(_smartSceneManagement.SwitchSceneGroupAsync(sceneConfig));
    }
    
    private void OnLoadingStart()
    {
        Debug.Log("Loading started"); // Log quando o carregamento começa
    }
    
    private void OnLoadingEnd()
    {
        Debug.Log("Loading ended"); // Log quando o carregamento termina
    }
}
```

#### Explicação:

- **PersistentSingleton**: Garante que o `GameManager` persista entre cenas e tenha apenas uma instância.
- **SceneGroupDataSo**: Referência ao ScriptableObject que contém o grupo de cenas inicial.
- **SwitchSceneGroup**: Método para trocar o grupo de cenas ativo.
- **Eventos de Carregamento**: `OnLoadingStart` e `OnLoadingEnd` são usados para notificar o início e o fim do carregamento.

---

### 4. Configurando o `SmartSceneManagement`

Adicione o componente `SmartSceneManagement` ao mesmo GameObject que contém o `GameManager`. Isso permitirá que o `GameManager` acesse e controle o carregamento de cenas.

---

### 5. Criando e Configurando o `SceneGroupDataSo`

1. Crie um `SceneGroupDataSo` no Unity (como explicado na seção 3).
2. Defina as cenas que devem ser carregadas no grupo (por exemplo, `MainMenu`, `UI`, `Logic`).
3. Arraste o `SceneGroupDataSo` criado para o campo `initialSceneGroupData` no Inspector do `GameManager`.

---

### 6. Executando o Projeto

Ao iniciar o jogo, o `GameManager` carregará automaticamente o grupo de cenas definido em `initialSceneGroupData`. Você verá logs no console indicando o início e o fim do carregamento.

---

## Documentação da API

### `SmartSceneManagement`

- **Propriedades**:
  - `CurrentSceneGroup`: Grupo de cenas atualmente carregadas.
  - `CurrentPersistentSceneGroup`: Grupo de cenas persistentes carregadas.
  - `ActiveScene`: Cena ativa no momento.
  - `SceneCount`: Número de cenas carregadas.
  - `OnLoadingStart`: Evento disparado ao iniciar o carregamento.
  - `OnLoadingEnd`: Evento disparado ao finalizar o carregamento.

- **Métodos**:
  - `SwitchSceneGroupAsync(SceneConfig)`: Troca para um novo grupo de cenas.
  - `LoadSceneToCurrentGroupAsync(SceneConfig)`: Adiciona cenas ao grupo atual.
  - `LoadPersistentSceneAsync(SceneConfig)`: Carrega cenas persistentes.
  - `UnloadSceneAsync(string)`: Descarrega uma cena do grupo atual.
  - `UnloadPersistentSceneAsync(string)`: Descarrega uma cena persistente.
  - `GetSceneAt(int)`: Retorna a cena no índice especificado.

### `SceneConfig`

- **Propriedades**:
  - `Progress`: Objeto para reportar o progresso do carregamento.
  - `SceneGroup`: Grupo de cenas a serem carregadas.
  - `IgnoreIfAlreadyLoaded`: Ignora cenas já carregadas.

- **Builder**:
  - `SetSceneGroup(SceneGroupData)`: Define o grupo de cenas.
  - `SetProgress(IProgress<float>)`: Define o objeto de progresso.
  - `SetIgnoreIfAlreadyLoaded(bool)`: Define se cenas já carregadas devem ser ignoradas.
  - `Build()`: Constrói o objeto `SceneConfig`.

### `SceneGroupDataSo`

- **Métodos**:
  - `GetData()`: Retorna o `SceneGroupData` armazenado no ScriptableObject.

---

## Exemplo Completo

Aqui está um exemplo completo de como usar a biblioteca com o `GameManager`:

1. Crie um `SceneGroupDataSo` chamado `InitialSceneGroup` e defina as cenas `MainMenu`, `UI` e `Logic`.
2. Adicione o `GameManager` e o `SmartSceneManagement` a um GameObject na cena.
3. Arraste o `InitialSceneGroup` para o campo `initialSceneGroupData` no Inspector do `GameManager`.
4. Execute o projeto. O grupo de cenas será carregado automaticamente, e você verá logs no console indicando o progresso.

---

## Contribuindo

Se você quiser contribuir para o projeto, sinta-se à vontade para abrir uma **issue** ou enviar um **pull request**. Todas as contribuições são bem-vindas!

---

## Licença

Este projeto está licenciado sob a licença MIT. Consulte o arquivo [LICENSE](LICENSE) para mais detalhes.