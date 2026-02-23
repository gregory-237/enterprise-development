namespace CarRental.Domain.Models;

/// <summary>
/// Договор аренды автомобиля.
/// Используется в качестве контракта — фиксирует факт выдачи ТС клиенту.
/// </summary>
public class Rental
{
    /// <summary>
    /// Уникальный идентификатор договора аренды
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// FK на арендованный автомобиль
    /// </summary>
    public required int CarId { get; set; }

    /// <summary>
    /// FK на клиента
    /// </summary>
    public required int ClientId { get; set; }

    /// <summary>
    /// Дата и время выдачи автомобиля
    /// </summary>
    public required DateTime RentalDate { get; set; }

    /// <summary>
    /// Продолжительность аренды в часах
    /// </summary>
    public required int RentalHours { get; set; }

    /// <summary>
    /// Навигационное свойство: арендованный автомобиль
    /// </summary>
    public required Car Car { get; set; }

    /// <summary>
    /// Навигационное свойство: клиент-арендатор
    /// </summary>
    public required Client Client { get; set; }
}
