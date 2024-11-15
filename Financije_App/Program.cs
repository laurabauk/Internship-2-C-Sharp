using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

var userList = new List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)>();

var nextId = 1;
var nextTransactionId = 1;

while (true)
{
    Console.Clear();

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
                    DeleteUser(userList); 
                    break;
                case 3:
                    CreateTransaction(userList, ref nextTransactionId);
                    //EditUser();
                    break;
                case 4:
                    PrintUsers(userList);
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
        Console.WriteLine("Unesite ime i prezime korisnika cijim racunima zelite upravljati: ");

        var wantedUser = Console.ReadLine();
        var user = userList.Find(u => u.Item2== wantedUser);

        if (user.Equals(default((int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>))))
        {
            Console.WriteLine("Korisnik nije pronaden!");
            return;
        }

        Console.WriteLine("1 - Pregled racuna");

        int.TryParse(Console.ReadLine(), out var accountOption);

        switch (accountOption) {
            case 1:
                PrintAccounts(user.Item5);
                break;
            default:
                Console.WriteLine("Opcija nije ponudena!");
                break;
        }


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
static void PrintAccounts(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList)
{
    if (userList.Count == 0)
    {
        Console.WriteLine("Nema dostupnih korisnika za prikaz računa.");
        return;
    }

    Console.WriteLine("Odaberite korisnika po ID-u za pregled računa:");
    foreach (var user in userList)
    {
        Console.WriteLine($"ID: {user.Item1}, Ime: {user.Item2}, Prezime: {user.Item3}");
    }

    int.TryParse(Console.ReadLine(), out var userId);
    var selectedUser = userList.Find(user => user.Item1 == userId);

    if (selectedUser.Equals(default((int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>))))
    {
        Console.WriteLine("Korisnik nije pronađen.");
        return;
    }

    Console.WriteLine($"\nRačuni korisnika {selectedUser.Item2} {selectedUser.Item3}:");

    if (selectedUser.Item5.Count == 0)
    {
        Console.WriteLine("Ovaj korisnik nema računa.");
        return;
    }

    for (int i = 0; i < selectedUser.Item5.Count; i++)
    {
        var account = selectedUser.Item5[i];
        Console.WriteLine($"[{i + 1}] Vrsta računa: {account.Item1}, Stanje: {account.Item2} EUR");

        if (account.Item3.Count > 0)
        {
            Console.WriteLine("  Transakcije:");
            foreach (var transaction in account.Item3)
            {
                Console.WriteLine($"    ID: {transaction.Item1}, Iznos: {transaction.Item2} EUR, Opis: {transaction.Item3}, Kategorija: {transaction.Item4}, Tip: {transaction.Item5}, Datum: {transaction.Item6}");
            }
        }
        else
        {
            Console.WriteLine("  Nema transakcija za ovaj račun.");
        }
    }

    Console.WriteLine("\nPritisnite Enter za povratak na glavni izbornik...");
    Console.ReadKey();
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
    Console.WriteLine("\nPritisnite 1 za unos jos jednog korisnika ili Enter za povratak u izbornik...\n");

    if (int.TryParse(Console.ReadLine(), out var choice) && choice == 1)
    {
        CreateUser(userList, ref nextId);
    }
    else
        return;
}

static void CreateTransaction(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList, ref int nextTransactionId)
{
    bool userFound = false;
    (int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>) selectedUser = default;

    if (userList.Count == 0)
    {
        Console.WriteLine("Lista korisnika je prazna. Vratite se na glavni izbornik pritiskom bilokoje tipke i unesite korisnike...");
        Console.ReadKey();
        return;
    }

    while (true)
    {
        Console.WriteLine("Odaberite korisnika po ID-u:");
        foreach (var user in userList)
        {
            Console.WriteLine($"ID: {user.Item1}, Ime: {user.Item2}, Prezime: {user.Item3}");
        }

        int.TryParse(Console.ReadLine(), out var userId);

        foreach (var user in userList)
        {
            if (user.Item1 == userId)
            {
                selectedUser = user;
                userFound = true;
                break;
            }
        }

        if (userFound)
        {
            Console.WriteLine($"Pronaden korisnik: {selectedUser.Item2} {selectedUser.Item3}");
            break;
        }
        else
        {
            Console.WriteLine("Korisnik nije pronaden. Pokusajte ponovno.");
        }

    }

    double amount = 0;

    Console.WriteLine("Unesite iznos transakcije (mora biti veci od 0): ");

    while (true)
    {

        if ((double.TryParse(Console.ReadLine(), out amount) && amount > 0))
        {
            break;
        }
        else {
            Console.WriteLine("Iznos transakcije mora biti veci od 0! Pokusajte ponovno.");
            continue;
        }
    }

    Console.WriteLine("Unesite opis transakcije (pritisnite Enter za default transakciju):");
    var description = Console.ReadLine();

    if (string.IsNullOrEmpty(description))
    {
        description = "Standardna transakcija";
    }

    int transactionType;
    string category = "";

    Console.WriteLine("Odaberite tip transakcije (1 - Prihod, 2 - Rashod)");

    while (true)
    {
        int.TryParse(Console.ReadLine(), out transactionType);


        var type = (transactionType == 1) ? "Prihod" : "Rashod";

        if (transactionType == 1)
        {
            Console.WriteLine("Odaberite kategoriju prihoda (1 - Placa, 2 - Honorar, 3 - Poklon)");
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
            break;
        }
        else if (transactionType == 2)
        {
            Console.WriteLine("Odaberite kategoriju rashoda (1 - Hrana, 2 - Prijevoz, 3 - Sport");
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
            break;
        }

        else
        {
            Console.WriteLine("Nevazeci tip transakcije. Unesite 1 za Prihod ili 2 za Rashod");
            continue;
        }
    }

    var transaction = (nextTransactionId++, amount, description, (transactionType == 1) ? "Prihod" : "Rashod", category, DateTime.Now);

    for (int i = 0; i < userList.Count; i++)
    {
        if (userList[i].Item1 == selectedUser.Item1)
        {
            userList[i].Item5.Add((category, amount, new List<(int, double, string, string, string, DateTime)> {transaction}));
            break;
        }
    }

    Console.WriteLine("Transakcija uspjesno dodana! Pritisnite bilokoju tipku za povratak na glavni izbornik...");
    Console.ReadKey();
    return;
}

static void DeleteUser(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList)
{

    Console.Clear();

    Console.WriteLine("Izaberi način brisanja korisnika:\n\t 1 - po ID-u\n\t 2 - po imenu i prezimenu");

    var choice = 0;

    while (true)
    {

        if ((!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2)))

        {
            Console.WriteLine("Pogrešan unos! Unesite broj 1 ili 2.");
            continue;
        }

        break;
    }

    if (choice == 1)
    {
        DeleteUserById(userList);
        Console.WriteLine("Pritisni Enter za povratak na glavni izbornik...");
        Console.ReadKey();
        return;
    }
    else {
        DeleteUserByFullName(userList);
        Console.WriteLine("Pritisni Enter za povratak na glavni izbornik...");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("Pritisnite Enter za povratak u glavni izbornik...");
    return;
}

static void DeleteUserById(List < (int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>) > userList) {

    if (userList.Count == 0)
    {
        Console.WriteLine("Lista korisnika je prazna. Molimo unesite neke korisnike prije brisanja.");
        return;
    }

    else
    {

        Console.WriteLine("Unesite ID korinsika kojeg zelite obrisati: ");

        foreach (var user in userList)
        {
            Console.WriteLine($"ID: {user.Item1}, Ime: {user.Item2}, Prezime: {user.Item3}");
        }

    }

    var userId = 0;
    bool userFound = false;

    do
    {

        if (!int.TryParse(Console.ReadLine(), out userId))
        {

            Console.WriteLine("Pogresan unos! ID mora biti broj.");
            continue;
        }

        userFound = false;

        for (int i = 0; i < userList.Count(); i++) {
            if (userList[i].Item1 == userId) {
                userList.RemoveAt(i);
                userFound = true;
                Console.WriteLine($"Korisnik s ID-om {userId} uspješno izbrisan.");
                break;
            }
        }

        if (!userFound)
        {
            Console.WriteLine($"Korisnik s upisanim ID-om ({userId}) ne postoji! Pokusajte ponovno.");
        }

    } while (!userFound);

    return;
}

static void DeleteUserByFullName(List < (int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>) > userList) {

    if (userList.Count == 0)
    {
        Console.WriteLine("Lista korisnika je prazna. Molimo unesite neke korisnike prije brisanja.");
        return;
    }

    else
    {

        Console.WriteLine("Unesite ime i prezime korinsika kojeg zelite obrisati: ");

        foreach (var user in userList)
        {
            Console.WriteLine($"ID: {user.Item1}, Ime: {user.Item2}, Prezime: {user.Item3}");
        }

    }

    var fullName = Console.ReadLine();
    var splitName = fullName.Split(' ');

    if (splitName.Length != 2)
    {
        Console.WriteLine("Pogrešan unos. Unos mora biti u formatu 'Ime Prezime'.");
        return;
    }

    var firstName = splitName[0];
    var lastName = splitName[1];

    bool userFound = false;

    do
    {
        userFound = false;

        for (int i = 0; i < userList.Count; i++)
        {
            if (userList[i].Item2 == firstName && userList[i].Item3 == lastName)
            {
                userList.RemoveAt(i);
                userFound = true;
                Console.WriteLine($"Korisnik {firstName} {lastName} uspješno izbrisan.");
                i--;
            }
        }

        if (!userFound)
        {
            Console.WriteLine($"Korisnik s upisanim imenom ({firstName} {lastName}) ne postoji! Pokusajte ponovno.");
            continue;
        }

    } while(!userFound);

    return;
}

static void PrintAlphabetically(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList)
{
    if (userList.Count == 0)
    {
        Console.WriteLine("Nema korisnika za ispis.");
    }

    for (int i = 0; i < userList.Count; i++)
    {
        for (int j = i + 1; j < userList.Count; j++)
        {
            if (string.Compare(userList[i].Item3, userList[j].Item3) > 0)
            {
                var temp = userList[i];
                userList[i] = userList[j];
                userList[j] = temp;
            }
        }
    }
    Console.WriteLine("Ispis korisnika abecedno po prezimenu:");
    foreach (var user in userList)
    {
        Console.WriteLine($"{user.Item1} - {user.Item2} - {user.Item3} - {user.Item4}");
    }
    Console.WriteLine("Pritisni Enter za izlaz...");
    Console.ReadKey();
    return;
}

static void PrintOver30Years(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList)
{
    if (userList.Count == 0)
    {
        Console.WriteLine("Nema korisnika za ispis.");
        return;
    }

    DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

    Console.WriteLine("Ispis korisnika starijih od 30 godina:");

    foreach (var user in userList)
    {
        int age = currentDate.Year - user.Item4.Year;

        if (currentDate < user.Item4.AddYears(age))
        {
            age--;
        }

        if (age > 30)
        {
            Console.WriteLine($"{user.Item1} - {user.Item2} - {user.Item3} - {user.Item4}");
        }
    }

    Console.WriteLine("Pritisni Enter za izlaz...");
    Console.ReadKey();
}

static void PrintNegativeAccount(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList)
{
    if (userList.Count == 0)
    {
        Console.WriteLine("Nema korisnika za ispis.");
        return;
    }

    bool foundNegativeAccount = false;
    bool hasNegativeAccount = false;

    Console.WriteLine("Ispis korisnika s barem jednim računom u minusu:");

    foreach (var user in userList)
    {
        hasNegativeAccount = false;

        foreach (var account in user.Item5)
        {

            foreach (var transaction in account.Item3)
            {
                if (transaction.Item2 < 0)
                {
                    hasNegativeAccount = true;
                    break;
                }
            }

            if (hasNegativeAccount) break;

        }

        if (hasNegativeAccount)
        {
            foundNegativeAccount = true;
            Console.WriteLine($"{user.Item1} - {user.Item2} - {user.Item3} - {user.Item4}");
        }

    }

    if (!hasNegativeAccount)
    {
        Console.WriteLine("Nema korisnika s računima u minusu.");
    }

    Console.WriteLine("Pritisni Enter za izlaz...");
    Console.ReadKey();
    return;
}

static void PrintUsers(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList)
{

    Console.WriteLine("Odaberi nacin ispisivanja korisnika:\n\t1 - abecedno po prezimenu\n\t2 - svih onih koji imaju više od 30 godina\n\t3 - svih onih koji imaju barem jedan račun u minusu");

    while (true)
    {
        Console.Write("Unesite broj opcije: ");
        if (int.TryParse(Console.ReadLine(), out var choice))
        {
            switch (choice)
            {
                case 1:
                    PrintAlphabetically(userList);
                    Console.WriteLine("Pritisni Enter za povratak na glavni izbornik...");
                    Console.ReadKey();
                    break;
                case 2:
                    PrintOver30Years(userList);
                    Console.WriteLine("Pritisni Enter za povratak na glavni izbornik...");
                    Console.ReadKey();
                    break;
                case 3:
                    PrintNegativeAccount(userList);
                    Console.WriteLine("Pritisni Enter za povratak na glavni izbornik..");
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine("Opcija nije ponudena! Pokusajte ponovno.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Unos mora biti broj (1, 2, ili 3)!");
            continue;
        }
        break;
    }
}

