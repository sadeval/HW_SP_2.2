using System;
using System.Collections.Generic;
using System.Threading;

class Customer
{
    public int Id { get; private set; }

    public Customer(int id)
    {
        Id = id;
    }
}

class CashRegister
{
    private int _id;
    private Queue<Customer> _queue;
    private static Random _random = new Random();

    public CashRegister(int id, Queue<Customer> queue)
    {
        _id = id;
        _queue = queue;
    }

    public void ProcessQueue()
    {
        while (_queue.Count > 0)
        {
            Customer customer = _queue.Dequeue();

            Console.WriteLine($"Касса {_id}: Начинается обслуживание покупателя {customer.Id}.");

            int processingTime = _random.Next(1000, 3000);
            Thread.Sleep(processingTime);

            Console.WriteLine($"Касса {_id}: Завершено обслуживание покупателя {customer.Id} (время обработки: {processingTime / 1000} секунд).");
        }

        Console.WriteLine($"Касса {_id} завершила работу.");
    }
}

class Program
{
    static void Main()
    {
        Queue<Customer> customerQueue = new Queue<Customer>();
        for (int i = 1; i <= 20; i++)
        {
            customerQueue.Enqueue(new Customer(i));
        }

        List<Queue<Customer>> dividedQueues = DivideQueue(customerQueue, 3);
        List<Thread> cashRegisters = new List<Thread>();

        for (int i = 0; i < dividedQueues.Count; i++)
        {
            CashRegister register = new CashRegister(i + 1, dividedQueues[i]);
            Thread thread = new Thread(register.ProcessQueue);
            cashRegisters.Add(thread);
        }

        foreach (Thread cashRegister in cashRegisters)
        {
            cashRegister.Start();
        }

        foreach (Thread cashRegister in cashRegisters)
        {
            cashRegister.Join();
        }

        Console.WriteLine("Все кассы завершили работу.");
    }

    static List<Queue<Customer>> DivideQueue(Queue<Customer> customers, int numberOfRegisters)
    {
        List<Queue<Customer>> dividedQueues = new List<Queue<Customer>>();

        for (int i = 0; i < numberOfRegisters; i++)
        {
            dividedQueues.Add(new Queue<Customer>());
        }

        int index = 0;
        while (customers.Count > 0)
        {
            dividedQueues[index].Enqueue(customers.Dequeue());
            index = (index + 1) % numberOfRegisters;
        }

        return dividedQueues;
    }
}
