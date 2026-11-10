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

    





}
