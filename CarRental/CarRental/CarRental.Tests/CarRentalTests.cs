using CarRental.Domain.Data;
using Xunit;

namespace CarRental.Tests;

/// <summary>
/// Юнит-тесты для пункта проката автомобилей.
/// Петров Григорий Алексеевич, группа 6413-100503D
/// </summary>
public class CarRentalTests : IClassFixture<CarRentalFixture>
{
    private readonly CarRentalFixture fixture;

    public CarRentalTests(CarRentalFixture fixture)
    {
        this.fixture = fixture;
        fixture.WireNavigations();
    }

    /// <summary>
    /// ТЕСТ 1: Вывести информацию обо всех клиентах,
    /// которые брали в аренду автомобили указанной модели, упорядочить по ФИО.
    /// Ожидаем трёх клиентов, арендовавших Toyota RAV4.
    /// </summary>
    [Fact]
    public void GetClientsByModelSortedByName()
    {
        const string targetModel = "Toyota RAV4";
        const int expectedCount = 3;
        const string expectedFirstName  = "Alexei Nikitin";
        const string expectedSecondName = "Sergei Volkov";
        const string expectedThirdName  = "Vasily Nekrasov";

        var result = fixture.Rentals
            .Where(r => r.Car.ModelGeneration.Model.Name == targetModel)
            .Select(r => r.Client)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        Assert.Equal(expectedCount, result.Count);
        Assert.Equal(expectedFirstName,  result[0].FullName);
        Assert.Equal(expectedSecondName, result[1].FullName);
        Assert.Equal(expectedThirdName,  result[2].FullName);
    }

    /// <summary>
    /// ТЕСТ 2: Вывести информацию об автомобилях, находящихся в аренде
    /// на указанный момент времени.
    /// </summary>
    [Fact]
    public void GetCurrentlyRentedCars()
    {
        var checkDate = new DateTime(2025, 3, 5, 12, 0, 0);
        // Toyota RAV4 (Id=4, "E444UF77") взята 2025-03-04 10:00 на 48ч → возврат 2025-03-06 10:00
        const string expectedActivePlate = "E444UF77";

        var activeCars = fixture.Rentals
            .Where(r => r.RentalDate.AddHours(r.RentalHours) > checkDate)
            .Select(r => r.Car)
            .Distinct()
            .ToList();

        Assert.Contains(activeCars, c => c.LicensePlate == expectedActivePlate);
    }

    /// <summary>
    /// ТЕСТ 3: Вывести топ 5 наиболее часто арендуемых автомобилей.
    /// Toyota RAV4 (Id=4) лидирует с 3 арендами.
    /// </summary>
    [Fact]
    public void GetTop5MostRentedCars()
    {
        const int expectedTopCount = 5;
        const string expectedTopPlate = "E444UF77";   // Toyota RAV4
        const int expectedTopRentalCount = 3;

        var topCars = fixture.Rentals
            .GroupBy(r => r.Car)
            .Select(g => new { Car = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.Equal(expectedTopCount, topCars.Count);
        Assert.Equal(expectedTopPlate, topCars[0].Car.LicensePlate);
        Assert.Equal(expectedTopRentalCount, topCars[0].Count);
    }

    /// <summary>
    /// ТЕСТ 4: Для каждого автомобиля вывести число аренд.
    /// Всего 15 ТС; Toyota RAV4 (Car.Id=4) — 3 аренды, Mercedes (Car.Id=1) — 2 аренды.
    /// </summary>
    [Fact]
    public void GetRentalCountPerCar()
    {
        const int expectedTotalVehicles   = 15;
        const int toyotaCarId             = 4;
        const int mercedesCarId           = 1;
        const int expectedToyotaRentals   = 3;
        const int expectedMercedesRentals = 2;

        var stats = fixture.Cars
            .Select(car => new
            {
                Car         = car,
                RentalCount = fixture.Rentals.Count(r => r.CarId == car.Id)
            })
            .ToList();

        Assert.Equal(expectedTotalVehicles, stats.Count);

        var toyotaStat   = stats.Single(s => s.Car.Id == toyotaCarId);
        var mercedesStat = stats.Single(s => s.Car.Id == mercedesCarId);

        Assert.Equal(expectedToyotaRentals,   toyotaStat.RentalCount);
        Assert.Equal(expectedMercedesRentals, mercedesStat.RentalCount);
        Assert.True(stats.All(s => s.RentalCount >= 0));
    }

    /// <summary>
    /// ТЕСТ 5: Вывести топ 5 клиентов по суммарной стоимости аренды.
    /// Лидер — Konstantin Zhukov (аренда Ferrari 488: 96ч × 15 000₽ = 1 440 000₽).
    /// </summary>
    [Fact]
    public void GetTop5ClientsByRentalAmount()
    {
        const int expectedCount = 5;
        const string expectedTopClientName = "Konstantin Zhukov";

        var topClients = fixture.Rentals
            .GroupBy(r => r.Client)
            .Select(g => new
            {
                Client      = g.Key,
                TotalAmount = g.Sum(r => r.RentalHours * r.Car.ModelGeneration.RentalPricePerHour)
            })
            .OrderByDescending(x => x.TotalAmount)
            .Take(5)
            .ToList();

        Assert.Equal(expectedCount, topClients.Count);
        Assert.Equal(expectedTopClientName, topClients[0].Client.FullName);
    }
}
