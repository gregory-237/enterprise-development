using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Domain.Entities;

/// <summary>
/// Транспортное средство в парке проката
/// </summary>
[Table("cars")]
public class Car
{
    /// <summary>
    /// Уникальный идентификатор ТС
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Государственный регистрационный номер
    /// </summary>
    [Column("license_plate")]
    [MaxLength(20)]
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Цвет кузова
    /// </summary>
    [Column("color")]
    [MaxLength(30)]
    public required string Color { get; set; }

    /// <summary>
    /// FK на поколение модели
    /// </summary>
    [Column("model_generation_id")]
    public required int ModelGenerationId { get; set; }

    /// <summary>
    /// Навигационное свойство: поколение модели
    /// </summary>
    public ModelGeneration? ModelGeneration { get; set; }
}
