namespace CarRental.Domain.Models;

/// <summary>
/// Модель автомобиля (справочник)
/// </summary>
public class CarModel
{
    /// <summary>
    /// Уникальный идентификатор модели
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название модели (например, "Toyota RAV4")
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Тип привода (FWD / RWD / AWD / 4WD)
    /// </summary>
    public required string DriveType { get; set; }

    /// <summary>
    /// Число посадочных мест
    /// </summary>
    public required int SeatsCount { get; set; }

    /// <summary>
    /// Тип кузова (Sedan, SUV, Coupe и т.д.)
    /// </summary>
    public required string BodyType { get; set; }

    /// <summary>
    /// Класс автомобиля (Economy, Premium, Luxury и т.д.)
    /// </summary>
    public required string Class { get; set; }
}
