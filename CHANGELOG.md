Aqui está o arquivo **CHANGELOG.md** atualizado com todas as alterações que discutimos neste chat. As mudanças foram agrupadas em uma única versão (`1.3.0`), que inclui todas as melhorias e funcionalidades adicionadas.

---

# Changelog

## [1.0.0] - 2024-06-30
### First Release
- **Inserção da Smart Scene**: Primeira versão da biblioteca para gerenciamento de cenas na Unity.

---

## [1.1.0] - 2024-07-02
### Improvements
- **Criação do Smart Scene Management**: Implementação inicial do gerenciador de cenas, permitindo agrupar cenas e carregá-las de forma assíncrona.

---

## [1.2.0] - 2024-10-13
### Support Coroutine
- **Suporte a Corrotinas**: As funções principais agora suportam operações assíncronas usando corrotinas.

---

## [1.3.0] - 2024-10-14
### Major Improvements and Features
- **Refatoração do Código**:
    - Substituição de `IList<string>` por `SceneGroupData` para representar grupos de cenas.
    - Adição de métodos auxiliares em `SceneGroupData` (`AddScene`, `RemoveScene`, `ContainsScene`, `ClearScenes`, etc.).
    - Uso de `SceneGroupDataSO` (ScriptableObject) para facilitar a criação e gerenciamento de grupos de cenas no Editor da Unity.
    - Adição de suporte a `SceneGroupDataSO` como parâmetro nos métodos principais.
    - Criação de métodos auxiliares para encapsular a lógica de carregamento e descarregamento de cenas.

- **Melhorias na Interface**:
    - Adição de novos métodos na interface `ISmartSceneManagement` para suportar `SceneGroupData` e `SceneGroupDataSO`.
    - Documentação XML completa para todas as classes, métodos e propriedades.

- **Exemplos e Tutoriais**:
    - Adição de exemplos práticos no `README.md`, incluindo um `GameManager` completo para gerenciar a troca de cenas.
    - Passo a passo detalhado para instalação e configuração da biblioteca.

- **Correções de Bugs**:
    - Correção de recursão em métodos que causavam conflitos com a interface `ISmartSceneManagement`.
    - Melhoria no fluxo de carregamento e descarregamento de cenas para evitar vazamentos de memória.