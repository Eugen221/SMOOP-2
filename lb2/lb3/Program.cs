using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class FileRecord
{
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public int AccessCount { get; set; }
    public long SizeBytes { get; set; }

    public FileRecord(string name, DateTime created, int access, long size)
    {
        Name = name;
        Created = created;
        AccessCount = access;
        SizeBytes = size;
    }

    public override string ToString()
    {
        return $"{Name} | {Created:yyyy-MM-dd} | {AccessCount} | {SizeBytes} bytes";
    }
}

class PrintJob
{
    public string User { get; set; }
    public int Priority { get; set; }
    public DateTime Time { get; set; }
}

class Program
{
    static List<FileRecord> filesList = new List<FileRecord>();
    static List<PrintJob> printHistory = new List<PrintJob>();
    static Queue<PrintJob> normalQueue = new Queue<PrintJob>();
    static Queue<PrintJob> highQueue = new Queue<PrintJob>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== МЕНЮ ЛР2 ===");
            Console.WriteLine("1 — Завдання 1 (List<FileRecord>)");
            Console.WriteLine("2 — Завдання 2 (аналіз тексту)");
            Console.WriteLine("3 — Завдання 3 (черга друку)");
            Console.WriteLine("4 — Завдання 4 (словники)");
            Console.WriteLine("0 — Вийти");

            string choice = Console.ReadLine();
            if (choice == "0") break;

