namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для создания и обновления автомобиля
/// </summary>
/// <param name="LicensePlate">Государственный регистрационный номер</param>
/// <param name="Color">Цвет кузова</param>
/// <param name="ModelGenerationId">Идентификатор поколения модели</param>
public record CarEditDto(
    string LicensePlate,
    string Color,
    int ModelGenerationId
);
