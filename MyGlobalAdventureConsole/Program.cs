using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; //Работа с файлами

namespace ConsoleGame 
{
    class Program
    {
        static char[,] map; // Карту игры в виде двумерного массива
        static int playerX = 14; // Начальная позиция игрока по оси X
        static int playerY = 4; // Начальная позиция игрока по оси Y
        static int coins = 0; // Начальное количество монет
        static int healthPotions = 1; // Начальное количество бутылочек здоровья
        static int manaPotions = 0; // Начальное количество бутылочек маны
        static int food = 0; // Начальное количество еды
        static int level = 1; // Начальный уровень игрока
        static int health = 3; // Начальное количество здоровья
        static int mana = 1; // Начальное количество маны
        const int maxHealth = 10; // Максимальное здоровье
        const int maxMana = 10; // Максимальная мана

        static void Main(string[] args)
        {
            LoadMap("map.txt"); // Загрузка карты из файла
            while (true)
            {
                Console.Clear(); // Очистка экрана
                DisplayMap(); // Отображение карты
                DisplayStats(); // Отображение статистики игрока
                HandleInput(); // Обработка ввода для перемещения игрока
            }
        }

        static void LoadMap(string filePath)
        {
            var lines = File.ReadAllLines(filePath); // Чтение всех строк из файла карты
            map = new char[lines.Length, lines[0].Length]; // Создание двумерного массива для карты
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    map[i, j] = lines[i][j]; // Заполнение массива значениями из файла
                }
            }
        }

        static void DisplayMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == playerY && j == playerX)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue; // Цвет игрока
                        Console.Write('@'); // Отображение игрока
                    }
                    else
                    {
                        switch (map[i, j])
                        {
                            case '#':
                                Console.ForegroundColor = ConsoleColor.Gray; // Цвет стены
                                Console.Write('#'); // Отображение стены
                                break;

                            case '0':
                                Console.ForegroundColor = ConsoleColor.Yellow; // Цвет монеты
                                Console.Write('0'); // Отображение монеты
                                break;

                            case '$':
                                Console.ForegroundColor = ConsoleColor.Green; // Цвет магазина
                                Console.Write('$'); // Отображение магазина
                                break;

                            case 'L':
                                Console.ForegroundColor = ConsoleColor.DarkYellow; // Цвет лагеря
                                Console.Write('L'); // Отображение лагеря
                                break;

                            case 'D':
                                Console.ForegroundColor = ConsoleColor.DarkMagenta; // Цвет подземелья
                                Console.Write('D'); // Отображение подземелья
                                break;

                            default:
                                Console.Write(map[i, j]); // Отображение пустой клетки
                                break;
                        }
                    }
                }
                Console.WriteLine(); // Переход на следующую строку
            }
            Console.ResetColor(); // Сброс цвета консоли
        }

        static void DisplayStats()
        {
            // Отображение уровня, здоровья, маны и инвентаря
            Console.WriteLine($"Level: {level}");
            Console.WriteLine($"Health: {health}/{maxHealth}   Mana: {mana}/{maxMana}");
            Console.WriteLine($"Coins: {coins}   Health Potions: {healthPotions}   Mana Potions: {manaPotions}   Food: {food}");
        }

        static void HandleInput()
        {
            var key = Console.ReadKey(true).Key; // Чтение нажатой клавиши

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    MovePlayer(playerX, playerY - 1); // Движение вверх
                    break;

                case ConsoleKey.DownArrow:
                    MovePlayer(playerX, playerY + 1); // Движение вниз
                    break;

                case ConsoleKey.LeftArrow:
                    MovePlayer(playerX - 1, playerY); // Движение влево
                    break;

                case ConsoleKey.RightArrow:
                    MovePlayer(playerX + 1, playerY); // Движение вправо
                    break;
            }
        }

        static void MovePlayer(int newX, int newY)
        {
            // Проверка выхода за границы карты
            if (newX < 0 || newY < 0 || newX >= map.GetLength(1) || newY >= map.GetLength(0))
                return;

            if (map[newY, newX] == '#') // Проверка на стену
                return;

            if (map[newY, newX] == '0') // Проверка на монету
            {
                coins += 2; // Увеличиваем количество монет
                map[newY, newX] = ' '; // Убираем монету с карты
            }

            if (map[newY, newX] == '$') // Проверка на магазин
            {
                Shop(); // Открытие магазина
            }

            if (map[newY, newX] == 'L') // Проверка на лагерь
            {
                Camp(); // Открытие лагеря
            }

            if (map[newY, newX] == 'D') // Проверка на подземелье
            {
                // Загрузка подземелья (файла Dunge1.txt)
                LoadMap("Dunge1.txt");
            }

            playerX = newX; // Обновление позиции игрока
            playerY = newY; // Обновление позиции игрока
        }

        static void Shop()
        {
            Console.Clear(); // Очистка экрана
            Console.WriteLine("Добро пожаловать в магазин!"); // Приветствие в магазине
            int availableHealthPotions = new Random().Next(0, 6); // Случайное количество бутылочек здоровья
            int availableManaPotions = new Random().Next(0, 4); // Случайное количество бутылочек маны
            int availableFood = new Random().Next(0, 11); // Случайное количество еды

            // Отображение ассортимента магазина
            Console.WriteLine($"Доступные бутылочки здоровья: {availableHealthPotions}");
            Console.WriteLine($"Доступные бутылочки маны: {availableManaPotions}");
            Console.WriteLine($"Доступная еда: {availableFood}");

            while (true) // Цикл для взаимодействия с магазином
            {
                Console.WriteLine("Введите номер команды: \n1 - купить бутылочки здоровья\n2 - купить бутылочки маны\n3 - купить еду\n4 - покинуть магазин");
                var choice = Console.ReadKey(true).Key; // Чтение выбора пользователя

                if (choice == ConsoleKey.D1) // Выбор покупки бутылочек здоровья
                {
                    BuyHealthPotions(availableHealthPotions);
                }
                else if (choice == ConsoleKey.D2) // Выбор покупки бутылочек маны
                {
                    BuyManaPotions(availableManaPotions);
                }
                else if (choice == ConsoleKey.D3) // Выбор покупки еды
                {
                    BuyFood(availableFood);
                }
                else if (choice == ConsoleKey.D4) // Покинуть магазин
                {
                    break; // Выход из цикла магазина
                }
            }
        }

        static void BuyHealthPotions(int available)
        {
            Console.Clear(); // Очистка экрана
            Console.Write("Сколько бутылочек здоровья вы хотите купить? "); // Запрос на покупку
            if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0 && amount <= available) // Проверка ввода и доступности
            {
                int cost = amount; // 1 монета за бутылочка
                if (cost <= coins) // Проверка наличия монет
                {
                    coins -= cost; // Снижение количества монет
                    healthPotions += amount; // Увеличение количества бутылочек здоровья
                    Console.WriteLine($"Вы купили {amount} бутылочек здоровья."); // Подтверждение покупки
                }
                else
                {
                    Console.WriteLine("Недостаточно монет!"); // Ошибка, если монет недостаточно
                }
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения."); // Ожидание ввода
            Console.ReadKey(); // Ожидание нажатия клавиши
        }

        static void BuyManaPotions(int available)
        {
            Console.Clear(); // Очистка экрана
            Console.Write("Сколько бутылочек маны вы хотите купить? "); // Запрос на покупку
            if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0 && amount <= available) // Проверка ввода и доступности
            {
                int cost = amount * 2; // 2 монеты за бутылочка
                if (cost <= coins) // Проверка наличия монет
                {
                    coins -= cost; // Снижение количества монет
                    manaPotions += amount; // Увеличение количества бутылочек маны
                    Console.WriteLine($"Вы купили {amount} бутылочек маны."); // Подтверждение покупки
                }
                else
                {
                    Console.WriteLine("Недостаточно монет!"); // Ошибка, если монет недостаточно
                }
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения."); // Ожидание ввода
            Console.ReadKey(); // Ожидание нажатия клавиши
        }

        static void BuyFood(int available)
        {
            Console.Clear(); // Очистка экрана
            Console.Write("Сколько еды вы хотите купить? "); // Запрос на покупку
            if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0 && amount <= available) // Проверка ввода и доступности
            {
                int cost = amount; // 1 монета за еду
                if (cost <= coins) // Проверка наличия монет
                {
                    coins -= cost; // Снижение количества монет
                    food += amount; // Увеличение количества еды
                    Console.WriteLine($"Вы купили {amount} еды."); // Подтверждение покупки
                }
                else
                {
                    Console.WriteLine("Недостаточно монет!"); // Ошибка, если монет недостаточно
                }
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения."); // Ожидание ввода
            Console.ReadKey(); // Ожидание нажатия клавиши
        }

        //Функция работы Лагеря
        static void Camp()
        {
            Console.Clear(); // Очистка экрана
            while (true) // Цикл для взаимодействия в лагере
            {
                Console.WriteLine("Вы находитесь в лагере."); // Информация о лагере
                Console.WriteLine("1 - Поесть"); // Опция поесть
                Console.WriteLine("2 - Отдохнуть"); // Опция отдохнуть
                Console.WriteLine("3 - Свернуть лагерь"); // Опция свернуть лагерь
                var choice = Console.ReadKey(true).Key; // Чтение выбора пользователя

                if (choice == ConsoleKey.D1) // Если выбор - поесть
                {
                    if (food > 0) // Проверка наличия еды
                    {
                        food--; // Уменьшение еды
                        health = Math.Min(health + 1, maxHealth); // Восстановление здоровья
                        Console.WriteLine("Вы поели и восстановили 1 здоровье."); // Подтверждение действия
                    }
                    else
                    {
                        Console.WriteLine("У вас нет еды, чтобы поесть."); // Ошибка, если еды нет
                    }
                }
                else if (choice == ConsoleKey.D2) // Если выбор - отдохнуть
                {
                    mana = Math.Min(mana + 2, maxMana); // Восстановление маны
                    Console.WriteLine("Вы отдохнули и восстановили 2 маны."); // Подтверждение действия
                }
                else if (choice == ConsoleKey.D3) // Если выбор - свернуть лагерь
                {
                    break; // Выход из цикла лагеря
                }
                Console.WriteLine("Нажмите любую клавишу для продолжения."); // Ожидание ввода
                Console.ReadKey(); // Ожидание нажатия клавиши
            }
        }
    }
}
