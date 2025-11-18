// ------------------------------------------------------ //

4.1 (Exemplo #1 do item '4' - Com o uso do construtor): 

    public class ProdutoService
    {
        // [...]

        public Produto CriarNovoProduto(CriarProdutoDto dto)
        {
            // 1. Validações de banco (ex: nome duplicado)
            if (_produtoRepository.ExisteProdutoComNome(dto.Nome))
            {
                throw new InvalidOperationException("Já existe um produto com este nome.");
            }

            // AutoMapper (tem que ser definido nos profiles) chama o construtor da classe de domínio para criar um objeto dessa classe
            // Dentro do construtor teria validações da classe de domínio
            var novoProduto = _mapper.Map<Produto>(dto);

            // 3. Persistência
            _produtoRepository.Adicionar(novoProduto);

            return novoProduto;
        }
    }

// ------------------------------------------------------ //

4.2 (Exemplo #2 do item '4' - Com o uso do métodos de manipulação, próprios, do objeto):

    public void RealizarBaixaDeEstoque(int produtoId, int quantidade)
    {
        // Busca a entidade no repositório (ObterPorId)
        var produto = _produtoRepository.GetById(produtoId);
        
        // Verifica se produto existe
        if (produto == null)
        {
            throw new KeyNotFoundException("Produto não encontrado.");
        }

        // Service não modifica o objeto diretamente. Ele deixa a responsabilidade para a entidade
        produto.DarBaixaNoEstoque(quantidade);

        // O Service chama o repositório (update) e depois salva
        _produtoRepository.Update(produto);
        _context.SaveChanges();
        // Commit aqui
    }

    public class Produto
    {
        // Exemplos de propriedades
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public decimal Preco { get; private set; }
        public int Estoque { get; private set; }

        // Construtor(es)
         public Produto(string nome, decimal preco, int estoqueInicial)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do produto não pode ser vazio.");

            if (preco <= 0) // Regra de negócio
                throw new ArgumentException("O preço deve ser positivo.");

            if (estoqueInicial < 0) // Regra de negócio
                throw new ArgumentException("O estoque inicial não pode ser negativo.");

            Nome = nome;
            Preco = preco;
            Estoque = estoqueInicial;
        }

        public void DarBaixaNoEstoque(int quantidade)
        {
            // Validação regra de negócio #1
            if (quantidade <= 0)
            {
                throw new InvalidOperationException("A quantidade da baixa deve ser positiva.");
            }

            // Validação regra de negócio #2
            if (this.Estoque < quantidade)
            {
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {this.Estoque}, Solicitado: {quantidade}");
            }

            // Alteração do estado interno do objeto
            // Apenas se todas as validações (guardas) passarem, o estado é modificado.
            this.Estoque -= quantidade;
        }

        public void AdicionarAoEstoque(int quantidade)
        {
            if (quantidade <= 0)
            {
                throw new InvalidOperationException("A quantidade a ser adicionada deve ser positiva.");
            }
            this.Estoque += quantidade;
        }
    }

// ------------------------------------------------------ //