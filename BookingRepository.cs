using MySqlConnector;


public class BookingRepository(MySqlConnection connection)
{
    // Start the function. It takes customer info and promises to return an ID number.
    public async Task<int> CreateCustomerAsync(string name, string lastName, string email, string telephone)
    {
        // Create the SQL command and tell the datebase where to insert the data
        await using var command = new MySqlCommand(
           "INTSERT INTO customers (name, last_name, email, telephone) VALUES (@name, @last_name, @email, @telephone);", 
           connection);

        // Safety step: Link the real customer data to the SQL command.
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@last_name", lastName);
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@telephone", telephone);

        // Prepare and send the command to the database to save customer.
        await command.PrepareAsync();
        await command.ExecuteNonQueryAsync();

        // Get the new customer ID from the datebase and send it back
        return checked((int)command.LastInsertedId);
        
    }
    // Start function. Takes a last name returns a list of customers
    public async Task<List<Customer>> FindCustomersByLastNameAsync(string lastName)
    {
        // Create SQL command to select customer colums where the last name matches
        await using var command = new MySqlCommand(
            "SELECT id, name, last_name, email, telephone FROM customers WHERE last_name LIKE @search;", 
            connection);

        // Safety step Add % signs to allow partial matching (e.g. "Smi" find "Smith")   
        command.Parameters.AddWithValue("@search", "%" + lastName + "%");
        await command.PrepareAsync();
        
        // Execute command and open a reader to scan the database rows
        await using var reader = await command.ExecuteReaderAsync();
        var customers = new List<Customer>(); // Create an empty list to store the found customers
        
        // Loop through the database result, row by row
        while (await reader.ReadAsync())
        {
            // Convert the datebase row colums into a Customers object and add to list
            customers.Add(new Customer(
                reader.GetInt32("id"),
                reader.GetString("name"),
                reader.GetString("last_name"),
                reader.GetString("email"),
                reader.GetString("telephone")
            ));
        }
        return customers; // Return the final list of customers
        
    }
}

