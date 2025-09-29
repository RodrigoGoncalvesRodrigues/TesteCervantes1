-- Tabela principal de cadastro
CREATE TABLE cadastros (
    Id SERIAL PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Numero NUMERIC(15) NOT NULL UNIQUE,
);

-- Tabela de log
CREATE TABLE log_operacoes (
    Id SERIAL PRIMARY KEY,
    DataHora TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    TipoOperacao VARCHAR(10) NOT NULL,
    IdCadastro INTEGER,
    NomeAnterior VARCHAR(100),
    NumeroAnterior NUMERIC(15),
    NomeNovo VARCHAR(100),
    NumeroNovo NUMERIC(15)
);
-- Função para registrar operações na tabela de log
CREATE OR REPLACE FUNCTION log_operacoes_function()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO log_operacoes (TipoOperacao, IdCadastro, NomeNovo, NumeroNovo)
        VALUES ('INSERT', NEW.Id, NEW.Nome, NEW.Numero);
        RETURN NEW;
    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO log_operacoes (TipoOperacao, IdCadastro, NomeAnterior, NumeroAnterior, NomeNovo, NumeroNovo)
        VALUES ('UPDATE', NEW.Id, OLD.Nome, OLD.Numero, NEW.Nome, NEW.Numero);
        RETURN NEW;
    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO log_operacoes (TipoOperacao, IdCadastro, NomeAnterior, NumeroAnterior)
        VALUES ('DELETE', OLD.Id, OLD.Nome, OLD.Numero);
        RETURN OLD;
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_operacoes
    AFTER INSERT OR UPDATE OR DELETE ON cadastros
    FOR EACH ROW
    EXECUTE FUNCTION log_operacoes_function();