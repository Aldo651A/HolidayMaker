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

        // 3. CREATE & READ: Create a booking and calculate total price directly in SQL
    public async Task<(int BookingNumber, decimal TotalPrice)> CreateBookingAsync(
        int customerId, int roomId, DateTime startDate, DateTime endDate, int numPersons)
    {
        // Step A: Save the booking in the database
        await using var insertCmd = new MySqlCommand(
            "INSERT INTO bookings (customer_id, room_id, start_date, end_date, num_persons) VALUES (@customer_id, @room_id, @start_date, @end_date, @num_persons);", 
            connection);
            
        insertCmd.Parameters.AddWithValue("@customer_id", customerId);
        insertCmd.Parameters.AddWithValue("@room_id", roomId);
        insertCmd.Parameters.AddWithValue("@start_date", startDate);
        insertCmd.Parameters.AddWithValue("@end_date", endDate);
        insertCmd.Parameters.AddWithValue("@num_persons", numPersons);

        await insertCmd.PrepareAsync();
        await insertCmd.ExecuteNonQueryAsync();
        int newBookingId = checked((int)insertCmd.LastInsertedId);

        // Step B: Get the total price (number of nights * room price) directly via SQL
        await using var selectCmd = new MySqlCommand(
            """
            SELECT (DATEDIFF(b.end_date, b.start_date) * r.price) AS total_price
            FROM bookings b
            JOIN rooms r ON b.room_id = r.id
            WHERE b.id = @id;
            """, connection);
            
        selectCmd.Parameters.AddWithValue("@id", newBookingId);

        await selectCmd.PrepareAsync();
        await using var reader = await selectCmd.ExecuteReaderAsync();
        await reader.ReadAsync();
        
        return (newBookingId, reader.GetDecimal("total_price"));
    }

        // Helper method: Fetch all rooms so the operator can see available Room IDs
    public async Task<List<AvailableRoom>> GetAllRoomsAsync()
    {
        string sql = """
            SELECT h.name AS hotel_name, h.city, r.id AS room_id, r.name AS room_name, r.capacity, r.price
            FROM rooms r
            JOIN hotels h ON r.hotel_id = h.id
            ORDER BY h.name, r.id;
            """;

        await using var command = new MySqlCommand(sql, connection);
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

        // 4. READ: Show all bookings for a specific customer
    public async Task<List<BookingDetail>> GetCustomerBookingsAsync(int customerId)
    {
        string sql = """
            SELECT 
                b.id AS booking_number,
                h.name AS hotel_name,
                r.name AS room_name,
                b.start_date,
                b.end_date,
                b.num_persons,
                (DATEDIFF(b.end_date, b.start_date) * r.price) AS total_price
            FROM bookings b
            JOIN rooms r ON b.room_id = r.id
            JOIN hotels h ON r.hotel_id = h.id
            WHERE b.customer_id = @customer_id
            ORDER BY b.start_date ASC;
            """;

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@customer_id", customerId);

        await command.PrepareAsync();
        await using var reader = await command.ExecuteReaderAsync();
        var bookings = new List<BookingDetail>();

        while (await reader.ReadAsync())
        {
            bookings.Add(new BookingDetail(
                reader.GetInt32("booking_number"),
                reader.GetString("hotel_name"),
                reader.GetString("room_name"),
                reader.GetDateTime("start_date"),
                reader.GetDateTime("end_date"),
                reader.GetInt32("num_persons"),
                reader.GetDecimal("total_price")));
        }
        return bookings;
    }
    
        // 6. DELETE: Cancel a booking so the room becomes available again
    public async Task<int> DeleteBookingAsync(int bookingId)
    {
        await using var command = new MySqlCommand(
            "DELETE FROM bookings WHERE id = @id LIMIT 1;", connection);
        command.Parameters.AddWithValue("@id", bookingId);

        await command.PrepareAsync();
        return await command.ExecuteNonQueryAsync();
    }
    

        // 5. UPDATE: Change the number of persons in a booking (database trigger checks room capacity)
    public async Task<int> UpdateBookingPersonsAsync(int bookingId, int newNumPersons)
    {
        await using var command = new MySqlCommand(
            "UPDATE bookings SET num_persons = @num_persons WHERE id = @id;", connection);
        command.Parameters.AddWithValue("@num_persons", newNumPersons);
        command.Parameters.AddWithValue("@id", bookingId);

        await command.PrepareAsync();
        return await command.ExecuteNonQueryAsync();
    }

} 











