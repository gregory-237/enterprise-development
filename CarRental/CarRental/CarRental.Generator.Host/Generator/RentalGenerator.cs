using Bogus;
using CarRental.Application.Contracts.Dto;

namespace CarRental.Generator.Host.Generator;

/// <summary>
/// Генерирует случайные договоры аренды с помощью библиотеки Bogus
/// </summary>
public static class RentalGenerator
{
    /// <summary>
    /// Сгенерировать список DTO договоров аренды
    /// </summary>
    /// <param name="count">Количество записей</param>
    /// <returns>Список сгенерированных DTO</returns>
    public static IList<RentalEditDto> Generate(int count) =>
        new Faker<RentalEditDto>()
            .CustomInstantiator(f => new RentalEditDto(
                RentalDate:  f.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddMonths(2)),
                RentalHours: f.Random.Int(1, 72),
                CarId:       f.Random.Int(1, 15),
                ClientId:    f.Random.Int(1, 15)))
            .Generate(count);
}
