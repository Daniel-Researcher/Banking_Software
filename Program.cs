﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;
/*
As we are using system libraries such as Console, we import the [System] namespace.
Same as above, but this time we import [System.Collections.Generic] namespace to use Dictionary.
[System.Linq] namespace will allow querying collections of data (eg. Sorting, Grouping, etc.).
We will use [System.Net.Mail] namespace to verify email addressess.
Lastly, we will use [System.Text.RegularExpressions] that will enable extra functions such as email validation.
*/
class Bank
// We define a [Bank] class as we are developing Banking Software.
{
    private Dictionary<string, UserDetails> users = new Dictionary<string, UserDetails>();
    /*
    The [private] keyword means that this [Dictionary] can only be accessed within the [Bank] class.
    We use the [Dictionary] collection to store [users], where they key is the username and the value contains the user's details [UserDetails]
    */
    public static void Main()
    // We define the starting point of the program within the class keyword.
    {
        Console.Title = "Daniel Solano: A00151824, Gabrielle-Lyn Simmonds: A00128712";
        // This is just our group members names and Student IDs as requested, it'll be displayed as a title of the command prompt.

        Bank bank = new Bank();
        // We create an instance of the Bank class to call its methods.

        bank.InitializePreStoredUsers();
        // We initialize the pre-stored user [Joe.Doe] requested in AS2 - Task 1.

        bank.DisplayWelcomeScreen();
        // We call the method to display the welcome screen.
    }
    private void InitializePreStoredUsers()
    // This method creates the pre-stored user [Joe.Doe]
    {
        var joeDoe = new UserDetails("Mr.", "Joe Doe", 30, "joe.doe@example.com", "+614123456789", "Password123");
        users["Joe.Doe"] = joeDoe;
        // We create the user [Joe.Doe] with predefined details and store it in the [users] Dictionary.
    }
    private void DisplayWelcomeScreen()
    // We define a method that only this class can use (private) and that does not return any value (void).
    {
        Console.Clear();
        // This is to clear the screen and create a neat interface for the user.

        Console.WriteLine("Welcome to ING Bank Australia:\n_______________________________");
        // We display the welcome message and add a new line to separate it from the options, enables better readibility.

        while (true)
        // We start an infinite loop for user input to make sure user selects a valid option.
        {
            Console.WriteLine("1. Log-in");
            // Option to login.

            Console.WriteLine("2. Sign-up");
            // Option to sign up.

            Console.WriteLine("3. Quit");
            // Option to exit the program.

            Console.Write("_______________________________\nPlease choose an option: ");
            // We prompt the user to choose an option. We added a new line at the beginning for readability.

            string choice = Console.ReadLine();
            // We define the variable [choice], and then we read the user's input and store it in the variable.

            switch (choice)
            // We start a [switch] statement, which is an improved version of [if-else].
            {
                case "1":
                    Login();
                    // We call the Login method if user's input equals [1].
                    return;
                // We exit the method to prevent the loop after sucessful login.

                case "2":
                    SignUp();
                    // We call the SignUp method if user's input equals [2].
                    break;
                // Same as above.

                case "3":
                    Console.WriteLine("\nThank you for being part of ING Bank Australia. Goodbye!");
                    Environment.Exit(0);
                    // We display an exit message to the user and terminate the program.
                    break;
                // Same as above.

                default:
                    // This is the [else] version for switch statements. This is to handle any other input non-considered valid.

                    Console.WriteLine("\nInvalid input. Please try again.");
                    // We handle invalid inputs and prompt the user to try again.

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    DisplayWelcomeScreen();
                    // We prompt the user to press a key to continue, we clear the console and we display the Welcome Screen.

                    break;
                    // Same as above.
            }
        }
    }
    private void SignUp()
    // We create the SignUp method and begin details collection process.
    {
        Console.Clear();
        // This is to clear the screen and create a neat interface for the user.

        Console.WriteLine("Welcome to the sign-up process!");
        // We display a welcome message to the sign-up process.

        string prefix;
        // This string will host the prefix upon being provided by the user.

        while (true)
        // We start a loop until a valid input has been provided.
        {
            Console.Write("\nEnter Prefix (Mr., Ms., etc.): ");
            prefix = Console.ReadLine();
            // We prompt the user for a prefix and we assign the input to the variable [prefix].

            if (!string.IsNullOrWhiteSpace(prefix)) break;
            /*
            We call the method [IsNullorWhiteSpace] which checks whether the input is empty or there are white spaces.
            We use the is NOT [!] logical to return a false if there's no content, and a true if there is content.
            If there's content, we exit the loop and proceed to the next one.
            */

            Console.WriteLine("\nPrefix cannot be blank.");
            // If there's no content, we display this error message to the user and we remain in the loop.
        }

        string fullName;
        // This string will host the fullName upon being provided by the user.

        while (true)
        // We start a loop until a valid input has been provided.
        {
            Console.Write("Enter your Full Name (First + Last): ");
            fullName = Console.ReadLine();
            /* 
            We prompt the user for a fullName and we assign the input to the variable [prefix].
            Full name must be two words separate by a space.
            */

            if (ValidateFullName(fullName)) break;
            /*
            We call the method [ValidateFullName] which checks whether fullName variable meets the criteria or not.
            This method is explained later on, please see line number: 
            If the name meets the criteria, we exit the loop and proceed to the next one.
            */

            Console.WriteLine("\nFull Name must contain min/max two words separated by a space.");
            // If the fullName has fewer or more than 2 words, we display this error message to the user and we remain in the loop.
        }

        int age;
        // This integer will host the age upon being provided by the user.

        while (true)
        // We start a loop until a valid input has been provided.
        {
            Console.Write("Enter Age: ");
            /*
            We prompt the user for an age and we assign the input to the variable [age].
            Age can neither be a negative nor decimal (float) number.
            */

            if (int.TryParse(Console.ReadLine(), out age) && age > 0) break;
            /* 
            When we use [Console.ReadLine], the input is always considered a string, which would be an invalid input.
            We use [int.TryParse(Console.Readline(), out age] to convert the input from string to integer.
            If it's an invalid string (eg. abc), [TryParse] will return false and the user won't exit the loop.
            If the parsing is successfull, we assign the number to age by using [out].
            We use the logical AND [&&] operator to make sure both conditions on either side are true.
            We check if the parsed integer is greater [>] than zero, which makes sure the user doesn't input negative numbers nor zero.
            If the age meets the criteria, we exit the loop and proceed to the next one.               
            */

            Console.WriteLine("\nAge can't be a negative number nor zero (0).");
            // If the age is a negative number or zero, we display this error message to the user and we remain in the loop.
        }

        string email;
        // This string will host the email upon being provided by the user.

        while (true)
        // We start a loop until a valid input has been provided.
        {
            Console.Write("Enter Email: ");
            email = Console.ReadLine();
            // We prompt the user for an email address and we assign the input to the variable [email].

            if (IsValidEmail(email)) break;
            /*
            We call the method [IsValidEmail] which checks whether email variable meets the criteria or not.
            This method is explained later on, please see line number: 
            If the email meets the criteria, we exit the loop and proceed to the next one.
            */

            Console.WriteLine("\nInvalid email format. Please enter a valid email address.");
            // If the email has an invalid format, we display this error message to the user and we remain in the loop.
        }

        string phone;
        // This string will host the phone upon being provided by the user.

        while (true)
        // We start a loop until a valid input has been provided.
        {
            Console.Write("Enter Phone Number (with country code, e.g., +61 for AU): ");
            phone = Console.ReadLine();
            /*
            We prompt the user for a phone number and we assign the input to the variable [phone].
            Phone number must start with plus [+] character and country code followed by the number.
            */

            if (phone.StartsWith("+")) break;
            /*
            We use the [.StartsWith("+")] method to verify if the plus character was entered.
            If the phone number meets the criteria, we exit the loop and proceed to the next one.
            */

            Console.WriteLine("\nPhone number must start with (+) followed by country code, e.g., +61.");
            // If the phone number is missing the plus character, we display this error message to the user and we remain in the loop.
        }

        string username;
        // This string will host the username upon being provided by the user.

        while (true)
        // We start a loop until a valid input has been provided.
        {
            Console.Write("Enter your desired Username: ");
            username = Console.ReadLine();
            /*
            We prompt the user for an username and we assign the input to the variable [username].
            Username can't be an existing one.
            */

            if (users.ContainsKey(username))
            /*
            We check the Dictionary looking for existing username matches.
            If the username meets the criteria, we exit the loop and proceed to the next one.
            */
            {
                Console.WriteLine("\nUsername already exists. Please choose another.");
                // If the username already exists, we display this error message to the user and we remain in the loop.
            }
            else
            // We remain in the loop until the user has input a valid non-existing username.
            {
                break;
            }
        }

        string password;
        // This string will host the password upon being provided by the user.

        while (true)
        // We start a loop until a valid input has been provided.
        {
            Console.Write("Enter an 8-digit Password (Must contain at least 1 letter, 1 digit, 1 symbol, and 1 uppercase letter): ");
            password = Console.ReadLine();
            /*
            We prompt the user for a password and we assign the input to the variable [password].
            Password needs to have min. 8-digits including: 1 letter, digit, symbol and uppercase letter.
            */

            if (IsValidPassword(password)) break;
            /*
            We call the method [IsValidPassword] which checks whether password variable meets the criteria or not.
            This method is explained later on, please see line number: 
            If the password meets the criteria, we exit the loop and proceed to the next one.
            */

            Console.WriteLine("\nYour Password doesn't meet the valid criteria, try again.");
            // If the password doesn't meet the criteria, we display this error message to the user and we remain in the loop.
        }

        users[username] = new UserDetails(prefix, fullName, age, email, phone, password);
        // We store the new user details in the [users] dictionary.

        Console.Clear();
        Console.WriteLine("Sign-up successful! You can now log in.");
        Console.WriteLine("\nPress any key to return to the Main Screen...");
        Console.ReadKey();
        DisplayWelcomeScreen();
        // We clear the console, inform the user of the successful sign-up process, prompt for a key and display the Welcome Screen.
    }
    private void Login()
    // We create the Login method.
    {
        int maxAttempts = 3;
        int attempt = 0;
        bool isLoggedIn = false;
        /*
        We create the integer [maxAttempts] and set it to 3 (max. 3 wrong password attempts).
        We create the integer [attempt] and set it to 0 (it will increase as the user inputs an invalid password).
        We create a boolean [isloggedIn] and set it to false (it will be set to true if the user input valid username + password).
        */

        while (attempt < maxAttempts && !isLoggedIn)
        // We start a loop until both conditions on either side of the logical AND [&&] operator are true, then the user is taken to the Main Screen.

        {
            Console.Clear();
            Console.Write("Enter Username: ");
            string inputUsername = Console.ReadLine();
            // We clear the console, prompt the user to enter username, and create a string to host the user input [inputUsername].

            if (!users.ContainsKey(inputUsername))
            /*
            Using the logical is NOT [!] operator we compare the user input [inputUsername] with the [users] Dictionary.
            If the input doesn't match an existing username, me continue the loop. 
            */
            {
                Console.WriteLine("\nUsername does not exist. You may consider signing up first.");
                Console.WriteLine("\nPress any key to return to the Welcome Screen...");
                Console.ReadKey();
                DisplayWelcomeScreen();
                /* 
                If the username doesn't exist, we display this error message to the user and we remain in the loop.
                We prompt the user for a key, clear the console and display the Welcome Screen.
                */

                return;
                // If the username does exist, we move onto the password authentication.
            }

            while (attempt < maxAttempts)
            // We start an inner loop until the user has provided a valid password or exhausted the attempts.

            {
                Console.Write("\nEnter Password: ");
                string password = Console.ReadLine();
                // We prompt the user to enter password, and create a string to host the user input [password].

                if (users[inputUsername].Password == password)
                /*
                We check the Dictionary [users] for possible a match for [inputUsername].
                [.Password == password] retrieves the stored [Password] for this username in the [users] Dictionary.
                We check if [Password] is equal [==] to [password].
                If both [inputUsername] and [password] meet the criteria, we exit the loop.
                */
                {
                    MainScreen(inputUsername);
                    isLoggedIn = true;
                    break;
                    /* 
                    Upon exiting the loop, we call the MainScreen for [inputUsername].
                    We set [isLoggedIn] to TRUE to exit the main loop for Login method.
                    */
                }
                else
                // We continue the loop until a valid password has been provided.
                {
                    attempt++;
                    // We increase [++] the attempt counter by 1 as the user already input a wrong password.

                    int remainingAttempts = maxAttempts - attempt;
                    /* 
                    We create create an integer [remainingAttempts] to host the user remaining attempts.
                    We calculate remaining attempts substracting [maxAttempts] 3 - 1/2/3 [attempt].
                    */

                    Console.WriteLine($"\nIncorrect password. You have {remainingAttempts} attempt(s) remaining.");
                    // We display an error message and we display the remaining attempts by using an interpolated string [$].

                    if (remainingAttempts == 0)
                    // Once the user has exhausted the attempts...
                    {
                        Console.Clear();
                        Console.WriteLine("| Error 404: Login unsuccessful |\nPlease call the bank for assistance: +61 07 999 888.");
                        Console.WriteLine("\nPress any key to return to the Welcome Screen...");
                        Console.ReadKey();
                        DisplayWelcomeScreen();
                        /* 
                        We clear the screen, inform user the login failed, and we advise him to call the bank. This is a made-up phone number.
                        We wait for a key press and return the user to the Welcome Screen.
                        */

                        return;
                        // This is to return a value and make sure the program doesn't close after an unsucessful login.
                    }
                }
            }
        }
    }
    private void MainScreen(string inputUsername)
    // We create the MainScreen for the user that just logged in [inputUsername].
    {
        while (true)
        {
            Console.Clear();
            var user = users[inputUsername];
            /*
            We clear the Welcome Screen and move onto the Main Screen.
            We create a variable [user] that will retrieve the user details of the current user [inputUsername] and store them in the variable itself.
            */

            Console.WriteLine($"Hello {user.FullName}, welcome back!\n");
            // We display a welcome message using the user [FullName] retrieved from [users] Dictionary using an interpolate string [$].

            Console.WriteLine("1. View balance (coming soon)");
            Console.WriteLine("2. Make a deposit (coming soon)");
            Console.WriteLine("3. Withdraw money (coming soon)");
            Console.WriteLine("4. Make a bank transfer (coming soon)");
            Console.WriteLine("5. Update personal details");
            Console.WriteLine("6. Log-out");
            // We display the Main Screen and the available options (only 5 and 6 work).

            Console.Write("\nPlease pick an option: ");
            // We prompt user to pick an option.

            string choice = Console.ReadLine();
            // We define a variable [choice] and assign user's input to it using [Console.ReadLine].

            switch (choice)
            // We start a [switch] statement to handle different user choices.
            {
                case "1":
                case "2":
                case "3":
                case "4":
                    Console.WriteLine("\nFeature coming soon!");
                    // For options from 1-4 we notify user that the feature is not yet available.

                    Console.WriteLine("Press any key to continue...");
                    // We prompt for key press before clearing.

                    Console.ReadKey();
                    // Waiting for user to press a key...

                    break;
                // We continue the loop and allow for a valid input.

                case "5":
                    UpdateDetails(inputUsername);
                    /*
                    If user picks option [5] we call the [UpdateDetails] method.
                    This method is explained later on, please see line number: 
                    */

                    break;

                case "6":
                    Console.WriteLine("\nYou have successfully logged out.");
                    DisplayWelcomeScreen();
                    // If user picks option [6] we log the user out by taking it to the Welcome Screen.

                    return;
                // We terminate the loop.

                default:
                    Console.WriteLine("\nInvalid choice, please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    // We handle invalid inputs and prompt the user to press a key and try again.

                    break;
            }
        }
    }
    private void UpdateDetails(string inputUsername)
    // We create the UpdateDetails method for the logged in user [inputUsername].
    {
        Console.Clear();
        var user = users[inputUsername];
        /*            
        We clear the Main Screen and move onto the Update Details screen.
        We create a variable [user] that will retrieve the user details of the current user [inputUsername] and store them in the variable itself.
        */

        Console.WriteLine("Your current details:\n");
        Console.WriteLine($"Prefix: {user.Prefix}");
        Console.WriteLine($"Name: {user.FullName}");
        Console.WriteLine($"Age: {user.Age}");
        Console.WriteLine($"Email: {user.Email}");
        Console.WriteLine($"Phone: {user.Phone}");
        /*
        We display the [inputUsername] details stored in the [users] dictionary.
        We call the variable [user] for each detail and display their information using an interpolated string [$].
        */

        Console.WriteLine("\nFeature coming soon!");
        Console.WriteLine("To update your details, please call the bank at: +61 07 999 888.");
        Console.WriteLine("\nPress any key to return to the Main Screen...");
        Console.ReadKey();
        /*
        We inform the user that updating is not implemented yet and encourage it to call the bank. This is a made-up number.
        We prompt for a key press and return to the Main Screen's loop.
        */
    }
    private bool ValidateFullName(string fullName)
    /*
    We create the ValidateFullName method to verify that [fullName] meets the criteria.
    This is a boolean [true-false] and will return a true if the criteria is met.
    */
    {
        return fullName.Split(' ').Length == 2;
        /*
        We call the [fullName] variable that stores the Full Name of the user during Sign-up process.
        Full Name criteria is: No blank spaces and min/max two strings [First + Last].
        We use [(' ')] to split strings per space in between them.
        As there are two strings, this creates two arrays eg. ["John", "Doe"]
        [.Length == 2] ensures that the number of arrays equals [==] 2
        If the above criteria is met, the boolean returns [true] and allows the user exit the [fullname] variable loop for Signup method.
        */
    }
    private bool IsValidEmail(string email)
    /*
    We create the IsValidEmail method to verify that [email] meets the criteria.
    This is a boolean [true-false] and will return a true if the criteria is met.
    */
    {
        try
        // We try to create a new MailAddress object to validate the email format.
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            /* 
            We attempt to parse the email address using the MailAddress constructor.
            MailAddress constructor ensures that [email] contains a valid string before the [@] character, and a proper domain (eg., .com, .org).
            */

            return mailAddress.Address == email;
            /*
            We check if mailAddress criteria equals [==] what the user entered in [email]
            If the above criteria is met, the boolean returns [true] and allows the user exit the [email] variable loop for Signup method.
            */
        }
        catch
        // We cath any exception thrown (invalid format).
        {
            return false;
            // If the email criteria is not met, the boolean returns a false and won't allow the user exit the [email] variable loop for Signup method.
        }
    }
    private static bool IsValidPassword(string password)
    /*
    We create the IsValidPassword method to verify that [password] meets the criteria.
    This is a boolean [true-false] and will return a true if the criteria is met.
    */
    {
        if (password.Length < 8)
            // First we ensure the password the user input has min. 8-characters.

            return false;
        // If the lenght criteria is not met, the boolean returns a false and prompts the user to enter a valid password druing the Signup method.

        bool hasLetter = false;
        bool hasDigit = false;
        bool hasUpper = false;
        bool hasSpecialChar = false;
        /*
        We create 4 flags for letter, digit, uppercase and special character respectively.
        They are set to false by default, they will change if the password criteria is met.
        */

        foreach (var character in password)
        /* 
        We start a [foreach] loop to iterate over each character [var] in [password]
        In order for the loop to succeed, we need to create a variable [character] that will
        store every character as it validates it will move onto the next one until having
        validated the whole password.
        */
        {
            if (char.IsLetter(character)) hasLetter = true;
            /* 
            Using the [char.IsLetter] built-in function, we check if [password] contains an alphabetic character. 
            If the criteria is met, the hasLetter flag is set to TRUE.
            */

            if (char.IsDigit(character)) hasDigit = true;
            /* 
            Using the [char.IsDigit] built-in function, we check if [password] contains a digit (0-9). 
            If the criteria is met, the hasDigit flag is set to TRUE.
            */

            if (char.IsUpper(character)) hasUpper = true;
            /* 
            Using the [char.IsUpper] built-in function, we check if [password] contains an uppercase letter (A-Z). 
            If the criteria is met, the hasUpper flag is set to TRUE.
            */

            if (!char.IsLetterOrDigit(character)) hasSpecialChar = true;
            /* 
            We use the logical NOT operator [!] to confirm special characters by NOT looking for letters nor digits.
            Using the [!char.IsLetterOrDigit] built-in function we check if [password] has anything but letter or digits (which would be a special character).
            If the criteria is met, the hasSpecialChar flag is set to TRUE.
            */
        }

        return hasLetter && hasDigit && hasUpper && hasSpecialChar;
        /*
        We check if all 4 flags are set to [TRUE] by using the logical AND [&&] operator.
        If the above condition is met, the program returns a [TRUE] to the IsValidPassword boolean and allows the user to complete the Sign Up process.
        */
    }
    class UserDetails
    // We define a [UserDetails] class to store the users details.
    {
        public string Prefix { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        /*
        This class contains 6 public properties each with [get] and [set] accessors.
        This will allow the code to retrieve/modify properties' values.
        */

        public UserDetails(string prefix, string fullName, int age, string email, string phone, string password)
        /*
        We define a constructor that will initialize a new [UserDetails] object with specific values.
        The values are listed below.
        */
        {
            Prefix = prefix;
            FullName = fullName;
            Age = age;
            Email = email;
            Phone = phone;
            Password = password;
        }
    }
}
//End of the program.