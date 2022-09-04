Dictionary<string, int> getFrequencies(List<string> values)
{
    Dictionary<string, int> result = new Dictionary<string, int>();
    foreach (string value in values)
    {
        if (result.TryGetValue(value, out int count)) result[value] = count + 1;
        else result.Add(value, 1);
    }

    return result;
}

Order selectOrder()
{
    string[] initialToppings = Globals.toppings;
    int choiceCount = 0;
    Console.Write($"Choose a pizza size ({String.Join(", ", Globals.sizes)}): ");
    string size = Console.ReadLine()!.ToLower();

    while (!Globals.sizes.Contains(size))
    {
        Console.Write($"Choose a valid pizza size ({String.Join(", ", Globals.sizes)}): ");
        size = Console.ReadLine()!.ToLower();
    }

    choiceCount++;

    Console.Write($"Choose a pizza base ({String.Join(", ", Globals.bases)}): ");
    string baseType = Console.ReadLine()!.ToLower();

    while (!Globals.bases.Contains(baseType))
    {
        Console.Write($"Choose a valid pizza base ({String.Join(", ", Globals.bases)}): ");
        baseType = Console.ReadLine()!.ToLower();
    }

    choiceCount++;

    Console.Write("How many extra toppings do you want? (Max. 3): ");
    int i;
    int extraToppingsAmount;
    string toppingsInput = Console.ReadLine()!;
    bool isNumber = int.TryParse(toppingsInput, out i);

    while (!isNumber)
    {
        Console.Write("Please enter a valid number of toppings: ");
        toppingsInput = Console.ReadLine()!;
        isNumber = int.TryParse(toppingsInput, out i);
    }

    extraToppingsAmount = int.Parse(toppingsInput);

    while (extraToppingsAmount > 3 || extraToppingsAmount <= 0)
    {
        Console.Write("Please enter a number less than 3 and greater than 0: ");
        toppingsInput = Console.ReadLine()!;
        isNumber = int.TryParse(toppingsInput!, out i);

        while (!isNumber)
        {
            Console.Write("Please enter a valid number of toppings: ");
            toppingsInput = Console.ReadLine()!;
            isNumber = int.TryParse(toppingsInput, out i);
        }

        extraToppingsAmount = int.Parse(toppingsInput);
    }

    choiceCount++;

    List<string> extraToppings = new List<string>();

    for (int index = 0; index < extraToppingsAmount; index++)
    {
        if (index == 0) Console.Write($"Choose an extra topping ({String.Join(", ", Globals.toppings)}): ");
        else Console.Write($"Choose another extra topping ({String.Join(", ", Globals.toppings)}): ");
        string topping = Console.ReadLine()!.ToLower();

        while (!Globals.toppings.Contains(topping))
        {
            Console.Write($"Choose a valid topping ({String.Join(", ", Globals.toppings)}): ");
            topping = Console.ReadLine()!.ToLower();
        }

        Globals.toppings = Globals.toppings.Where(t => t != topping).ToArray();
        extraToppings.Add(topping);
        choiceCount++;
    }

    Globals.chosenToppings = Globals.chosenToppings.Concat(extraToppings).ToList();
    Globals.toppings = initialToppings;

    return new Order(size, baseType, extraToppings, choiceCount);
}

List<Order> orderPizzas()
{
    List<Order> orders = new List<Order>();
    int orderNumber = int.Parse(new Random().Next(0, 1000000).ToString("D6"));

    Console.Write("How many pizzas do you want to order?: ");
    int i;
    int pizzaAmount;
    string pizzasInput = Console.ReadLine()!;
    bool isNumber = int.TryParse(pizzasInput, out i);

    while (!isNumber)
    {
        Console.Write("Please enter a valid number of pizzas: ");
        pizzasInput = Console.ReadLine()!;
        isNumber = int.TryParse(pizzasInput, out i);
    }

    pizzaAmount = int.Parse(pizzasInput);

    while (pizzaAmount <= 0)
    {
        Console.Write("Please enter a number greater than 0: ");
        pizzasInput = Console.ReadLine()!;
        isNumber = int.TryParse(pizzasInput, out i);

        while (!isNumber)
        {
            Console.Write("Please enter a number greater than 0: ");
            pizzasInput = Console.ReadLine()!;
            isNumber = int.TryParse(pizzasInput, out i);
        }

        pizzaAmount = int.Parse(pizzasInput);
    }

    for (int index = 0; index < pizzaAmount; index++)
    {
        Order order = selectOrder();
        orders.Add(order);
    }

    Console.Write("Would you like to confirm, alter or cancel this order?: ");
    string confirm = Console.ReadLine()!.ToLower();

    while (!Globals.confirmOptions.Contains(confirm))
    {
        Console.Write($"Please enter a valid option ({String.Join(", ", Globals.confirmOptions)}): ");
        confirm = Console.ReadLine()!.ToLower();
    }

    switch (confirm)
    {
        case "confirm":
            Console.WriteLine($"Succesfully created order! Your order number is {orderNumber}.");
            return orders;
        case "alter":
            Console.WriteLine("Restarting order process...");
            return orderPizzas();
        case "cancel":
            Console.WriteLine("Cancelling order...");
            return new List<Order>();
        default:
            return new List<Order>();
    }
}

List<Order> orders = orderPizzas();

if (orders.Count() > 0)
{
    Topping mostPopularTopping = new Topping(getFrequencies(Globals.chosenToppings).First().Key, getFrequencies(Globals.chosenToppings).First().Value);
    Topping leastPopularTopping = new Topping(getFrequencies(Globals.chosenToppings).Last().Key, getFrequencies(Globals.chosenToppings).Last().Value);

    Console.WriteLine($"The most popular topping was {mostPopularTopping.Name} with a popularity of {mostPopularTopping.Percentage}%.");
    Console.WriteLine($"The least popular topping was {leastPopularTopping.Name} with a popularity of {leastPopularTopping.Percentage}%.");
}

public class Topping
{
    public string Name;
    public int Count;
    public int Percentage;

    public Topping(string name, int count)
    {
        this.Name = name;
        this.Count = count;
        this.Percentage = (int) (((float) this.Count / (float) Globals.chosenToppings.Count()) * 100);
    }
}

public static class Globals
{
    public static string[] sizes = new string[] {
        "small",
        "medium",
        "large"
    };
    public static string[] bases = new string[] {
        "thin",
        "thick"
    };

    public static string[] toppings = new string[] {
        "pepperoni",
        "chicken",
        "extra cheese",
        "mushrooms",
        "spinach",
        "olives"
    };

    public static string[] confirmOptions = new string[] {
        "confirm",
        "alter",
        "cancel"
    };

    public static List<string> chosenToppings = new List<string>();
}


public class Order
{
    public string Size;
    public string BaseType;
    public List<string> Toppings;
    public int ChoiceCount;

    public Order(string size, string baseType, List<string> toppings, int choiceCount)
    {
        this.Size = size;
        this.BaseType = baseType;
        this.Toppings = toppings;
        this.ChoiceCount = choiceCount;
    }
}