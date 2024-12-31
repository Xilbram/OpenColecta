using NUnit.Framework;
using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace MyApp.NUnitTests
{
    [TestFixture]
    public class DatabaseConnectionTests
    {
        // Replace with your actual connection string
        private readonly string _connectionString =
            "Host=localhost;Database=cco;Username=gix;Password=1309";

        [Test]
        public async Task ConnectionShouldOpenAsync()
        {
            // Create a new Npgsql connection
            await using var connection = new NpgsqlConnection(_connectionString);

            // Attempt to open the connection
            await connection.OpenAsync();
            Assert.That(connection.State, Is.EqualTo(ConnectionState.Open), 
                "Expected the connection to be open.");

            // (Optional) Execute a simple query to confirm you can run commands
            await using var command = new NpgsqlCommand("SELECT 1", connection);
            var result = await command.ExecuteScalarAsync();
            
            Assert.That(result, Is.EqualTo(1), "SELECT 1 did not return 1.");
        }
    }
}