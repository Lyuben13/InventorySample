using InventoryManagement.Models;

namespace InventoryManagement.Services;

/// <summary>
/// Интерфейс за операции по управление на склада.
/// </summary>
public interface IInventoryService
{
    /// <summary>
    /// Добавя нов продукт в склада.
    /// </summary>
    /// <param name="product">Продукт за добавяне</param>
    /// <returns>True при успех</returns>
    bool AddProduct(Product product);

    /// <summary>
    /// Обновява съществуващ продукт.
    /// </summary>
    /// <param name="id">ID на продукта</param>
    /// <param name="name">Ново име</param>
    /// <param name="quantity">Ново количество</param>
    /// <param name="price">Нова цена</param>
    /// <param name="supplier">Нов доставчик</param>
    /// <returns>True при успех</returns>
    bool UpdateProduct(int id, string name, int quantity, decimal price, string supplier);

    /// <summary>
    /// Изтрива продукт по ID.
    /// </summary>
    /// <param name="id">ID на продукта за изтриване</param>
    /// <returns>True при успех</returns>
    bool DeleteProduct(int id);

    /// <summary>
    /// Връща продукт по ID.
    /// </summary>
    /// <param name="id">ID на продукта</param>
    /// <returns>Продукт или null</returns>
    Product? GetProductById(int id);

    /// <summary>
    /// Връща всички продукти.
    /// </summary>
    /// <returns>Списък с всички продукти</returns>
    IEnumerable<Product> GetAll();

    /// <summary>
    /// Търси продукти по име.
    /// </summary>
    /// <param name="keyword">Ключова дума</param>
    /// <returns>Намерени продукти</returns>
    IEnumerable<Product> SearchByName(string keyword);

    /// <summary>
    /// Търси продукти по ID.
    /// </summary>
    /// <param name="id">ID за търсене</param>
    /// <returns>Намерени продукти</returns>
    IEnumerable<Product> SearchById(int id);

    /// <summary>
    /// Търси продукти по доставчик.
    /// </summary>
    /// <param name="supplierName">Име на доставчик</param>
    /// <returns>Намерени продукти</returns>
    IEnumerable<Product> SearchBySupplier(string supplierName);

    /// <summary>
    /// Филтрира продукти по минимална наличност.
    /// </summary>
    /// <param name="minQuantity">Минимално количество</param>
    /// <returns>Филтрирани продукти</returns>
    IEnumerable<Product> FilterByMinimumStock(int minQuantity);

    /// <summary>
    /// Филтрира продукти по ценови диапазон.
    /// </summary>
    /// <param name="minPrice">Минимална цена</param>
    /// <param name="maxPrice">Максимална цена</param>
    /// <returns>Филтрирани продукти</returns>
    IEnumerable<Product> FilterByPriceRange(decimal minPrice, decimal maxPrice);

    /// <summary>
    /// Сортира продукти.
    /// </summary>
    /// <param name="sortBy">Критерий за сортиране</param>
    /// <param name="ascending">Посока на сортиране</param>
    /// <returns>Сортирани продукти</returns>
    IEnumerable<Product> SortProducts(string sortBy, bool ascending = true);

    /// <summary>
    /// Изчислява общата стойност на склада.
    /// </summary>
    /// <returns>Обща стойност</returns>
    decimal GetTotalInventoryValue();
}
