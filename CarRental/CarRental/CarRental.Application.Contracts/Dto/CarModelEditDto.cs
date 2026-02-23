namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для создания и обновления модели автомобиля
/// </summary>
/// <param name="Name">Название модели</param>
/// <param name="DriveType">Тип привода</param>
/// <param name="SeatsCount">Число мест</param>
/// <param name="BodyType">Тип кузова</param>
/// <param name="Class">Класс автомобиля</param>
public record CarModelEditDto(
    string Name,
    string DriveType,
    int SeatsCount,
    string BodyType,
    string Class
);
