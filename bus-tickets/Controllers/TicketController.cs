using bus_tickets.Models;

namespace bus_tickets.Controllers
{
    internal class TicketController
    {
        private readonly Database database;

        public TicketController(Database database)
        {
            this.database = database;
        }

        //Получение списка билетов
        internal void List()
        {
            List<Ticket> tickets = database.Tickets.OrderByDescending(t => t.Id).ToList();
            Print(tickets);
        }

        //Вывод списка билетов
        internal static void Print(List<Ticket> tickets)
        {
            if (tickets.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Билетов по вашему запросу не найдено");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(string.Format("{0,5} | {1,15} | {2,8} | {3,10} | {4,9}",
                    "Id ", "Id пользователя", "Id рейса", "Количество", "Стоимость"));

                foreach (Ticket ticket in tickets)
                {
                    Console.WriteLine(ticket);
                }
            }
        }

        //Получение списка билетов пользователя
        internal void List(int userId)
        {
            List<Ticket> tickets = database.Tickets.Where(t => t.UserId == userId).OrderByDescending(t => t.Id).ToList();
            List<Flight> flights = database.Flights.Where(f => tickets.Select(t => t.FlightId).Contains(f.Id)).ToList();
            Print(tickets, flights);
        }

        //Вывод списка билетов пользователя
        internal static void Print(List<Ticket> tickets, List<Flight> flights)
        {
            if (tickets.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Вы еще не приобрели ни одного билета");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(string.Format("{0,17} | {1,10} | {2,11} | {3,8} | {4,10} | {5,9} | {6,7}",
                   "Пункт назначения", "Дата   ", "Отправление", "Прибытие", "Количество", "Стоимость", "Итого "));

                foreach (Ticket ticket in tickets)
                {
                    Flight flight = flights.First(f => f.Id == ticket.FlightId);
                    Console.WriteLine(ticket.ToString(flight));
                }
            }
        }

        //Покупка билета пользователем
        internal int Buy(int userId)
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.WriteLine("Введите Id рейса, на который хотите купить билеты");
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
                    uint count = FlightController.GetCount();

                    if (flight.Left < count)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("На данный рейс осталось меньше билетов, чем вы хотите приобрести");
                        Console.ResetColor();

                        return 0;
                    }

                    if (flight.Date < DateOnly.FromDateTime(DateTime.Now) || (flight.Date == DateOnly.FromDateTime(DateTime.Now) && flight.DeparturesTime <= TimeOnly.FromDateTime(DateTime.Now)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Покупка билета на данный рейс недоступна, так как автобус уже в пути");
                        Console.ResetColor();

                        return 0;
                    }

                    Console.Write("Введите 1 если вы действительно хотите приобрести билеты ");
                    if (Console.ReadLine() == "1")
                    {
                        Ticket ticket = new()
                        {
                            UserId = userId,
                            FlightId = flight.Id,
                            Count = count,
                            Cost = flight.Cost * count
                        };
                        _ = database.Tickets.Add(ticket);

                        flight.Left -= count;
                        flight.Sold += count;
                        _ = database.Flights.Update(flight);

                        _ = database.SaveChanges();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Вы успешно приобрели билеты");
                        Console.ResetColor();
                    }
                    return 0;
                }
            }
        }
    }
}
