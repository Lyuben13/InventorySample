using InventoryManagement.Extensions;
using InventoryManagement.Helpers;
using InventoryManagement.Models;
using InventoryManagement.Reports;
using InventoryManagement.Services;

/// <summary>
/// Основна точка на вход за конзолното приложение за управление на склада.
/// </summary>
class Program
{
    // Инициализация на услуги
    static readonly IInventoryService inventoryService = new InventoryService();
    static readonly InventoryReportService reportService = new(inventoryService);

    /// <summary>
    /// Точка на вход на приложението, която оркестрира основния програмен поток.
    /// Инициализира приложението с примерни данни и изпълнява основния цикъл на менюто.
    /// </summary>
    /// <param name="args">Аргументи от командния ред (не се използват в това приложение)</param>
    static void Main(string[] args)
    {
        // Настройка на заглавието на конзолния прозорец
        Console.Title = "Система за управление на склада";
        
        // Опит за настройка на персонализиран шрифт (optional)
        ConsoleAppearanceHelper.TrySetCustomFont();
        
        try
        {
            // Попълване на склада с първоначални примерни данни за демонстрация
            SeedData(inventoryService);

            // Основен цикъл на приложението - продължава, докато потребителят избере да излезе
            bool isRunning = true;
            while (isRunning)
            {
                // Показване на основното меню и получаване на избора на потребителя
                ConsoleMenu.PrintMenu();
                string? choice = Console.ReadLine();

                Console.WriteLine(); // Добавяне на разстояние за по-добра четимост

                try
                {
                    // Обработка на избора на потребителя с помощта на switch израз
                    isRunning = ProcessMenuChoice(choice, inventoryService, reportService);
                }
                catch (Exception ex)
                {
                    // Обработка на всякакви неочаквани грешки по елегантен начин
                    Console.WriteLine($"Грешка: {ex.Message}");
                    Console.WriteLine("Опитайте отново или се свържете с поддръжка, ако проблемът продължава.");
                }
            }

            Console.WriteLine("Благодарим ви, че използвахте Системата за управление на склада!");
        }
        catch (Exception ex)
        {
            // Обработка на критични грешки при стартиране на приложението
            Console.WriteLine($"Приложението не успя да стартира: {ex.Message}");
        }
    }

    /// <summary>
    /// Обработва избора на потребителя от менюто и изпълнява съответната операция.
    /// </summary>
    /// <param name="choice">Изборът на потребителя от менюто</param>
    /// <param name="inventoryService">Услугата за склад за операции с данни</param>
    /// <param name="reportService">Услугата за отчети за генериране на отчети</param>
    /// <returns>True за продължаване на изпълнението, false за изход от приложението</returns>
    private static bool ProcessMenuChoice(string? choice, IInventoryService inventoryService, InventoryReportService reportService)
    {
        return choice switch
        {
            "1" => ExecuteAndContinue(() => AddProduct(inventoryService)),
            "2" => ExecuteAndContinue(() => UpdateProduct(inventoryService)),
            "3" => ExecuteAndContinue(() => DeleteProduct(inventoryService)),
            "4" => ExecuteAndContinue(() => ListProducts(inventoryService.GetAll())),
            "5" => ExecuteAndContinue(() => SearchProductsByName(inventoryService)),
            "6" => ExecuteAndContinue(() => SearchProductsById(inventoryService)),
            "7" => ExecuteAndContinue(() => SearchProductsBySupplier(inventoryService)),
            "8" => ExecuteAndContinue(() => FilterProductsByStock(inventoryService)),
            "9" => ExecuteAndContinue(() => FilterProductsByPrice(inventoryService)),
            "10" => ExecuteAndContinue(() => SortProducts(inventoryService)),
            "11" => ExecuteAndContinue(() => Console.WriteLine(reportService.GenerateSummary())),
            "0" => false,
            _ => ExecuteAndContinue(() => Console.WriteLine("Непозната опция. Опитайте отново."))
        };
    }

