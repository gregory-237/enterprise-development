namespace CarRental.Domain.Models;

/// <summary>
/// Поколение модели автомобиля (справочник)
/// </summary>
public class ModelGeneration
{
    /// <summary>
    /// Уникальный идентификатор поколения
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// FK на модель автомобиля
    /// </summary>
    public required int ModelId { get; set; }

    /// <summary>
    /// Год выпуска данного поколения
    /// </summary>
    public required int Year { get; set; }

    /// <summary>
    /// Объем двигателя в литрах
    /// </summary>
    public required double EngineVolume { get; set; }

    /// <summary>
    /// Тип коробки передач (MT / AT / CVT)
    /// </summary>
    public required string Transmission { get; set; }

    /// <summary>
    /// Стоимость аренды в рублях за час
    /// </summary>
    public required decimal RentalPricePerHour { get; set; }

    /// <summary>
    /// Навигационное свойство: модель автомобиля
    /// </summary>
    public required CarModel Model { get; set; }
}
