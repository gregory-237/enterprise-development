using Bogus;
using CarRental.Application.Contracts.Dto;

namespace CarRental.Generator.Host.Generator;

/// <summary>
/// Генерирует случайные договоры аренды с помощью библиотеки Bogus
/// </summary>
public static class RentalGenerator
{
    /// <summary>
    /// Сгенерировать список DTO договоров аренды на основе реальных идентификаторов
    /// </summary>
    /// <param name="count">Количество записей</param>
    /// <param name="carIds">Список допустимых идентификаторов автомобилей из базы данных</param>
    /// <param name="clientIds">Список допустимых идентификаторов клиентов из базы данных</param>
    /// <returns>Список сгенерированных DTO</returns>
    public static IList<RentalEditDto> Generate(int count, IList<int> carIds, IList<int> clientIds) =>
        new Faker<RentalEditDto>()
            .CustomInstantiator(f => new RentalEditDto(
                RentalDate:  f.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddMonths(2)),
                RentalHours: f.Random.Int(1, 72),
                CarId:       f.PickRandom(carIds),
                ClientId:    f.PickRandom(clientIds)))
            .Generate(count);
}
