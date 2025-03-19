using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

// Interface Segregation Principle (ISP) - Separate interfaces for event management and data saving
public interface IEventService
{
    void AddEvent(EventDetails eventDetails);
    IEnumerable<EventDetails> GetAllEvents();
}

public interface IDataSaver
{
    void SaveData(List<EventDetails> events);
}

// Single Responsibility Principle (SRP) - Defines event properties
public class EventDetails
{
    public string Name { get; set; }
    public string Date { get; set; }
    public string Location { get; set; }

    public EventDetails(string name, string date, string location)
    {
        Name = name;
        Date = date;
        Location = location;
    }
}

// SRP - Manages event operations separately
public class EventService : IEventService
{
    private readonly List<EventDetails> _events = new List<EventDetails>();

    public void AddEvent(EventDetails eventDetails)
    {
        _events.Add(eventDetails);
        Console.WriteLine("Event added successfully!");
    }

    public IEnumerable<EventDetails> GetAllEvents()
    {
        return _events;
    }
}

// Open/Closed Principle (OCP) - Allows different ways to save data without modifying existing code
public class FileSaver : IDataSaver
{
    public void SaveData(List<EventDetails> events)
    {
        Console.WriteLine("Saving event data to a file...");
    }
}

public class DatabaseSaver : IDataSaver
{
    public void SaveData(List<EventDetails> events)
    {
        Console.WriteLine("Saving event data to a database...");
    }
}

// Dependency Inversion Principle (DIP) - High-level module depends on abstractions
public class EventApplication
{
    private readonly IEventService _eventService;
    private readonly IDataSaver _dataSaver;

    public EventApplication(IEventService eventService, IDataSaver dataSaver)
    {
        _eventService = eventService;
        _dataSaver = dataSaver;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n1. Add Event\n2. View Events\n3. Save & Exit");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter Event Name: ");
                string name = Console.ReadLine();
                Console.Write("Enter Event Date: ");
                string date = Console.ReadLine();
                Console.Write("Enter Event Location: ");
                string location = Console.ReadLine();

                _eventService.AddEvent(new EventDetails(name, date, location));
            }
            else if (choice == "2")
            {
                Console.WriteLine("\nEvent List:");
                foreach (var evt in _eventService.GetAllEvents())
                {
                    Console.WriteLine($"Name: {evt.Name}, Date: {evt.Date}, Location: {evt.Location}");
                }
            }
            else if (choice == "3")
            {
                _dataSaver.SaveData(new List<EventDetails>(_eventService.GetAllEvents()));
                Console.WriteLine("Data saved. Exiting...");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice, try again.");
            }
        }
    }
}

// Main Program
class Program
{
    static void Main()
    {
        IEventService eventService = new EventService();
        IDataSaver dataSaver = new FileSaver(); // Change to DatabaseSaver if needed

        EventApplication app = new EventApplication(eventService, dataSaver);
        app.Run();
    }
}

