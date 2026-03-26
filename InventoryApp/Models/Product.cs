namespace InventoryManagement.Models;

/// <summary>
/// Продукт в системата за управление на склада.
/// </summary>
public class Product
{
    /// <summary>
    /// Уникален идентификатор на продукта.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Име на продукта.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Количество на продукта на склад.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Цена на единица продукт.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Доставчик на продукта.
    /// </summary>
    public string Supplier { get; set; } = string.Empty;

    /// <summary>
    /// Обща стойност на текущата наличност.
    /// </summary>
    /// <returns>Количество × Цена</returns>
    public decimal TotalValue => Quantity * Price;
}
