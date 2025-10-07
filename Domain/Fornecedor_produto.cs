﻿using System;
using System.Collections.Generic;

namespace Hortifruti.Domain;

/// <summary>
/// Necessário a chave primária id? 
/// </summary>

public partial class Fornecedor_produto
{
    public int FornecedorId { get; set; }

    public int ProdutoId { get; set; }

    public string CodigoFornecedor { get; set; } = null!;

    public DateOnly DataRegistro { get; set; }

    public bool Disponibilidade { get; set; }

    public virtual Fornecedor? Fornecedor { get; set; }

    public virtual Produto? Produto { get; set; }
}
