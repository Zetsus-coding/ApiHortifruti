﻿using System;
using System.Collections.Generic;

namespace Hortifruti.Domain;

public partial class Fornecedor
{
    public int Id { get; set; }

    public string NomeFantasia { get; set; } = null!;

    public string CadastroPessoa { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string? TelefoneExtra { get; set; }

    public string Email { get; set; } = null!;

    public DateOnly DataRegistro { get; set; }

    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    public virtual ICollection<Fornecedor_produto> FornecedorProdutos { get; set; } = new List<Fornecedor_produto>();
}
