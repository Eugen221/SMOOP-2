using System;
using System.Collections.Generic;
using System.Linq;

public class Firm
{
    public string Name { get; set; }
    public DateTime Founded { get; set; }
    public string Profile { get; set; }
    public string Director { get; set; }
    public int Employees { get; set; }
    public string Address { get; set; }

    public override string ToString() => $"{Name} ({Founded:yyyy}) — {Profile}, {Employees} чел., {Director}";
}

public class Phone
{
    public string Model { get; set; }
    public string Vendor { get; set; }
    public decimal Price { get; set; }
    public DateTime Release { get; set; }

    public override string ToString() => $"{Vendor} {Model} — ${Price} ({Release:yyyy-MM-dd})";
}

public class Employer
{
    public string Name { get; set; }
    public DateTime Birth { get; set; }
    public DateTime WorkStart { get; set; }
    public bool HigherEducation { get; set; }
    public decimal Salary { get; set; }

    public override string ToString() => $"{Name}, {Birth:yyyy-MM-dd}, ЗП: {Salary}";
}

public class President : Employer { }
public class Manager : Employer { }
public class Worker : Employer { }

public class Company
{
    public string Name { get; set; }
    public President President { get; set; }
    public List<Manager> Managers { get; set; } = new List<Manager>();
    public List<Worker> Workers { get; set; } = new List<Worker>();
}

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== LINQ ПРАКТИКА ===");
            Console.WriteLine("1 — Завдання 1 (Фірми)");
            Console.WriteLine("2 — Завдання 2 (Телефони)");
            Console.WriteLine("3 — Завдання 3 (Компанія та працівники)");
            Console.WriteLine("0 — Вихід");
            Console.Write("\nВиберіть завдання: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": Task1(); break;
                case "2": Task2(); break;
                case "3": Task3(); break;
                case "0": return;
                default:
                    Console.WriteLine("Невірний вибір! Натисніть будь-яку клавішу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void Task1()
    {
        var now = DateTime.Now;

        List<Firm> firms = new List<Firm>()
        {
            new Firm(){ Name="FoodLine", Founded=new DateTime(2019,4,1), Profile="Marketing", Director="John White", Employees=150, Address="London"},
            new Firm(){ Name="TechSoft", Founded=new DateTime(2020,1,15), Profile="IT", Director="Mark Black", Employees=250, Address="Berlin"},
            new Firm(){ Name="FoodMaster", Founded=new DateTime(2018,5,20), Profile="Food", Director="Anna Smith", Employees=80, Address="Paris"},
            new Firm(){ Name="MarketPro", Founded=new DateTime(2021,8,10), Profile="Marketing", Director="Peter White", Employees=300, Address="London"}
        };

        Console.Clear();
        Console.WriteLine("=== ЗАВДАННЯ 1 — ФІРМИ ===\n");

        Console.WriteLine("q2: Фірми з 'Food' у назві");
        foreach (var f in firms.Where(f => f.Name.Contains("Food"))) Console.WriteLine("  " + f);

        Console.WriteLine("\nq3: Тільки Marketing");
        foreach (var f in firms.Where(f => f.Profile == "Marketing")) Console.WriteLine("  " + f);

        Console.WriteLine("\nq7: Фірми в Лондоні");
        foreach (var f in firms.Where(f => f.Address == "London")) Console.WriteLine("  " + f);

        Console.WriteLine("\nq9: Існують більше 2 років");
        foreach (var f in firms.Where(f => (now - f.Founded).TotalDays > 365 * 2)) Console.WriteLine("  " + f);

        Wait();
    }

    static void Task2()
    {
        List<Phone> phones = new List<Phone>()
        {
            new Phone(){ Model="iPhone 13", Vendor="Apple", Price=900m, Release=new DateTime(2021,9,24)},
            new Phone(){ Model="Galaxy S22", Vendor="Samsung", Price=850m, Release=new DateTime(2022,2,15)},
            new Phone(){ Model="iPhone 10", Vendor="Apple", Price=500m, Release=new DateTime(2017,11,3)},
            new Phone(){ Model="Xperia Z5", Vendor="Sony", Price=450m, Release=new DateTime(2015,10,2)},
            new Phone(){ Model="Galaxy A52", Vendor="Samsung", Price=400m, Release=new DateTime(2021,3,17)},
            new Phone(){ Model="Nokia 3310", Vendor="Nokia", Price=59m, Release=new DateTime(2000,9,1)}
        };

        Console.Clear();
        Console.WriteLine("=== ЗАВДАННЯ 2 — ТЕЛЕФОНИ ===\n");

        Console.WriteLine($"p1: Всього телефонів: {phones.Count()}");
        Console.WriteLine($"p5: Найдешевший: {phones.MinBy(p => p.Price)}");
        Console.WriteLine($"p6: Найдорожчий: {phones.MaxBy(p => p.Price)}");
        Console.WriteLine($"p9: Середня ціна: {phones.Average(p => p.Price):F2} $");

        Console.WriteLine("\np10: Топ-3 найдорожчих:");
        foreach (var p in phones.OrderByDescending(p => p.Price).Take(3))
            Console.WriteLine("  " + p);

        Console.WriteLine("\np14: Кількість телефонів по виробниках:");
        foreach (var g in phones.GroupBy(p => p.Vendor).Select(g => new { g.Key, Count = g.Count() }).OrderByDescending(x => x.Count))
            Console.WriteLine($"  {g.Key}: {g.Count}");

        Wait();
    }

    static void Task3()
    {
        var now = DateTime.Now;

        var comp = new Company
        {
            Name = "SuperCorp",
            President = new President { Name = "Ivan Petrov", Birth = new DateTime(1975, 5, 12), WorkStart = new DateTime(2000, 1, 10), Salary = 25000m }
        };

        comp.Managers.AddRange(new[]
        {
            new Manager { Name="Oleg", Birth=new DateTime(1980,1,1), WorkStart=new DateTime(2005,5,10), Salary=12000m },
            new Manager { Name="Yuriy", Birth=new DateTime(1995,6,10), WorkStart=new DateTime(2020,1,1), Salary=10000m }
        });

        comp.Workers.AddRange(new[]
        {
            new Worker { Name="Vladimir", Birth=new DateTime(2001,10,10), WorkStart=new DateTime(2019,1,10), HigherEducation=true, Salary=8000m },
            new Worker { Name="Vladimir", Birth=new DateTime(1999,10,15), WorkStart=new DateTime(2017,1,10), HigherEducation=true, Salary=8500m },
            new Worker { Name="Oksana", Birth=new DateTime(1990,10,20), WorkStart=new DateTime(2010,2,10), HigherEducation=false, Salary=9000m },
            new Worker { Name="Anton", Birth=new DateTime(1985,3,15), WorkStart=new DateTime(2005,3,5), HigherEducation=true, Salary=9500m },
            new Worker { Name="Marina", Birth=new DateTime(1993,7,1), WorkStart=new DateTime(2016,6,1), HigherEducation=true, Salary=7500m }
        });

        Console.Clear();
        Console.WriteLine("=== ЗАВДАННЯ 3 — КОМПАНІЯ ===\n");

        Console.WriteLine($"Кількість працівників: {comp.Workers.Count}");
        Console.WriteLine($"Сума зарплат працівників: {comp.Workers.Sum(w => w.Salary)} грн");

        var longestWorker = comp.Workers
            .OrderByDescending(w => (now - w.WorkStart).TotalDays)
            .First();

        Console.WriteLine($"\nПрацює найдовше: {longestWorker.Name} (з {longestWorker.WorkStart:yyyy-MM-dd})");

        Console.WriteLine("\nПрацівники, що народились у жовтні:");
        foreach (var w in comp.Workers.Where(w => w.Birth.Month == 10).OrderBy(w => w.Name))
            Console.WriteLine("  " + w);

        var youngestVladimir = comp.Workers
            .Where(w => w.Name == "Vladimir")
            .OrderBy(w => w.Birth)
            .FirstOrDefault();

        if (youngestVladimir != null)
            Console.WriteLine($"\nНаймолодший Володимир: {youngestVladimir.Name}, премія = {youngestVladimir.Salary / 3m} грн");

        Wait();
    }

    static void Wait()
    {
        Console.WriteLine("\nНатисніть будь-яку клавішу для повернення в меню...");
        Console.ReadKey();
    }
}