namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для создания и обновления поколения модели
/// </summary>
/// <param name="ModelId">Идентификатор модели</param>
/// <param name="Year">Год выпуска</param>
/// <param name="EngineVolume">Объём двигателя (л)</param>
/// <param name="Transmission">Тип КПП</param>
/// <param name="RentalPricePerHour">Стоимость аренды в час (₽)</param>
public record ModelGenerationEditDto(
    int ModelId,
    int Year,
    double EngineVolume,
    string Transmission,
    decimal RentalPricePerHour
);
