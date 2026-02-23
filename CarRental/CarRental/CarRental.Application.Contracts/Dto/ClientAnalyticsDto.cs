namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для отображения сумм арендной платы клиентов
/// </summary>
/// <param name="Client">Информация о клиенте</param>
/// <param name="TotalAmount">Общая сумма арендной платы для данного клиента</param>
public record ClientRentalAmountDto(ClientGetDto Client, decimal TotalAmount);
