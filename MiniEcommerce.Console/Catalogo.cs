namespace MiniEcommerce.Dominio;

public class Catalogo
{
    private readonly List<Produto> _produtos = new();
    private int _proximoId = 1;

    public Produto Cadastrar(string nome, decimal preco, int estoque)
    {
        var produto = new Produto(nome, preco, estoque);

        bool nomeJaExiste = _produtos.Any(p =>
            p.Nome.Equals(produto.Nome, StringComparison.OrdinalIgnoreCase));

        if (nomeJaExiste)
            throw new InvalidOperationException($"Já existe um produto chamado \"{produto.Nome}\".");

        produto.Id = _proximoId++;
        _produtos.Add(produto);
        return produto;
    }

    public IReadOnlyList<Produto> Listar() => _produtos.AsReadOnly();

    public Produto? BuscarPorId(int id) => _produtos.FirstOrDefault(p => p.Id == id);
}
