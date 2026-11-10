using System.Xml;
using MySqlConnector;

// ANSLUTNING TILL DATABASEN 
// Här talar vi om var databasen finns 
string connectionString = "Server=localhost;Database=holiday_maker;User ID=root;Password=kurslosenord;UseAffectedRows=fals;";

await using var connection = new MySqlConnection(connectionString);
await connection.OpenAsync();

Console.WriteLine("System: Ansluten till Holiday Maker-databasen!");
Console.WriteLine();

// REPOSITORY (klass med databas-queries)
var repository = new BookingRepository(connection);

// 4. Här skapar vi det globala minnet för vem som har ringt (börjar som tom/null)
Customer selectedCustomer = null;



while (true)
{
    Console.WriteLine("=======================================");
    Console.WriteLine("-----HOLIDAY MAKER OPERATORSYSTEM------");
    Console.WriteLine("=======================================");

    // Check if a customer is currently selected
    if (selectedCustomer != null)
    {
        // Print the selected customer's full name and id number
        Console.WriteLine($"Aktiv kund: {selectedCustomer.Name} {selectedCustomer.LastName} (ID: {selectedCustomer.Id})");
    }
    else
    {
        Console.WriteLine("Ingen kund aktiv i systemet just nu.");
    }


    Console.WriteLine();
    Console.WriteLine("1. Registreraen helt ny kund");
    Console.WriteLine("2. Sök och välj en befintlig kund");
    Console.WriteLine("0. Avsluta programmet");
    Console.WriteLine("Välj ett alternativ: ");

    string input = Console.ReadLine()!;
    Console.WriteLine();

    


}
