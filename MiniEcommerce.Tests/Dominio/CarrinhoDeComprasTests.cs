using MiniEcommerce.Dominio;

namespace MiniEcommerce.Tests;

public class CarrinhoDeComprasTests
{
    [Fact]
    public void CarrinhoNovo_DeveEstarVazioComSubtotalZero()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.True(carrinho.EstaVazio);
        Assert.Equal(0m, carrinho.Subtotal);
    }

    [Fact]
    public void Adicionar_ProdutoComEstoqueSuficiente_DeveIncluirItem()
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var carrinho = new CarrinhoDeCompras();

        carrinho.Adicionar(mouse, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Same(mouse, item.Produto);
        Assert.Equal(3, item.Quantidade);
    }

    [Fact]
    public void Adicionar_QuantidadeIgualAoEstoque_DevePermitir()
    {
        var mouse = new Produto("Mouse", 50m, 4);
        var carrinho = new CarrinhoDeCompras();

        carrinho.Adicionar(mouse, 4);

        Assert.Equal(4, carrinho.Itens[0].Quantidade);
    }

    [Fact]
    public void Adicionar_MesmoProdutoDuasVezes_DeveSomarAsQuantidades()
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var carrinho = new CarrinhoDeCompras();

        carrinho.Adicionar(mouse, 2);
        carrinho.Adicionar(mouse, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Equal(5, item.Quantidade);
    }

    [Fact]
    public void Adicionar_QuantidadeMaiorQueOEstoque_DeveLancarExcecao()
    {
        var mouse = new Produto("Mouse", 50m, 2);
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<InvalidOperationException>(() => carrinho.Adicionar(mouse, 3));
        Assert.True(carrinho.EstaVazio);
    }

    [Fact]
    public void Adicionar_SomaComOQueJaEstaNoCarrinhoPassaDoEstoque_DeveLancarExcecao()
    {
        var mouse = new Produto("Mouse", 50m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(mouse, 4);

        Assert.Throws<InvalidOperationException>(() => carrinho.Adicionar(mouse, 2));
        Assert.Equal(4, carrinho.Itens[0].Quantidade);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Adicionar_QuantidadeZeroOuNegativa_DeveLancarExcecao(int quantidade)
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<ArgumentException>(() => carrinho.Adicionar(mouse, quantidade));
        Assert.True(carrinho.EstaVazio);
    }

    [Fact]
    public void Subtotal_DeveSomarPrecoVezesQuantidadeDeCadaItem()
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var teclado = new Produto("Teclado", 120.50m, 10);
        var carrinho = new CarrinhoDeCompras();

        carrinho.Adicionar(mouse, 2);   // 100,00
        carrinho.Adicionar(teclado, 1); // 120,50

        Assert.Equal(220.50m, carrinho.Subtotal);
    }

    [Fact]
    public void Remover_ItemQueEstaNoCarrinho_DeveTirarDoCarrinho()
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var teclado = new Produto("Teclado", 120m, 10);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(mouse, 1);
        carrinho.Adicionar(teclado, 1);

        bool removido = carrinho.Remover(mouse);

        Assert.True(removido);
        var item = Assert.Single(carrinho.Itens);
        Assert.Same(teclado, item.Produto);
        Assert.Equal(120m, carrinho.Subtotal);
    }

    [Fact]
    public void Remover_ItemQueNaoEstaNoCarrinho_DeveRetornarFalso()
    {
        var mouse = new Produto("Mouse", 50m, 10);
        var carrinho = new CarrinhoDeCompras();

        Assert.False(carrinho.Remover(mouse));
    }
}
