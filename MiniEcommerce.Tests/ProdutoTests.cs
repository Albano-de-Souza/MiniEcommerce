using MiniEcommerce.Dominio;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
    [Fact]
    public void CriarProduto_ComDadosValidos_DeveGuardarOsValores()
    {
        var produto = new Produto("Mouse sem fio", 59.90m, 15);

        Assert.Equal("Mouse sem fio", produto.Nome);
        Assert.Equal(59.90m, produto.Preco);
        Assert.Equal(15, produto.Estoque);
    }

    [Fact]
    public void CriarProduto_ComPrecoEEstoqueZero_DevePermitir()
    {
        var produto = new Produto("Brinde", 0m, 0);

        Assert.Equal(0m, produto.Preco);
        Assert.Equal(0, produto.Estoque);
    }

    [Fact]
    public void CriarProduto_ComPrecoNegativo_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Produto("Mouse", -0.01m, 10));
    }

    [Fact]
    public void CriarProduto_ComEstoqueNegativo_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Produto("Mouse", 50m, -1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CriarProduto_ComNomeVazio_DeveLancarExcecao(string nome)
    {
        Assert.Throws<ArgumentException>(() => new Produto(nome, 50m, 10));
    }
}
