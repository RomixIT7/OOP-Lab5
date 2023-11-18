using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Numerics;

public enum AbonentType
{
    usual,
    special
}

public struct Abonent
{
    public int accountNumber;
    public string fullName;
    public string address;
    public string phoneNumber;
    public int contractNumber;
    public DateTime DateOfConclusionOfTheAgreement;
    public bool AvailabilityOfBenefits;
    public AbonentType typeOfAbonent;
    public string tariffPlan;
}

class Program
{
    static void Main()
    {
        string fileName = "абоненти.txt";
        List<Abonent> abonents = new List<Abonent>();

        ReadFromFile(fileName, abonents);

        while (true)
        {
            Console.Clear();

            Console.WriteLine("Оберіть опцію:");
            Console.WriteLine("1. Ввести інформацію про нового абонента");
            Console.WriteLine("2. Видалити базу даних");
            Console.WriteLine("3. Пошук за номером договору");
            Console.WriteLine("4. Пошук за іменем абонента");
            Console.WriteLine("5. Пошук за тарифним планом");
            Console.WriteLine("6. Вийти");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        Console.Clear();
                        Abonent newAbonent = ReadAbonentFromConsole();
                        abonents.Add(newAbonent);
                        break;

                    case 2:
                        Console.Clear();
                        DeleteDatabase(abonents);
                        break;

                    case 3:
                        Console.Clear();
                        Console.Write("Введіть номер договору для пошуку: ");
                        if (int.TryParse(Console.ReadLine(), out int contractNumberSearch))
                        {
                            SearchByContractNumber(abonents, contractNumberSearch);
                        }
                        else
                        {
                            Console.WriteLine("Некоректний формат номеру договору.");
                        }
                        break;

                    case 4:
                        Console.Clear();
                        Console.Write("Введіть ім'я абонента для пошуку: ");
                        string name = Console.ReadLine();
                        SearchByName(abonents, name);
                        break;

                    case 5:
                        Console.Clear();
                        Console.Write("Введіть тарифний план для пошуку: ");
                        string tariffPlan = Console.ReadLine();
                        SearchByTariffPlan(abonents, tariffPlan);
                        break;

                    case 6:
                        Console.Clear();
                        WriteToFile(fileName, abonents);
                        return;

                    default:
                        Console.Clear();
                        Console.WriteLine("Некоректний вибір опції.");
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Некоректний формат опції. Введіть ціле число.");
            }

            Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }
    }

    static void DeleteDatabase(List<Abonent> abonents)
    {
        Console.Write("Ви впевнені, що хочете видалити всю базу даних? (y/n): ");
        string response = Console.ReadLine();

        if (response.ToLower() == "y")
        {
            abonents.Clear();
            Console.WriteLine("Базу даних видалено.");
        }
        else
        {
            Console.WriteLine("Видалення бази даних скасовано.");
        }
    }

    static void SearchByContractNumber(List<Abonent> abonents, int contractNumber)
    {
        Abonent result = abonents.Find(abonent => abonent.contractNumber == contractNumber);

        if (result.Equals(default(Abonent)))
        {
            Console.WriteLine("Абонент з таким номером договору не знайдений.");
        }
        else
        {
            Console.WriteLine("Результат пошуку за номером договору:");
            PrintAbonentInfo(result);
        }
    }

    static void SearchByName(List<Abonent> abonents, string name)
    {
        List<Abonent> results = abonents.FindAll(abonent => abonent.fullName?.Contains(name) ?? false);

        if (results.Count > 0)
        {
            Console.WriteLine("Результат пошуку за іменем абонента:");
            foreach (Abonent abonent in results)
            {
                PrintAbonentInfo(abonent);
            }
        }
        else
        {
            Console.WriteLine("Абонентів з таким іменем не знайдено.");
        }
    }

    static void SearchByTariffPlan(List<Abonent> abonents, string tariffPlan)
    {
        List<Abonent> results = abonents.FindAll(abonent => abonent.tariffPlan?.Equals(tariffPlan, StringComparison.OrdinalIgnoreCase) ?? false);

        if (results.Count > 0)
        {
            Console.WriteLine("Результат пошуку за тарифним планом:");
            foreach (Abonent abonent in results)
            {
                PrintAbonentInfo(abonent);
            }
        }
        else
        {
            Console.WriteLine("Абонентів з таким тарифним планом не знайдено.");
        }
    }

    static void PrintAbonentInfo(Abonent abonent)
    {
        Console.WriteLine($"Номер рахунку: {abonent.accountNumber}");
        Console.WriteLine($"ПІП абонента: {abonent.fullName}");
        Console.WriteLine($"Адреса: {abonent.address}");
        Console.WriteLine($"Номер телефону: {abonent.phoneNumber}");
        Console.WriteLine($"Номер договору: {abonent.contractNumber}");
        Console.WriteLine($"Дата укладення договору: {abonent.DateOfConclusionOfTheAgreement:yyyy-MM-dd}");
        Console.WriteLine($"Наявність пільг: {abonent.AvailabilityOfBenefits}");
        Console.WriteLine($"Тип абонента: {abonent.typeOfAbonent}");
        Console.WriteLine($"Тарифний план: {abonent.tariffPlan}");
        Console.WriteLine();
    }

    static Abonent ReadAbonentFromConsole()
    {
        Abonent abonent = new Abonent();

        Console.Write("Номер рахунку: ");
        abonent.accountNumber = int.Parse(Console.ReadLine());

        Console.Write("ПІП абонента: ");
        abonent.fullName = Console.ReadLine();

        Console.Write("Адреса: ");
        abonent.address = Console.ReadLine();

        Console.Write("Номер телефону: ");
        abonent.phoneNumber = Console.ReadLine();

        Console.Write("Номер договору: ");
        abonent.contractNumber = int.Parse(Console.ReadLine());

        Console.Write("Дата укладення договору (yyyy-MM-dd): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime дата))
        {
            abonent.DateOfConclusionOfTheAgreement = дата;
        }
        else
        {
            Console.WriteLine("Некоректний формат дати. Встановлено поточну дату.");
            abonent.DateOfConclusionOfTheAgreement = DateTime.Now;
        }

        Console.Write("Наявність пільг (true/false): ");
        if (bool.TryParse(Console.ReadLine(), out bool AvailabilityOfBenefits))
        {
            abonent.AvailabilityOfBenefits = AvailabilityOfBenefits;
        }
        else
        {
            Console.WriteLine("Некоректний формат. Встановлено значення за замовчуванням.");
            abonent.AvailabilityOfBenefits = default;
        }

        Console.Write("Тип абонента (usual/special): ");
        string typeOfAbonentStr = Console.ReadLine();

        if (Enum.TryParse(typeof(AbonentType), typeOfAbonentStr, out object? typeOfAbonentObj) && typeOfAbonentObj != null)
        {
            abonent.typeOfAbonent = (AbonentType)typeOfAbonentObj;
        }
        else
        {
            Console.WriteLine("Некоректний тип абонента. Встановлено значення за замовчуванням.");
            abonent.typeOfAbonent = default;
        }

        Console.Write("Тарифний план: ");
        abonent.tariffPlan = Console.ReadLine();

        return abonent;
    }

    static void ReadFromFile(string fileName, List<Abonent> abonents)
    {
        if (File.Exists(fileName))
        {
            string[] lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                if (values.Length == 9)
                {
                    Abonent abonent = new Abonent
                    {
                        accountNumber = int.Parse(values[0]),
                        fullName = values[1],
                        address = values[2],
                        phoneNumber = values[3],
                        contractNumber = int.Parse(values[4]),
                        DateOfConclusionOfTheAgreement = DateTime.ParseExact(values[5], "yyyy-MM-dd", null),
                        AvailabilityOfBenefits = bool.Parse(values[6]),
                        typeOfAbonent = (AbonentType)Enum.Parse(typeof(AbonentType), values[7]),
                        tariffPlan = values[8]
                    };

                    abonents.Add(abonent);
                }
            }

            Console.WriteLine("Дані з файлу успішно зчитано.");
        }
        else
        {
            Console.WriteLine("Файл не існує. Створіть новий або введіть дані.");
        }
    }

    static void WriteToFile(string fileName, List<Abonent> abonents)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (Abonent abonent in abonents)
            {
                writer.WriteLine($"{abonent.accountNumber},{abonent.fullName},{abonent.address},{abonent.phoneNumber}," +
                    $"{abonent.contractNumber},{abonent.DateOfConclusionOfTheAgreement:yyyy-MM-dd},{abonent.AvailabilityOfBenefits},{abonent.typeOfAbonent},{abonent.tariffPlan}");
            }

            Console.WriteLine("Дані успішно записано у файл.");
        }
    }
}

