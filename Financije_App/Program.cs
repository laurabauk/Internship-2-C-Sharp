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
                    EditUser(userList);
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

        Console.WriteLine("Unesite ime zeljenog korisnika: ");
        var firstName = Console.ReadLine();
        Console.WriteLine("Unesite prezime zeljenog korisnika:");
        var lastName = Console.ReadLine();

        var selectedUser = userList.Find(user => user.Item2.Equals(firstName) && user.Item3.Equals(lastName));

        if (selectedUser.Item1 == 0)
        {
            Console.WriteLine("Korisnik nije pronaden. Pritisnite Enter za povratak na glavni izbornik...");
            Console.ReadKey();
            continue;
        }

        Console.WriteLine($"Odabrani korisnik: {selectedUser.Item2} {selectedUser.Item3}");

        if (selectedUser.Item5.Count == 0)
        {
            Console.WriteLine("Nema računa za ovog korisnika.");
            Console.ReadKey();
            continue;
        }

        Console.WriteLine("Odaberite račun:");
        for (int i = 0; i < selectedUser.Item5.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {selectedUser.Item5[i].Item1} (Stanje: {selectedUser.Item5[i].Item2} EUR)");
        }

        int.TryParse(Console.ReadLine(), out var accountIndex);

        if (accountIndex < 1 || accountIndex > selectedUser.Item5.Count)
        {
            Console.WriteLine("Nevazeci odabir računa. Pritisnite Enter za povratak...");
            Console.ReadKey();
            continue;
        }

        var selectedAccount = selectedUser.Item5[accountIndex - 1];

        Console.Clear();

        Console.WriteLine($"Odabran je {selectedAccount.Item1} racun.");
        Console.WriteLine("Unesite zeljenu opciju za upravljanje transakcijama na tom racunu:\n1 - Unos nove transakcije\n\ta) trenutno izvrsena transakcija\n\tb) ranije izvrsena transakcija\n2 - Brisanje transakcije\n\ta) po id-u\n\tb) ispod unesenog iznosa\n\tc) iznad unesenog iznosa\n\td) svih prihoda\n\te) svih rashoda\n\tf) svih transakcija za odabranu kategoriju\n3 - Uredivanje transakcije\n\ta) po id-u\n4 - Pregled transakcija\n\ta) sve transakcije kako su spremljene\n\tb) sve transakcije sortirane po iznosu uzlazno\n\tc)sve transakcije po iznosu silazno\n\td)sve transakcije sortirane po opisu abecedno\n\te) sve transakcije po datumu uzlazno\n\tf)sve transakcije po datumu silazno\n\tg)svi prihodi\n\th)svi rashodi\n\ti)sve transakcije za odabranu kategoriju\n\tj)sve transakcije za odabrani tip i kategoriju\n5 - Financijsko izvjesce\n\ta)trenutno stanje racuna\n\tb)broj ukupnih transakcija\n\tc)ukupan iznos prihoda i rashoda za odabrani mjesec i godinu\n\td)postotak udjela rashoda za odabranu kategoriju\n\te)prosjecni iznos transakcije za odabrani mjesec i godinu\n\tf)prosjecni iznos transakcije za odabranu kategoriju");

        int.TryParse(Console.ReadLine(), out var transactionOption);

        switch (transactionOption)
        {
            case 1:
                CreateTransaction(userList, ref nextTransactionId);
                continue;
            case 2:
                DeleteTransactions(selectedAccount.Item3);
                break;
            case 3:
                EditTransaction(selectedAccount.Item3);
                break;
            case 4:
                ViewTransactions(selectedAccount.Item3);
                break;
            case 5:
                PrintFinancialReport(selectedAccount.Item3);
                break;
            default:
                Console.WriteLine("Nevazeca Opcija! Pokusajte ponovno.");
                continue;
        }

        return;

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
        else
        {
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

    Console.WriteLine("Odaberite račun za transakciju:");
    for (int i = 0; i < selectedUser.Item5.Count; i++)
    {
        Console.WriteLine($"{i + 1} - {selectedUser.Item5[i].Item1} (Stanje: {selectedUser.Item5[i].Item2} EUR)");
    }

    int accountIndex;
    while (true)
    {
        int.TryParse(Console.ReadLine(), out accountIndex);

        if (accountIndex >= 1 && accountIndex <= selectedUser.Item5.Count)
        {
            selectedUser.Item5[accountIndex - 1].Item3.Add(transaction);
            break;
        }
        else
        {
            Console.WriteLine("Nevažeći odabir računa. Pokušajte ponovo.");
        }
    }

    Console.WriteLine("Transakcija uspješno dodana na odabrani račun! Pritisnite Enter za izlaz...");
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
    else
    {
        DeleteUserByFullName(userList);
        Console.WriteLine("Pritisni Enter za povratak na glavni izbornik...");
        Console.ReadKey();
        return;
    }

}

