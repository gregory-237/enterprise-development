using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Domain.Entities;

/// <summary>
/// Договор аренды автомобиля.
/// Контракт — фиксирует факт выдачи ТС клиенту.
/// </summary>
[Table("rentals")]
public class Rental
{
    /// <summary>
    /// Уникальный идентификатор договора
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// FK на арендованный автомобиль
    /// </summary>
    [Column("car_id")]
    public required int CarId { get; set; }

    /// <summary>
    /// FK на клиента-арендатора
    /// </summary>
    [Column("client_id")]
    public required int ClientId { get; set; }

    /// <summary>
    /// Дата и время выдачи автомобиля
    /// </summary>
    [Column("rental_date")]
    public required DateTime RentalDate { get; set; }

    /// <summary>
    /// Продолжительность аренды в часах
    /// </summary>
    [Column("rental_hours")]
    public required int RentalHours { get; set; }

    /// <summary>
    /// Навигационное свойство: арендованный автомобиль
    /// </summary>
    public Car? Car { get; set; }

    /// <summary>
    /// Навигационное свойство: клиент
    /// </summary>
    public Client? Client { get; set; }
}
