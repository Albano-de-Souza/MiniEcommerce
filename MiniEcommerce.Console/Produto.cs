namespace MiniEcommerce.Dominio;

public class Produto
{
    public int Id { get; internal set; }
    public string Nome { get; }
    public decimal Preco { get; }
    public int Estoque { get; private set; }

    public Produto(string nome, decimal preco, int estoque)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do produto é obrigatório.");

        if (preco < 0)
            throw new ArgumentException("O preço do produto não pode ser negativo.");

        if (estoque < 0)
            throw new ArgumentException("O estoque do produto não pode ser menor que zero.");

        Nome = nome.Trim();
        Preco = preco;
        Estoque = estoque;
    }

    public void BaixarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade a baixar deve ser maior que zero.");

        if (quantidade > Estoque)
            throw new InvalidOperationException(
                $"Estoque insuficiente para \"{Nome}\". Disponível: {Estoque}, solicitado: {quantidade}.");

        Estoque -= quantidade;
    }
}
