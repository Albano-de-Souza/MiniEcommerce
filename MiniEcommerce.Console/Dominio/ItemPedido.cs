namespace MiniEcommerce.Dominio;

/// <summary>
/// "Foto" de um item no momento da compra: guarda nome, preço e quantidade
/// para que o pedido não mude se o produto for alterado depois.
/// </summary>
public record ItemPedido(string NomeProduto, decimal PrecoUnitario, int Quantidade)
{
    public decimal Subtotal => PrecoUnitario * Quantidade;
}
