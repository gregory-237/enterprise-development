using CarRental.Domain.Entities;

namespace CarRental.Domain.Data;

/// <summary>
/// Тестовые данные для пункта проката автомобилей.
/// Используется для первоначального наполнения БД через EF Core HasData.
/// </summary>
public class CarRentalFixture
{
    public List<CarModel> CarModels { get; }
    public List<ModelGeneration> ModelGenerations { get; }
    public List<Car> Cars { get; }
    public List<Client> Clients { get; }
    public List<Rental> Rentals { get; }

    public CarRentalFixture()
    {
        CarModels =
        [
            new() { Id = 1,  Name = "Mercedes C-Class",      DriveType = "RWD", SeatsCount = 5, BodyType = "Sedan",  Class = "Premium"   },
            new() { Id = 2,  Name = "Volkswagen Passat",      DriveType = "FWD", SeatsCount = 5, BodyType = "Sedan",  Class = "Business"  },
            new() { Id = 3,  Name = "Kia Rio",                DriveType = "FWD", SeatsCount = 5, BodyType = "Sedan",  Class = "Economy"   },
            new() { Id = 4,  Name = "Toyota RAV4",            DriveType = "AWD", SeatsCount = 5, BodyType = "SUV",    Class = "Mid-size"  },
            new() { Id = 5,  Name = "Ferrari 488",            DriveType = "RWD", SeatsCount = 2, BodyType = "Coupe",  Class = "Supercar"  },
            new() { Id = 6,  Name = "Nissan Patrol",          DriveType = "4WD", SeatsCount = 7, BodyType = "SUV",    Class = "Full-size" },
            new() { Id = 7,  Name = "Renault Logan",          DriveType = "FWD", SeatsCount = 5, BodyType = "Sedan",  Class = "Economy"   },
            new() { Id = 8,  Name = "Mazda CX-5",             DriveType = "AWD", SeatsCount = 5, BodyType = "SUV",    Class = "Mid-size"  },
            new() { Id = 9,  Name = "Ford Transit",           DriveType = "RWD", SeatsCount = 3, BodyType = "Van",    Class = "Commercial"},
            new() { Id = 10, Name = "Mitsubishi Outlander",   DriveType = "AWD", SeatsCount = 5, BodyType = "SUV",    Class = "Mid-size"  },
            new() { Id = 11, Name = "Land Rover Defender",    DriveType = "4WD", SeatsCount = 5, BodyType = "SUV",    Class = "Luxury"    },
            new() { Id = 12, Name = "Volvo XC60",             DriveType = "AWD", SeatsCount = 5, BodyType = "SUV",    Class = "Premium"   },
            new() { Id = 13, Name = "Cadillac Escalade",      DriveType = "AWD", SeatsCount = 7, BodyType = "SUV",    Class = "Luxury"    },
            new() { Id = 14, Name = "Skoda Octavia",          DriveType = "FWD", SeatsCount = 5, BodyType = "Sedan",  Class = "Business"  },
            new() { Id = 15, Name = "Niva Legend",            DriveType = "4WD", SeatsCount = 5, BodyType = "SUV",    Class = "Off-road"  },
        ];

        ModelGenerations =
        [
            new() { Id = 1,  Year = 2023, EngineVolume = 2.0, Transmission = "AT",  RentalPricePerHour = 2500,  ModelId = 1  },
            new() { Id = 2,  Year = 2022, EngineVolume = 1.8, Transmission = "AT",  RentalPricePerHour = 1800,  ModelId = 2  },
            new() { Id = 3,  Year = 2024, EngineVolume = 1.4, Transmission = "AT",  RentalPricePerHour = 900,   ModelId = 3  },
            new() { Id = 4,  Year = 2023, EngineVolume = 2.5, Transmission = "AT",  RentalPricePerHour = 2200,  ModelId = 4  },
            new() { Id = 5,  Year = 2021, EngineVolume = 3.9, Transmission = "AT",  RentalPricePerHour = 15000, ModelId = 5  },
            new() { Id = 6,  Year = 2023, EngineVolume = 4.0, Transmission = "AT",  RentalPricePerHour = 4000,  ModelId = 6  },
            new() { Id = 7,  Year = 2024, EngineVolume = 1.6, Transmission = "MT",  RentalPricePerHour = 800,   ModelId = 7  },
            new() { Id = 8,  Year = 2024, EngineVolume = 2.0, Transmission = "AT",  RentalPricePerHour = 2000,  ModelId = 8  },
            new() { Id = 9,  Year = 2022, EngineVolume = 2.2, Transmission = "MT",  RentalPricePerHour = 1600,  ModelId = 9  },
            new() { Id = 10, Year = 2023, EngineVolume = 2.0, Transmission = "CVT", RentalPricePerHour = 1900,  ModelId = 10 },
            new() { Id = 11, Year = 2024, EngineVolume = 3.0, Transmission = "AT",  RentalPricePerHour = 7000,  ModelId = 11 },
            new() { Id = 12, Year = 2023, EngineVolume = 2.0, Transmission = "AT",  RentalPricePerHour = 3500,  ModelId = 12 },
            new() { Id = 13, Year = 2022, EngineVolume = 6.2, Transmission = "AT",  RentalPricePerHour = 5500,  ModelId = 13 },
            new() { Id = 14, Year = 2024, EngineVolume = 1.5, Transmission = "AT",  RentalPricePerHour = 1400,  ModelId = 14 },
            new() { Id = 15, Year = 2023, EngineVolume = 1.7, Transmission = "MT",  RentalPricePerHour = 950,   ModelId = 15 },
        ];

        Cars =
        [
            new() { Id = 1,  LicensePlate = "A001MB77",  Color = "Black",  ModelGenerationId = 1  },
            new() { Id = 2,  LicensePlate = "B222NO77",  Color = "White",  ModelGenerationId = 2  },
            new() { Id = 3,  LicensePlate = "C333RT99",  Color = "Silver", ModelGenerationId = 3  },
            new() { Id = 4,  LicensePlate = "E444UF77",  Color = "Blue",   ModelGenerationId = 4  },
            new() { Id = 5,  LicensePlate = "K555FH77",  Color = "Red",    ModelGenerationId = 5  },
            new() { Id = 6,  LicensePlate = "M666HC99",  Color = "Gray",   ModelGenerationId = 6  },
            new() { Id = 7,  LicensePlate = "N777CH77",  Color = "White",  ModelGenerationId = 7  },
            new() { Id = 8,  LicensePlate = "O888SH77",  Color = "Brown",  ModelGenerationId = 8  },
            new() { Id = 9,  LicensePlate = "P999SH99",  Color = "Yellow", ModelGenerationId = 9  },
            new() { Id = 10, LicensePlate = "R100SE77",  Color = "Black",  ModelGenerationId = 10 },
            new() { Id = 11, LicensePlate = "S200EY77",  Color = "Green",  ModelGenerationId = 11 },
            new() { Id = 12, LicensePlate = "T300YA99",  Color = "White",  ModelGenerationId = 12 },
            new() { Id = 13, LicensePlate = "U400AB77",  Color = "Black",  ModelGenerationId = 13 },
            new() { Id = 14, LicensePlate = "H500BV99",  Color = "Gray",   ModelGenerationId = 14 },
            new() { Id = 15, LicensePlate = "SH600VG77", Color = "Beige",  ModelGenerationId = 15 },
        ];

        Clients =
        [
            new() { Id = 1,  LicenseNumber = "2025-011", FullName = "Vasily Nekrasov",    BirthDate = new DateOnly(1985, 3,  20) },
            new() { Id = 2,  LicenseNumber = "2025-022", FullName = "Irina Morozova",      BirthDate = new DateOnly(1990, 7,  15) },
            new() { Id = 3,  LicenseNumber = "2025-033", FullName = "Sergei Volkov",       BirthDate = new DateOnly(1988, 11,  5) },
            new() { Id = 4,  LicenseNumber = "2025-044", FullName = "Natalia Stepanova",   BirthDate = new DateOnly(1992, 5,  28) },
            new() { Id = 5,  LicenseNumber = "2025-055", FullName = "Alexei Nikitin",      BirthDate = new DateOnly(1978, 9,  12) },
            new() { Id = 6,  LicenseNumber = "2025-066", FullName = "Yulia Borisova",      BirthDate = new DateOnly(1995, 2,   3) },
            new() { Id = 7,  LicenseNumber = "2025-077", FullName = "Dmitry Kirillov",     BirthDate = new DateOnly(1983, 8,  25) },
            new() { Id = 8,  LicenseNumber = "2025-088", FullName = "Vera Sorokina",       BirthDate = new DateOnly(1997, 12, 18) },
            new() { Id = 9,  LicenseNumber = "2025-099", FullName = "Konstantin Zhukov",   BirthDate = new DateOnly(1986, 6,  30) },
            new() { Id = 10, LicenseNumber = "2025-100", FullName = "Polina Veselova",     BirthDate = new DateOnly(1993, 4,   7) },
            new() { Id = 11, LicenseNumber = "2025-111", FullName = "Nikolai Kuznetsov",   BirthDate = new DateOnly(1980, 10, 14) },
            new() { Id = 12, LicenseNumber = "2025-122", FullName = "Ekaterina Savelyeva", BirthDate = new DateOnly(1998, 1,  22) },
            new() { Id = 13, LicenseNumber = "2025-133", FullName = "Andrei Kotov",        BirthDate = new DateOnly(1975, 7,   9) },
            new() { Id = 14, LicenseNumber = "2025-144", FullName = "Valentina Osipova",   BirthDate = new DateOnly(1982, 3,  16) },
            new() { Id = 15, LicenseNumber = "2025-155", FullName = "Maxim Panin",         BirthDate = new DateOnly(1999, 11,  1) },
        ];

        Rentals =
        [
            new() { Id = 1,  CarId = 4,  ClientId = 1,  RentalDate = new DateTime(2025, 3, 4,  10,  0, 0), RentalHours = 48  },
            new() { Id = 2,  CarId = 4,  ClientId = 3,  RentalDate = new DateTime(2025, 2, 25, 14, 30, 0), RentalHours = 72  },
            new() { Id = 3,  CarId = 4,  ClientId = 5,  RentalDate = new DateTime(2025, 2, 20,  9, 15, 0), RentalHours = 24  },
            new() { Id = 4,  CarId = 1,  ClientId = 2,  RentalDate = new DateTime(2025, 2, 27, 11, 45, 0), RentalHours = 96  },
            new() { Id = 5,  CarId = 1,  ClientId = 4,  RentalDate = new DateTime(2025, 3, 1,  16,  0, 0), RentalHours = 120 },
            new() { Id = 6,  CarId = 2,  ClientId = 6,  RentalDate = new DateTime(2025, 2, 23, 13, 20, 0), RentalHours = 72  },
            new() { Id = 7,  CarId = 2,  ClientId = 8,  RentalDate = new DateTime(2025, 2, 18, 10, 10, 0), RentalHours = 48  },
            new() { Id = 8,  CarId = 3,  ClientId = 7,  RentalDate = new DateTime(2025, 2, 28,  8, 30, 0), RentalHours = 36  },
            new() { Id = 9,  CarId = 5,  ClientId = 9,  RentalDate = new DateTime(2025, 3, 3,  12,  0, 0), RentalHours = 96  },
            new() { Id = 10, CarId = 6,  ClientId = 10, RentalDate = new DateTime(2025, 2, 28,  7,  0, 0), RentalHours = 168 },
            new() { Id = 11, CarId = 7,  ClientId = 11, RentalDate = new DateTime(2025, 2, 22, 15, 45, 0), RentalHours = 72  },
            new() { Id = 12, CarId = 8,  ClientId = 12, RentalDate = new DateTime(2025, 2, 26,  9, 20, 0), RentalHours = 48  },
            new() { Id = 13, CarId = 9,  ClientId = 13, RentalDate = new DateTime(2025, 2, 28, 22,  0, 0), RentalHours = 60  },
            new() { Id = 14, CarId = 10, ClientId = 14, RentalDate = new DateTime(2025, 2, 24, 11, 30, 0), RentalHours = 96  },
            new() { Id = 15, CarId = 11, ClientId = 15, RentalDate = new DateTime(2025, 2, 10, 14, 15, 0), RentalHours = 120 },
            new() { Id = 16, CarId = 12, ClientId = 1,  RentalDate = new DateTime(2025, 2, 28, 14,  0, 0), RentalHours = 48  },
            new() { Id = 17, CarId = 13, ClientId = 2,  RentalDate = new DateTime(2025, 2, 5,  16, 45, 0), RentalHours = 72  },
            new() { Id = 18, CarId = 14, ClientId = 3,  RentalDate = new DateTime(2025, 2, 12, 10, 10, 0), RentalHours = 36  },
            new() { Id = 19, CarId = 15, ClientId = 4,  RentalDate = new DateTime(2025, 2, 16, 13, 30, 0), RentalHours = 84  },
        ];
    }
}
