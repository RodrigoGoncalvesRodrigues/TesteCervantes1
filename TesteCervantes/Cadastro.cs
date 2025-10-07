using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Npgsql;

public class Cadastro
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=meu_banco;Username=postgres;Password=4798";

    public int Id { get; set; }
    public string Nome { get; set; }
    public long Numero { get; set; }

    
    public Cadastro() { }

    
    public Cadastro(string nome, long numero)
    {
        Nome = nome;
        Numero = numero;
    }

    
    public Cadastro Post(string nome, long numero)
    {
        

        
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = "INSERT INTO cadastros (Nome, Numero) VALUES (@nome, @numero) RETURNING Id";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nome", nome);
            command.Parameters.AddWithValue("@numero", numero); 

            var novoId = (int)command.ExecuteScalar();

            return new Cadastro(nome, numero) { Id = novoId };
       
        
    }

    
    public List<Cadastro> GetAll()
    {
        var cadastros = new List<Cadastro>();

        
        
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = "SELECT Id, Nome, Numero FROM cadastros ORDER BY Id";
            using var command = new NpgsqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                cadastros.Add(new Cadastro(
                    reader.GetString(1),
                    reader.GetInt64(2) 
                )
                { Id = reader.GetInt32(0) });
            }
        
        

        return cadastros;
    }

    
    public Cadastro GetByNomeNumero(string nome, long numero)
    {
        
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = "SELECT Id, Nome, Numero FROM cadastros WHERE Nome = @nome AND Numero = @numero";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nome", nome);
            command.Parameters.AddWithValue("@numero", numero); 

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Cadastro(
                    reader.GetString(1),
                    reader.GetInt64(2) 
                )
                { Id = reader.GetInt32(0) };
            }

        return null;
    }

    
    public bool Put(int id, string novoNome, long novoNumero)
    {
        

        
        string novoNumeroStr = novoNumero.ToString();
       

       

        
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = "UPDATE cadastros SET Nome = @nome, Numero = @numero WHERE Id = @id";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nome", novoNome);
            command.Parameters.AddWithValue("@numero", novoNumero); // Agora é long
            command.Parameters.AddWithValue("@id", id);

            var linhasAfetadas = command.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                MessageBox.Show("Cadastro atualizado com sucesso!");
                return true;
            }

            return false;
        
        
    }

    
    public bool Delete(int id)
    {
        
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = "DELETE FROM cadastros WHERE Id = @id";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            var linhasAfetadas = command.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                MessageBox.Show("Cadastro removido com sucesso!");
                return true;
            }

            
            return false;
        
        
    }
}