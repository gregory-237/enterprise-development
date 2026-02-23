namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для чтения данных модели автомобиля
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="Name">Название модели</param>
/// <param name="DriveType">Тип привода</param>
/// <param name="SeatsCount">Число мест</param>
/// <param name="BodyType">Тип кузова</param>
/// <param name="Class">Класс автомобиля</param>
public record CarModelGetDto(
    int Id,
    string Name,
    string DriveType,
    int SeatsCount,
    string BodyType,
    string Class
);
