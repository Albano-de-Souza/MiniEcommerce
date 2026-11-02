using MiniEcommerce.Dominio;

namespace MiniEcommerce.Tests;

public class PedidoTests
{
    [Fact]
    public void Fechar_CarrinhoValido_DeveBaixarOEstoqueDosProdutos()
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var teclado = new Produto("Teclado", 120m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(mouse, 3);
        carrinho.Adicionar(teclado, 5);

        Pedido.Fechar(carrinho);

        Assert.Equal(7, mouse.Estoque);
        Assert.Equal(0, teclado.Estoque);
    }

    [Fact]
    public void Fechar_DeveCalcularOTotalComBaseNosItens()
    {
        var mouse = new Produto("Mouse", 49.90m, 10);
        var teclado = new Produto("Teclado", 120m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(mouse, 2);   //  99,80
        carrinho.Adicionar(teclado, 1); // 120,00

        var pedido = Pedido.Fechar(carrinho);

        Assert.Equal(219.80m, pedido.Total);
        Assert.Equal(2, pedido.Itens.Count);
    }

    [Fact]
    public void Fechar_DeveEsvaziarOCarrinho()
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(mouse, 1);

        Pedido.Fechar(carrinho);

        Assert.True(carrinho.EstaVazio);
    }

    [Fact]
    public void Fechar_CarrinhoVazio_DeveLancarExcecao()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<InvalidOperationException>(() => Pedido.Fechar(carrinho));
    }

    [Fact]
    public void Fechar_EstoqueAcabouAntesDoCheckout_NaoDeveBaixarNenhumProduto()
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var teclado = new Produto("Teclado", 120m, 5);

        var meuCarrinho = new CarrinhoDeCompras();
        meuCarrinho.Adicionar(mouse, 2);
        meuCarrinho.Adicionar(teclado, 4);

        // Outro cliente compra antes e sobra só 1 teclado
        var outroCarrinho = new CarrinhoDeCompras();
        outroCarrinho.Adicionar(teclado, 4);
        Pedido.Fechar(outroCarrinho);

        Assert.Throws<InvalidOperationException>(() => Pedido.Fechar(meuCarrinho));
        Assert.Equal(10, mouse.Estoque);  // nada foi baixado
        Assert.Equal(1, teclado.Estoque);
        Assert.False(meuCarrinho.EstaVazio);
    }

    [Fact]
    public void Fechar_DeveGuardarOsDadosDeCadaItemNoPedido()
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(mouse, 2);

        var pedido = Pedido.Fechar(carrinho);
        var item = Assert.Single(pedido.Itens);

        Assert.Equal("Mouse", item.NomeProduto);
        Assert.Equal(50m, item.PrecoUnitario);
        Assert.Equal(2, item.Quantidade);
        Assert.Equal(100m, item.Subtotal);
    }

    [Fact]
    public void GerarResumo_DeveConterOsProdutosEOTotal()
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(mouse, 2);

        var resumo = Pedido.Fechar(carrinho).GerarResumo();

        Assert.Contains("Mouse", resumo);
        Assert.Contains("TOTAL", resumo);
    }

    [Fact]
    public void BaixarEstoque_QuantidadeMaiorQueOEstoque_DeveLancarExcecao()
    {
        var mouse = new Produto("Mouse", 50m, 2);

        Assert.Throws<InvalidOperationException>(() => mouse.BaixarEstoque(3));
        Assert.Equal(2, mouse.Estoque);
    }

    [Fact]
    public void BaixarEstoque_QuantidadeZero_DeveLancarExcecao()
    {
        var mouse = new Produto("Mouse", 50m, 2);

        Assert.Throws<ArgumentException>(() => mouse.BaixarEstoque(0));
    }
}
