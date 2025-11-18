using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace PostalIndexFinder
{
    // Клас, який інкапсулює логіку пошуку поштових індексів
    class PostalIndexSearcher
    {
        // Приватне поле зберігає вхідний текст
        private string _text;

        // Конструктор для ініціалізації тексту
        public PostalIndexSearcher(string text)
        {
            _text = text;
        }

        // Публічний метод для пошуку поштових індексів
        public List<string> FindPostalIndexes()
        {
            List<string> indexes = new List<string>();
            // Регулярний вираз для пошуку п'яти цифр підряд
            Regex regex = new Regex(@"\b\d{5}\b");

            MatchCollection matches = regex.Matches(_text);

            foreach (Match match in matches)
            {
                indexes.Add(match.Value);
            }

            return indexes;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Введіть текст:");
            string input = Console.ReadLine();

            // Створюємо об’єкт класу
            PostalIndexSearcher searcher = new PostalIndexSearcher(input);

            // Отримуємо результати
            List<string> indexes = searcher.FindPostalIndexes();

            Console.WriteLine("\nЗнайдені поштові індекси:");
            if (indexes.Count > 0)
            {
                foreach (string index in indexes)
                {
                    Console.WriteLine(index);
                }
            }
            else
            {
                Console.WriteLine("Поштових індексів не знайдено.");
            }

            Console.ReadKey();
        }
    }
}
