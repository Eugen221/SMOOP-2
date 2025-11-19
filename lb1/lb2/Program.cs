using System;
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
        return $"{Name} | Створено: {Created:yyyy-MM-dd} | Звернення: {AccessCount} | Розмір: {SizeBytes} байт";
    }
}

class Program
{
    static void Main()
    {
       //МАСИВ
        FileRecord[] files =
        {
            new FileRecord("report.docx", new DateTime(2023, 4, 10), 12, 24000),
            new FileRecord("data.db", new DateTime(2022, 11, 5), 45, 15000000),
            new FileRecord("notes.txt", new DateTime(2024, 1, 20), 7, 900),
            new FileRecord("archive.zip", new DateTime(2021, 6, 1), 3, 980000000),
            new FileRecord("presentation.pptx", new DateTime(2023, 12, 12), 20, 5242880)
        };

        Console.WriteLine("Введіть дату початку (yyyy-MM-dd):");
        DateTime start = DateTime.Parse(Console.ReadLine());

        Console.WriteLine("Введіть дату кінця (yyyy-MM-dd):");
        DateTime end = DateTime.Parse(Console.ReadLine());

       
        var result =
            files
            .Where(f => f.Created >= start && f.Created <= end)
            .OrderByDescending(f => f.AccessCount)
            .ToArray();

        Console.WriteLine("\nФайли у заданому часовому діапазоні:");
        if (result.Length == 0)
        {
            Console.WriteLine("Немає файлів у цьому діапазоні.");
        }
        else
        {
            foreach (var f in result)
                Console.WriteLine(f);
        }
    }
}
