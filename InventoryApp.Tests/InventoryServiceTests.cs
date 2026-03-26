using InventoryManagement.Models;
using InventoryManagement.Services;
using Xunit;

namespace InventoryApp.Tests;

/// <summary>
/// Unit тестове за InventoryService.
/// </summary>
public class InventoryServiceTests
{
    private readonly InventoryService inventoryService;

    public InventoryServiceTests()
    {
        inventoryService = new InventoryService();
    }

    [Fact]
    public void AddProduct_ValidProduct_ReturnsTrue()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Тестов продукт",
            Quantity = 10,
            Price = 50.00m,
            Supplier = "Тестов доставчик"
        };

        // Act
        bool result = inventoryService.AddProduct(product);

        // Assert
        Assert.True(result);
        Assert.Equal(1, inventoryService.GetAll().Count());
    }

    [Fact]
    public void AddProduct_DuplicateId_ReturnsFalse()
    {
        // Arrange
        var product1 = new Product { Id = 1, Name = "Продукт 1", Quantity = 5, Price = 10.00m, Supplier = "Доставчик 1" };
        var product2 = new Product { Id = 1, Name = "Продукт 2", Quantity = 3, Price = 20.00m, Supplier = "Доставчик 2" };

        // Act
        bool result1 = inventoryService.AddProduct(product1);
        bool result2 = inventoryService.AddProduct(product2);

        // Assert
        Assert.True(result1);
        Assert.False(result2);
        Assert.Single(inventoryService.GetAll());
    }

    [Fact]
    public void UpdateProduct_ExistingProduct_ReturnsTrue()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Продукт", Quantity = 5, Price = 10.00m, Supplier = "Доставчик" };
        inventoryService.AddProduct(product);

        // Act
        bool result = inventoryService.UpdateProduct(1, "Обновен продукт", 15, 25.00m, "Нов доставчик");

        // Assert
        Assert.True(result);
        var updatedProduct = inventoryService.GetProductById(1);
        Assert.NotNull(updatedProduct);
        Assert.Equal("Обновен продукт", updatedProduct.Name);
        Assert.Equal(15, updatedProduct.Quantity);
        Assert.Equal(25.00m, updatedProduct.Price);
        Assert.Equal("Нов доставчик", updatedProduct.Supplier);
    }

    [Fact]
    public void UpdateProduct_NonExistingProduct_ReturnsFalse()
    {
        // Act
        bool result = inventoryService.UpdateProduct(999, "Обновен продукт", 15, 25.00m, "Нов доставчик");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DeleteProduct_ExistingProduct_ReturnsTrue()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Продукт", Quantity = 5, Price = 10.00m, Supplier = "Доставчик" };
        inventoryService.AddProduct(product);

        // Act
        bool result = inventoryService.DeleteProduct(1);

        // Assert
        Assert.True(result);
        Assert.Empty(inventoryService.GetAll());
    }

    [Fact]
    public void DeleteProduct_NonExistingProduct_ReturnsFalse()
    {
        // Act
        bool result = inventoryService.DeleteProduct(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SearchByName_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Клавиатура", Quantity = 10, Price = 50.00m, Supplier = "Доставчик" };
        inventoryService.AddProduct(product);

        // Act
        var result = inventoryService.SearchByName("Клавиатура");

        // Assert
        Assert.Single(result);
        Assert.Equal("Клавиатура", result.First().Name);
    }

    [Fact]
    public void SearchByName_CaseInsensitive_ReturnsProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Клавиатура", Quantity = 10, Price = 50.00m, Supplier = "Доставчик" };
        inventoryService.AddProduct(product);

        // Act
        var result = inventoryService.SearchByName("клавиатура");

        // Assert
        Assert.Single(result);
        Assert.Equal("Клавиатура", result.First().Name);
    }

    [Fact]
    public void FilterByMinimumStock_ReturnsCorrectProducts()
    {
        // Arrange
        inventoryService.AddProduct(new Product { Id = 1, Name = "Продукт 1", Quantity = 5, Price = 10.00m, Supplier = "Доставчик" });
        inventoryService.AddProduct(new Product { Id = 2, Name = "Продукт 2", Quantity = 15, Price = 20.00m, Supplier = "Доставчик" });
        inventoryService.AddProduct(new Product { Id = 3, Name = "Продукт 3", Quantity = 3, Price = 30.00m, Supplier = "Доставчик" });

        // Act
        var result = inventoryService.FilterByMinimumStock(10);

        // Assert
        Assert.Single(result);
        Assert.True(result.First().Quantity >= 10);
    }

    [Fact]
    public void FilterByPriceRange_ReturnsCorrectProducts()
    {
        // Arrange
        inventoryService.AddProduct(new Product { Id = 1, Name = "Продукт 1", Quantity = 5, Price = 10.00m, Supplier = "Доставчик" });
        inventoryService.AddProduct(new Product { Id = 2, Name = "Продукт 2", Quantity = 15, Price = 25.00m, Supplier = "Доставчик" });
        inventoryService.AddProduct(new Product { Id = 3, Name = "Продукт 3", Quantity = 3, Price = 50.00m, Supplier = "Доставчик" });

        // Act
        var result = inventoryService.FilterByPriceRange(15.00m, 30.00m);

        // Assert
        Assert.Single(result);
        Assert.Equal(25.00m, result.First().Price);
    }

    [Fact]
    public void SortProducts_SortsCorrectly()
    {
        // Arrange
        inventoryService.AddProduct(new Product { Id = 1, Name = "B Продукт", Quantity = 5, Price = 10.00m, Supplier = "Доставчик" });
        inventoryService.AddProduct(new Product { Id = 2, Name = "A Продукт", Quantity = 15, Price = 20.00m, Supplier = "Доставчик" });
        inventoryService.AddProduct(new Product { Id = 3, Name = "C Продукт", Quantity = 3, Price = 30.00m, Supplier = "Доставчик" });

        // Act
        var result = inventoryService.SortProducts("name", true);

        // Assert
        Assert.Equal(3, result.Count());
        Assert.Equal("A Продукт", result.ElementAt(0).Name);
        Assert.Equal("B Продукт", result.ElementAt(1).Name);
        Assert.Equal("C Продукт", result.ElementAt(2).Name);
    }

    [Fact]
    public void GetTotalInventoryValue_ReturnsCorrectSum()
    {
        // Arrange
        inventoryService.AddProduct(new Product { Id = 1, Name = "Продукт 1", Quantity = 5, Price = 10.00m, Supplier = "Доставчик" });
        inventoryService.AddProduct(new Product { Id = 2, Name = "Продукт 2", Quantity = 3, Price = 20.00m, Supplier = "Доставчик" });

        // Act
        decimal result = inventoryService.GetTotalInventoryValue();

        // Assert
        Assert.Equal(110.00m, result); // (5 * 10) + (3 * 20) = 50 + 60 = 110
    }
}
