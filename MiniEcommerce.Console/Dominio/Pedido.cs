using System.Text;

namespace MiniEcommerce.Dominio;

/// <summary>Pedido fechado a partir de um carrinho (checkout).</summary>
public class Pedido
{
    private static int _ultimoNumero;

    public int Numero { get; }
    public DateTime Data { get; }
    public IReadOnlyList<ItemPedido> Itens { get; }
    public decimal Total { get; }

    private Pedido(List<ItemPedido> itens)
    {
        Numero = Interlocked.Increment(ref _ultimoNumero);
        Data = DateTime.Now;
        Itens = itens.AsReadOnly();
        Total = itens.Sum(i => i.Subtotal);
    }

    /// <summary>
    /// Fecha o pedido: confere o estoque de todos os itens, baixa o estoque
    /// e esvazia o carrinho. Se algum item não tiver estoque, nada é alterado.
    /// </summary>
    public static Pedido Fechar(CarrinhoDeCompras carrinho)
    {
        ArgumentNullException.ThrowIfNull(carrinho);

        if (carrinho.EstaVazio)
            throw new InvalidOperationException("Não é possível fechar um pedido com o carrinho vazio.");

        GarantirEstoqueDeTodosOsItens(carrinho);

        var itensDoPedido = carrinho.Itens
            .Select(i => new ItemPedido(i.Produto.Nome, i.Produto.Preco, i.Quantidade))
            .ToList();

        foreach (var item in carrinho.Itens)
            item.Produto.BaixarEstoque(item.Quantidade);

        carrinho.Limpar();

        return new Pedido(itensDoPedido);
    }

    public string GerarResumo()
    {
        var resumo = new StringBuilder();
        resumo.AppendLine($"===== PEDIDO Nº {Numero} — {Formato.DataHora(Data)} =====");

        foreach (var item in Itens)
        {
            resumo.AppendLine(
                $"{item.Quantidade}x {item.NomeProduto} " +
                $"({Formato.Moeda(item.PrecoUnitario)} cada) = {Formato.Moeda(item.Subtotal)}");
        }

        resumo.AppendLine($"TOTAL: {Formato.Moeda(Total)}");
        return resumo.ToString();
    }

    private static void GarantirEstoqueDeTodosOsItens(CarrinhoDeCompras carrinho)
    {
        foreach (var item in carrinho.Itens)
        {
            if (item.Quantidade > item.Produto.Estoque)
                throw new InvalidOperationException(
                    $"Estoque insuficiente para \"{item.Produto.Nome}\". " +
                    $"Disponível: {item.Produto.Estoque}, no carrinho: {item.Quantidade}.");
        }
    }
}