            switch (choice)
            {
                case "1": Task1(); break;
                case "2": Task2(); break;
                case "3": Task3(); break;
                case "4": Task4(); break;
            }
        }
    }

    // --------------------- ЗАВДАННЯ 1 ---------------------
    static void Task1()
    {
        while (true)
        {
            Console.WriteLine("\n--- Завдання 1 ---");
            Console.WriteLine("1 — додати запис");
            Console.WriteLine("2 — видалити");
            Console.WriteLine("3 — редагувати");
            Console.WriteLine("4 — виконати запит");
            Console.WriteLine("5 — показати всі");
            Console.WriteLine("0 — назад");

            string c = Console.ReadLine();
            if (c == "0") break;

            if (c == "1")
            {
                Console.Write("Name: "); string n = Console.ReadLine();
                Console.Write("Date yyyy-MM-dd: "); DateTime d = DateTime.Parse(Console.ReadLine());
                Console.Write("AccessCount: "); int a = int.Parse(Console.ReadLine());
                Console.Write("SizeBytes: "); long s = long.Parse(Console.ReadLine());
                filesList.Add(new FileRecord(n, d, a, s));
            }
            else if (c == "2")
            {
                Console.Write("Індекс: ");
                int i = int.Parse(Console.ReadLine());
                if (i >= 0 && i < filesList.Count) filesList.RemoveAt(i);
            }
            else if (c == "3")
            {
                Console.Write("Індекс: ");
                int i = int.Parse(Console.ReadLine());
                if (i >= 0 && i < filesList.Count)
                {
                    Console.Write("New Name: "); filesList[i].Name = Console.ReadLine();
                    Console.Write("New Date yyyy-MM-dd: "); filesList[i].Created = DateTime.Parse(Console.ReadLine());
                    Console.Write("New AccessCount: "); filesList[i].AccessCount = int.Parse(Console.ReadLine());
                    Console.Write("New SizeBytes: "); filesList[i].SizeBytes = long.Parse(Console.ReadLine());
                }
            }
            else if (c == "4")
            {
                Console.Write("Start date yyyy-MM-dd: ");
                DateTime start = DateTime.Parse(Console.ReadLine());
                Console.Write("End date yyyy-MM-dd: ");
                DateTime end = DateTime.Parse(Console.ReadLine());

                var res = filesList
                          .Where(f => f.Created >= start && f.Created <= end)
                          .OrderByDescending(f => f.AccessCount)
                          .ToList();

                foreach (var f in res) Console.WriteLine(f);

                File.WriteAllLines("task1_result.txt", res.Select(x => x.ToString()));
            }
            else if (c == "5")
            {
                for (int i = 0; i < filesList.Count; i++)
                    Console.WriteLine($"{i}: {filesList[i]}");
            }
        }
    }

    // --------------------- ЗАВДАННЯ 2 ---------------------
    static void Task2()
    {
        Console.Write("Введіть ім'я файлу firstFile: ");
        string listFile = Console.ReadLine();

        if (!File.Exists(listFile)) return;

        string[] filenames = File.ReadAllLines(listFile);

        while (true)
        {
            Console.WriteLine("\nФайли:");
            for (int i = 0; i < filenames.Length; i++)
                Console.WriteLine($"{i}: {filenames[i]}");

            Console.Write("Виберіть файл (-1 назад): ");
            int c = int.Parse(Console.ReadLine());
            if (c == -1) break;

            if (c < 0 || c >= filenames.Length) continue;

            string text = File.ReadAllText(filenames[c]);
            var words = text
                .ToLower()
                .Split(new[] { ' ', ',', '.', '!', '?', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            var stat = words
                .GroupBy(w => w)
                .Select(g => $"{g.Key}: {g.Count()}")
                .ToList();

            foreach (var s in stat) Console.WriteLine(s);

            Console.Write("Зберегти у файл? (y/n): ");
            if (Console.ReadLine() == "y")
            {
                File.WriteAllLines("task2_result.txt", stat);
            }
        }
    }

    // --------------------- ЗАВДАННЯ 3 ---------------------
    static void Task3()
    {
        while (true)
        {
            Console.WriteLine("\n--- Черга друку ---");
            Console.WriteLine("1 — додати задачу");
            Console.WriteLine("2 — надрукувати наступну");
            Console.WriteLine("3 — показати історію друку");
            Console.WriteLine("0 — назад");

            string c = Console.ReadLine();
            if (c == "0") break;

            if (c == "1")
            {
                Console.Write("User: ");
                string u = Console.ReadLine();
                Console.Write("Priority (0=normal, 1=high): ");
                int p = int.Parse(Console.ReadLine());

                var job = new PrintJob() { User = u, Priority = p, Time = DateTime.Now };

                if (p == 1) highQueue.Enqueue(job);
                else normalQueue.Enqueue(job);
            }
            else if (c == "2")
            {
                PrintJob job = null;

                if (highQueue.Count > 0) job = highQueue.Dequeue();
                else if (normalQueue.Count > 0) job = normalQueue.Dequeue();

                if (job != null)
                {
                    printHistory.Add(job);
                    Console.WriteLine($"Друк: {job.User} | {job.Priority} | {job.Time}");
                }
            }
            else if (c == "3")
            {
                foreach (var j in printHistory)
                    Console.WriteLine($"{j.User} | {j.Priority} | {j.Time}");

                File.WriteAllLines("task3_history.txt",
                    printHistory.Select(j => $"{j.User} | {j.Priority} | {j.Time}"));
            }
        }
    }

    // --------------------- ЗАВДАННЯ 4 ---------------------
    static Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

    static void Task4()
    {
        while (true)
        {
            Console.WriteLine("\n--- Словники ---");
            Console.WriteLine("1 — створити словник");
            Console.WriteLine("2 — додати слово");
            Console.WriteLine("3 — замінити слово або переклад");
            Console.WriteLine("4 — видалити слово або переклад");
            Console.WriteLine("5 — пошук перекладу");
            Console.WriteLine("6 — експортувати слово");
            Console.WriteLine("0 — назад");

            string c = Console.ReadLine();
            if (c == "0") break;

            if (c == "1") dict.Clear();

            else if (c == "2")
            {
                Console.Write("Слово: "); string w = Console.ReadLine();
                Console.Write("Переклад: "); string t = Console.ReadLine();

                if (!dict.ContainsKey(w)) dict[w] = new List<string>();
                dict[w].Add(t);
            }
            else if (c == "3")
            {
                Console.Write("Слово: "); string w = Console.ReadLine();
                if (!dict.ContainsKey(w)) continue;

                Console.Write("1=замінити слово, 2=замінити переклад: ");
                string mode = Console.ReadLine();

                if (mode == "1")
                {
                    Console.Write("Нове слово: ");
                    string nw = Console.ReadLine();
                    dict[nw] = dict[w];
                    dict.Remove(w);
                }
                else
                {
                    Console.Write("Старий переклад: ");
                    string o = Console.ReadLine();
                    Console.Write("Новий переклад: ");
                    string n = Console.ReadLine();

                    if (dict[w].Contains(o))
                    {
                        int i = dict[w].IndexOf(o);
                        dict[w][i] = n;
                    }
                }
            }
            else if (c == "4")
            {
                Console.Write("Слово: "); string w = Console.ReadLine();
                if (!dict.ContainsKey(w)) continue;

                Console.Write("1=видалити слово, 2=видалити переклад: ");
                string m = Console.ReadLine();

                if (m == "1") dict.Remove(w);

                else
                {
                    Console.Write("Переклад: "); string t = Console.ReadLine();
                    if (dict[w].Count > 1) dict[w].Remove(t);
                }
            }
            else if (c == "5")
            {
                Console.Write("Слово: ");
                string w = Console.ReadLine();

                if (dict.ContainsKey(w))
                    foreach (var t in dict[w]) Console.WriteLine(t);
            }
            else if (c == "6")
            {
                Console.Write("Слово: "); string w = Console.ReadLine();
                if (dict.ContainsKey(w))
                    File.WriteAllLines("task4_export.txt", dict[w]);
            }
        }
    }
}
