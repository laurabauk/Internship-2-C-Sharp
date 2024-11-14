using System;
using System.Globalization;

//var accountList = new List<(string, double)>();
var userList = new List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)>();

var nextId = 1;
var nextTransactionId = 1;

while (true)
{
    Console.WriteLine("1 - Korisnici\n2 - Racuni\n3 - Izlaz iz aplikacije");
    Console.Write("\nIzaberite neku od ponudenih opcija: ");

    int.TryParse(Console.ReadLine(), out var option);

    if (option == 1)
    {
        Console.WriteLine("\n1. Korisnici\n\t1 - Unos novog korisnika\n\t2 - Brisanje korisnika\n\t\ta) po id-u\n\t\tb) po imenu i prezimenu\n\t3 - Uredivanje korisnika\n\t\ta) po id-u\n\t4 - Pregled korisnika\n\t\ta)ispis svih korisnika abecedno po prezimenu\n\t\tb) svih onih koji imaju vise od 30 godina\n\t\tc) svih onih koji imaju barem jedan racun u minusu");
        Console.Write("\nIzaberite neku od podopcija: ");

        while (true)
        {
            int.TryParse(Console.ReadLine(), out var secondOption);

            switch (secondOption)
            {
                case 1:
                    CreateUser(userList, ref nextId);
                    break;
                case 2:
                    //DeleteUser();
                    break;
                case 3:
                    //EditUser();
                    break;
                case 4:
                    //PrintUsers();
                    break;
                default:
                    Console.WriteLine("Nevazeca Opcija! Pokusajte ponovno.");
                    continue;
            }
            break;
        }
    }

    else if (option == 2)
    {
        Console.WriteLine("2. Racuni");
    }
    else if (option == 3)
    {
        Console.WriteLine("Izlaz iz aplikacije..");
        return;
    }
    else
    {
        Console.WriteLine("Nevazeca opcija! Pokusajte ponovno.");
    }
}

static void CreateUser(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList, ref int nextId)
{

    Console.Clear();

    Console.WriteLine("Unos novog korisniika:");

    Console.Write("Unesite ime korisnika: ");
    var firstName = Console.ReadLine();

    Console.Write("Unesite prezime korisnika: ");
    var lastName = Console.ReadLine();

    while (true)
    {
        Console.Write("Unesite datum rodenja korisnika u formatu (year-month-date): ");
        var birthInput = Console.ReadLine();

        DateOnly birthDate;

        if (DateOnly.TryParse(birthInput, out birthDate))
        {
            var accountList = new List<(string, double, List<(int, double, string, string, string, DateTime)>)> {
                ("Tekuci", 100.00, new List<(int,double,string,string,string,DateTime)>()),
                ("Ziro", 0.00, new List<(int,double,string,string,string,DateTime)>()),
                ("Prepaid", 0.00, new List<(int,double,string,string,string,DateTime)>())
            };

            userList.Add((nextId++, firstName, lastName, birthDate, accountList));
            Console.WriteLine($"Korisnik {firstName} {lastName} roden {birthDate} uspjesno unesen.");
            break;
        }
        else
        {
            Console.WriteLine("Neispravan format datuma! Pokusajte ponovno.");
            continue;
        }
    }
    Console.WriteLine("Pritisnite bilokoju tipku za povratak u izbornik...\n");
    Console.ReadKey();
}

static void CreateTransaction(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList, ref int nextTransactionId)
{

    if (userList.Count == 0)
    {
        Console.WriteLine("Lista korisnika je prazna.");
        return;
    }

    Console.WriteLine("Odaberite korisnika po ID-u:");
    foreach (var user in userList)
    {
        Console.WriteLine($"ID: {user.Item1}, Ime: {user.Item2}, Prezime: {user.Item3}");
    }

    int.TryParse(Console.ReadLine(), out var userId);

    bool userFound = false;
    (int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>) selectedUser = default;

    foreach (var user in userList)
    {
        if (user.Item1 == userId)
        {
            selectedUser = user;
            userFound = true;
            break;
        }
    }

    if (!userFound)
    {
        Console.WriteLine("Korisnik nije pronaden.");
    }
    else
    {
        Console.WriteLine($"Pronaden korisnik: {selectedUser.Item2} {selectedUser.Item3}");

    }

    Console.WriteLine("Unesite iznos transakcije:");
    double.TryParse(Console.ReadLine(), out var amount);

    if (amount <= 0)
    {
        Console.WriteLine("Iznos transakcije mora biti pozitivan. Pokusajte ponovno.");
        return;
    }

    Console.WriteLine("Unesite opis transakcije (pritisnite Enter za default transakciju):");
    var description = Console.ReadLine();

    if (string.IsNullOrEmpty(description))
    {
        description = "Standardna transakcija";
    }

    Console.WriteLine("Odaberite tip transakcije (1 - Prihod, 2 - Rashod)");
    int.TryParse(Console.ReadLine(), out var transactionType);

    var category = "";
    var type = transactionType = 1 ? "Prihod" : "Rashod";

    if (transactionType == 1)
    {
        Console.WriteLine("Odaberite kategoriju prihoda (1 - Placa, 2 - Honorar, 3 - Poklon");
        int.TryParse(Console.ReadLine(), out var incomeCategory);

        switch (incomeCategory)
        {
            case 1:
                category = "Plaća";
                break;
            case 2:
                category = "Honorar";
                break;
            case 3:
                category = "Poklon";
                break;
            default:
                category = "Ostalo";
                break;
        }
    }
    else if (transactionType == 2)
    {
        Console.WriteLine("Odaberite kategoriju rashoda ( 1 - Hrana, 2 - Prijevoz, 3 - Sport");
        int.TryParse(Console.ReadLine(), out var expenseCategory);
        switch (expenseCategory)
        {
            case 1:
                category = "Hrana";
                break;
            case 2:
                category = "Prijevoz";
                break;
            case 3:
                category = "Sport";
                break;
            default:
                category = "Ostalo";
                break;
        }
    }
    else
    {
        Console.WriteLine("Nevazeci tip transakcije.");
        return;
    }

    var transaction = (nextTransactionId++, amount, description, transactionType, category, DateTime.Now());
}
