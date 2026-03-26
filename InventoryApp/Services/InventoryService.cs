using InventoryManagement.Models;

namespace InventoryManagement.Services;

/// <summary>
/// Имплементация на услугата за управление на склада.
/// </summary>
public class InventoryService : IInventoryService
{
    private readonly List<Product> products = new();

    /// <summary>
    /// Добавя нов продукт в склада.
    /// </summary>
    /// <param name="product">Продукт за добавяне</param>
    /// <returns>True при успех</returns>
    public bool AddProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        ValidateProduct(product.Name, product.Quantity, product.Price, product.Supplier);

        if (products.Any(p => p.Id == product.Id))
        {
            return false;
        }

        products.Add(product);
        return true;
    }

    /// <summary>
    /// Обновява съществуващ продукт.
    /// </summary>
    /// <param name="id">ID на продукта</param>
    /// <param name="name">Ново име</param>
    /// <param name="quantity">Ново количество</param>
    /// <param name="price">Нова цена</param>
    /// <param name="supplier">Нов доставчик</param>
    /// <returns>True при успех</returns>
    public bool UpdateProduct(int id, string name, int quantity, decimal price, string supplier)
    {
        ValidateProduct(name, quantity, price, supplier);

        var product = GetProductById(id);
        if (product == null)
        {
            return false;
        }

        product.Name = name;
        product.Quantity = quantity;
        product.Price = price;
        product.Supplier = supplier;
        
        return true;
    }

    /// <summary>
    /// Изтрива продукт по ID.
    /// </summary>
    /// <param name="id">ID на продукта за изтриване</param>
    /// <returns>True при успех</returns>
    public bool DeleteProduct(int id)
    {
        var product = GetProductById(id);
        if (product == null)
        {
            return false;
        }

        return products.Remove(product);
    }

    /// <summary>
    /// Връща продукт по ID.
    /// </summary>
    /// <param name="id">ID на продукта</param>
    /// <returns>Продукт или null</returns>
    public Product? GetProductById(int id)
    {
        return products.FirstOrDefault(p => p.Id == id);
    }

    /// <summary>
    /// Връща всички продукти.
    /// </summary>
    /// <returns>Списък с всички продукти</returns>
    public IEnumerable<Product> GetAll()
    {
        return products;
    }

    /// <summary>
    /// Търси продукти по име.
    /// </summary>
    /// <param name="keyword">Ключова дума</param>
    /// <returns>Намерени продукти</returns>
    public IEnumerable<Product> SearchByName(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return products;
        }

        return products.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Търси продукти по ID.
    /// </summary>
    /// <param name="id">ID за търсене</param>
    /// <returns>Намерени продукти</returns>
    public IEnumerable<Product> SearchById(int id)
    {
        return products.Where(p => p.Id == id);
    }

    /// <summary>
    /// Търси продукти по доставчик.
    /// </summary>
    /// <param name="supplierName">Име на доставчик</param>
    /// <returns>Намерени продукти</returns>
    public IEnumerable<Product> SearchBySupplier(string supplierName)
    {
        if (string.IsNullOrWhiteSpace(supplierName))
        {
            return Enumerable.Empty<Product>();
        }

        return products.Where(p => p.Supplier.Contains(supplierName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Филтрира продукти по минимална наличност.
    /// </summary>
    /// <param name="minQuantity">Минимално количество</param>
    /// <returns>Филтрирани продукти</returns>
    public IEnumerable<Product> FilterByMinimumStock(int minQuantity)
    {
        if (minQuantity < 0)
        {
            throw new ArgumentException("Минималното количество не може да бъде отрицателно.", nameof(minQuantity));
        }

        return products.Where(p => p.Quantity >= minQuantity);
    }

    /// <summary>
    /// Филтрира продукти по ценови диапазон.
    /// </summary>
    /// <param name="minPrice">Минимална цена</param>
    /// <param name="maxPrice">Максимална цена</param>
    /// <returns>Филтрирани продукти</returns>
    public IEnumerable<Product> FilterByPriceRange(decimal minPrice, decimal maxPrice)
    {
        if (minPrice < 0)
        {
            throw new ArgumentException("Минималната цена не може да бъде отрицателна.", nameof(minPrice));
        }

        if (maxPrice < 0)
        {
            throw new ArgumentException("Максималната цена не може да бъде отрицателна.", nameof(maxPrice));
        }

        if (minPrice > maxPrice)
        {
            throw new ArgumentException("Минималната цена не може да бъде по-голяма от максималната цена.");
        }

        return products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
    }

    /// <summary>
    /// Сортира продукти.
    /// </summary>
    /// <param name="sortBy">Критерий за сортиране</param>
    /// <param name="ascending">Посока на сортиране</param>
    /// <returns>Сортирани продукти</returns>
    public IEnumerable<Product> SortProducts(string sortBy, bool ascending = true)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return products;
        }

        IEnumerable<Product> sortedProducts;
        var normalizedSortBy = sortBy.ToLowerInvariant();

        switch (normalizedSortBy)
        {
            case "име":
            case "name":
                sortedProducts = ascending ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name);
                break;
            case "цена":
            case "price":
                sortedProducts = ascending ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price);
                break;
            case "количество":
            case "quantity":
                sortedProducts = ascending ? products.OrderBy(p => p.Quantity) : products.OrderByDescending(p => p.Quantity);
                break;
            case "доставчик":
            case "supplier":
                sortedProducts = ascending ? products.OrderBy(p => p.Supplier) : products.OrderByDescending(p => p.Supplier);
                break;
            case "стойност":
            case "value":
                sortedProducts = ascending ? products.OrderBy(p => p.TotalValue) : products.OrderByDescending(p => p.TotalValue);
                break;
            default:
                sortedProducts = products;
                break;
        }

        return sortedProducts;
    }

    /// <summary>
    /// Изчислява общата стойност на склада.
    /// </summary>
    /// <returns>Обща стойност</returns>
    public decimal GetTotalInventoryValue() => products.Sum(p => p.TotalValue);

    /// <summary>
    /// Валидира данни на продукт.
    /// </summary>
    /// <param name="name">Име на продукта</param>
    /// <param name="quantity">Количество</param>
    /// <param name="price">Цена</param>
    /// <param name="supplier">Доставчик</param>
    private static void ValidateProduct(string name, int quantity, decimal price, string supplier)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Името на продукта е задължително.", nameof(name));
        }

        if (quantity < 0)
        {
            throw new ArgumentException("Количеството не може да бъде отрицателно.", nameof(quantity));
        }

        if (price < 0)
        {
            throw new ArgumentException("Цената не може да бъде отрицателна.", nameof(price));
        }

        if (string.IsNullOrWhiteSpace(supplier))
        {
            throw new ArgumentException("Доставчикът е задължителен.", nameof(supplier));
        }
    }
}
