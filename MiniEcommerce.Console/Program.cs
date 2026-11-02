using System.Text;
using MiniEcommerce.Dominio;
using static MiniEcommerce.Interface.Entrada;

Console.OutputEncoding = Encoding.UTF8;

var catalogo = new Catalogo();
var carrinho = new CarrinhoDeCompras();

// Alguns produtos de exemplo para facilitar a demonstração
catalogo.Cadastrar("Mouse sem fio", 59.90m, 15);
catalogo.Cadastrar("Teclado mecânico", 249.90m, 8);
catalogo.Cadastrar("Monitor 24 polegadas", 899.00m, 4);

bool executando = true;
while (executando)
{
    MostrarMenu();
    string opcao = LerTexto("Escolha uma opção: ");
    Console.WriteLine();

    try
    {
        switch (opcao)
        {
            case "1": CadastrarProduto(); break;
            case "2": ListarCatalogo(); break;
            case "3": AdicionarAoCarrinho(); break;
            case "4": RemoverDoCarrinho(); break;
            case "5": VerCarrinho(); break;
            case "6": FinalizarPedido(); break;
            case "0":
                executando = false;
                Console.WriteLine("Obrigado por usar o Mini E-commerce. Até logo!");
                break;
            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }

    Console.WriteLine();
}

void MostrarMenu()
{
    Console.WriteLine("========= MINI E-COMMERCE =========");
    Console.WriteLine("1 - Cadastrar produto");
    Console.WriteLine("2 - Listar catálogo");
    Console.WriteLine("3 - Adicionar produto ao carrinho");
    Console.WriteLine("4 - Remover produto do carrinho");
    Console.WriteLine("5 - Ver carrinho");
    Console.WriteLine("6 - Finalizar pedido");
    Console.WriteLine("0 - Sair");
    Console.WriteLine("===================================");
}

void CadastrarProduto()
{
    string nome = LerTexto("Nome do produto: ");
    decimal preco = LerDecimal("Preço (ex.: 19,90): ");
    int estoque = LerInteiro("Quantidade em estoque: ");

    var produto = catalogo.Cadastrar(nome, preco, estoque);
    Console.WriteLine($"Produto \"{produto.Nome}\" cadastrado com o código {produto.Id}.");
}

void ListarCatalogo()
{
    var produtos = catalogo.Listar();
    if (produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return;
    }

    Console.WriteLine($"{"Cód",-5}{"Produto",-25}{"Preço",14}{"Estoque",10}");
    foreach (var p in produtos)
        Console.WriteLine($"{p.Id,-5}{p.Nome,-25}{Formato.Moeda(p.Preco),14}{p.Estoque,10}");
}

void AdicionarAoCarrinho()
{
    ListarCatalogo();
    Console.WriteLine();

    int id = LerInteiro("Código do produto: ");
    var produto = catalogo.BuscarPorId(id)
        ?? throw new InvalidOperationException($"Não existe produto com o código {id}.");

    int quantidade = LerInteiro("Quantidade: ");
    carrinho.Adicionar(produto, quantidade);
    Console.WriteLine($"{quantidade}x \"{produto.Nome}\" adicionado(s) ao carrinho.");
}

void RemoverDoCarrinho()
{
    if (carrinho.EstaVazio)
    {
        Console.WriteLine("O carrinho está vazio.");
        return;
    }

    VerCarrinho();
    Console.WriteLine();

    int id = LerInteiro("Código do produto a remover: ");
    var produto = catalogo.BuscarPorId(id);

    if (produto is not null && carrinho.Remover(produto))
        Console.WriteLine($"\"{produto.Nome}\" removido do carrinho.");
    else
        Console.WriteLine("Esse produto não está no carrinho.");
}

void VerCarrinho()
{
    if (carrinho.EstaVazio)
    {
        Console.WriteLine("O carrinho está vazio.");
        return;
    }

    Console.WriteLine($"{"Cód",-5}{"Produto",-25}{"Qtd",5}{"Preço",14}{"Subtotal",14}");
    foreach (var item in carrinho.Itens)
    {
        Console.WriteLine(
            $"{item.Produto.Id,-5}{item.Produto.Nome,-25}{item.Quantidade,5}" +
            $"{Formato.Moeda(item.Produto.Preco),14}{Formato.Moeda(item.Subtotal),14}");
    }
    Console.WriteLine($"Subtotal do carrinho: {Formato.Moeda(carrinho.Subtotal)}");
}

void FinalizarPedido()
{
    var pedido = Pedido.Fechar(carrinho);
    Console.WriteLine("Pedido finalizado com sucesso!");
    Console.WriteLine();
    Console.Write(pedido.GerarResumo());
}
