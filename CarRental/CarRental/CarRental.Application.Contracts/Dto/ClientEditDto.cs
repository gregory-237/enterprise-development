namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для создания и обновления клиента
/// </summary>
/// <param name="LicenseNumber">Номер водительского удостоверения</param>
/// <param name="FullName">ФИО</param>
/// <param name="BirthDate">Дата рождения</param>
public record ClientEditDto(
    string LicenseNumber,
    string FullName,
    DateOnly BirthDate
);
