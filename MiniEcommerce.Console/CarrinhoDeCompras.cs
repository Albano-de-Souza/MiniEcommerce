namespace MiniEcommerce.Dominio;

public class CarrinhoDeCompras
{
    private readonly List<ItemCarrinho> _itens = new();

    public IReadOnlyList<ItemCarrinho> Itens => _itens.AsReadOnly();
    public decimal Subtotal => _itens.Sum(i => i.Subtotal);
    public bool EstaVazio => _itens.Count == 0;

    public void Adicionar(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        var item = _itens.FirstOrDefault(i => i.Produto == produto);
        int quantidadeNoCarrinho = item?.Quantidade ?? 0;

        if (quantidadeNoCarrinho + quantidade > produto.Estoque)
            throw new InvalidOperationException(
                $"Estoque insuficiente para \"{produto.Nome}\". " +
                $"Disponível: {produto.Estoque}, já no carrinho: {quantidadeNoCarrinho}.");

        if (item is null)
            _itens.Add(new ItemCarrinho(produto, quantidade));
        else
            item.Quantidade += quantidade;
    }

    public bool Remover(Produto produto)
    {
        var item = _itens.FirstOrDefault(i => i.Produto == produto);
        if (item is null)
            return false;

        _itens.Remove(item);
        return true;
    }

    public void Limpar() => _itens.Clear();
}
