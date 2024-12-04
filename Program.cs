using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;
class Bank
{
    private Dictionary<string, UserDetails> users = new Dictionary<string, UserDetails>();
    public static void Main()
    {
        Console.Title = "Daniel Solano: A00151824, Gabrielle-Lyn Simmonds: A00128712";
        Bank bank = new Bank();
        bank.InitializePreStoredUsers();
        bank.DisplayWelcomeScreen();
    }
    private void InitializePreStoredUsers()
    {
        var joeDoe = new UserDetails("Mr.", "Joe Doe", 30, "joe.doe@example.com", "+614123456789", "Password123", 1500.50m);
        users["Joe.Doe"] = joeDoe;
    }
    private void DisplayWelcomeScreen()
    {
        Console.Clear();
        Console.WriteLine("Welcome to ING Bank Australia:\n_______________________________");
        while (true)
        {
            Console.WriteLine("1. Log-in");
            Console.WriteLine("2. Sign-up");
            Console.WriteLine("3. Quit");
            Console.Write("_______________________________\nPlease choose an option: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Login();
                    return;
                case "2":
                    SignUp();
                    break;
                case "3":
                    Console.WriteLine("\nThank you for being part of ING Bank Australia. Goodbye!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nInvalid input. Please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    DisplayWelcomeScreen();
                    break;
            }
        }
    }
    private void SignUp()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the sign-up process!\n_______________________________");
        string prefix;
        while (true)
        {
            Console.Write("\nEnter Prefix (Mr., Ms., etc.): ");
            prefix = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(prefix)) break;
            Console.WriteLine("\nPrefix cannot be blank.");
        }
        string fullName;
        while (true)
        {
            Console.Write("Enter your Full Name (First + Last): ");
            fullName = Console.ReadLine();
            if (ValidateFullName(fullName)) break;
            Console.WriteLine("\nFull Name must contain min/max two words separated by a space.");
        }
        int age;
        while (true)
        {
            Console.Write("Enter Age: ");
            if (int.TryParse(Console.ReadLine(), out age) && age > 0) break;
            Console.WriteLine("\nAge can't be a negative number nor zero (0).");
        }
        string email;
        while (true)
        {
            Console.Write("Enter Email: ");
            email = Console.ReadLine();
            if (IsValidEmail(email)) break;
            Console.WriteLine("\nInvalid email format. Please enter a valid email address.");
        }
        string phone;
        while (true)
        {
            Console.Write("Enter Phone Number (with country code, e.g., +61 for AU): ");
            phone = Console.ReadLine();
            if (phone.StartsWith("+")) break;
            Console.WriteLine("\nPhone number must start with (+) followed by country code, e.g., +61.");
        }
        string username;
        while (true)
        {
            Console.Write("Enter your desired Username: ");
            username = Console.ReadLine();
            if (users.ContainsKey(username))
            {
                Console.WriteLine("\nUsername already exists. Please choose another.");
            }
            else
            {
                break;
            }
        }
        string password;
        while (true)
        {
            Console.Write("Enter an 8-digit Password (Must contain at least 1 letter, 1 digit, 1 symbol, and 1 uppercase letter): ");
            password = Console.ReadLine();
            if (IsValidPassword(password)) break;
            Console.WriteLine("\nYour Password doesn't meet the valid criteria, try again.");
        }
        users[username] = new UserDetails(prefix, fullName, age, email, phone, password);
        Console.Clear();
        Console.WriteLine("Sign-up successful! You can now log in.");
        Console.WriteLine("\nPress any key to return to the Main Screen...");
        Console.ReadKey();
        DisplayWelcomeScreen();
    }
    private void Login()
    {
        int maxAttempts = 3;
        int attempt = 0;
        bool isLoggedIn = false;
        while (attempt < maxAttempts && !isLoggedIn)
        {
            Console.Clear();
            Console.Write("Enter Username: ");
            string inputUsername = Console.ReadLine();
            if (!users.ContainsKey(inputUsername))
            {
                Console.WriteLine("\nUsername does not exist. You may consider signing up first.");
                Console.WriteLine("\nPress any key to return to the Welcome Screen...");
                Console.ReadKey();
                DisplayWelcomeScreen();
                return;
            }
            while (attempt < maxAttempts)
            {
                Console.Write("\nEnter Password: ");
                string password = Console.ReadLine();
                if (users[inputUsername].Password == password)
                {
                    users[inputUsername].SessionDepositCount = 0;
                    users[inputUsername].SessionWithdrawCount = 0;
                    MainScreen(inputUsername);
                    isLoggedIn = true;
                    break;
                }
                else
                {
                    attempt++;
                    int remainingAttempts = maxAttempts - attempt;
                    Console.WriteLine($"\nIncorrect password. You have {remainingAttempts} attempt(s) remaining.");
                    if (remainingAttempts == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("| Error 404: Login unsuccessful |\n\nPlease call the bank for assistance: +61 07 999 888.");
                        Console.WriteLine("Press any key to return to the Welcome Screen...");
                        Console.ReadKey();
                        DisplayWelcomeScreen();
                        return;
                    }
                }
            }
        }
    }
    private void MainScreen(string inputUsername)
    {
        while (true)
        {
            Console.Clear();
            var user = users[inputUsername];
            Console.WriteLine($"Hello {user.FullName}, welcome back!\n_______________________________\n");
            Console.WriteLine("1. View balance");
            Console.WriteLine("2. Make a deposit");
            Console.WriteLine("3. Withdraw money");
            Console.WriteLine("4. Make a bank transfer (coming soon)");
            Console.WriteLine("5. Update personal details");
            Console.WriteLine("6. Log-out");
            Console.Write("_______________________________\nPlease choose an option: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewBalance(inputUsername);
                    break;
                case "2":
                    MakeDeposit(inputUsername);
                    break;
                case "3":
                    Withdraw(inputUsername);
                    break;
                case "4":
                    Console.WriteLine("\nFeature coming soon!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "5":
                    UpdateDetails(inputUsername);
                    break;
                case "6":
                    Console.WriteLine("\nYou have successfully logged out.");
                    DisplayWelcomeScreen();
                    return;
                default:
                    Console.WriteLine("\nInvalid choice, please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
    private void ViewBalance(string inputUsername)
    {
        var user = users[inputUsername];
        Console.Clear();
        Console.WriteLine("Account Balance:\n_______________________________\n");
        Console.WriteLine($"Your current balance is: ${user.Balance:N2} AUD");
        Console.WriteLine("\nPress any key to return to the Main Screen...");
        Console.ReadKey();
    }
    public void MakeDeposit(string inputUsername)
    {
        const decimal TransactionLimit = 15000m;
        const decimal DailyLimit = 100000m;
        const int MaxDepositsPerSession = 5;
        var user = users[inputUsername];
        if (user.DepositStartTime == DateTime.MinValue || DateTime.Now >= user.DepositStartTime.AddHours(24))
        {
            user.DailyDepositTotal = 0;
            user.DepositStartTime = DateTime.Now;
        }
        Console.Clear();
        Console.WriteLine("Disclaimer:\n_______________________________\n");
        Console.WriteLine($"Maximum transaction limit per deposit: ${TransactionLimit:N2} AUD");
        Console.WriteLine($"Maximum daily deposit limit: ${DailyLimit:N2} AUD");
        Console.WriteLine($"Maximum number of deposits per session: {MaxDepositsPerSession}");
        Console.WriteLine("\nPlease ensure you are aware of these limits.\n");
        Console.WriteLine("Press any key to agree with the disclaimer and continue...");
        Console.ReadKey();
        if (user.ReachedSessionLimit("Deposit"))
        {
            Console.Clear();
            Console.WriteLine($"You have reached the maximum number of deposits ({MaxDepositsPerSession}) for this session.\nPlease log-out to reset your counter.");
            Console.WriteLine("\nPress any key to return to the Main Screen...");
            Console.ReadKey();
            return;
        }
        while (true)
        {
            decimal remainingDailyLimit = DailyLimit - user.DailyDepositTotal;
            Console.Clear();
            Console.WriteLine("Make a deposit:\nEnter '0' at any time to return to the Main Screen.\n_______________________________\n");
            Console.WriteLine($"Your current balance is: ${user.Balance:N2} AUD.");
            Console.WriteLine($"Maximum transaction limit per deposit: ${TransactionLimit:N2} AUD");
            Console.WriteLine($"Remaining daily deposit limit: ${remainingDailyLimit:N2} AUD");
            Console.WriteLine($"You have {MaxDepositsPerSession - user.SessionDepositCount} deposits remaining.\n_______________________________\n");
            if (remainingDailyLimit <= 0)
            {
                DateTime resetTime = user.DepositStartTime.AddHours(24);
                TimeSpan timeUntilReset = resetTime - DateTime.Now;
                Console.WriteLine($"You have reached your daily deposit limit of ${DailyLimit:N2} AUD");
                Console.WriteLine($"Time remaining until daily limit resets: {timeUntilReset.Hours} hours and {timeUntilReset.Minutes} minutes.");
                Console.WriteLine("\nPress any key to return to the Main Screen...");
                Console.ReadKey();
                return;
            }
            Console.Write("Enter the deposit amount (numbers only): $");
            string userInput = Console.ReadLine()?.Trim();
            if (userInput == "0")
            {
                Console.WriteLine("\nReturning to the Main Screen...");
                Console.ReadKey();
                return;
            }
            if (!decimal.TryParse(userInput, out decimal depositAmount) || depositAmount <= 0)
            {
                Console.WriteLine("\nInvalid input. Please ONLY enter positive numeric digits.");
                Console.WriteLine("\nPress any key to try again...");
                Console.ReadKey();
                continue;
            }
            if (depositAmount > TransactionLimit)
            {
                Console.Clear();
                Console.WriteLine($"Deposit exceeds the transaction limit of ${TransactionLimit:N2} AUD.");
                Console.WriteLine("\nIf you wish to make a higher deposit, please visit your closest bank branch and bring your ID.");
                Console.WriteLine("Alternatively, you can call the bank for assistance: +61 07 999 888.");
                Console.WriteLine("\nPress any key to try again...");
                Console.ReadKey();
                continue;
            }
            if (user.DailyDepositTotal + depositAmount > DailyLimit)
            {
                Console.WriteLine($"Deposit exceeds the daily limit of ${DailyLimit:N2} AUD");
                Console.WriteLine("\nPress any key to try again...");
                Console.ReadKey();
                continue;
            }
            user.Balance += depositAmount;
            user.DailyDepositTotal += depositAmount;
            user.SessionDepositCount++;
            Console.Clear();
            Console.WriteLine($"Deposit successful! Your new balance is: ${user.Balance:N2}");
            Console.WriteLine($"\nRemaining daily deposit limit: ${DailyLimit - user.DailyDepositTotal:N2} AUD");
            Console.WriteLine($"Remaining deposits for this session: {MaxDepositsPerSession - user.SessionDepositCount} deposits");
            Console.WriteLine("\nPress any key to return to the Main Screen...");
            Console.ReadKey();
            return;
        }
    }
    private void Withdraw(string inputUsername)
    {
        const int MaxWithdrawsPerSession = 5;
        var user = users[inputUsername];
        if (user.ReachedSessionLimit("Withdraw"))
        {
            Console.Clear();
            Console.WriteLine($"You have reached the maximum number of withdraws ({MaxWithdrawsPerSession}) for this session.\nPlease log-out to reset your counter.");
            Console.WriteLine("\nPress any key to return to the Main Screen...");
            Console.ReadKey();
            return;
        }
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Withdraw money:\nEnter '0' at any time to return to the Main Screen.\n_______________________________\n");
            Console.WriteLine($"Your current balance is: ${user.Balance:N2} AUD.");
            Console.WriteLine($"You have {MaxWithdrawsPerSession - user.SessionWithdrawCount} withdrawals remaining.\n_______________________________");
            Console.WriteLine();
            Console.Write("Enter the amount to withdraw (numbers only): $");
            string userInput = Console.ReadLine()?.Trim();
            if (userInput == "0")
            {
                Console.WriteLine("\nReturning to the Main Screen...");
                Console.ReadKey();
                return;
            }
            if (!decimal.TryParse(userInput, out decimal withdrawAmount) || withdrawAmount <= 0)
            {
                Console.WriteLine("\nInvalid input. Please ONLY enter positive numeric digits.");
                Console.WriteLine("Press any key to try again...");
                Console.ReadKey();
                continue;
            }
            if (withdrawAmount > user.Balance)
            {
                Console.WriteLine("\nYour funds are insufficient.");
                Console.WriteLine("Press any key to try again...");
                Console.ReadKey();
                continue;
            }
            user.Balance -= withdrawAmount;
            user.SessionWithdrawCount++;
            Console.Clear();
            Console.WriteLine($"Withdrawal successful! Your new balance is: ${user.Balance:N2}");
            Console.WriteLine($"\nRemaining withdrawals for this session: {MaxWithdrawsPerSession - user.SessionWithdrawCount}");
            Console.WriteLine("\nPress any key to return to the Main Screen...");
            Console.ReadKey();
            return;
        }
    }
    private void UpdateDetails(string inputUsername)
    {
        Console.Clear();
        var user = users[inputUsername];
        Console.Clear();
        Console.WriteLine("Your current details:\n_______________________________\n");
        Console.WriteLine($"Prefix: {user.Prefix}");
        Console.WriteLine($"Name: {user.FullName}");
        Console.WriteLine($"Age: {user.Age}");
        Console.WriteLine($"Email: {user.Email}");
        Console.WriteLine($"Phone: {user.Phone}");
        Console.WriteLine("\nFeature coming soon!");
        Console.WriteLine("To update your details, please call the bank at: +61 07 999 888.");
        Console.WriteLine("\nPress any key to return to the Main Screen...");
        Console.ReadKey();
    }
    private bool ValidateFullName(string fullName)
    {
        return fullName.Split(' ').Length == 2;
    }
    private bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }
    private static bool IsValidPassword(string password)
    {
        if (password.Length < 8)
            return false;
        bool hasLetter = false;
        bool hasDigit = false;
        bool hasUpper = false;
        bool hasSpecialChar = false;
        foreach (var character in password)
        {
            if (char.IsLetter(character)) hasLetter = true;
            if (char.IsDigit(character)) hasDigit = true;
            if (char.IsUpper(character)) hasUpper = true;
            if (!char.IsLetterOrDigit(character)) hasSpecialChar = true;
        }

        return hasLetter && hasDigit && hasUpper && hasSpecialChar;
    }
    class UserDetails
    {
        public string Prefix { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public decimal DailyDepositTotal { get; set; }
        public int SessionDepositCount { get; set; }
        public int SessionWithdrawCount { get; set; }

        public const int MaxDepositsPerSession = 5;
        public const int MaxWithdrawsPerSession = 5;

        public DateTime DepositStartTime { get; set; } = DateTime.MinValue;
        public bool ReachedSessionLimit(string transactionType)
        {
            return transactionType switch
            {
                "Deposit" => SessionDepositCount >= MaxDepositsPerSession,
                "Withdraw" => SessionWithdrawCount >= MaxWithdrawsPerSession,
                _ => throw new ArgumentException("Invalid transaction type", nameof(transactionType))
            };
        }
        public UserDetails(string prefix, string fullName, int age, string email, string phone, string password, decimal balance = 0.0m)
        {
            Prefix = prefix;
            FullName = fullName;
            Age = age;
            Email = email;
            Phone = phone;
            Password = password;
            Balance = balance;
            DailyDepositTotal = 0;
            SessionDepositCount = 0;
            SessionWithdrawCount = 0;
            DepositStartTime = DateTime.MinValue;
        }
    }
}