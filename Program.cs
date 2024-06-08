namespace ConsoleApp35
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 13, 21, 34, 55 };
            Console.WriteLine("Even Numbers: " + string.Join(", ", ArrayMethods.GetEvenNumbers(numbers)));
            Console.WriteLine("Odd Numbers: " + string.Join(", ", ArrayMethods.GetOddNumbers(numbers)));
            Console.WriteLine("Prime Numbers: " + string.Join(", ", ArrayMethods.GetPrimeNumbers(numbers)));
            Console.WriteLine("Fibonacci Numbers: " + string.Join(", ", ArrayMethods.GetFibonacciNumbers(numbers)));

            UtilityMethods.ShowCurrentTime();
            UtilityMethods.ShowCurrentDate();
            UtilityMethods.ShowCurrentDayOfWeek();
            double triangleArea = UtilityMethods.CalculateTriangleArea(10, 5);
            double rectangleArea = UtilityMethods.CalculateRectangleArea(10, 5);
            Console.WriteLine($"Triangle Area: {triangleArea}");
            Console.WriteLine($"Rectangle Area: {rectangleArea}");

            CreditCard card = new CreditCard("1234 5678 9012 3456", "John Doe", new DateTime(2025, 12, 31), "1234", 5000, 1000);
            card.OnAccountReplenished += amount => Console.WriteLine($"Account replenished with: {amount}");
            card.OnFundsSpent += amount => Console.WriteLine($"Funds spent: {amount}");
            card.OnCreditUsageStarted += () => Console.WriteLine("Started using credit funds.");
            card.OnCreditLimitReached += () => Console.WriteLine("Credit limit reached.");
            card.OnPinChanged += newPin => Console.WriteLine($"PIN changed to: {newPin}");
            card.ReplenishAccount(500);
            card.SpendFunds(2000);
            card.ChangePin("4321");
        }
    }
}
public class ArrayMethods
{
    public delegate bool NumberPredicate(int number);
    public static List<int> GetEvenNumbers(int[] numbers)
    {
        return FilterNumbers(numbers, IsEven);
    }
    public static List<int> GetOddNumbers(int[] numbers)
    {
        return FilterNumbers(numbers, IsOdd);
    }
    public static List<int> GetPrimeNumbers(int[] numbers)
    {
        return FilterNumbers(numbers, IsPrime);
    }
    public static List<int> GetFibonacciNumbers(int[] numbers)
    {
        return FilterNumbers(numbers, IsFibonacci);
    }
    private static List<int> FilterNumbers(int[] numbers, NumberPredicate predicate)
    {
        List<int> result = new List<int>();
        foreach (int number in numbers)
        {
            if (predicate(number))
            {
                result.Add(number);
            }
        }
        return result;
    }
    private static bool IsEven(int number) => number % 2 == 0;
    private static bool IsOdd(int number) => number % 2 != 0;
    private static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;
        for (int i = 3; i <= Math.Sqrt(number); i += 2)
        {
            if (number % i == 0) return false;
        }
        return true;
    }
    private static bool IsFibonacci(int number)
    {
        if (number <= 0) return false;
        int a = 0;
        int b = 1;
        while (b < number)
        {
            int temp = b;
            b += a;
            a = temp;
        }
        return b == number;
    }
}
public class UtilityMethods
{
    public static void ShowCurrentTime()
    {
        Action displayTime = () => Console.WriteLine("Current Time: " + DateTime.Now.ToString("HH:mm:ss"));
        displayTime();
    }
    public static void ShowCurrentDate()
    {
        Action displayDate = () => Console.WriteLine("Current Date: " + DateTime.Now.ToString("yyyy-MM-dd"));
        displayDate();
    }
    public static void ShowCurrentDayOfWeek()
    {
        Action displayDayOfWeek = () => Console.WriteLine("Current Day of the Week: " + DateTime.Now.DayOfWeek);
        displayDayOfWeek();
    }
    public static double CalculateTriangleArea(double baseLength, double height)
    {
        Func<double, double, double> calculateArea = (b, h) => 0.5 * b * h;
        return calculateArea(baseLength, height);
    }
    public static double CalculateRectangleArea(double width, double height)
    {
        Func<double, double, double> calculateArea = (w, h) => w * h;
        return calculateArea(width, height);
    }
}
public class CreditCard
{
    public string CardNumber { get; set; }
    public string CardHolderName { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Pin { get; private set; }
    public double CreditLimit { get; set; }
    public double Balance { get; private set; }
    public event Action<double> OnAccountReplenished;
    public event Action<double> OnFundsSpent;
    public event Action OnCreditUsageStarted;
    public event Action OnCreditLimitReached;
    public event Action<string> OnPinChanged;
    public CreditCard(string cardNumber, string cardHolderName, DateTime expiryDate, string pin, double creditLimit, double initialBalance)
    {
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        ExpiryDate = expiryDate;
        Pin = pin;
        CreditLimit = creditLimit;
        Balance = initialBalance;
    }
    public void ReplenishAccount(double amount)
    {
        Balance += amount;
        OnAccountReplenished?.Invoke(amount);
    }
    public void SpendFunds(double amount)
    {
        if (Balance + CreditLimit >= amount)
        {
            Balance -= amount;
            OnFundsSpent?.Invoke(amount);
            if (Balance < 0)
            {
                OnCreditUsageStarted?.Invoke();
            }
            if (Balance + CreditLimit == 0)
            {
                OnCreditLimitReached?.Invoke();
            }
        }
        else
        {
            Console.WriteLine("Insufficient funds.");
        }
    }
    public void ChangePin(string newPin)
    {
        Pin = newPin;
        OnPinChanged?.Invoke(newPin);
    }
}