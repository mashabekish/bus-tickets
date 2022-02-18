﻿using bus_tickets.Controllers;
using bus_tickets.Models;

namespace bus_tickets
{
    internal class Handler
    {
        private readonly UserController userController;
        private readonly FlightController flightController;
        private readonly TicketController ticketController;

        public Handler(Database database)
        {
            userController = new(database);
            flightController = new(database);
            ticketController = new(database);
        }

        internal void Start()
        {
            while (true)
            {
                Console.WriteLine("--- Продажа автобусных билетов ---");

                User user = userController.Access();
                if (user.IsAdmin)
                {
                    AdminMenu(user.Id);
                }
                else
                {
                    UserMenu(user.Id);
                }
            }
        }

        private int UserMenu(int id)
        {
            while (true)
            {
                Console.WriteLine("\n------- Меню пользователя -------");
                Console.WriteLine("1. Просмотреть рейсы");
                Console.WriteLine("2. Мои билеты");
                Console.WriteLine("3. Купить билеты");
                Console.WriteLine("4. Выйти из аккаунта");
                Console.WriteLine("5. Закрыть");
                string? key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        flightController.List();
                        break;
                    case "2":
                        ticketController.List(id);
                        break;
                    case "3":
                        flightController.List();
                        ticketController.Buy(id);
                        break;
                    case "4":
                        Console.Clear();
                        return 0;
                    case "5":
                        Environment.Exit(1);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Выбран несуществующий вариант");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private int AdminMenu(int id)
        {
            while (true)
            {
                Console.WriteLine("\n------ Меню администратора ------");
                Console.WriteLine("1. Посмотреть учетные записи");
                Console.WriteLine("2. Добавить новую учетную запись");
                Console.WriteLine("3. Изменить учетную запись");
                Console.WriteLine("4. Удалить учетную запись");
                Console.WriteLine("5. Подтвердить учетную запись");
                Console.WriteLine("6. Заблокировать учетную запись");
                Console.WriteLine("7. Просмотреть рейсы");
                Console.WriteLine("8. Добавить новый рейс");
                Console.WriteLine("9. Изменить рейс");
                Console.WriteLine("10. Удалить рейс");
                Console.WriteLine("11. Посмотреть историю приобретения билетов");
                Console.WriteLine("12. Выйти из аккаунта");
                Console.WriteLine("13. Закрыть");
                string? key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        userController.List();
                        break;
                    case "2":
                        userController.Create();
                        break;
                    case "3":
                        userController.Update(id);
                        break;
                    case "4":
                        userController.Delete(id);
                        break;
                    case "5":
                        userController.Activation();
                        break;
                    case "6":
                        userController.Deactivation(id);
                        break;
                    case "7":
                        flightController.List();
                        break;
                    case "8":
                        flightController.Create();
                        break;
                    case "9":
                        flightController.Update();
                        break;
                    case "10":
                        flightController.Delete();
                        break;
                    case "11":
                        ticketController.List();
                        break;
                    case "12":
                        Console.Clear();
                        return 0;
                    case "13":
                        Environment.Exit(1);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Выбран несуществующий вариант");
                        Console.ResetColor();
                        break;
                }
            }
        }
    }
}