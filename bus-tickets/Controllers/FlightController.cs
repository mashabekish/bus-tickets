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

        internal void List()
        {
            Console.WriteLine(string.Format("{0,5} | {1,5} | {2,12} | {3,16} | {4,10} | {5,11} | {6,8} | {7,9} | {8,8} | {9,7}",
                "Id ", "Номер", "Тип автобуса", "Пункт назначения", "Дата   ", "Отправление", "Прибытие", "Стоимость", "Осталось", "Продано"));

            List<Flight> flights = database.Flights.OrderByDescending(f => f.Id).ToList();
            foreach (Flight flight in flights)
            {
                Console.WriteLine(flight);
            }
        }

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

            int count = GetCount();

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
            database.Flights.Add(flight);
            database.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Рейс добавлен");
            Console.ResetColor();

            return 0;
        }

        private DateOnly GetDate()
        {
            Console.Write("Введите дату отправления ");
            if (DateOnly.TryParse(Console.ReadLine(), out DateOnly date))
            {
                return date;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введена некорректная дата\n");
                Console.ResetColor();

                return GetDate();
            }
        }

        private TimeOnly GetDeparturesTime()
        {
            Console.Write("Введите время отправления ");
            if (TimeOnly.TryParse(Console.ReadLine(), out TimeOnly departuresTime))
            {
                return departuresTime;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введено некорректное время\n");
                Console.ResetColor();

                return GetDeparturesTime();
            }
        }

        private TimeOnly GetArrivalTime(TimeOnly departuresTime)
        {
            Console.Write("Введите время прибытия ");
            if (TimeOnly.TryParse(Console.ReadLine(), out TimeOnly arrivalTime))
            {
                if (arrivalTime < departuresTime)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Время прибытия не может быть меньше времени отправления\n");
                    Console.ResetColor();

                    return GetArrivalTime(departuresTime);
                }

                return arrivalTime;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введено некорректное время\n");
                Console.ResetColor();

                return GetArrivalTime(departuresTime);
            }
        }

        private double GetCost()
        {
            Console.Write("Введите стоимость билета ");
            if (double.TryParse(Console.ReadLine(), out double cost))
            {
                return cost;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введена некорректная стоимость\n");
                Console.ResetColor();

                return GetCost();
            }
        }

        internal static int GetCount()
        {
            Console.Write("Введите количество билетов ");
            if (int.TryParse(Console.ReadLine(), out int count))
            {
                return count;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введено некорректное количество\n");
                Console.ResetColor();

                return GetCount();
            }
        }

        internal int Update()
        {
            Console.WriteLine("\n0. Назад");
            Console.WriteLine("Введите Id рейса, которого хотите изменить");
            string? key = Console.ReadLine();

            if (!int.TryParse(key, out int id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введены некорректные данные");
                Console.ResetColor();

                return Update();
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

                return Update();
            }
            else
            {
                return Change(flight);
            }
        }

        internal int Change(Flight flight)
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
                    int left = GetCount();
                    flight.Left = left;
                    break;
                case "9":
                    int sold = GetCount();
                    flight.Sold = sold;
                    break;
                case "0":
                    return 0;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Выбран несуществующий вариант");
                    Console.ResetColor();
                    return Change(flight);
            }

            database.Flights.Update(flight);
            database.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Рейс изменен");
            Console.ResetColor();

            return Change(flight);
        }

        internal int Delete()
        {
            Console.WriteLine("\n0. Назад");
            Console.WriteLine("Введите Id рейса, которого хотите удалить");
            string? key = Console.ReadLine();

            if (!int.TryParse(key, out int id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введены некорректные данные");
                Console.ResetColor();

                return Delete();
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

                return Delete();
            }
            else
            {
                Console.Write("Введите 1 если вы действительно хотите удалить рейс ");
                if (Console.ReadLine() == "1")
                {
                    database.Flights.Remove(flight);
                    database.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Рейс удален");
                    Console.ResetColor();
                }
                return 0;
            }
        }
    }
}