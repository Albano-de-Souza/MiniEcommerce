using System.Globalization;

namespace MiniEcommerce.Dominio;

/// <summary>Formatação padrão (pt-BR) usada em toda a aplicação.</summary>
public static class Formato
{
    public static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");

    public static string Moeda(decimal valor) => valor.ToString("C", Cultura);

    public static string DataHora(DateTime data) => data.ToString("dd/MM/yyyy HH:mm", Cultura);
}
