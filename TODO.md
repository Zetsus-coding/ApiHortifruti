// ---------------------------------- //

** ALL = (Tudo / Todos)
** TODO = (A fazer)
** NS = NOT STARTED (Não iniciado)
** DONE = (Finalizado)
** DOING = (Em andamento / Fazendo)
** MAYBE = (Talvez)

// ---------------------------------- //

1. (ALL) TODO -> DTO POST (Checar), DTO GET, DTO PUT, DTO RESPONSE;
2. (ALL) TODO -> Rever assinaturas dos métodos (padronização);
3. (ALL) TODO -> Método deletar dos endpoints precisa ter validação se o ID existe (ObterPorId) para evitar exception de id inválido (caso deletar esteja ativo)
    3.1. (ALL) TODO -> [REFERENTE AO ANTERIOR] Validação para todo mundo! Ex: Item referenciado em outra tabela (Categoria -> Frutas id: 1; Produto ->  Nome: Banana; idCategoria: 1)
4. (ALL) TODO -> Mover o auto mapper para a camada de serviço e adicionar validações nas próprias classes de domínio (ex: pelo construtor) além de métodos de manipulação do próprio objeto dentro dele
    4.1. & 4.2. Exemplo no arquivo de exemplos (TODO.examples.md)
5. (ALL) MAYBE -> Retirar do body os objetos criados (igual no atualizar), apenas responder com os códigos (ex: 2xx, 4xx etc.). Só preencher o body das requisições quando for necessário (em um get, por exemplo)
6. (ALL) MAYBE -> Alguns retornos não estão batendo (EntradaService CriarEntrada<void> enquanto que no EntradaController<Entrada>)
7. (FornecedorProduto) -> ObterPorId não faz sentido (porque BUSCAR só um registro?). Faz mais sentido para mim (Alexandre) buscar os registro por fornecedorId ou por produtoId;
8. (Produto) -> Obter por código precisa ser definido como será implementado no front para o back (provavelmente com o uso de LINQ "LIKE")

# DIAGRAMA DE CLASSES

- Estruturação : DONE
- Organização : DONE(?)
- TODO -> Checar stereotypes de todas as classes e relacionamentos

// ------------------------------------------------------ //

CONTROLLER ATÉ SERVICE: (DONE)

- Entrada : DONE
- ItemEntrada : DONE
- Produto : DONE
- Fornecedor : DONE
- MotivoMovimentacao : DONE
- Saida : DONE
- ItemSaida : DONE
- Funcionario : DONE
- Categoria : DONE
- UnidadeMedida : DONE
- HistoricoProduto : DONE
- FornecedorProduto : DONE
- Financeiro : DONE

// ------------------------------------------------------ //

UNIT OF WORK : DONE

- IUnitOfWork : DONE
- UnitOfWork : DONE

// ------------------------------------------------------ //

REPOSITORIES: DOING

- UnidadeMedida : DONE
- Financeiro : n/a (depende de Saida e/ou Entrada)
- Entrada : DONE
- ItemEntrada : DONE
- Produto : DONE
- Categoria : DONE
- Saida : DONE
- ItemSaida : DONE
- FornecedorProduto : DONE
- MotivoMovimentacao : DONE
- Fornecedor : DONE
- HistoricoProduto : DONE
- Funcionario : NS

// ------------------------------------------------------ //

CLASSES DE DOMÍNIO (NS)
RELACIONAMENTOS (NS)
DTOs (NS)