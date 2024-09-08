using System;
using System.Collections.Generic;

abstract class Delivery
{
    private string address;

    public string Address
    {
        get => address;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Адрес не может быть пустым");
            }
            address = value;
        }
    }

    protected Delivery(string address)
    {
        Address = address;
    }

    public abstract void DisplayDeliveryInfo();
}

class HomeDelivery : Delivery
{
    public string ContactPerson { get; set; }

    public HomeDelivery(string address, string contactPerson) : base(address)
    {
        ContactPerson = contactPerson;
    }

    public override void DisplayDeliveryInfo()
    {
        Console.WriteLine($"Домашняя доставка на {Address}, Контакты: {ContactPerson}");
    }
}

class PickPointDelivery : Delivery
{
    public int PickPointId { get; set; }

    public PickPointDelivery(string address, int pickPointId) : base(address)
    {
        PickPointId = pickPointId;
    }
    public override void DisplayDeliveryInfo()
    {
        Console.WriteLine($"Доставка в пункт выдачи: {Address}, ПВЗ ID: {PickPointId}");
    }
}

class ShopDelivery : Delivery
{
    public string ShopName { get; set; }

    public ShopDelivery(string address, string shopName) : base(address)
    {
        ShopName = shopName;
    }
    public override void DisplayDeliveryInfo()
    {
        Console.WriteLine($"Доставка в магазин {Address}, Магазин: {ShopName}");
    }
}

class Product
{
    public string Name { get; set; } 
    public decimal Price { get; set; }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

class Order<TDelivery> where TDelivery : Delivery
{
    public TDelivery Delivery { get; set; }
    public int Number { get; set; }
    public string Description { get; set; } 
    private List<Product> Products { get; } = new List<Product>(); 

    public Order(TDelivery delivery, int number, string description)
    {
        Delivery = delivery;
        Number = number;
        Description = description;
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public void DisplayOrderInfo()
    {
        Console.WriteLine($"Заказ #{Number}: {Description}");
        Delivery.DisplayDeliveryInfo();
        Console.WriteLine("Товары:");
        foreach (var product in Products)
        {
            Console.WriteLine($"- {product.Name}: {product.Price:C}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var homeDelivery = new HomeDelivery("Сибирская 21, Москва", "Иван Петров");

        var homeOrder = new Order<HomeDelivery>(homeDelivery, 1, "Заказ на домашнюю доставку");
        homeOrder.AddProduct(new Product("Принтер", 3222.99m));
        homeOrder.AddProduct(new Product("Фен", 1000.50m));
        homeOrder.DisplayOrderInfo();

        Console.WriteLine();

        var pickPointDelivery = new PickPointDelivery("Пункт выдачи, Сибирская", 101);
        
        var pickPointOrder = new Order<PickPointDelivery>(pickPointDelivery, 2, "Заказ в ПВЗ");
        pickPointOrder.AddProduct(new Product("Кофеварка", 2999.99m));
        pickPointOrder.DisplayOrderInfo(); 

        Console.WriteLine();

        var shopDelivery = new ShopDelivery("Сибирская 45", "Магазин электроники");

        var shopOrder = new Order<ShopDelivery>(shopDelivery, 3, "Заказ в розничный магазин");
        shopOrder.AddProduct(new Product("Ноутбук", 122150.75m));
        shopOrder.DisplayOrderInfo();
    }
}