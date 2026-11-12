using System.Xml;
using MySqlConnector;

// ANSLUTNING TILL DATABASEN 
// Här talar vi om var databasen finns 
string connectionString = "Server=localhost;Database=holiday_maker;User ID=root;Password=kurslosenord;UseAffectedRows=false;";

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
    Console.WriteLine("3. Sök lediga rum");
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

            try
            {
                // Call the datebase function to search for customers and get a list back
                List<Customer> matchingCustomers = await repository.FindCustomersByLastNameAsync(searchLastName);
                
                // Check if the list is empty (o custumors found)
                if (matchingCustomers.Count == 0)
                {
                    Console.WriteLine("\nTyvärr, hittade inga kunder med det efternamnet. ");
                    break;
                }
                
                Console.WriteLine("\nMatchande kunder funna i databasen");
                foreach (var customer in matchingCustomers) // Loop through each customer found in the list
                {
                    Console.WriteLine($"[ID: {customer.Id}] {customer.Name} {customer.LastName} {customer.Telephone}");
                }

                Console.WriteLine();
                Console.Write("Skriv in ID-numret på kunden du vill aktivera: ");
                if (int.TryParse(Console.ReadLine()!, out int chosenId))
                {
                    // Serach the list to find a customer with that exact id.
                    var found = matchingCustomers.Find(c => c.Id == chosenId);
                    if (found != null) // If the customer was found in the list
                    {
                        // Set this customer as the currently active/selected customer
                        selectedCustomer = found;
                        Console.WriteLine($"\nKunden {selectedCustomer.Name} {selectedCustomer.LastName} är nu aktiv!");
                    }
                    else
                    {
                        Console.WriteLine("\nDet ID-numret matchade ingen i listan.");
                    }
                }
                else
                {
                    Console.WriteLine("\nOgiltigt ID-format.");
                }

            }
            // Catch any unexpected database errors or system errors
            catch (Exception ex)
            {
                Console.WriteLine($"\nFel vid sökning: {ex.Message}");
            }
            break;

        case "3":
            Console.WriteLine("----SÖK LEDIGA RUM----");
            Console.WriteLine("Välj ort: ");
            Console.WriteLine("1. Visby");
            Console.WriteLine("2. Båstad");
            Console.WriteLine("3. Marstrand");
            Console.Write("Val (1-3): ");

            string cityChoice = Console.ReadLine()!;
            string city = "";
            
            if (cityChoice == "1") city = "Visby";
            else if (cityChoice == "2") city = "Båstad";
            else if (cityChoice == "3") city = "Marstrand";
            else
            {
                Console.WriteLine("Ogiltig val av ort. ");
                break;
            }

            Console.Write("Incheckningsdatum (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            {
                Console.WriteLine("Ogiltigt datumformat. ");
            }

            Console.Write("Utcheckningsdatum (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                Console.WriteLine("Ogiltigt datumformat. ");
            }

            Console.Write("Antal personer: ");
            if (!int.TryParse(Console.ReadLine(), out int numPersons))
            {
                Console.WriteLine("Ogiltig antal personer. ");
                break;
            }

            Console.Write("Krävs pool (j/n): ");
            bool poolRequired = Console.ReadLine()!.ToLower() == "j";

            Console.Write("Krävs restaurang (j/n): ");
            bool restaurantRequired = Console.ReadLine()!.ToLower() == "j";

            Console.Write("Max avstånd till strande i meter (Tryck enter för att hoppa över): ");
            string beachInput = Console.ReadLine()!;
            int? maxBeach = int.TryParse(beachInput, out int b) ? b : null;

            Console.Write("Max avstånd till centrum i meter (Tryck enter för att hoppa över): ");
            string centerInput = Console.ReadLine()!;
            int? maxCenter = int.TryParse(centerInput, out int cDist) ? cDist : null;

            Console.Write("Sortera efter pris (p) eller omdöme (o): ");
            string sortBy = Console.ReadLine()!.ToLower() == "o" ? "rating" : "price";

            try
            {
                List<AvailableRoom> rooms = await repository.FindAvailableRoomsAsync(
                    city, startDate, endDate, numPersons, poolRequired, restaurantRequired, maxBeach, maxCenter, sortBy);

                if (rooms.Count == 0)
                {
                    Console.WriteLine("\nTyvärr, inga lediga rum matchade sökningen. ");
                    break;
                }

                Console.WriteLine("\nLediga rum: ");
                foreach (var room in rooms)
                {
                    Console.WriteLine($"[Rum-ID: {room.RoomId}] {room.HotelName} ({room.City}) | {room.RoomName} | Kapacitet: {room.Capacity} | Pris: {room.Price} kr/natt");
                }   
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nFel vid sökning: {ex.Message}" );
            }
            break;


        case "0":
            Console.WriteLine("Avslutar programmet. Hejdå!");
            return;

        default:
            Console.WriteLine("Ogiltig menyval, försök igen");
            break;
        
    }
    Console.WriteLine("\nTryck på Enter för att återgå till menyn...");
    Console.ReadLine();
    Console.Clear();

}

