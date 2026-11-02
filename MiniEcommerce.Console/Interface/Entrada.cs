using System.Globalization;
using MiniEcommerce.Dominio;

namespace MiniEcommerce.Interface;

/// <summary>Leitura e validação do que o usuário digita no console.</summary>
public static class Entrada
{
    public static string LerTexto(string mensagem)
    {
        Console.Write(mensagem);
        string? texto = Console.ReadLine();

        if (texto is null) // entrada encerrada (Ctrl+Z / Ctrl+D)
            Environment.Exit(0);

        return texto?.Trim() ?? string.Empty;
    }

    public static int LerInteiro(string mensagem)
    {
        while (true)
        {
            if (int.TryParse(LerTexto(mensagem), out int valor))
                return valor;

            Console.WriteLine("Digite um número inteiro válido.");
        }
    }

    public static decimal LerDecimal(string mensagem)
    {
        while (true)
        {
            string texto = LerTexto(mensagem);

            // Aceita "19,90" e também "19.90"
            if (!texto.Contains(','))
                texto = texto.Replace('.', ',');

            if (decimal.TryParse(texto, NumberStyles.Number, Formato.Cultura, out decimal valor))
                return valor;

            Console.WriteLine("Digite um valor válido (ex.: 19,90).");
        }
    }
}
