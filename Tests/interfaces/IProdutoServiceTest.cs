namespace ApiHortifruti.Tests.Interfaces.IProdutoServiceTests
{
    public interface IDateTimeProvider
    {
        DateOnly Today { get; }
    }
}