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
        if (string.IsNullOrWhiteSpace(nome))
        {
            MessageBox.Show("Nome é obrigatório");
            return null;
        }

        
        string numeroStr = numero.ToString();
        if (numeroStr.Length < 10 || numeroStr.Length > 12)
        {
            MessageBox.Show("Telefone deve ter entre 10 e 12 dígitos");
            return null;
        }

        if (numero <= 0)
        {
            MessageBox.Show("Telefone deve ser maior que zero");
            return null;
        }

        try
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = "INSERT INTO cadastros (Nome, Numero) VALUES (@nome, @numero) RETURNING Id";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nome", nome);
            command.Parameters.AddWithValue("@numero", numero); // Agora é long

            var novoId = (int)command.ExecuteScalar();

            return new Cadastro(nome, numero) { Id = novoId };
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            MessageBox.Show("Este telefone já existe! Escolha outro telefone.");
            return null;
        }
        catch (PostgresException ex) when (ex.SqlState == "23514")
        {
            MessageBox.Show("Número inválido. Deve ser maior que zero.");
            return null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar: {ex.Message}");
            return null;
        }
    }

    
    public List<Cadastro> GetAll()
    {
        var cadastros = new List<Cadastro>();

        try
        {
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
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao buscar dados: {ex.Message}");
        }

        return cadastros;
    }

    
    public Cadastro GetByNomeNumero(string nome, long numero)
    {
        try
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = "SELECT Id, Nome, Numero FROM cadastros WHERE Nome = @nome AND Numero = @numero";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nome", nome);
            command.Parameters.AddWithValue("@numero", numero); // Agora é long

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Cadastro(
                    reader.GetString(1),
                    reader.GetInt64(2) 
                )
                { Id = reader.GetInt32(0) };
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao buscar cadastro: {ex.Message}");
        }

        return null;
    }

    
    public bool Put(int id, string novoNome, long novoNumero)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
        {
            MessageBox.Show("Nome é obrigatório");
            return false;
        }

        
        string novoNumeroStr = novoNumero.ToString();
        if (novoNumeroStr.Length < 10 || novoNumeroStr.Length > 12)
        {
            MessageBox.Show("Telefone deve ter entre 10 e 12 dígitos");
            return false;
        }

        if (novoNumero <= 0)
        {
            MessageBox.Show("Telefone deve ser maior que zero");
            return false;
        }

        try
        {
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
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            MessageBox.Show("Este telefone já existe! Escolha outro telefone.");
            return false;
        }
        catch (PostgresException ex) when (ex.SqlState == "23514")
        {
            MessageBox.Show("Número inválido. Deve ser maior que zero.");
            return false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao atualizar: {ex.Message}");
            return false;
        }
    }

    
    public bool Delete(int id)
    {
        try
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

            MessageBox.Show("Cadastro não encontrado");
            return false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao deletar: {ex.Message}");
            return false;
        }
    }
}