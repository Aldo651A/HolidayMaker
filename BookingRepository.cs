using MySqlConnector;


public class BookingRepository(MySqlConnection connection)
{
    // Start the function. It takes customer info and promises to return an ID number.
    public async Task<int> CreateCustomerAsync(string name, string lastName, string email, string telephone)
    {
        // Create the SQL command and tell the datebase where to insert the data
        await using var command = new MySqlCommand(
           "INSERT INTO customers (name, last_name, email, telephone) VALUES (@name, @last_name, @email, @telephone);", 
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
    
    // 
    public async Task<List<AvailableRoom>> FindAvailableRoomsAsync(
        string city, 
        DateTime startDate, 
        DateTime endDate, 
        int numPersons,
        bool poolRequired,
        bool restaurantRequired,
        int? maxBeach,
        int? maxCenter,
        string sortBy)
    {
        string sql = """
            SELECT 
                h.name AS hotel_name,
                h.city,
                r.id AS room_id,
                r.name AS room_name,
                r.capacity,
                r.price
            FROM rooms r
            JOIN hotels h ON r.hotel_id = h.id
            WHERE h.city = @city
              AND r.capacity >= @num_persons
              AND (@pool_required = 0 OR h.has_pool = 1)
              AND (@restaurant_required = 0 OR h.has_restaurant = 1)
              AND (@max_beach IS NULL OR h.distance_to_beach <= @max_beach)
              AND (@max_center IS NULL OR h.distance_to_city_center <= @max_center)
              AND NOT EXISTS (
                  SELECT 1 FROM bookings b 
                  WHERE b.room_id = r.id
                    AND @start_date < b.end_date 
                    AND @end_date > b.start_date
              )
            ORDER BY 
                CASE WHEN @sort_by = 'price' THEN r.price END ASC,
                CASE WHEN @sort_by = 'rating' THEN h.rating END DESC;
            """;

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@city", city);
        command.Parameters.AddWithValue("@num_persons", numPersons);
        command.Parameters.AddWithValue("@start_date", startDate);
        command.Parameters.AddWithValue("@end_date", endDate);
        command.Parameters.AddWithValue("@pool_required", poolRequired ? 1 : 0);
        command.Parameters.AddWithValue("@restaurant_required", restaurantRequired ? 1 : 0);
        command.Parameters.AddWithValue("@max_beach", (object?)maxBeach ?? DBNull.Value);
        command.Parameters.AddWithValue("@max_center", (object?)maxCenter ?? DBNull.Value);
        command.Parameters.AddWithValue("@sort_by", sortBy);

        await command.PrepareAsync();
        await using var reader = await command.ExecuteReaderAsync();
        var rooms = new List<AvailableRoom>();

        while (await reader.ReadAsync())
        {
            rooms.Add(new AvailableRoom(
                reader.GetString("hotel_name"),
                reader.GetString("city"),
                reader.GetInt32("room_id"),
                reader.GetString("room_name"),
                reader.GetInt32("capacity"),
                reader.GetDecimal("price")));
        }
        return rooms;
    }
} 











