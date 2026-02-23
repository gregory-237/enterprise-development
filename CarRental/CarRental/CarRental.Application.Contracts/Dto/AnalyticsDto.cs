namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO аналитики: автомобиль и число его аренд
/// </summary>
/// <param name="Car">Данные автомобиля</param>
/// <param name="RentalCount">Количество аренд</param>
public record CarRentalCountDto(CarGetDto Car, int RentalCount);

/// <summary>
/// DTO аналитики: клиент и суммарная стоимость его аренд
/// </summary>
/// <param name="Client">Данные клиента</param>
/// <param name="TotalAmount">Суммарная стоимость аренды в рублях</param>
public record ClientRentalAmountDto(ClientGetDto Client, decimal TotalAmount);
