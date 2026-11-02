using System.Globalization;
using System.Text;

namespace MiniEcommerce.Dominio;

public record ItemPedido(string NomeProduto, decimal PrecoUnitario, int Quantidade)
{
    public decimal Subtotal => PrecoUnitario * Quantidade;
}

public class Pedido
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");
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

    public static Pedido Fechar(CarrinhoDeCompras carrinho)
    {
        ArgumentNullException.ThrowIfNull(carrinho);

        if (carrinho.EstaVazio)
            throw new InvalidOperationException("Não é possível fechar um pedido com o carrinho vazio.");

        // 1) Confere todo o estoque ANTES de baixar qualquer coisa
        foreach (var item in carrinho.Itens)
        {
            if (item.Quantidade > item.Produto.Estoque)
                throw new InvalidOperationException(
                    $"Estoque insuficiente para \"{item.Produto.Nome}\". " +
                    $"Disponível: {item.Produto.Estoque}, no carrinho: {item.Quantidade}.");
        }

        // 2) Guarda uma "foto" dos itens (nome, preço e quantidade no momento da compra)
        var itensDoPedido = carrinho.Itens
            .Select(i => new ItemPedido(i.Produto.Nome, i.Produto.Preco, i.Quantidade))
            .ToList();

        // 3) Baixa o estoque e esvazia o carrinho
        foreach (var item in carrinho.Itens)
            item.Produto.BaixarEstoque(item.Quantidade);

        carrinho.Limpar();

        return new Pedido(itensDoPedido);
    }

    public string GerarResumo()
    {
        var resumo = new StringBuilder();
        resumo.AppendLine($"===== PEDIDO Nº {Numero} — {Data.ToString("dd/MM/yyyy HH:mm", Cultura)} =====");

        foreach (var item in Itens)
        {
            resumo.AppendLine(
                $"{item.Quantidade}x {item.NomeProduto} " +
                $"({item.PrecoUnitario.ToString("C", Cultura)} cada) = {item.Subtotal.ToString("C", Cultura)}");
        }

        resumo.AppendLine($"TOTAL: {Total.ToString("C", Cultura)}");
        return resumo.ToString();
    }
}
