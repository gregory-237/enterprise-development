using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Domain.Entities;

/// <summary>
/// Модель автомобиля (справочник)
/// </summary>
[Table("car_models")]
public class CarModel
{
    /// <summary>
    /// Уникальный идентификатор модели
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Название модели (например, "Toyota RAV4")
    /// </summary>
    [Column("name")]
    [MaxLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// Тип привода (FWD / RWD / AWD / 4WD)
    /// </summary>
    [Column("drive_type")]
    [MaxLength(10)]
    public required string DriveType { get; set; }

    /// <summary>
    /// Число посадочных мест
    /// </summary>
    [Column("seats_count")]
    public required int SeatsCount { get; set; }

    /// <summary>
    /// Тип кузова (Sedan, SUV, Coupe и т.д.)
    /// </summary>
    [Column("body_type")]
    [MaxLength(30)]
    public required string BodyType { get; set; }

    /// <summary>
    /// Класс автомобиля (Economy, Premium, Luxury и т.д.)
    /// </summary>
    [Column("class")]
    [MaxLength(30)]
    public required string Class { get; set; }
}
