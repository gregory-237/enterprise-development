using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace CarRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "car_models",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    drive_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    seats_count = table.Column<int>(type: "int", nullable: false),
                    body_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    @class = table.Column<string>(name: "class", type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_car_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    license_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "model_generations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    engine_volume = table.Column<double>(type: "float", nullable: false),
                    transmission = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    rental_price_per_hour = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_model_generations", x => x.id);
                    table.ForeignKey(
                        name: "FK_model_generations_car_models_model_id",
                        column: x => x.model_id,
                        principalTable: "car_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    license_plate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    model_generation_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cars", x => x.id);
                    table.ForeignKey(
                        name: "FK_cars_model_generations_model_generation_id",
                        column: x => x.model_generation_id,
                        principalTable: "model_generations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    car_id = table.Column<int>(type: "int", nullable: false),
                    client_id = table.Column<int>(type: "int", nullable: false),
                    rental_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    rental_hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentals", x => x.id);
                    table.ForeignKey(
                        name: "FK_rentals_cars_car_id",
                        column: x => x.car_id,
                        principalTable: "cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rentals_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "car_models",
                columns: new[] { "id", "body_type", "class", "drive_type", "name", "seats_count" },
                values: new object[,]
                {
                    { 1,  "Sedan", "Premium",    "RWD", "Mercedes C-Class",    5 },
                    { 2,  "Sedan", "Business",   "FWD", "Volkswagen Passat",   5 },
                    { 3,  "Sedan", "Economy",    "FWD", "Kia Rio",             5 },
                    { 4,  "SUV",   "Mid-size",   "AWD", "Toyota RAV4",         5 },
                    { 5,  "Coupe", "Supercar",   "RWD", "Ferrari 488",         2 },
                    { 6,  "SUV",   "Full-size",  "4WD", "Nissan Patrol",       7 },
                    { 7,  "Sedan", "Economy",    "FWD", "Renault Logan",       5 },
                    { 8,  "SUV",   "Mid-size",   "AWD", "Mazda CX-5",          5 },
                    { 9,  "Van",   "Commercial", "RWD", "Ford Transit",        3 },
                    { 10, "SUV",   "Mid-size",   "AWD", "Mitsubishi Outlander", 5 },
                    { 11, "SUV",   "Luxury",     "4WD", "Land Rover Defender", 5 },
                    { 12, "SUV",   "Premium",    "AWD", "Volvo XC60",          5 },
                    { 13, "SUV",   "Luxury",     "AWD", "Cadillac Escalade",   7 },
                    { 14, "Sedan", "Business",   "FWD", "Skoda Octavia",       5 },
                    { 15, "SUV",   "Off-road",   "4WD", "Niva Legend",         5 }
                });

            migrationBuilder.InsertData(
                table: "clients",
                columns: new[] { "id", "birth_date", "full_name", "license_number" },
                values: new object[,]
                {
                    { 1,  new DateOnly(1985, 3,  20), "Vasily Nekrasov",      "2025-011" },
                    { 2,  new DateOnly(1990, 7,  15), "Irina Morozova",       "2025-022" },
                    { 3,  new DateOnly(1988, 11,  5), "Sergei Volkov",        "2025-033" },
                    { 4,  new DateOnly(1992, 5,  28), "Natalia Stepanova",    "2025-044" },
                    { 5,  new DateOnly(1978, 9,  12), "Alexei Nikitin",       "2025-055" },
                    { 6,  new DateOnly(1995, 2,   3), "Yulia Borisova",       "2025-066" },
                    { 7,  new DateOnly(1983, 8,  25), "Dmitry Kirillov",      "2025-077" },
                    { 8,  new DateOnly(1997, 12, 18), "Vera Sorokina",        "2025-088" },
                    { 9,  new DateOnly(1986, 6,  30), "Konstantin Zhukov",    "2025-099" },
                    { 10, new DateOnly(1993, 4,   7), "Polina Veselova",      "2025-100" },
                    { 11, new DateOnly(1980, 10, 14), "Nikolai Kuznetsov",    "2025-111" },
                    { 12, new DateOnly(1998, 1,  22), "Ekaterina Savelyeva",  "2025-122" },
                    { 13, new DateOnly(1975, 7,   9), "Andrei Kotov",         "2025-133" },
                    { 14, new DateOnly(1982, 3,  16), "Valentina Osipova",    "2025-144" },
                    { 15, new DateOnly(1999, 11,  1), "Maxim Panin",          "2025-155" }
                });

            migrationBuilder.InsertData(
                table: "model_generations",
                columns: new[] { "id", "engine_volume", "model_id", "rental_price_per_hour", "transmission", "year" },
                values: new object[,]
                {
                    { 1,  2.0, 1,  2500m,  "AT",  2023 },
                    { 2,  1.8, 2,  1800m,  "AT",  2022 },
                    { 3,  1.4, 3,  900m,   "AT",  2024 },
                    { 4,  2.5, 4,  2200m,  "AT",  2023 },
                    { 5,  3.9, 5,  15000m, "AT",  2021 },
                    { 6,  4.0, 6,  4000m,  "AT",  2023 },
                    { 7,  1.6, 7,  800m,   "MT",  2024 },
                    { 8,  2.0, 8,  2000m,  "AT",  2024 },
                    { 9,  2.2, 9,  1600m,  "MT",  2022 },
                    { 10, 2.0, 10, 1900m,  "CVT", 2023 },
                    { 11, 3.0, 11, 7000m,  "AT",  2024 },
                    { 12, 2.0, 12, 3500m,  "AT",  2023 },
                    { 13, 6.2, 13, 5500m,  "AT",  2022 },
                    { 14, 1.5, 14, 1400m,  "AT",  2024 },
                    { 15, 1.7, 15, 950m,   "MT",  2023 }
                });

            migrationBuilder.InsertData(
                table: "cars",
                columns: new[] { "id", "color", "license_plate", "model_generation_id" },
                values: new object[,]
                {
                    { 1,  "Black",  "A001MB77",  1  },
                    { 2,  "White",  "B222NO77",  2  },
                    { 3,  "Silver", "C333RT99",  3  },
                    { 4,  "Blue",   "E444UF77",  4  },
                    { 5,  "Red",    "K555FH77",  5  },
                    { 6,  "Gray",   "M666HC99",  6  },
                    { 7,  "White",  "N777CH77",  7  },
                    { 8,  "Brown",  "O888SH77",  8  },
                    { 9,  "Yellow", "P999SH99",  9  },
                    { 10, "Black",  "R100SE77",  10 },
                    { 11, "Green",  "S200EY77",  11 },
                    { 12, "White",  "T300YA99",  12 },
                    { 13, "Black",  "U400AB77",  13 },
                    { 14, "Gray",   "H500BV99",  14 },
                    { 15, "Beige",  "SH600VG77", 15 }
                });

            migrationBuilder.InsertData(
                table: "rentals",
                columns: new[] { "id", "car_id", "client_id", "rental_date", "rental_hours" },
                values: new object[,]
                {
                    { 1,  4,  1,  new DateTime(2025, 3, 4,  10,  0, 0, 0, DateTimeKind.Unspecified), 48  },
                    { 2,  4,  3,  new DateTime(2025, 2, 25, 14, 30, 0, 0, DateTimeKind.Unspecified), 72  },
                    { 3,  4,  5,  new DateTime(2025, 2, 20,  9, 15, 0, 0, DateTimeKind.Unspecified), 24  },
                    { 4,  1,  2,  new DateTime(2025, 2, 27, 11, 45, 0, 0, DateTimeKind.Unspecified), 96  },
                    { 5,  1,  4,  new DateTime(2025, 3, 1,  16,  0, 0, 0, DateTimeKind.Unspecified), 120 },
                    { 6,  2,  6,  new DateTime(2025, 2, 23, 13, 20, 0, 0, DateTimeKind.Unspecified), 72  },
                    { 7,  2,  8,  new DateTime(2025, 2, 18, 10, 10, 0, 0, DateTimeKind.Unspecified), 48  },
                    { 8,  3,  7,  new DateTime(2025, 2, 28,  8, 30, 0, 0, DateTimeKind.Unspecified), 36  },
                    { 9,  5,  9,  new DateTime(2025, 3, 3,  12,  0, 0, 0, DateTimeKind.Unspecified), 96  },
                    { 10, 6,  10, new DateTime(2025, 2, 28,  7,  0, 0, 0, DateTimeKind.Unspecified), 168 },
                    { 11, 7,  11, new DateTime(2025, 2, 22, 15, 45, 0, 0, DateTimeKind.Unspecified), 72  },
                    { 12, 8,  12, new DateTime(2025, 2, 26,  9, 20, 0, 0, DateTimeKind.Unspecified), 48  },
                    { 13, 9,  13, new DateTime(2025, 2, 28, 22,  0, 0, 0, DateTimeKind.Unspecified), 60  },
                    { 14, 10, 14, new DateTime(2025, 2, 24, 11, 30, 0, 0, DateTimeKind.Unspecified), 96  },
                    { 15, 11, 15, new DateTime(2025, 2, 10, 14, 15, 0, 0, DateTimeKind.Unspecified), 120 },
                    { 16, 12, 1,  new DateTime(2025, 2, 28, 14,  0, 0, 0, DateTimeKind.Unspecified), 48  },
                    { 17, 13, 2,  new DateTime(2025, 2, 5,  16, 45, 0, 0, DateTimeKind.Unspecified), 72  },
                    { 18, 14, 3,  new DateTime(2025, 2, 12, 10, 10, 0, 0, DateTimeKind.Unspecified), 36  },
                    { 19, 15, 4,  new DateTime(2025, 2, 16, 13, 30, 0, 0, DateTimeKind.Unspecified), 84  }
                });

            migrationBuilder.CreateIndex(
                name: "IX_cars_model_generation_id",
                table: "cars",
                column: "model_generation_id");

            migrationBuilder.CreateIndex(
                name: "IX_model_generations_model_id",
                table: "model_generations",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_car_id",
                table: "rentals",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_client_id",
                table: "rentals",
                column: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "rentals");
            migrationBuilder.DropTable(name: "cars");
            migrationBuilder.DropTable(name: "clients");
            migrationBuilder.DropTable(name: "model_generations");
            migrationBuilder.DropTable(name: "car_models");
        }
    }
}
