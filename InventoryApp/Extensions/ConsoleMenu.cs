namespace InventoryManagement.Extensions;

/// <summary>
/// Предоставя utility методи за конзолни взаимодействия с меню и валидация на потребителски вход.
/// Този статичен клас капсулира общи конзолни операции за намаляване на дублирането на код
/// и осигуряване на последователно потребителско изживяване в цялото приложение.
/// </summary>
public static class ConsoleMenu
{
    /// <summary>
    /// Показва основното меню с опции за системата за управление на склада.
    /// Предоставя ясен, номериран списък с налични операции за избор от потребителя.
    /// Менюто е проектирано да бъде интуитивно и лесно за навигация.
    /// </summary>
    public static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("==== МЕНЮ ЗА СКЛАД ====");
        Console.WriteLine("1. Добавяне на продукт");
        Console.WriteLine("2. Обновяване на продукт");
        Console.WriteLine("3. Изтриване на продукт");
        Console.WriteLine("4. Списък на всички продукти");
        Console.WriteLine("5. Търсене по име");
        Console.WriteLine("6. Търсене по ID");
        Console.WriteLine("7. Търсене по доставчик");
        Console.WriteLine("8. Филтриране по минимална наличност");
        Console.WriteLine("9. Филтриране по ценов диапазон");
        Console.WriteLine("10. Сортиране на продукти");
        Console.WriteLine("11. Показване на отчет");
        Console.WriteLine("0. Изход");
        Console.Write("Изберете опция: ");
    }

    /// <summary>
    /// Чете и валидира цяло число от потребителския вход.
    /// Продължава да подканва, докато не бъде въведено валидно цяло число.
    /// Предоставя ясни съобщения за грешка при невалиден вход за насочване на потребителя.
    /// </summary>
    /// <param name="message">Съобщението за подканване, което да се покаже на потребителя</param>
    /// <returns>Валидно цяло число, въведено от потребителя</returns>
    public static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);
            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value; // Въведено е валидно цяло число
            }

            // Предоставяне на обратна връзка за невалиден вход и повторен опит
            Console.WriteLine("Невалидно число. Опитайте отново.");
        }
    }

    /// <summary>
    /// Чете и валидира decimal стойност от потребителския вход.
    /// Продължава да подканва, докато не бъде въведено валидно decimal число.
    /// Съществено за въвеждане на цени, където точността е критична за финансови изчисления.
    /// </summary>
    /// <param name="message">Съобщението за подканване, което да се покаже на потребителя</param>
    /// <returns>Валидно decimal число, въведено от потребителя</returns>
    public static decimal ReadDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);
            if (decimal.TryParse(Console.ReadLine(), out decimal value))
            {
                return value; // Въведено е валидно decimal число
            }

            // Предоставяне на обратна връзка за невалиден вход и повторен опит
            Console.WriteLine("Невалидно decimal число. Опитайте отново.");
        }
    }

    /// <summary>
    /// Чете и валидира непразен низ от потребителския вход.
    /// Продължава да подканва, докато не бъде въведен валиден, непразен низ.
    /// Премахва whitespace от входа за осигуряване на чисто съхранение на данни.
    /// Съществено за задължителни полета като имена на продукти и информация за доставчици.
    /// </summary>
    /// <param name="message">Съобщението за подканване, което да се покаже на потребителя</param>
    /// <returns>Валиден, непразен низ, въведен от потребителя (почистен)</returns>
    public static string ReadRequiredString(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            // Проверка дали е предоставен вход и не е само whitespace
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim(); // Връщане на почистен вход
            }

            // Предоставяне на обратна връзка за празен вход и повторен опит
            Console.WriteLine("Стойността е задължителна.");
        }
    }
}
