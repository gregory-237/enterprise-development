namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для чтения данных клиента
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="LicenseNumber">Номер водительского удостоверения</param>
/// <param name="FullName">ФИО</param>
/// <param name="BirthDate">Дата рождения</param>
public record ClientGetDto(
    int Id,
    string LicenseNumber,
    string FullName,
    DateOnly BirthDate
);
