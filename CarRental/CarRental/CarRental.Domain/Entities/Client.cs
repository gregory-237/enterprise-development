using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Domain.Entities;

/// <summary>
/// Клиент пункта проката
/// </summary>
[Table("clients")]
public class Client
{
    /// <summary>
    /// Уникальный идентификатор клиента
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Серия и номер водительского удостоверения
    /// </summary>
    [Column("license_number")]
    [MaxLength(20)]
    public required string LicenseNumber { get; set; }

    /// <summary>
    /// ФИО клиента
    /// </summary>
    [Column("full_name")]
    [MaxLength(150)]
    public required string FullName { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    [Column("birth_date")]
    public required DateOnly BirthDate { get; set; }
}
