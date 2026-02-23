namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для чтения данных договора аренды
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="CarId">Идентификатор автомобиля</param>
/// <param name="ClientId">Идентификатор клиента</param>
/// <param name="RentalDate">Дата и время выдачи</param>
/// <param name="RentalHours">Продолжительность аренды в часах</param>
/// <param name="Car">Данные автомобиля</param>
/// <param name="Client">Данные клиента</param>
public record RentalGetDto(
    int Id,
    int CarId,
    int ClientId,
    DateTime RentalDate,
    int RentalHours,
    CarGetDto? Car,
    ClientGetDto? Client
);
