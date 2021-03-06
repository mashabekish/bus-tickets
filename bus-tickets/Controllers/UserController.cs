using bus_tickets.Models;

namespace bus_tickets.Controllers
{
    internal class UserController
    {
        private readonly Database database;

        public UserController(Database database)
        {
            this.database = database;
        }

        //Получение данных пользователя
        internal User GetUser()
        {
            while (true)
            {
                Console.WriteLine("\n1. Авторизация");
                Console.WriteLine("2. Регистрация");
                Console.WriteLine("3. Выход");

                string? key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        User? user = Authenticate();
                        if (user != null)
                        {
                            return user;
                        }
                        break;
                    case "2":
                        _ = Register();
                        break;
                    case "3":
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

        //Получение пароля и его отображение с заменой всех символов на "*"
        private static string GetPassword()
        {
            Console.Write("Введите пароль ");
            string password = "";
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Remove(password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password += i.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();

            return password;
        }

        //Аутентификация пользователя
        private User? Authenticate()
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.Write("Введите логин ");
                string? login = Console.ReadLine();

                if (login == "0")
                {
                    return null;
                }

                string password = GetPassword();

                User? user = database.Users.FirstOrDefault(u => u.Login == login);
                if (user == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пользователя с таким логином не существует");
                    Console.ResetColor();

                    continue;
                }
                else if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный пароль");
                    Console.ResetColor();

                    continue;
                }
                else if (!user.IsActive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пользователь не активирован");
                    Console.ResetColor();

                    continue;
                }
                else
                {
                    return user;
                }
            }
        }

        //Регистрация пользователя
        private int Register()
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.Write("Введите логин ");
                string? login = Console.ReadLine();

                if (login == "0")
                {
                    return 0;
                }

                if (database.Users.Where(u => u.Login == login).Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пользователь с таким логином уже существует");
                    Console.ResetColor();

                    continue;
                }

                string password = GetPassword();

                User user = new()
                {
                    Login = login,
                    Password = BCrypt.Net.BCrypt.HashPassword(password),
                    IsAdmin = false,
                    IsActive = false
                };
                _ = database.Users.Add(user);
                _ = database.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Пользователь успешно загеристрирован, дождитесь подтверждения администратора");
                Console.ResetColor();

                return 0;
            }
        }

        //Вывод списка пользователей
        internal void List()
        {
            Console.WriteLine(string.Format("{0,5} | {1,9} | {2,8} | {3,5} | {4,11}",
                "Id ", "Логин  ", "Пароль ", "Админ", "Подтвержден"));

            List<User> users = database.Users.ToList();
            foreach (User user in users)
            {
                Console.WriteLine(user);
            }
        }

        //Добавление нового пользователя
        internal int Create()
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.Write("Введите логин ");
                string? login = Console.ReadLine();

                if (login == "0")
                {
                    return 0;
                }

                if (database.Users.Where(u => u.Login == login).Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пользователь с таким логином уже существует");
                    Console.ResetColor();

                    continue;
                }

                string password = GetPassword();

                Console.Write("Введите 1 если пользователь - администратор ");
                string? key = Console.ReadLine();

                bool isAdmin = false;
                if (key == "1")
                {
                    isAdmin = true;
                }

                User user = new()
                {
                    Login = login,
                    Password = BCrypt.Net.BCrypt.HashPassword(password),
                    IsAdmin = isAdmin,
                    IsActive = true
                };
                _ = database.Users.Add(user);
                _ = database.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Пользователь добавлен");
                Console.ResetColor();

                return 0;
            }
        }

        //Редактирование учетной записи пользователя
        internal int Update(int adminId)
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.WriteLine("Введите Id пользователя, которого хотите изменить");
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

                User? user = database.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Учетная запись с таким id не существует");
                    Console.ResetColor();

                    continue;
                }
                else
                {
                    return Change(adminId, user);
                }
            }
        }

        //Выбор данных для редактирования учетной записи
        internal int Change(int adminId, User user)
        {
            while (true)
            {
                Console.WriteLine("\nВыберете, что хотите изменить");
                Console.WriteLine("1. Логин");
                Console.WriteLine("2. Пароль");
                Console.WriteLine("3. Роль");
                Console.WriteLine("0. Назад");

                string? key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        Console.Write("Введите логин ");
                        string? login = Console.ReadLine();
                        user.Login = login;
                        break;
                    case "2":
                        string password = GetPassword();
                        user.Password = password;
                        break;
                    case "3":
                        if (adminId == user.Id)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Нельзя изменить свою роль");
                            Console.ResetColor();

                            continue;
                        }

                        Console.Write("Введите 1 если пользователь - администратор ");
                        bool isAdmin = false;
                        if (Console.ReadLine() == "1")
                        {
                            isAdmin = true;
                        }
                        user.IsAdmin = isAdmin;
                        break;
                    case "0":
                        return 0;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Выбран несуществующий вариант");
                        Console.ResetColor();

                        continue;
                }

                _ = database.Users.Update(user);
                _ = database.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Пользователь изменен");
                Console.ResetColor();
            }
        }

        //Удаление существующего пользователя
        internal int Delete(int adminId)
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.WriteLine("Введите Id пользователя, которого хотите удалить");
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

                if (adminId == id)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Нельзя удалить свою учетную запись");
                    Console.ResetColor();

                    continue;
                }

                User? user = database.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Учетная запись с таким id не существует");
                    Console.ResetColor();

                    continue;
                }
                else
                {
                    Console.Write("Введите 1 если вы действительно хотите удалить учетную запись ");
                    if (Console.ReadLine() == "1")
                    {
                        _ = database.Users.Remove(user);
                        _ = database.SaveChanges();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Пользователь удален");
                        Console.ResetColor();
                    }
                    return 0;
                }
            }
        }

        //Активация администратором учетной записи пользователя
        internal int Activate()
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.WriteLine("Введите Id пользователя, которого хотите подтвердить");
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

                User? user = database.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Учетная запись с таким id не существует");
                    Console.ResetColor();

                    continue;
                }
                else if (user.IsActive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пользователь уже подтвержден");
                    Console.ResetColor();

                    continue;
                }
                else
                {
                    user.IsActive = true;
                    _ = database.Users.Update(user);
                    _ = database.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Пользователь подтвержден");
                    Console.ResetColor();

                    return 0;
                }
            }
        }

        //Блокировка администратором учетной записи пользователя
        internal int Deactivate(int adminId)
        {
            while (true)
            {
                Console.WriteLine("\n0. Назад");
                Console.WriteLine("Введите Id пользователя, которого хотите заблокировать");
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

                if (adminId == id)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Нельзя заблокировать свою учетную запись");
                    Console.ResetColor();

                    continue;
                }

                User? user = database.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Учетная запись с таким id не существует");
                    Console.ResetColor();

                    continue;
                }
                else if (!user.IsActive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пользователь уже заблокирован");
                    Console.ResetColor();

                    continue;
                }
                else
                {
                    user.IsActive = false;
                    _ = database.Users.Update(user);
                    _ = database.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Пользователь заблокирован");
                    Console.ResetColor();

                    return 0;
                }
            }
        }
    }
}
