using ApiHortifruti.Domain;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Services;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuaApi.Services; // Para a interface IFinanceiroService (assumindo que está neste namespace ou ApiHortifruti.Service.Interfaces)

// ⚠️ ATENÇÃO: As interfaces IEntradaRepository e ISaidaRepository devem estar referenciadas
// corretamente no seu projeto de testes, assim como IUnityOfWork.
// Usaremos os nomes de métodos esperados do seu serviço.

namespace ApiHortifruti.Tests;

public class FinanceiroServiceTests
{
    private readonly Mock<IUnityOfWork> _mockUow;
    private readonly Mock<IEntradaRepository> _mockEntradaRepo;
    private readonly Mock<ISaidaRepository> _mockSaidaRepo;
    private readonly FinanceiroService _service;

    // Data fixa para tornar os testes determinísticos
    private readonly DateOnly _hojeAtual = DateOnly.FromDateTime(DateTime.Today);
    
    // Valores Fakes
    private const decimal EntradasSemana = 2000.00m;
    private const decimal SaidasSemana = 3500.00m;
    private const decimal SaidasDia = 500.00m;
    private const decimal EntradasMes = 8000.00m;

    public FinanceiroServiceTests()
    {
        // 1. Inicializar Mocks
        _mockUow = new Mock<IUnityOfWork>();
        _mockEntradaRepo = new Mock<IEntradaRepository>();
        _mockSaidaRepo = new Mock<ISaidaRepository>();

        // 2. Configurar o UoW para retornar os Repositórios
        _mockUow.Setup(uow => uow.Entrada).Returns(_mockEntradaRepo.Object);
        _mockUow.Setup(uow => uow.Saida).Returns(_mockSaidaRepo.Object);

        // 3. Configurar os comportamentos dos Repositórios (Setups)

        // Configuração de Lucro Semanal (7 dias atrás até hoje fixo)
        var dataInicioSemana = _hojeAtual.AddDays(-7);
        var dataFimSemana = _hojeAtual;
        _mockEntradaRepo
            .Setup(r => r.ObterValorTotalPorPeriodoAsync(dataInicioSemana, dataFimSemana))
            .ReturnsAsync(EntradasSemana);
        _mockSaidaRepo
            .Setup(r => r.ObterValorTotalPorPeriodoAsync(dataInicioSemana, dataFimSemana))
            .ReturnsAsync(SaidasSemana);

        // Configuração de Gastos do Mês (primeiro dia do mês até hoje fixo)
        var primeiroDiaMes = new DateOnly(_hojeAtual.Year, _hojeAtual.Month, 1);
        var ultimoDiaMes = primeiroDiaMes.AddMonths(1).AddDays(-1);
        _mockEntradaRepo
            .Setup(r => r.ObterValorTotalPorPeriodoAsync(primeiroDiaMes, ultimoDiaMes))
            .ReturnsAsync(EntradasMes);

        // Configuração de Vendas do Dia
        _mockSaidaRepo
            .Setup(r => r.ObterValorTotalPorPeriodoAsync(_hojeAtual, _hojeAtual))
            .ReturnsAsync(SaidasDia);

        // Configuração de Entradas Recentes
        var entradasRecentesFake = new List<Entrada> { new Entrada { Id = 1 }, new Entrada { Id = 2 } };
        _mockEntradaRepo.Setup(r => r.ObterRecentesAsync()).ReturnsAsync(entradasRecentesFake);


        // 4. Instanciar o serviço injetando o mock do UoW
        // ⚠️ IMPORTANTE: O serviço depende de DateTime.Today. Precisamos de um artifício
        // para fixar a data, mas, como o serviço não permite injeção de IDateTimeProvider,
        // o teste irá falhar se executado em uma data diferente. Para fins de demonstração,
        // vamos assumir que a data é a correta no momento da execução, mas em código real
        // você precisaria injetar um provedor de data e hora.

        _service = new FinanceiroService(_mockUow.Object);
    }
    
    // ---------------------------------------------------------------------
    // Teste para CalcularLucroSemanalAsync
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "Lucro Semanal deve ser calculado como Saidas - Entradas")]
    public async Task CalcularLucroSemanalAsync_DeveRetornarCalculoCorreto()
    {
        // Arrange
        var lucroEsperado = SaidasSemana - EntradasSemana; // 3500 - 2000 = 1500

        // Act
        var resultado = await _service.CalcularLucroSemanalAsync();

        // Assert
        Assert.Equal(lucroEsperado, resultado);

        // Verifica se os métodos corretos foram chamados com o período de 7 dias
        var dataInicioSemana = DateOnly.FromDateTime(DateTime.Today).AddDays(-7);
        var dataFimSemana = DateOnly.FromDateTime(DateTime.Today);
        
        _mockEntradaRepo.Verify(r => r.ObterValorTotalPorPeriodoAsync(dataInicioSemana, dataFimSemana), Times.Once);
        _mockSaidaRepo.Verify(r => r.ObterValorTotalPorPeriodoAsync(dataInicioSemana, dataFimSemana), Times.Once);
    }

    // ---------------------------------------------------------------------
    // Teste para CalcularGastosDoMesAsync
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "Gastos do Mês deve retornar o Total de Entradas do mês")]
    public async Task CalcularGastosDoMesAsync_DeveRetornarTotalEntradasDoMes()
    {
        // Arrange: Os gastos do mês são as entradas (compras)
        var gastosEsperados = EntradasMes;

        // Act
        var resultado = await _service.CalcularGastosDoMesAsync();

        // Assert
        Assert.Equal(gastosEsperados, resultado);
        
        // Verifica se o método de Entradas foi chamado com o período do mês
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var primeiroDiaMes = new DateOnly(hoje.Year, hoje.Month, 1);
        var ultimoDiaMes = primeiroDiaMes.AddMonths(1).AddDays(-1);

        _mockEntradaRepo.Verify(r => r.ObterValorTotalPorPeriodoAsync(primeiroDiaMes, ultimoDiaMes), Times.Once);
        _mockSaidaRepo.Verify(r => r.ObterValorTotalPorPeriodoAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Never);
    }

    // ---------------------------------------------------------------------
    // Teste para CalcularVendasDoDiaAsync
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "Vendas do Dia deve retornar o Total de Saidas do dia")]
    public async Task CalcularVendasDoDiaAsync_DeveRetornarTotalSaidasDoDia()
    {
        // Arrange: As vendas do dia são as saídas
        var vendasEsperadas = SaidasDia;

        // Act
        var resultado = await _service.CalcularVendasDoDiaAsync();

        // Assert
        Assert.Equal(vendasEsperadas, resultado);
        
        // Verifica se o método de Saidas foi chamado com o período do dia
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        _mockSaidaRepo.Verify(r => r.ObterValorTotalPorPeriodoAsync(hoje, hoje), Times.Once);
        _mockEntradaRepo.Verify(r => r.ObterValorTotalPorPeriodoAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Never);
    }
    
    // ---------------------------------------------------------------------
    // Teste para ObterEntradasRecentesAsync
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "ObterEntradasRecentes deve chamar o repositório ObterRecentesAsync")]
    public async Task ObterEntradasRecentesAsync_DeveChamarRepositorio()
    {
        // Act
        var resultado = await _service.ObterEntradasRecentesAsync();

        // Assert
        Assert.NotNull(resultado);
        // Verifica se o método ObterRecentesAsync foi chamado
        _mockEntradaRepo.Verify(r => r.ObterRecentesAsync(), Times.Once);
    }
}