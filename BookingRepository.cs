using MySqlConnector;

public class BookingRepository(MySqlConnection connection)
{
    public async Task<int> CreateCustomerAsync(string name, string lastName, string email, string telephone)
    {
        await using var command = new MySqlCommand(
           "INTSERT INTO customers (name, last_name, )" 
        )
        





    }

    













}
