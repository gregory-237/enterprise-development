namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для чтения данных автомобиля
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="LicensePlate">Государственный регистрационный номер</param>
/// <param name="Color">Цвет кузова</param>
/// <param name="ModelGenerationId">Идентификатор поколения модели</param>
/// <param name="ModelGeneration">Данные поколения модели</param>
public record CarGetDto(
    int Id,
    string LicensePlate,
    string Color,
    int ModelGenerationId,
    ModelGenerationGetDto? ModelGeneration
);