static void DeleteUserById(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList)
{

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

        for (int i = 0; i < userList.Count(); i++)
        {
            if (userList[i].Item1 == userId)
            {
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

static void DeleteUserByFullName(List<(int, string, string, DateOnly, List<(string, double, List<(int, double, string, string, string, DateTime)>)>)> userList)
{

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

    } while (!userFound);

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

static void DeleteTransactions(List<(int, double, string, string, string, DateTime)> transactions)
{
    if (transactions.Count == 0)
    {
        Console.WriteLine("Nema dostupnih transakcija za brisanje.");
        return;
    }

    Console.WriteLine("Odaberite opciju za brisanje transakcija:\n\t1 - po ID-u\n\t2 - ispod unesenog iznosa\n\t3 - iznad unesenog iznosa\n\t4 - svih prihoda\n\t5 - svih rashoda\n\t6 - svih transakcija za odabranu kategoriju");


    int.TryParse(Console.ReadLine(), out var option);

    switch (option)
    {
        case 1:
            DeleteTransactionById(transactions);
            break;
        case 2:
            DeleteTransactionsBelowAmount(transactions);
            break;
        case 3:
            DeleteTransactionsAboveAmount(transactions);
            break;
        case 4:
            DeleteAllIncome(transactions);
            break;
        case 5:
            DeleteAllExpenses(transactions);
            break;
        case 6:
            DeleteTransactionsByCategory(transactions);
            break;
        default:
            Console.WriteLine("Nevazeca opcija!");
            break;
    }
}

static void DeleteTransactionById(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.WriteLine("Sve transakcije:");
    foreach (var transaction in transactions)
    {
        Console.WriteLine($"ID: {transaction.Item1}, Iznos: {transaction.Item2} EUR, Opis: {transaction.Item3}, Tip: {transaction.Item4}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }

    Console.Write("Unesite ID transakcije za brisanje: ");
    int.TryParse(Console.ReadLine(), out var id);

    var transactionToRemove = transactions.Find(t => t.Item1 == id);

    if (transactionToRemove.Item1 != 0)
    {
        transactions.Remove(transactionToRemove);
        Console.WriteLine("Transakcija uspješno obrisana!");
        return;
    }
    else
    {
        Console.WriteLine("Transakcija s tim ID-om nije pronađena.");
    }
}

static void DeleteTransactionsBelowAmount(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.Write("Unesite iznos ispod kojeg želite izbrisati sve transakcije: ");
    double.TryParse(Console.ReadLine(), out var amount);

    transactions.RemoveAll(t => t.Item2 < amount);
    Console.WriteLine($"Sve transakcije ispod {amount} EUR su obrisane.");
}

static void DeleteTransactionsAboveAmount(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.Write("Unesite iznos iznad kojeg želite izbrisati sve transakcije: ");
    double.TryParse(Console.ReadLine(), out var amount);

    transactions.RemoveAll(t => t.Item2 > amount);
    Console.WriteLine($"Sve transakcije iznad {amount} EUR su obrisane.");
}

static void DeleteAllIncome(List<(int, double, string, string, string, DateTime)> transactions)
{
    transactions.RemoveAll(t => t.Item4 == "Prihod");
    Console.WriteLine("Sve transakcije tipa 'Prihod' su obrisane.");
}

static void DeleteAllExpenses(List<(int, double, string, string, string, DateTime)> transactions)
{
    transactions.RemoveAll(t => t.Item4 == "Rashod");
    Console.WriteLine("Sve transakcije tipa 'Rashod' su obrisane.");
}

static void DeleteTransactionsByCategory(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.Write("Unesite kategoriju za brisanje transakcija: ");
    var category = Console.ReadLine();

    transactions.RemoveAll(t => t.Item5.Equals(category, StringComparison.OrdinalIgnoreCase));
    Console.WriteLine($"Sve transakcije za kategoriju '{category}' su obrisane.");
}

static void ViewTransactions(List<(int, double, string, string, string, DateTime)> transactions)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Odaberite nacin na koji zelite pregledati transakcije:\n\t1 - sve transakcije kako su spremljene\n\t2 - sve transakcije sortirane po iznosu uzlazno\n\t3 - sve transakcije sortirane po iznosu silazno\n\t4 - sve transakcije sortirane po opisu abecedno\n\t5 - sve transakcije sortirane po datumu uzlazno\n\t6 - sve transakcije sortirane po datumu silazno\n\t7 - svi prihodi\n\t8 - svi rashodi\n\t9 - sve transakcije za odabranu kategoriju\n\t10 - sve transakcije za odabrani tip i kategoriju\n\t11 - izlaz");

        int option;
        if (int.TryParse(Console.ReadLine(), out option))
        {
            switch (option)
            {
                case 1:
                    PrintAllTransactions(transactions);
                    break;
                case 2:
                    PrintTransactionsAscending(transactions);
                    break;
                case 3:
                    PrintTransactionsDescending(transactions);
                    break;
                case 4:
                    PrintTransactionsSortedByDescription(transactions);
                    break;
                case 5:
                    PrintTransactionsAscendingDate(transactions);
                    break;
                case 6:
                    PrintTransactionsDescendingDate(transactions);
                    break;
                case 7:
                    PrintIncomeTransactions(transactions);
                    break;
                case 8:
                    PrintExpenseTransactions(transactions);
                    break;
                case 9:
                    PrintTransactionsByCategory(transactions);
                    break;
                case 10:
                    PrintTransactionsByTypeAndCategory(transactions);
                    break;
                case 11:
                    return;
                default:
                    Console.WriteLine("Nevazeca opcija! Pokušajte ponovno.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Nevazeci unos! Pokusajte ponovno.");
        }
    }
}

static void PrintAllTransactions(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.WriteLine("Sve transakcije:");
    foreach (var transaction in transactions)
    {
        Console.WriteLine($"Tip: {transaction.Item4}, Iznos: {transaction.Item2} Eur, Opis: {transaction.Item3}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }
    Console.WriteLine("Pritisnite Enter za povratak na izbornik...");
    Console.ReadKey();
}

static void PrintTransactionsAscending(List<(int, double, string, string, string, DateTime)> transactions)
{
    var sortedTransactions = transactions.OrderBy(t => t.Item2).ToList();
    Console.WriteLine("Transakcije sortirane po iznosu uzlazno:");
    foreach (var transaction in sortedTransactions)
    {
        Console.WriteLine($"Tip: {transaction.Item4}, Iznos: {transaction.Item2} Eur, Opis: {transaction.Item3}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }
    Console.WriteLine("Pritisnite Enter za povratak na izbornik...");
    Console.ReadKey();
}

static void PrintTransactionsDescending(List<(int, double, string, string, string, DateTime)> transactions)
{
    var sortedTransactions = transactions.OrderByDescending(t => t.Item2).ToList();
    Console.WriteLine("Transakcije sortirane po iznosu silazno:");
    foreach (var transaction in sortedTransactions)
    {
        Console.WriteLine($"Tip: {transaction.Item4}, Iznos: {transaction.Item2} Eur, Opis: {transaction.Item3}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }
    Console.WriteLine("Pritisnite Enter za povratak na izbornik...");
    Console.ReadKey();
}

static void PrintTransactionsSortedByDescription(List<(int, double, string, string, string, DateTime)> transactions)
{
    var sortedTransactions = transactions.OrderBy(t => t.Item3).ToList();
    Console.WriteLine("Transakcije sortirane po opisu abecedno:");
    foreach (var transaction in sortedTransactions)
    {
        Console.WriteLine($"Tip: {transaction.Item4}, Iznos: {transaction.Item2} Eur, Opis: {transaction.Item3}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }
    Console.WriteLine("Pritisnite Enter za povratak na izbornik...");
    Console.ReadKey();
}

static void PrintTransactionsAscendingDate(List<(int, double, string, string, string, DateTime)> transactions)
{
    var sortedTransactions = transactions.OrderBy(t => t.Item6).ToList();
    Console.WriteLine("Transakcije sortirane po datumu uzlazno:");
    foreach (var transaction in sortedTransactions)
    {
        Console.WriteLine($"Tip: {transaction.Item4}, Iznos: {transaction.Item2} Eur, Opis: {transaction.Item3}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }
    Console.WriteLine("Pritisnite Enter za povratak na izbornik...");
    Console.ReadKey();
}

static void PrintTransactionsDescendingDate(List<(int, double, string, string, string, DateTime)> transactions)
{
    var sortedTransactions = transactions.OrderByDescending(t => t.Item6).ToList();
    Console.WriteLine("Transakcije sortirane po datumu silazno:");
    foreach (var transaction in sortedTransactions)
    {
        Console.WriteLine($"Tip: {transaction.Item4}, Iznos: {transaction.Item2} Eur, Opis: {transaction.Item3}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }
    Console.WriteLine("Pritisnite Enter za povratak na izbornik...");
    Console.ReadKey();
}

static void PrintIncomeTransactions(List<(int, double, string, string, string, DateTime)> transactions)
{
    var incomeTransactions = transactions.Where(t => t.Item4 == "Prihod").ToList();
    Console.WriteLine("Svi prihodi:");
    foreach (var transaction in incomeTransactions)
    {
        Console.WriteLine($"Tip: {transaction.Item4}, Iznos: {transaction.Item2} Eur, Opis: {transaction.Item3}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }
    Console.WriteLine("Pritisnite Enter za povratak na izbornik...");
    Console.ReadKey();
}

static void PrintExpenseTransactions(List<(int, double, string, string, string, DateTime)> transactions)
{
    var expenseTransactions = transactions.Where(t => t.Item4 == "Rashod").ToList();
    Console.WriteLine("Svi rashodi:");
    foreach (var transaction in expenseTransactions)
    {
        Console.WriteLine($"Tip: {transaction.Item4}, Iznos: {transaction.Item2} Eur, Opis: {transaction.Item3}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }
    Console.WriteLine("Pritisnite Enter za povratak na izbornik...");
    Console.ReadKey();
}

static void PrintTransactionsByCategory(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.Write("Unesite kategoriju: ");
    string category = Console.ReadLine();
    var categoryTransactions = transactions.Where(t => t.Item5.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    Console.WriteLine($"Transakcije za kategoriju {category}:");
    foreach (var transaction in categoryTransactions)
    {
        Console.WriteLine($"Tip: {transaction.Item4}, Iznos: {transaction.Item2} Eur, Opis: {transaction.Item3}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }
    Console.WriteLine("Pritisnite Enter za povratak na izbornik...");
    Console.ReadKey();
}

static void PrintTransactionsByTypeAndCategory(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.Write("Unesite tip (Prihod/Rashod): ");
    string type = Console.ReadLine();
    Console.Write("Unesite kategoriju: ");
    string category = Console.ReadLine();

    var filteredTransactions = transactions.Where(t => t.Item4.Equals(type, StringComparison.OrdinalIgnoreCase) && t.Item5.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    Console.WriteLine($"Transakcije za tip '{type}' i kategoriju '{category}':");
    foreach (var transaction in filteredTransactions)
    {
        Console.WriteLine($"Tip: {transaction.Item4}, Iznos: {transaction.Item2} Eur, Opis: {transaction.Item3}, Kategorija: {transaction.Item5}, Datum: {transaction.Item6}");
    }
    Console.WriteLine("Pritisnite Enter za povratak na izbornik...");
    Console.ReadKey();
}

static void PrintFinancialReport(List<(int, double, string, string, string, DateTime)> transactions)
{
    if (transactions.Count == 0)
    {
        Console.WriteLine("Nema dostupnih transakcija za financijsko izvješće.");
        return;
    }

    Console.Clear();
    Console.WriteLine("\nOdaberite opciju za financijsko izvjesce:\n\t1 - trenutno stanje racuna\n\t2 - broj ukupnih transakcija\n\t3 - ukupan iznos prihoda i rashoda za odabrani mjesec i godinu\n\t4 - postotak udjela rashoda za odabranu kategoriju\n\t5 - prosječni iznos transakcije za odabrani mjesec i godinu\n\t6 - prosječni iznos transakcije za odabranu kategoriju\n\t7 - izlaz");

    int.TryParse(Console.ReadLine(), out var reportOption);

    switch (reportOption)
    {
        case 1:
            PrintCurrentBalance(transactions);
            break;
        case 2:
            PrintTotalTransactionCount(transactions);
            break;
        case 3:
            PrintIncomeExpenseForMonth(transactions);
            break;
        case 4:
            PrintExpensePercentageByCategory(transactions);
            break;
        case 5:
            PrintAverageTransactionForMonth(transactions);
            break;
        case 6:
            PrintAverageTransactionForCategory(transactions);
            break;
        case 7:
            return;
        default:
            Console.WriteLine("Nevazeca opcija! Pokušajte ponovno.");
            break;
    }

    Console.WriteLine("\nPritisnite Enter za izlaz...");
    Console.ReadLine();
    return;
}

static void PrintCurrentBalance(List<(int, double, string, string, string, DateTime)> transactions)
{
    double totalIncome = 0;
    double totalExpense = 0;

    foreach (var transaction in transactions)
    {
        if (transaction.Item4 == "Prihod")
        {
            totalIncome += transaction.Item2;
        }
        else if (transaction.Item4 == "Trošak")
        {
            totalExpense += transaction.Item2;
        }
    }

    double currentBalance = totalIncome - totalExpense;

    Console.WriteLine("\nTrenutno stanje racuna:");
    Console.WriteLine($"Ukupni prihodi: {totalIncome} Eur");
    Console.WriteLine($"Ukupni troskovi: {totalExpense} Eur");
    Console.WriteLine($"Stanje: {currentBalance} EUR");

    if (currentBalance < 0)
    {
        Console.WriteLine("Upozorenje: Trenutno ste u minusu!");
    }

}

static void PrintTotalTransactionCount(List<(int, double, string, string, string, DateTime)> transactions)
{
    int transactionCount = transactions.Count;

    Console.WriteLine("\nUkupan broj transakcija:");
    Console.WriteLine($"Broj transakcija: {transactionCount}");
}

static void PrintIncomeExpenseForMonth(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.Write("Unesite mjesec (1-12): ");
    int.TryParse(Console.ReadLine(), out var month);

    Console.Write("Unesite godinu: ");
    int.TryParse(Console.ReadLine(), out var year);

    double totalIncome = 0;
    double totalExpense = 0;

    foreach (var transaction in transactions)
    {
        if (transaction.Item6.Month == month && transaction.Item6.Year == year)
        {
            if (transaction.Item4 == "Prihod")
            {
                totalIncome += transaction.Item2;
            }
            else if (transaction.Item4 == "Trosak")
            {
                totalExpense += transaction.Item2;
            }
        }
    }

    Console.WriteLine($"\nUkupno za {month}/{year}:");
    Console.WriteLine($"Prihodi: {totalIncome} Eur");
    Console.WriteLine($"Troškovi: {totalExpense} Eur");
}

static void PrintExpensePercentageByCategory(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.Write("Unesite kategoriju (npr. Hrana, Sport): ");
    var category = Console.ReadLine();

    double totalExpense = 0;
    double categoryExpense = 0;

    foreach (var transaction in transactions)
    {
        if (transaction.Item4 == "Trosak")
        {
            totalExpense += transaction.Item2;
            if (transaction.Item5.Equals(category, StringComparison.OrdinalIgnoreCase))
            {
                categoryExpense += transaction.Item2;
            }
        }
    }

    if (totalExpense > 0)
    {
        double percentage = (categoryExpense / totalExpense) * 100;
        Console.WriteLine($"\nPostotak troskova za kategoriju {category}: {percentage:F2}%");
    }
    else
    {
        Console.WriteLine("Nema dostupnih troskova za izracun postotka.");
    }
}

static void PrintAverageTransactionForMonth(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.Write("Unesite mjesec (1-12): ");
    int.TryParse(Console.ReadLine(), out var month);

    Console.Write("Unesite godinu: ");
    int.TryParse(Console.ReadLine(), out var year);

    double totalAmount = 0;
    int transactionCount = 0;

    foreach (var transaction in transactions)
    {
        if (transaction.Item6.Month == month && transaction.Item6.Year == year)
        {
            totalAmount += transaction.Item2;
            transactionCount++;
        }
    }

    if (transactionCount > 0)
    {
        double average = totalAmount / transactionCount;
        Console.WriteLine($"\nProsjecni iznos transakcije za {month}/{year}: {average:F2} EUR");
    }
    else
    {
        Console.WriteLine("Nema pronadenih transakcija za odabrani mjesec i godinu.");
    }
}

static void PrintAverageTransactionForCategory(List<(int, double, string, string, string, DateTime)> transactions)
{
    Console.Write("Unesite kategoriju (npr. Hrana, Sport): ");
    var category = Console.ReadLine();

    double totalAmount = 0;
    var transactionCount = 0;

    foreach (var transaction in transactions)
    {
        if (transaction.Item5.Equals(category, StringComparison.OrdinalIgnoreCase))
        {
            totalAmount += transaction.Item2;
            transactionCount++;
        }
    }

    if (transactionCount > 0)
    {
        double average = totalAmount / transactionCount;
        Console.WriteLine($"\nProsjecni iznos transakcije za kategoriju {category}: {average:F2} Eur");
    }
    else
    {
        Console.WriteLine($"Nema pronadenih transakcija za kategoriju {category}.");
    }
}
static void EditTransaction(List<(int transactionId, double amount, string description, string type, string category, DateTime date)> transactions)
{

    Console.WriteLine("Popis svih transakcija:");
    for (int i = 0; i < transactions.Count; i++)
    {
        var transaction = transactions[i];
        Console.WriteLine($"{i + 1}. ID: {transaction.transactionId} - {transaction.type} - {transaction.amount} Eur - {transaction.description} - {transaction.category} - {transaction.date:dd.MM.yyyy}");
    }

    Console.Write("Unesite ID transakcije za uredivanje: ");
    int.TryParse(Console.ReadLine(), out int transactionId);

    var transactionIndex = -1;
    for (int i = 0; i < transactions.Count; i++)
    {
        if (transactions[i].transactionId == transactionId)
        {
            transactionIndex = i;
            break;
        }
    }

    if (transactionIndex == -1)
    {
        Console.WriteLine("Transakcija s tim ID-em nije pronadena.");
        return;
    }

    var selectedTransaction = transactions[transactionIndex];
    Console.WriteLine($"Pronadena transakcija: {selectedTransaction.type} - {selectedTransaction.amount} Eur - {selectedTransaction.description}");

    Console.WriteLine("\nSto zelite promijeniti?\n1 - Iznos\n2 - Opis\n3 - Tip (prihod/rashod)\n4 - Kategorija\n5 - Datum");
    int.TryParse(Console.ReadLine(), out int editOption);

    switch (editOption)
    {
        case 1:
            Console.Write("Unesite novi iznos: ");
            double.TryParse(Console.ReadLine(), out double newAmount);
            selectedTransaction.amount = newAmount;
            break;
        case 2:
            Console.Write("Unesite novi opis: ");
            selectedTransaction.description = Console.ReadLine();
            break;
        case 3:
            Console.Write("Unesite novi tip (prihod/rashod): ");
            selectedTransaction.type = Console.ReadLine();
            break;
        case 4:
            Console.Write("Unesite novu kategoriju: ");
            selectedTransaction.category = Console.ReadLine();
            break;
        case 5:
            Console.Write("Unesite novi datum (date-month-year): ");
            DateTime.TryParse(Console.ReadLine(), out DateTime newDate);
            selectedTransaction.date = newDate;
            break;
        default:
            Console.WriteLine("Nevažeća opcija! Ponovno pokušajte.");
            return;
    }

    transactions[transactionIndex] = selectedTransaction;
    Console.WriteLine("Transakcija uspješno uredena!");
}

static void EditUser(List<(int id, string firstName, string lastName, DateOnly birthDate, List<(string accountName, double balance, List<(int transactionId, double amount, string description, string type, string category, DateTime date)>)>)> userList)
{
    Console.WriteLine("Popis svih korisnika:");
    for (int i = 0; i < userList.Count; i++)
    {
        var user = userList[i];
        Console.WriteLine($"{i + 1}. ID: {user.id} - {user.firstName} {user.lastName} - Roden: {user.birthDate:dd.MM.yyyy}");
    }

    Console.Write("\nUnesite ID korisnika za uredivanje: ");
    int.TryParse(Console.ReadLine(), out int userId);


    var userIndex = -1;
    for (int i = 0; i < userList.Count; i++)
    {
        if (userList[i].id == userId)
        {
            userIndex = i;
            break;
        }
    }

    if (userIndex == -1)
    {
        Console.WriteLine("Korisnik s tim ID-om nije pronaden.");
        return;
    }


    var selectedUser = userList[userIndex];
    Console.WriteLine($"Pronaden korisnik: {selectedUser.firstName} {selectedUser.lastName}");


    Console.WriteLine("\nSto zelite promijeniti?\n1 - Ime\n2 - Prezime\n3 - Datum rodenja");
    int.TryParse(Console.ReadLine(), out int editOption);


    switch (editOption)
    {
        case 1:
            Console.Write("Unesite novo ime: ");
            selectedUser.firstName = Console.ReadLine();
            break;
        case 2:
            Console.Write("Unesite novo prezime: ");
            selectedUser.lastName = Console.ReadLine();
            break;
        case 3:
            Console.Write("Unesite novi datum rođenja (date-month-year): ");
            DateOnly.TryParse(Console.ReadLine(), out DateOnly newBirthDate);
            selectedUser.birthDate = newBirthDate;
            break;
        default:
            Console.WriteLine("Nevazeca opcija! Pokusajte ponovno.");
            return;
    }


    userList[userIndex] = selectedUser;
    Console.WriteLine("Korisnik uspjesno ureden!");
}