    /// <summary>
    /// Изпълнява действие и връща true за продължаване на цикъла.
    /// </summary>
    /// <param name="action">Действието за изпълнение</param>
    /// <returns>Винаги връща true за продължаване на изпълнението</returns>
    private static bool ExecuteAndContinue(Action action)
    {
        action();
        return true;
    }

    /// <summary>
    /// Попълва склада с първоначални примерни данни.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад</param>
    private static void SeedData(IInventoryService inventoryService)
    {
        try
        {
            inventoryService.AddProduct(new Product 
            { 
                Id = 1, 
                Name = "Клавиатура", 
                Quantity = 10, 
                Price = 55.90m, 
                Supplier = "ТехноСток" 
            });
            
            inventoryService.AddProduct(new Product 
            { 
                Id = 2, 
                Name = "Мишка", 
                Quantity = 22, 
                Price = 24.50m, 
                Supplier = "ОфисСуплай" 
            });
            
            inventoryService.AddProduct(new Product 
            { 
                Id = 3, 
                Name = "Монитор", 
                Quantity = 4, 
                Price = 319.99m, 
                Supplier = "ДисплейМаркет" 
            });
            
            inventoryService.AddProduct(new Product 
            { 
                Id = 4, 
                Name = "Лаптоп", 
                Quantity = 8, 
                Price = 1299.00m, 
                Supplier = "КомпютърЦентър" 
            });
            
            inventoryService.AddProduct(new Product 
            { 
                Id = 5, 
                Name = "Принтер", 
                Quantity = 3, 
                Price = 289.50m, 
                Supplier = "ТехноСток" 
            });
            
            inventoryService.AddProduct(new Product 
            { 
                Id = 6, 
                Name = "USB кабел", 
                Quantity = 50, 
                Price = 12.99m, 
                Supplier = "АксесоариБГ" 
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Предупреждение: Неуспешно попълване с примерни данни: {ex.Message}");
        }
    }

    /// <summary>
    /// Добавя нов продукт в склада.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад</param>
    private static void AddProduct(IInventoryService inventoryService)
    {
        try
        {
            Console.WriteLine("=== Добавяне на нов продукт ===");
            int id = ConsoleMenu.ReadInt("ID на продукта: ");
            string name = ConsoleMenu.ReadRequiredString("Име на продукта: ");
            int quantity = ConsoleMenu.ReadInt("Количество: ");
            decimal price = ConsoleMenu.ReadDecimal("Цена: ");
            string supplier = ConsoleMenu.ReadRequiredString("Доставчик: ");

            var newProduct = new Product
            {
                Id = id,
                Name = name,
                Quantity = quantity,
                Price = price,
                Supplier = supplier
            };

            bool success = inventoryService.AddProduct(newProduct);
            Console.WriteLine(success ? "✅ Продуктът е добавен успешно!" : "❌ Продукт с такова ID вече съществува.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Грешка при добавяне: {ex.Message}");
        }
    }

    /// <summary>
    /// Обработва процеса на обновяване на съществуващ продукт в склада.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад</param>
    private static void UpdateProduct(IInventoryService inventoryService)
    {
        try
        {
            // Получаване на ID на продукта за обновяване
            int id = ConsoleMenu.ReadInt("Въведете ID на продукта за обновяване: ");
            
            // Събиране на нова информация за продукта
            string name = ConsoleMenu.ReadRequiredString("Ново име: ");
            int quantity = ConsoleMenu.ReadInt("Ново количество: ");
            decimal price = ConsoleMenu.ReadDecimal("Нова цена: ");
            string supplier = ConsoleMenu.ReadRequiredString("Нов доставчик: ");

            // Опит за обновяване на продукта
            bool isUpdated = inventoryService.UpdateProduct(id, name, quantity, price, supplier);
            Console.WriteLine(isUpdated ? "Продуктът е обновен успешно." : "Продуктът не е намерен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Неуспешно обновяване на продукт: {ex.Message}");
        }
    }

    /// <summary>
    /// Обработва процеса на изтриване на продукт от склада.
    /// Подканва за ID на продукта и се опитва да премахне продукта.
    /// Предоставя ясна обратна връзка дали изтриването е било успешно.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад, използвана за изтриване на продукта</param>
    private static void DeleteProduct(IInventoryService inventoryService)
    {
        try
        {
            // Получаване на ID на продукта за изтриване
            int id = ConsoleMenu.ReadInt("Въведете ID на продукта за изтриване: ");
            
            // Опит за изтриване на продукта
            bool isDeleted = inventoryService.DeleteProduct(id);
            Console.WriteLine(isDeleted ? "Продуктът е изтрит успешно." : "Продуктът не е намерен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Неуспешно изтриване на продукт: {ex.Message}");
        }
    }

    /// <summary>
    /// Обработва процеса на търсене на продукти по ключова дума в името.
    /// Извършва case-insensitive търсене и показва съвпадащи резултати.
    /// Показва подходяща обратна връзка, когато няма продукти, отговарящи на критериите за търсене.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад, използвана за търсене на продукти</param>
    private static void SearchProductsByName(IInventoryService inventoryService)
    {
        try
        {
            // Получаване на ключова дума за търсене от потребителя
            string keyword = ConsoleMenu.ReadRequiredString("Въведете ключова дума за име на продукта: ");
            
            // Извършване на търсенето
            var products = inventoryService.SearchByName(keyword).ToList();

            // Обработка на празни резултати
            if (products.Count == 0)
            {
                Console.WriteLine("Няма намерени продукти.");
                return;
            }

            // Показване на резултатите от търсенето
            ListProducts(products);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Търсенето е неуспешно: {ex.Message}");
        }
    }

    /// <summary>
    /// Обработва процеса на търсене на продукти по уникален ID.
    /// Показва продукт, който съответства на точното ID.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад, използвана за търсене на продукти</param>
    private static void SearchProductsById(IInventoryService inventoryService)
    {
        try
        {
            // Получаване на ID за търсене от потребителя
            int id = ConsoleMenu.ReadInt("Въведете ID на продукта: ");
            
            // Извършване на търсенето
            var products = inventoryService.SearchById(id).ToList();

            // Обработка на резултати
            if (products.Count == 0)
            {
                Console.WriteLine("Продукт с такова ID не е намерен.");
                return;
            }

            // Показване на резултатите от търсенето
            ListProducts(products);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Търсенето е неуспешно: {ex.Message}");
        }
    }

    /// <summary>
    /// Обработва процеса на търсене на продукти по доставчик.
    /// Извършва case-insensitive търсене по име на доставчик.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад, използвана за търсене на продукти</param>
    private static void SearchProductsBySupplier(IInventoryService inventoryService)
    {
        try
        {
            // Получаване на име на доставчик за търсене
            string supplierName = ConsoleMenu.ReadRequiredString("Въведете име на доставчик: ");
            
            // Извършване на търсенето
            var products = inventoryService.SearchBySupplier(supplierName).ToList();

            // Обработка на празни резултати
            if (products.Count == 0)
            {
                Console.WriteLine("Няма намерени продукти от този доставчик.");
                return;
            }

            // Показване на резултатите от търсенето
            ListProducts(products);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Търсенето е неуспешно: {ex.Message}");
        }
    }

    /// <summary>
    /// Обработва процеса на филтриране на продукти по минимална наличност.
    /// Полезно за задачи по управление на склада като идентифициране на продукти, които се нуждаят от презареждане.
    /// Показва подходяща обратна връзка, когато няма продукти, отговарящи на критериите за филтриране.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад, използвана за филтриране на продукти</param>
    private static void FilterProductsByStock(IInventoryService inventoryService)
    {
        try
        {
            // Получаване на минимален праг на количество от потребителя
            int minQuantity = ConsoleMenu.ReadInt("Въведете минимална наличност: ");
            
            // Извършване на филтрирането
            var products = inventoryService.FilterByMinimumStock(minQuantity).ToList();

            // Обработка на празни резултати
            if (products.Count == 0)
            {
                Console.WriteLine("Няма намерени продукти.");
                return;
            }

            // Показване на филтрираните резултати
            ListProducts(products);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Филтрирането е неуспешно: {ex.Message}");
        }
    }

    /// <summary>
    /// Обработва процеса на филтриране на продукти по ценов диапазон.
    /// Полезно за анализ на продукти в определена ценова категория.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад, използвана за филтриране на продукти</param>
    private static void FilterProductsByPrice(IInventoryService inventoryService)
    {
        try
        {
            // Получаване на ценови диапазон от потребителя
            decimal minPrice = ConsoleMenu.ReadDecimal("Въведете минимална цена: ");
            decimal maxPrice = ConsoleMenu.ReadDecimal("Въведете максимална цена: ");
            
            // Извършване на филтрирането
            var products = inventoryService.FilterByPriceRange(minPrice, maxPrice).ToList();

            // Обработка на празни резултати
            if (products.Count == 0)
            {
                Console.WriteLine("Няма продукти в този ценови диапазон.");
                return;
            }

            // Показване на филтрираните резултати
            ListProducts(products);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Филтрирането е неуспешно: {ex.Message}");
        }
    }

    /// <summary>
    /// Обработва процеса на сортиране на продукти по указан критерий.
    /// Позволява на потребителя да избере критерий и посока на сортиране.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад, използвана за сортиране на продукти</param>
    private static void SortProducts(IInventoryService inventoryService)
    {
        try
        {
            // Показване на опции за сортиране
            Console.WriteLine("Критерии за сортиране:");
            Console.WriteLine("1. Име");
            Console.WriteLine("2. Цена");
            Console.WriteLine("3. Количество");
            Console.WriteLine("4. Доставчик");
            Console.WriteLine("5. Обща стойност");
            
            int sortChoice = ConsoleMenu.ReadInt("Изберете критерий за сортиране: ");
            
            // Избор на критерий
            string sortBy = sortChoice switch
            {
                1 => "име",
                2 => "цена",
                3 => "количество",
                4 => "доставчик",
                5 => "стойност",
                _ => "име"
            };

            // Избор на посока
            bool ascending = ConsoleMenu.ReadInt("Сортиране (1-възходящо, 2-низходящо): ") == 1;
            
            // Извършване на сортирането
            var products = inventoryService.SortProducts(sortBy, ascending).ToList();

            // Показване на сортираните резултати
            ListProducts(products);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сортирането е неуспешно: {ex.Message}");
        }
    }

    /// <summary>
    /// Показва форматиран списък от продукти в табличен формат.
    /// Използва колони с фиксирана ширина за правилно подравняване и четимост.
    /// Обработва празни колекции по елегантен начин с подходящи съобщения.
    /// </summary>
    /// <param name="products">Колекцията от продукти за показване</param>
    private static void ListProducts(IEnumerable<Product> products)
    {
        // Конвертиране в списък за проверка на брой и позволяване на множествени изброявания
        var productList = products.ToList();
        
        // Обработка на празна колекция
        if (productList.Count == 0)
        {
            Console.WriteLine("Няма продукти за показване.");
            return;
        }

        // Показване на хедър на таблицата
        Console.WriteLine("ID | Име                  | Налич. | Цена     | Доставчик         ");
        Console.WriteLine(new string('-', 68));

        // Показване на всеки продукт в форматиран ред на таблицата
        foreach (var product in productList)
        {
            Console.WriteLine($"{product.Id,-2} | {product.Name,-20} | {product.Quantity,7} | {product.Price,8:F2} | {product.Supplier,-18}");
        }
    }
}
