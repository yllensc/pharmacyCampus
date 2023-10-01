using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Persistence;

public class PharmacyContextSeed
{
    private readonly UserManager<User> _userManager;
    public PharmacyContextSeed(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public static async Task SeedAsync(PharmacyDbContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Rol>
                {
                    new Rol { Name = "Administrator" },
                    new Rol { Name = "Employee" },
                    new Rol { Name = "Patient" },
                    new Rol { Name = "WithoutRol" }
                };
                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<User>();
                using var readerUsers = new StreamReader("../Persistence/Data/Csvs/users.csv");
                {
                    using (var csv = new CsvReader(readerUsers, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        var usersList = csv.GetRecords<User>();
                        List<User> users = new List<User>();
                        foreach (var user in usersList)
                        {
                            var hashedPassword = passwordHasher.HashPassword(null, user.Password);
                            var newUser = new User
                            {
                                Id = user.Id,
                                UserName = user.UserName,
                                IdenNumber = user.IdenNumber,
                                Email = user.Email,
                                Password = hashedPassword
                            };
                            users.Add(newUser);
                        }
                        context.Users.AddRange(users);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.UserRoles.Any())
            {
                using (var readerUserRols = new StreamReader("../Persistence/Data/Csvs/userRols.csv"))
                {
                    using (var csv = new CsvReader(readerUserRols, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        var userRolList = csv.GetRecords<UserRol>();
                        List<UserRol> userRols = new List<UserRol>();
                        foreach (var userRol in userRolList)
                        {
                            userRols.Add(new UserRol
                            {
                                Id = userRol.Id,
                                UserId = userRol.UserId,
                                RolId = userRol.RolId
                            });
                        }
                        context.UserRoles.AddRange(userRols);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Positions.Any())
            {
                var positions = new List<Position>
                {
                    new Position { Name = "Seller" },
                    new Position { Name = "Assistant" },
                    new Position { Name = "Cleaner" },
                    new Position { Name = "Domiciliary" }
                };
                context.Positions.AddRange(positions);
                await context.SaveChangesAsync();
            }

            if (!context.Employees.Any())
            {
                using (var reader = new StreamReader("../Persistence/Data/Csvs/employees.csv"))
                {
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        // Resto de tu código para leer y procesar el archivo CSV
                        var list = csv.GetRecords<Employee>();
                        List<Employee> entidad = new List<Employee>();
                        foreach (var item in list)
                        {
                            entidad.Add(new Employee
                            {
                                Id = item.Id,
                                UserId = item.UserId,
                                DateContract = item.DateContract,
                                Name = item.Name,
                                PositionId = item.PositionId
                            });
                        }
                        context.Employees.AddRange(entidad);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Patients.Any())
            {
                using (var readerPatients = new StreamReader("../Persistence/Data/Csvs/patients.csv"))
                {
                    using (var csv = new CsvReader(readerPatients, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        var patientList = csv.GetRecords<Patient>();
                        List<Patient> patients = new List<Patient>();
                        foreach (var patient in patientList)
                        {
                            patients.Add(new Patient
                            {
                                Id = patient.Id,
                                Name = patient.Name,
                                Address = patient.Address,
                                PhoneNumber = patient.PhoneNumber,
                                IdenNumber = patient.IdenNumber
                            });
                        }
                        context.Patients.AddRange(patients);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Providers.Any())
            {
                using (var readerProviders = new StreamReader("../Persistence/Data/Csvs/providers.csv"))
                {
                    using (var csv = new CsvReader(readerProviders, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        var providerList = csv.GetRecords<Provider>();

                        List<Provider> providers = new List<Provider>();
                        foreach (var provider in providerList)
                        {
                            providers.Add(new Provider
                            {
                                Id = provider.Id,
                                Name = provider.Name,
                                IdenNumber = provider.IdenNumber,
                                Email = provider.Email,
                                Address = provider.Address
                            });
                        }
                        context.Providers.AddRange(providers);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Purchases.Any())
            {
                using (var readerPurchases = new StreamReader("../Persistence/Data/Csvs/purchases.csv"))
                {
                    using (var csv = new CsvReader(readerPurchases, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        var purchasesList = csv.GetRecords<Purchase>();

                        List<Purchase> purchases = new List<Purchase>();
                        foreach (var purchase in purchasesList)
                        {
                            purchases.Add(new Purchase
                            {
                                Id = purchase.Id,
                                DatePurchase = purchase.DatePurchase,
                                ProviderId = purchase.ProviderId
                            });
                        }
                        context.Purchases.AddRange(purchases);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Medicines.Any())
            {
                using (var readerMedicines = new StreamReader("../Persistence/Data/Csvs/medicines.csv"))
                {
                    using (var csv = new CsvReader(readerMedicines, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        var medicineList = csv.GetRecords<Medicine>();

                        List<Medicine> medicines = new List<Medicine>();
                        foreach (var medicine in medicineList)
                        {
                            medicines.Add(new Medicine
                            {
                                Id = medicine.Id,
                                Name = medicine.Name,
                                Price = medicine.Price,
                                Stock = medicine.Stock,
                                ProviderId = medicine.ProviderId,
                            });
                        }
                        context.Medicines.AddRange(medicines);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Sales.Any())
            {
                using (var readerSales = new StreamReader("../Persistence/Data/Csvs/sales.csv"))
                {
                    using (var csv = new CsvReader(readerSales, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        var salesList = csv.GetRecords<Sale>();

                        List<Sale> sales = new List<Sale>();
                        foreach (var sale in salesList)
                        {
                            sales.Add(new Sale
                            {
                                Id = sale.Id,
                                DateSale = sale.DateSale,
                                PatientId = sale.PatientId,
                                EmployeeId = sale.EmployeeId,
                                Prescription = sale.Prescription,
                                DatePrescription = sale.DatePrescription
                            });
                        }
                        context.Sales.AddRange(sales);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.SaleMedicines.Any())
            {
                using (var readerSaleMedicines = new StreamReader("../Persistence/Data/Csvs/saleMedicines.csv"))
                {
                    using (var csv = new CsvReader(readerSaleMedicines, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        var saleMedicineList = csv.GetRecords<SaleMedicine>();

                        List<SaleMedicine> saleMedicines = new List<SaleMedicine>();
                        foreach (var saleMedicine in saleMedicineList)
                        {
                            saleMedicines.Add(new SaleMedicine
                            {
                                Id = saleMedicine.Id,
                                SaleId = saleMedicine.SaleId,
                                MedicineId = saleMedicine.MedicineId,
                                SaleQuantity = saleMedicine.SaleQuantity,
                                Price = saleMedicine.Price
                            });
                        }
                        context.SaleMedicines.AddRange(saleMedicines);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.PurchasedMedicines.Any())
            {
                using (var reader = new StreamReader("../Persistence/Data/Csvs/purchasedMedicines.csv"))
                {
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        // Resto de tu código para leer y procesar el archivo CSV
                        var list = csv.GetRecords<PurchasedMedicine>();
                        List<PurchasedMedicine> entidad = new List<PurchasedMedicine>();
                        foreach (var item in list)
                        {
                            entidad.Add(new PurchasedMedicine
                            {

                                Id = item.Id,
                                PurchasedId = item.PurchasedId,
                                MedicineId = item.MedicineId,
                                CantPurchased = item.CantPurchased,
                                Stock = item.Stock,
                                PricePurchase = item.PricePurchase,
                                ExpirationDate = item.ExpirationDate

                            });
                        }

                        context.PurchasedMedicines.AddRange(entidad);
                        await context.SaveChangesAsync();
                    }

                }
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<PharmacyDbContext>();
            logger.LogError(ex.Message);
        }
    }
}
