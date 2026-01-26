using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;

namespace Playground;

public class SqlDataAccess 
{
    private string _connectionString = "";
    private SqliteConnection? dbConnection;
    private static string connStringConstructor = "Data Source={0};Cache=Shared";

    public SqlDataAccess(string filePathLocationToDB)
    {
        UpdateConnection(filePathLocationToDB);
    }

    /// <summary>
    /// Given the file path to the .db file, we generate a connection string for our SQLite Connection
    /// </summary>
    /// <param name="filePathLocationToDB"></param>
    public void UpdateConnection(string filePathLocationToDB)
    {
        _connectionString = string.Format(connStringConstructor, filePathLocationToDB);
        dbConnection = new SqliteConnection(_connectionString);
        dbConnection.Open();
    }

    //public async Task<bool> VerifyTableExists(string tableName)
    //{
    //    string sql = $"SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{tableName}';";
    //    var result = await dbConnection.ExecuteScalarAsync(sql);
    //    return result is not null && Convert.ToInt32(result) > 0;
    //}


    /// <summary>
    /// Execute a procedure that returns some result, IE Read
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlProcedure"></param>
    /// <returns></returns>
    public async Task<List<T>> GetDataASYNC<T>(string sqlProcedure)
    {
        try
        {
            dbConnection ??= new(_connectionString);
            var result = await dbConnection.QueryAsync<T>(sqlProcedure);
            return [.. result];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return [];
        }
    }


    /// <summary>
    /// Execute a Procedure that has no return result (IE Delete)
    /// </summary>
    /// <param name="storedProcedure"></param>
    /// <returns></returns>
    public async Task ExecuteProcedureASYNC(string storedProcedure)
    {
        try
        {
            dbConnection ??= new(_connectionString);
            await dbConnection.ExecuteAsync(storedProcedure);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }       

    
}