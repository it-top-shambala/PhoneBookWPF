using Dapper;
using Microsoft.Data.Sqlite;

namespace PhoneBook.Lib;

public class DbContext
{
    private readonly SqliteConnection _sqlite;

    public DbContext()
    {
        const string connectionString = "Data Source=P:\\PhoneBook\\PhoneBook.Lib\\phonebook.db";
        _sqlite = new SqliteConnection(connectionString);
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    private bool ExecuteNonQuery(string sql)
    {
        _sqlite.Open();
        var result = _sqlite.Execute(sql);
        _sqlite.Close();

        return result > 0;
    }

    public bool Insert(PhoneBookItem item)
    {
        var sql = $"""
                   INSERT INTO table_phonebooks(last_name, first_name, phone)
                   VALUES ('{item.LastName}', '{item.FirstName}', '{item.Phone}');
                   """;
        return ExecuteNonQuery(sql);
    }

    public bool Update(PhoneBookItem item)
    {
        var sql = $"""
                   UPDATE table_phonebooks
                   SET last_name = '{item.LastName}',
                       first_name = '{item.FirstName}',
                       phone = '{item.Phone}'
                   WHERE id = {item.Id}
                   """;
        return ExecuteNonQuery(sql);
    }

    public bool Delete(int id)
    {
        var sql = $"DELETE FROM table_phonebooks WHERE id = {id};";
        return ExecuteNonQuery(sql);
    }

    private IEnumerable<PhoneBookItem> ExecuteQuery(string sql)
    {
        _sqlite.Open();
        var result = _sqlite.Query<PhoneBookItem>(sql);
        _sqlite.Close();
        
        return result;
    }

    public IEnumerable<PhoneBookItem> GetAll()
    {
        const string SQL = "SELECT * FROM table_phonebooks";
        return ExecuteQuery(SQL);
    }
    
    public IEnumerable<PhoneBookItem> GetByName(string name)
    {
        var sql = $"""
                   SELECT * 
                   FROM table_phonebooks
                   WHERE first_name = '{name}' 
                      OR last_name = '{name}';
                   """;
        return ExecuteQuery(sql);
    }
    
    public IEnumerable<PhoneBookItem> GetByPhone(string phone)
    {
        var sql = $"""
                   SELECT *
                   FROM table_phonebooks
                   WHERE phone = '{phone}';
                   """;
        return ExecuteQuery(sql);
    }

    private PhoneBookItem? ExecuteQuerySingle(string sql)
    {
        _sqlite.Open();
        var result = _sqlite.QuerySingleOrDefault<PhoneBookItem>(sql);
        _sqlite.Close();

        return result;
    }

    public PhoneBookItem? GetById(int id)
    {
        var sql = $"SELECT * FROM table_phonebooks WHERE id = {id}";
        return ExecuteQuerySingle(sql);
    }
}