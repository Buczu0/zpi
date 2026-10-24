namespace WebStore.Services.Validation;

internal static class Guard
{
    public const decimal MaxMoney = 9999999999999999.99m;

    public static decimal SumMoney(IEnumerable<decimal> amounts)
    {
        try
        {
            var total = amounts.Sum();
            Money(total, "TotalAmount");
            return total;
        }
        catch (OverflowException ex)
        {
            throw new ArgumentException("Suma kwot przekracza dozwolony zakres.", "TotalAmount", ex);
        }
    }

    public static void Id(int? value, string name = "Id")
    {
        if (value < 0) throw new ArgumentException("Id nie może być ujemne.", name);
    }

    public static void Text(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Pole nie może być puste.", name);
    }

    public static void Money(decimal value, string name)
    {
        if (value < 0 || value > MaxMoney || decimal.Round(value, 2) != value)
            throw new ArgumentException("Kwota musi być nieujemna, mieścić się w decimal(18,2) i mieć najwyżej dwa miejsca dziesiętne.", name);
    }

    public static void Utc(DateTime value, string name)
    {
        if (value.Kind != DateTimeKind.Utc || value == DateTime.MinValue)
            throw new ArgumentException("Wymagana jest poprawna data UTC.", name);
    }
}
