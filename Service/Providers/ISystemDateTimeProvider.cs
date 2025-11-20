using System;

namespace ApiHortifruti.Service.Interfaces;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateOnly Today { get; }
}