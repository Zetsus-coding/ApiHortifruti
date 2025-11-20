using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service.Provider;

public class SystemDateTimeProvider : IDateTimeProvider
{
    // Quando o código rodar em produção, ele pega a hora real do servidor
    public DateTime Now => DateTime.Now;
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
}