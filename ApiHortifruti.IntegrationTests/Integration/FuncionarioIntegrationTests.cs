using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.DTO.PutFuncionarioDTO;
using ApiHortifruti.IntegrationTests.Integration.config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.IntegrationTests.Integration;

public class FuncionarioIntegrationTests : BaseIntegrationTest
{
    public FuncionarioIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Fluxo CRUD Funcionario: Criar, Ler, Atualizar e Deletar")]
    public async Task FluxoCompleto_Funcionario_DeveFuncionar()
    {
        // ====================================================================
        // 1. CREATE (POST)
        // ====================================================================

        // Arrange
        // Nota: CPF e RG precisam ser válidos se houver validação rigorosa.
        // Usei um CPF gerado válido para teste.
        var postDto = new PostFuncionarioDTO
        {
            Nome = "João Silva",
            Cpf = "080.257.441-67",
            Rg = "12.345.678-9",
            Email = "joao.silva@teste.com",
            Telefone = "(11) 98765-4321",
            TelefoneExtra = null,
            ContaBancaria = "12345-6",
            AgenciaBancaria = "0001",
            Ativo = true
        };

        // Act
        var responsePost = await HttpClient.PostAsJsonAsync("/api/Funcionario", postDto);

        // Assert POST
        if (responsePost.StatusCode != HttpStatusCode.Created)
        {
            var erro = await responsePost.Content.ReadAsStringAsync();
            Assert.Fail($"Erro no POST: {erro}");
        }

        var funcionarioCriado = await responsePost.Content.ReadFromJsonAsync<Funcionario>();
        funcionarioCriado.Should().NotBeNull();
        funcionarioCriado!.Id.Should().BeGreaterThan(0);
        funcionarioCriado.Cpf.Should().Be(postDto.Cpf);

        // Verifica no Banco
        DbContext.ChangeTracker.Clear();
        var funcionarioNoBanco = await DbContext.Funcionario.FindAsync(funcionarioCriado.Id);
        funcionarioNoBanco.Should().NotBeNull();

        // ====================================================================
        // 2. GET (CONSULTAR)
        // ====================================================================

        var responseGet = await HttpClient.GetAsync($"/api/Funcionario/{funcionarioCriado.Id}");

        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
        var funcionarioGet = await responseGet.Content.ReadFromJsonAsync<Funcionario>();
        funcionarioGet!.Email.Should().Be(postDto.Email);

        // ====================================================================
        // 3. UPDATE (PUT) com PutFuncionarioDTO
        // ====================================================================

        var putDto = new PutFuncionarioDTO
        {
            Nome = "João Silva Editado",
            Email = "joao.novo@teste.com",
            Telefone = "(11) 99999-9999",

            // REMOVA AS LINHAS DE CPF E RG DAQUI
            // Cpf = "...",  <-- Apague
            // Rg = "...",   <-- Apague

            ContaBancaria = "12345-6",
            AgenciaBancaria = "0001",
            Ativo = true
        };

        var responsePut = await HttpClient.PutAsJsonAsync($"/api/Funcionario/{funcionarioCriado.Id}", putDto);

        // Assert PUT
        if (responsePut.StatusCode != HttpStatusCode.NoContent)
        {
            var erro = await responsePut.Content.ReadAsStringAsync();
            Assert.Fail($"Erro no PUT: {erro}");
        }

        // Verifica se atualizou o nome mas MANTEVE o CPF antigo
        DbContext.ChangeTracker.Clear();
        var funcionarioAtualizado = await DbContext.Funcionario.FindAsync(funcionarioCriado.Id);

        funcionarioAtualizado!.Nome.Should().Be("João Silva Editado");
        funcionarioAtualizado.Cpf.Should().Be(postDto.Cpf); // O CPF deve ser igual ao do POST original

        // ====================================================================
        // 4. DELETE
        // ====================================================================

        var responseDelete = await HttpClient.DeleteAsync($"/api/Funcionario/{funcionarioCriado.Id}");

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        DbContext.ChangeTracker.Clear();
        var funcionarioDeletado = await DbContext.Funcionario.FindAsync(funcionarioCriado.Id);
        funcionarioDeletado.Should().BeNull();
    }
}