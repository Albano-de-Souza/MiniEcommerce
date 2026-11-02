# 🛒 Mini E-commerce (.NET + xUnit)

Aplicação de console em C# que simula uma pequena loja virtual: cadastro de produtos, catálogo, carrinho de compras e fechamento de pedido com baixa automática de estoque.

Projeto desenvolvido para a disciplina **Gestão e Qualidade de Software** (Garantia da Qualidade de Software), com foco em **testes unitários (TDD básico)** e no padrão **Conventional Commits**.

**Aluno:** Albano de Souza — RA 32516336

---

## ✅ Funcionalidades

| # | Requisito | Onde está |
|---|-----------|-----------|
| 1 | **Cadastro de produtos** (nome, preço e estoque), sem preço negativo nem estoque menor que zero | `Produto`, `Catalogo` |
| 2 | **Carrinho de compras**: adicionar com quantidade, validar estoque disponível e remover itens | `CarrinhoDeCompras`, `ItemCarrinho` |
| 3 | **Checkout**: processa a compra, baixa o estoque e calcula o total | `Pedido`, `ItemPedido` |
| 4 | **Catálogo**: lista os produtos com preço e estoque atuais | `Catalogo.Listar()` + menu |

---

## 🏗️ Arquitetura

```
MiniEcommerce/
├── MiniEcommerce.sln
├── MiniEcommerce.Console/            → aplicação (regras de negócio + menu)
│   ├── Program.cs                    → menu interativo
│   ├── Interface/
│   │   └── Entrada.cs                → leitura e validação do que o usuário digita
│   └── Dominio/
│       ├── Produto.cs                → entidade com validações e baixa de estoque
│       ├── Catalogo.cs               → cadastro, listagem e busca de produtos
│       ├── ItemCarrinho.cs           → produto + quantidade no carrinho
│       ├── CarrinhoDeCompras.cs      → adicionar, remover, subtotal, limite de estoque
│       ├── ItemPedido.cs             → "foto" do item no momento da compra
│       ├── Pedido.cs                 → checkout e resumo financeiro
│       └── Formato.cs                → formatação de moeda e data (pt-BR)
└── MiniEcommerce.Tests/              → testes unitários com xUnit
    └── Dominio/
        ├── ProdutoTests.cs
        ├── CatalogoTests.cs
        ├── CarrinhoDeComprasTests.cs
        └── PedidoTests.cs
```

**Decisões principais**

- **Regras de negócio separadas da interface:** as classes de `Dominio` não usam o console, por isso podem ser testadas isoladamente pelo projeto xUnit.
- **Validação na criação:** um `Produto` inválido nunca chega a existir — o construtor lança `ArgumentException`.
- **Erros de regra de negócio** (estoque insuficiente, nome repetido, carrinho vazio) lançam `InvalidOperationException`; o menu captura e mostra uma mensagem amigável.
- **Checkout "tudo ou nada":** antes de baixar o estoque, o pedido confere **todos** os itens. Se um deles não tiver estoque, nada é alterado.
- **Pedido guarda uma cópia dos itens** (`ItemPedido`), então o resumo não muda se o produto for alterado depois.

---

## ▶️ Como executar

**Pré-requisito:** [.NET SDK](https://dotnet.microsoft.com/download) 8 ou superior.

```bash
# 1. Clonar o repositório
git clone https://github.com/Albano-de-Souza/MiniEcommerce.git
cd MiniEcommerce

# 2. Rodar os testes unitários
dotnet test

# 3. Executar a aplicação
dotnet run --project MiniEcommerce.Console/MiniEcommerce.Console.csproj
```

O programa já inicia com 3 produtos de exemplo. Menu:

```
========= MINI E-COMMERCE =========
1 - Cadastrar produto
2 - Listar catálogo
3 - Adicionar produto ao carrinho
4 - Remover produto do carrinho
5 - Ver carrinho
6 - Finalizar pedido
0 - Sair
===================================
```

---

## 🧪 Testes

Os testes foram escritos **antes** de cada implementação (TDD básico) e cobrem:

- **Produto/Catálogo:** dados válidos, preço negativo, estoque negativo, nome vazio, nome repetido, códigos sequenciais e busca.
- **Carrinho:** adição com sucesso, soma de quantidades do mesmo produto, quantidade acima do estoque, quantidade zero/negativa, subtotal e remoção.
- **Pedido:** baixa de estoque, cálculo do total, carrinho esvaziado após o checkout, carrinho vazio, checkout "tudo ou nada" e resumo do pedido.

---

## 📝 Padrão de commits

O histórico segue o [Conventional Commits](https://www.conventionalcommits.org/pt-br/):

| Tipo | Uso |
|------|-----|
| `chore` | estrutura e configuração do projeto |
| `test` | criação de testes |
| `feat` | nova funcionalidade |
| `refactor` | melhoria de código sem mudar o comportamento |
| `docs` | documentação |

Para conferir: `git log --oneline`
