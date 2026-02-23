namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для чтения данных поколения модели
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="ModelId">Идентификатор модели</param>
/// <param name="Year">Год выпуска</param>
/// <param name="EngineVolume">Объём двигателя (л)</param>
/// <param name="Transmission">Тип КПП</param>
/// <param name="RentalPricePerHour">Стоимость аренды в час (₽)</param>
/// <param name="Model">Данные модели</param>
public record ModelGenerationGetDto(
    int Id,
    int ModelId,
    int Year,
    double EngineVolume,
    string Transmission,
    decimal RentalPricePerHour,
    CarModelGetDto? Model
);
