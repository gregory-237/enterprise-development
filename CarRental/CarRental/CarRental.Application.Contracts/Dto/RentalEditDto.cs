namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для создания и обновления договора аренды.
/// Используется в качестве контракта между генератором и сервером.
/// </summary>
/// <param name="RentalDate">Дата и время начала аренды</param>
/// <param name="RentalHours">Продолжительность аренды в часах</param>
/// <param name="CarId">Идентификатор арендованного автомобиля</param>
/// <param name="ClientId">Идентификатор клиента</param>
public record RentalEditDto(
    DateTime RentalDate,
    int RentalHours,
    int CarId,
    int ClientId
);
