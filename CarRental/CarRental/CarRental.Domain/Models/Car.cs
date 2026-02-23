namespace CarRental.Domain.Models;

/// <summary>
/// Транспортное средство в парке проката
/// </summary>
public class Car
{
    /// <summary>
    /// Уникальный идентификатор ТС
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// FK на поколение модели
    /// </summary>
    public required int ModelGenerationId { get; set; }

    /// <summary>
    /// Государственный регистрационный номер
    /// </summary>
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Цвет кузова
    /// </summary>
    public required string Color { get; set; }

    /// <summary>
    /// Навигационное свойство: поколение модели
    /// </summary>
    public required ModelGeneration ModelGeneration { get; set; }
}
