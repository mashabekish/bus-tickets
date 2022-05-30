using bus_tickets.Models;

namespace bus_tickets.Controllers
{
    internal class FlightController
    {
        private readonly Database database;

        public FlightController(Database database)
        {
            this.database = database;
        }

        //Получение списка рейсов
        internal void List()
        {
            List<Flight> flights = database.Flights.OrderByDescending(f => f.Id).ToList();
            Print(flights);

            if (flights.Count != 0)
            {
                _ = Filter();
            }
        }

        //Вывод списка рейсов
        internal static void Print(List<Flight> flights)
        {
            if (flights.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Рейсов по вашему запросу не найдено");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(string.Format("{0,5} | {1,5} | {2,12} | {3,16} | {4,10} | {5,11} | {6,8} | {7,9} | {8,8} | {9,7}",
                    "Id ", "Номер", "Тип автобуса", "Пункт назначения", "Дата   ", "Отправление", "Прибытие", "Стоимость", "Осталось", "Продано"));

                foreach (Flight flight in flights)
                {
                    Console.WriteLine(flight);
                }
            }
        }

        //Выбор метода фильтрации рейсов
        internal int Filter()
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.WriteLine("1. Поиск");
                Console.WriteLine("2. Сортировка");

                string? key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        return Search();
                    case "2":
                        return Sorting();
                    case "0":
                        return 0;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Выбран несуществующий вариант");
                        Console.ResetColor();
                        break;
                }
            }
        }

        //Поиск рейса
        internal int Search()
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.WriteLine("1. Поиск по номеру рейса");
                Console.WriteLine("2. Поиск по пункту назначения");
                Console.WriteLine("3. Поиск по дате отправления");

                List<Flight> flights = new();
                string? key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        Console.Write("Введите номер рейса ");
                        string? number = Console.ReadLine();
                        flights = database.Flights.OrderByDescending(f => f.Id)
                            .Where(f => !string.IsNullOrEmpty(number) && !string.IsNullOrEmpty(f.Number) && f.Number.Contains(number, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    case "2":
                        Console.Write("Введите пункт назначения ");
                        string? destination = Console.ReadLine();
                        flights = database.Flights.OrderByDescending(f => f.Id)
                            .Where(f => !string.IsNullOrEmpty(destination) && !string.IsNullOrEmpty(f.Destination) && f.Destination.Contains(destination, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    case "3":
                        DateOnly date = GetDate();
                        flights = database.Flights.OrderByDescending(f => f.Id).Where(f => f.Date == date).ToList();
                        break;
                    case "0":
                        return 0;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Выбран несуществующий вариант");
                        Console.ResetColor();
                        continue;
                }
                Print(flights);
            }
        }

        //Сортировка рейсов
        internal int Sorting()
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.WriteLine("1. По возрастанию даты отправления");
                Console.WriteLine("2. По возрастанию времени отправления");
                Console.WriteLine("3. По возрастанию времени прибытия");
                Console.WriteLine("4. По возрастанию стоимости");
                Console.WriteLine("5. По убыванию даты отправления");
                Console.WriteLine("6. По убыванию времени отправления");
                Console.WriteLine("7. По убыванию времени прибытия");
                Console.WriteLine("8. По убыванию стоимости");

                List<Flight> flights = new();
                string? key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        flights = database.Flights.OrderBy(f => f.Date).ToList();
                        break;
                    case "2":
                        flights = database.Flights.OrderBy(f => f.DeparturesTime).ToList();
                        break;
                    case "3":
                        flights = database.Flights.OrderBy(f => f.ArrivalTime).ToList();
                        break;
                    case "4":
                        flights = database.Flights.OrderBy(f => f.Cost).ToList();
                        break;
                    case "5":
                        flights = database.Flights.OrderByDescending(f => f.Date).ToList();
                        break;
                    case "6":
                        flights = database.Flights.OrderByDescending(f => f.DeparturesTime).ToList();
                        break;
                    case "7":
                        flights = database.Flights.OrderByDescending(f => f.ArrivalTime).ToList();
                        break;
                    case "8":
                        flights = database.Flights.OrderByDescending(f => f.Cost).ToList();
                        break;
                    case "0":
                        return 0;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Выбран несуществующий вариант");
                        Console.ResetColor();
                        continue;
                }
                Print(flights);
            }
        }

        //Добавление нового рейса
        internal int Create()
        {
            Console.WriteLine("\n0. Назад");
            Console.Write("Введите номер рейса ");
            string? number = Console.ReadLine();

            if (number == "0")
            {
                return 0;
            }

            Console.Write("Введите тип автобуса ");
            string? busType = Console.ReadLine();

            Console.Write("Введите пункт назначения ");
            string? destination = Console.ReadLine();

            DateOnly date = GetDate();

            TimeOnly departuresTime = GetDeparturesTime();

            TimeOnly arrivalTime = GetArrivalTime(departuresTime);

            double cost = GetCost();

            uint count = GetCount();

            Flight flight = new()
            {
                Number = number,
                BusType = busType,
                Destination = destination,
                Date = date,
                DeparturesTime = departuresTime,
                ArrivalTime = arrivalTime,
                Cost = cost,
                Left = count
            };
            _ = database.Flights.Add(flight);
            _ = database.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Рейс добавлен");
            Console.ResetColor();

            return 0;
        }

        //Получение даты отправления автобуса
        private static DateOnly GetDate()
        {
            while (true)
            {
                Console.Write("Введите дату отправления ");
                if (DateOnly.TryParse(Console.ReadLine(), out DateOnly date))
                {
                    if (date >= DateOnly.FromDateTime(DateTime.Now))
                    {
                        return date;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Дата не может быть меньше текущей\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введена некорректная дата\n");
                    Console.ResetColor();
                }
            }
        }

        //Получение времени отправления автобуса
        private static TimeOnly GetDeparturesTime()
        {
            while (true)
            {
                Console.Write("Введите время отправления ");
                if (TimeOnly.TryParse(Console.ReadLine(), out TimeOnly departuresTime))
                {
                    if (departuresTime > TimeOnly.FromDateTime(DateTime.Now))
                    {
                        return departuresTime;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Время отправления не может быть меньше текущего или равное ему\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введено некорректное время\n");
                    Console.ResetColor();
                }
            }
        }

        //Получение времени прибытия автобуса
        private static TimeOnly GetArrivalTime(TimeOnly departuresTime)
        {
            while (true)
            {
                Console.Write("Введите время прибытия ");
                if (TimeOnly.TryParse(Console.ReadLine(), out TimeOnly arrivalTime))
                {
                    if (arrivalTime > departuresTime)
                    {
                        return arrivalTime;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Время прибытия не может быть меньше или равно времени отправления\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введено некорректное время\n");
                    Console.ResetColor();
                }
            }
        }

        //Получение стоимости билета
        private static double GetCost()
        {
            while (true)
            {
                Console.Write("Введите стоимость билета ");
                if (double.TryParse(Console.ReadLine(), out double cost) && cost > 0)
                {
                    return Math.Round(cost, 2);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введена некорректная стоимость\n");
                    Console.ResetColor();
                }
            }
        }

        //Получение количества билетов
        internal static uint GetCount()
        {
            while (true)
            {
                Console.Write("Введите количество билетов ");
                if (uint.TryParse(Console.ReadLine(), out uint count) && count != 0)
                {
                    return count;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введено некорректное количество\n");
                    Console.ResetColor();
                }
            }
        }

        //Редактирование данных рейса
        internal int Update()
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.WriteLine("Введите Id рейса, которого хотите изменить");
                string? key = Console.ReadLine();

                if (!int.TryParse(key, out int id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введены некорректные данные");
                    Console.ResetColor();

                    continue;
                }

                if (id == 0)
                {
                    return 0;
                }

                Flight? flight = database.Flights.FirstOrDefault(f => f.Id == id);
                if (flight == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Рейс с таким id не существует");
                    Console.ResetColor();

                    continue;
                }
                else
                {
                    return Change(flight);
                }
            }
        }

        //Выбор полей для редактирования рейса
        internal int Change(Flight flight)
        {
            while (true)
            {
                Console.WriteLine("\nВыберете, что хотите изменить");
                Console.WriteLine("1. Номер рейса");
                Console.WriteLine("2. Тип автобуса");
                Console.WriteLine("3. Пункт назначения");
                Console.WriteLine("4. Дата отправления");
                Console.WriteLine("5. Время отправления");
                Console.WriteLine("6. Время прибытия");
                Console.WriteLine("7. Стоимость билета");
                Console.WriteLine("8. Количество оставщихся билетов");
                Console.WriteLine("9. Количество проданных билетов");
                Console.WriteLine("0. Назад");

                string? key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        Console.Write("Введите номер рейса ");
                        string? number = Console.ReadLine();
                        flight.Number = number;
                        break;
                    case "2":
                        Console.Write("Введите тип автобуса ");
                        string? busType = Console.ReadLine();
                        flight.BusType = busType;
                        break;
                    case "3":
                        Console.Write("Введите пункт назначения ");
                        string? destination = Console.ReadLine();
                        flight.Destination = destination;
                        break;
                    case "4":
                        DateOnly date = GetDate();
                        flight.Date = date;
                        break;
                    case "5":
                        TimeOnly departuresTime = GetDeparturesTime();
                        flight.DeparturesTime = departuresTime;
                        break;
                    case "6":
                        TimeOnly arrivalTime = GetArrivalTime(flight.DeparturesTime);
                        flight.ArrivalTime = arrivalTime;
                        break;
                    case "7":
                        double cost = GetCost();
                        flight.Cost = cost;
                        break;
                    case "8":
                        uint left = GetCount();
                        flight.Left = left;
                        break;
                    case "9":
                        uint sold = GetCount();
                        flight.Sold = sold;
                        break;
                    case "0":
                        return 0;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Выбран несуществующий вариант");
                        Console.ResetColor();
                        break;
                }

                _ = database.Flights.Update(flight);
                _ = database.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Рейс изменен");
                Console.ResetColor();
            }
        }

        //Удаление существующего рейса
        internal int Delete()
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.WriteLine("Введите Id рейса, которого хотите удалить");
                string? key = Console.ReadLine();

                if (!int.TryParse(key, out int id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введены некорректные данные");
                    Console.ResetColor();

                    continue;
                }

                if (id == 0)
                {
                    return 0;
                }

                Flight? flight = database.Flights.FirstOrDefault(f => f.Id == id);
                if (flight == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Рейс с таким id не существует");
                    Console.ResetColor();

                    continue;
                }
                else
                {
                    Console.Write("Введите 1 если вы действительно хотите удалить рейс ");
                    if (Console.ReadLine() == "1")
                    {
                        _ = database.Flights.Remove(flight);
                        _ = database.SaveChanges();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Рейс удален");
                        Console.ResetColor();
                    }
                    return 0;
                }
            }
        }
    }
}
