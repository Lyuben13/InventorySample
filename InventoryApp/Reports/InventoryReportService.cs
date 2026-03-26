using InventoryManagement.Models;
using InventoryManagement.Services;

namespace InventoryManagement.Reports;

/// <summary>
/// Клас услуга за генериране на складови отчети и обобщения.
/// Предоставя форматиране на отчети за анализ на склад, финансова оценка и управление на наличности.
/// Тази услуга разделя логиката на отчетите от бизнес логиката за по-добра организация на кода.
/// </summary>
public class InventoryReportService
{
    private readonly IInventoryService inventoryService;

    /// <summary>
    /// Инициализира нова инстанция на класа InventoryReportService.
    /// Използва dependency injection за получаване на услугата за склад за достъп до данни.
    /// </summary>
    /// <param name="inventoryService">Услугата за склад, използвана за извличане на данни за продукти</param>
    /// <exception cref="ArgumentNullException">Хвърля се, когато inventoryService е null</exception>
    public InventoryReportService(IInventoryService inventoryService)
    {
        // Валидиране на параметъра от dependency injection
        this.inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
    }

    /// <summary>
    /// Генерира всеобхватен складов отчет с детайли за продукти и обобщена статистика.
    /// Отчетът включва всички продукти, сортирани по азбучен ред, обща стойност на склада и аларми за ниски наличности.
    /// Форматът е оптимизиран за конзолен дисплей с правилно подравняване на колони.
    /// </summary>
    /// <returns>Форматиран низ, съдържащ пълния складов отчет</returns>
    public string GenerateSummary()
    {
        // Извличане на всички продукти и сортиране по име за последователно подреждане на отчета
        var products = inventoryService.GetAll().OrderBy(p => p.Name).ToList();

        // Обработка на случай с празен склад
        if (products.Count == 0)
        {
            return "Няма продукти в склада.";
        }

        // Изграждане на хедъра на отчета с времеви печат и заглавия на колони
        var lines = new List<string>
        {
            "=== СКЛАДОВ ОТЧЕТ ===",
            $"Генериран на: {DateTime.Now:dd.MM.yyyy HH:mm}",
            new string('-', 76),
            string.Format("{0,-4} {1,-20} {2,7} {3,8:F2} {4,-18}", "ID", "Име", "Налич.", "Цена", "Доставчик"),
            new string('-', 76)
        };

        // Добавяне на всеки продукт към отчета с правилно форматиране
        foreach (var product in products)
        {
            lines.Add(string.Format(
                "{0,-4} {1,-20} {2,7} {3,8:F2} {4,-18}",
                product.Id,
                Trim(product.Name, 20),    // Съкращаване на дълги имена за подравняване на колони
                product.Quantity,
                product.Price,
                Trim(product.Supplier, 18))); // Съкращаване на дълги имена на доставчици
        }

        // Добавяне на футър на отчета с обобщена информация
        lines.Add(new string('-', 76));
        lines.Add($"Обща стойност на склада: {inventoryService.GetTotalInventoryValue():F2} BGN");

        // Добавяне на аларми за ниски наличности за проактивно управление на склада
        var lowStockProducts = products.Where(p => p.Quantity < 5).ToList();
        if (lowStockProducts.Count > 0)
        {
            lines.Add("Продукти с ниски наличности: " + string.Join(", ", lowStockProducts.Select(p => $"{p.Name} ({p.Quantity})")));
        }

        // Обединяване на всички редове в един форматиран низ
        return string.Join(Environment.NewLine, lines);
    }

    /// <summary>
    /// Съкращава низ, за да се побере в указаната максимална дължина.
    /// Използва се за осигуряване на правилно подравняване на колони в конзолните отчети.
    /// Ако низът надвишава максималната дължина, той се съкращава и се добавя "...".
    /// </summary>
    /// <param name="value">Низът, който евентуално да бъде съкратен</param>
    /// <param name="maxLength">Максималната позволена дължина за низа</param>
    /// <returns>Оригиналният низ, ако е в рамките на лимита, или съкратена версия със суфикс "..."</returns>
    private static string Trim(string value, int maxLength)
    {
        // Връщане на оригиналния низ, ако вече е в рамките на лимита за дължина
        if (value.Length <= maxLength)
        {
            return value;
        }

        // Съкращаване на низа и добавяне на многоточие за индикация на съкращаване
        // Оставяне на място за 3-символното многоточие
        return value[..(maxLength - 3)] + "...";
    }
}
