using MiniEcommerce.Dominio;

namespace MiniEcommerce.Tests;

public class CatalogoTests
{
    [Fact]
    public void Cadastrar_ProdutoValido_DeveAparecerNaListagem()
    {
        var catalogo = new Catalogo();

        catalogo.Cadastrar("Teclado", 120m, 8);

        var produtos = catalogo.Listar();
        Assert.Single(produtos);
        Assert.Equal("Teclado", produtos[0].Nome);
        Assert.Equal(120m, produtos[0].Preco);
        Assert.Equal(8, produtos[0].Estoque);
    }

    [Fact]
    public void Cadastrar_DeveAtribuirCodigosSequenciais()
    {
        var catalogo = new Catalogo();

        var primeiro = catalogo.Cadastrar("Teclado", 120m, 8);
        var segundo = catalogo.Cadastrar("Monitor", 899m, 3);

        Assert.Equal(1, primeiro.Id);
        Assert.Equal(2, segundo.Id);
    }

    [Fact]
    public void Cadastrar_ComPrecoNegativo_NaoDeveIncluirNoCatalogo()
    {
        var catalogo = new Catalogo();

        Assert.Throws<ArgumentException>(() => catalogo.Cadastrar("Teclado", -5m, 8));
        Assert.Empty(catalogo.Listar());
    }

    [Fact]
    public void Cadastrar_ComEstoqueNegativo_NaoDeveIncluirNoCatalogo()
    {
        var catalogo = new Catalogo();

        Assert.Throws<ArgumentException>(() => catalogo.Cadastrar("Teclado", 120m, -3));
        Assert.Empty(catalogo.Listar());
    }

    [Fact]
    public void Cadastrar_NomeRepetido_DeveLancarExcecao()
    {
        var catalogo = new Catalogo();
        catalogo.Cadastrar("Teclado", 120m, 8);

        Assert.Throws<InvalidOperationException>(() => catalogo.Cadastrar("teclado", 100m, 2));
        Assert.Single(catalogo.Listar());
    }

    [Fact]
    public void BuscarPorId_CodigoExistente_DeveRetornarOProduto()
    {
        var catalogo = new Catalogo();
        var teclado = catalogo.Cadastrar("Teclado", 120m, 8);

        Assert.Same(teclado, catalogo.BuscarPorId(teclado.Id));
    }

    [Fact]
    public void BuscarPorId_CodigoInexistente_DeveRetornarNulo()
    {
        var catalogo = new Catalogo();

        Assert.Null(catalogo.BuscarPorId(99));
    }
}
