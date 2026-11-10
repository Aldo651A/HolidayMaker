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


    switch (input)
    {
        case "1":
            Console.WriteLine("--REGISTRERA NY KUND--");
            Console.Write("Ange förnamn: ");
            string firstName = Console.ReadLine()!;
            Console.Write("Ange efternamn: ");
            string lastName = Console.ReadLine()!;
            Console.Write("Ange e-post: ");
            string email = Console.ReadLine()!;
            Console.Write("Ange telefonnummer: ");
            string phone = Console.ReadLine()!;


            try
            {
                int newCustomerId = await repository.CreateCustomerAsync(firstName, lastName, email, phone);
                selectedCustomer = new Customer(newCustomerId, firstName, lastName, email, phone);
                Console.WriteLine($"\nKlart! Kunden registrerades med ID: {newCustomerId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nFel vid registrering: {ex.Message}");
            }
            break;

        case "2":
            Console.WriteLine("--- SÖK BEFINTLIG KUND ---");
            Console.Write("Ange efternamn att söka efter: ");
            string searchLastName = Console.ReadLine()!;
            
              
    }




}
