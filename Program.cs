using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PersonalFinancialPlanner
{
    // Main class
    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of FinanceManager to handle financial operations
            FinanceManager financeManager = new FinanceManager();
            // Load existing data from files
            financeManager.LoadData();

            // Main loop to display the menu and handle user input
            while (true)
            {
                // Display the main menu
                Console.WriteLine("Personal Financial Planner");
                Console.WriteLine("1. Add Transaction");
                Console.WriteLine("2. View Current Balance");
                Console.WriteLine("3. View Transactions by Category");
                Console.WriteLine("4. Manage Categories");
                Console.WriteLine("5. Calculate Investment Return");
                Console.WriteLine("6. View Transaction History");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");
                
                // Read user input and parse it as an integer
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    // Handle user choice
                    switch (choice)
                    {
                        case 1:
                            financeManager.AddTransaction();
                            break;
                        case 2:
                            financeManager.ViewCurrentBalance();
                            break;
                        case 3:
                            financeManager.ViewTransactionsByCategory();
                            break;
                        case 4:
                            financeManager.ManageCategories();
                            break;
                        case 5:
                            financeManager.CalculateInvestmentReturn();
                            break;
                        case 6:
                            financeManager.ViewTransactionHistory();
                            break;
                        case 7:
                            // Save data and exit the application
                            financeManager.SaveData();
                            return;
                        default:
                            Console.WriteLine("Invalid option, please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, please enter a number.");
                }
            }
        }
    }

    // Class to manage financial operations
    class FinanceManager
    {
        // List to store transactions
        private List<Transaction> transactions = new List<Transaction>();
        // Predefined list of expense categories
        private List<string> expenseCategories = new List<string>
        {
            "Food & Drinks",
            "Shopping",
            "Transportation",
            "Entertainment",
            "Investments",
            "Other"
        };

        // Predefined list of income categories
        private List<string> incomeCategories = new List<string>
        {
            "Salary",
            "Bonus",
            "Investment Income",
            "Other"
        };

        // File paths for storing data
        private const string TransactionsFile = "transactions.txt";
        private const string ExpenseCategoriesFile = "expense_categories.txt";
        private const string IncomeCategoriesFile = "income_categories.txt";

        // Load data from files
        public void LoadData()
        {
            // Load transactions
            if (File.Exists(TransactionsFile))
            {
                var lines = File.ReadAllLines(TransactionsFile);
                foreach (var line in lines)
                {
                    transactions.Add(Transaction.FromString(line));
                }
            }

            // Load expense categories
            if (File.Exists(ExpenseCategoriesFile))
            {
                var existingExpenseCategories = File.ReadAllLines(ExpenseCategoriesFile).ToList();
                expenseCategories = existingExpenseCategories.Union(expenseCategories).Distinct().ToList();
            }

            // Load income categories
            if (File.Exists(IncomeCategoriesFile))
            {
                var existingIncomeCategories = File.ReadAllLines(IncomeCategoriesFile).ToList();
                incomeCategories = existingIncomeCategories.Union(incomeCategories).Distinct().ToList();
            }
        }

        // Save data to files
        public void SaveData()
        {
            File.WriteAllLines(TransactionsFile, transactions.Select(t => t.ToString()));
            File.WriteAllLines(ExpenseCategoriesFile, expenseCategories);
            File.WriteAllLines(IncomeCategoriesFile, incomeCategories);
        }

        // Add a new transaction
        public void AddTransaction()
        {
            Console.WriteLine("Add Transaction");
            Console.WriteLine("1. Income");
            Console.WriteLine("2. Expense");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Choose an option: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 3)
                    return;

                bool isIncome = choice == 1;

                Console.Write("Enter amount: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    Console.WriteLine("Invalid amount. Transaction cancelled.");
                    return;
                }

                if (!isIncome) amount = -amount; // Expenses are negative amounts

                Console.Write("Enter description: ");
                string? description = Console.ReadLine();
                if (string.IsNullOrEmpty(description))
                {
                    Console.WriteLine("Description cannot be empty. Transaction cancelled.");
                    return;
                }

                List<string> categories = isIncome ? incomeCategories : expenseCategories;

                Console.WriteLine("Select category:");
                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                }

                if (!int.TryParse(Console.ReadLine(), out int categoryIndex) || categoryIndex < 1 || categoryIndex > categories.Count)
                {
                    Console.WriteLine("Invalid category. Transaction cancelled.");
                    return;
                }

                transactions.Add(new Transaction
                {
                    Amount = amount,
                    Description = description,
                    Category = categories[categoryIndex - 1],
                    Date = DateTime.Now
                });

                Console.WriteLine("Transaction added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid input, please enter a number.");
            }
        }

        // View the current balance
        public void ViewCurrentBalance()
        {
            decimal balance = transactions.Sum(t => t.Amount);
            Console.WriteLine($"Current balance: {balance:C}");
        }

        // View transactions by category
        public void ViewTransactionsByCategory()
        {
            Console.WriteLine("View Transactions by Category");
            Console.WriteLine("1. Income");
            Console.WriteLine("2. Expense");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Choose an option: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 3)
                    return;

                bool isIncome = choice == 1;

                List<string> categories = isIncome ? incomeCategories : expenseCategories;

                Console.WriteLine("Select category:");
                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                }

                if (!int.TryParse(Console.ReadLine(), out int categoryIndex) || categoryIndex < 1 || categoryIndex > categories.Count)
                {
                    Console.WriteLine("Invalid category. Operation cancelled.");
                    return;
                }

                string selectedCategory = categories[categoryIndex - 1];

                var transactionsByCategory = transactions
                    .Where(t => t.Category == selectedCategory && (isIncome ? t.Amount > 0 : t.Amount < 0));

                foreach (var transaction in transactionsByCategory)
                {
                    Console.WriteLine($"{transaction.Date}: {transaction.Description} - {transaction.Amount:C}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input, please enter a number.");
            }
        }

        // Manage categories
        public void ManageCategories()
        {
            while (true)
            {
                Console.WriteLine("Manage Categories");
                Console.WriteLine("1. View Expense Categories");
                Console.WriteLine("2. View Income Categories");
                Console.WriteLine("3. Add Category");
                Console.WriteLine("4. Delete Category");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Choose an option: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ViewCategories(expenseCategories, "Expense Categories");
                            break;
                        case 2:
                            ViewCategories(incomeCategories, "Income Categories");
                            break;
                        case 3:
                            AddCategory();
                            break;
                        case 4:
                            DeleteCategory();
                            break;
                        case 5:
                            return;
                        default:
                            Console.WriteLine("Invalid option, please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, please enter a number.");
                }
            }
        }

        // View categories
        public void ViewCategories(List<string> categories, string title)
        {
            Console.WriteLine(title);
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}");
            }
        }

        // Add a new category
        public void AddCategory()
        {
            Console.WriteLine("Add Category");
            Console.WriteLine("1. Income Category");
            Console.WriteLine("2. Expense Category");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Choose an option: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 3)
                    return;

                Console.Write("Enter new category: ");
                string? category = Console.ReadLine();
                if (string.IsNullOrEmpty(category))
                {
                    Console.WriteLine("Category cannot be empty. Operation cancelled.");
                    return;
                }

                if (choice == 1)
                {
                    if (!incomeCategories.Contains(category))
                    {
                        incomeCategories.Insert(incomeCategories.Count - 1, category);
                        Console.WriteLine("Income category added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Income category already exists.");
                    }
                }
                else if (choice == 2)
                {
                    if (!expenseCategories.Contains(category))
                    {
                        expenseCategories.Insert(expenseCategories.Count - 1, category);
                        Console.WriteLine("Expense category added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Expense category already exists.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input, please enter a number.");
            }
        }

        // Delete a category
        public void DeleteCategory()
        {
            Console.WriteLine("Delete Category");
            Console.WriteLine("1. Income Category");
            Console.WriteLine("2. Expense Category");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Choose an option: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 3)
                    return;

                List<string> categories = choice == 1 ? incomeCategories : expenseCategories;

                Console.WriteLine("Select category to delete:");
                for (int i = 0; i < categories.Count - 1; i++) // Exclude "Other" category
                {
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                }

                if (!int.TryParse(Console.ReadLine(), out int categoryIndex) || categoryIndex < 1 || categoryIndex > categories.Count - 1)
                {
                    Console.WriteLine("Invalid category. Operation cancelled.");
                    return;
                }

                if (categories == incomeCategories)
                {
                    incomeCategories.RemoveAt(categoryIndex - 1);
                    Console.WriteLine("Income category deleted successfully.");
                }
                else
                {
                    expenseCategories.RemoveAt(categoryIndex - 1);
                    Console.WriteLine("Expense category deleted successfully.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input, please enter a number.");
            }
        }

        // Calculate investment return
        public void CalculateInvestmentReturn()
        {
            Console.Write("Enter initial investment amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal principal))
            {
                Console.WriteLine("Invalid amount. Operation cancelled.");
                return;
            }

            Console.Write("Enter annual interest rate (in %): ");
            if (!double.TryParse(Console.ReadLine(), out double rate))
            {
                Console.WriteLine("Invalid interest rate. Operation cancelled.");
                return;
            }

            Console.Write("Enter number of years: ");
            if (!int.TryParse(Console.ReadLine(), out int years))
            {
                Console.WriteLine("Invalid number of years. Operation cancelled.");
                return;
            }

            decimal futureValue = principal * (decimal)Math.Pow(1 + rate / 100, years);
            decimal profit = futureValue - principal;

            Console.WriteLine($"Future value: {futureValue:C}");
            Console.WriteLine($"Profit: {profit:C}");
        }

        // View transaction history
        public void ViewTransactionHistory()
        {
            Console.WriteLine("Transaction History:");
            var sortedTransactions = transactions.OrderBy(t => t.Date);
            foreach (var transaction in sortedTransactions)
            {
                Console.WriteLine($"{transaction.Date}: {transaction.Description} - {transaction.Amount:C} ({transaction.Category})");
            }
        }
    }

    // Class to represent a financial transaction
    class Transaction
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"{Amount}|{Description}|{Category}|{Date}";
        }

        public static Transaction FromString(string str)
        {
            var parts = str.Split('|');
            return new Transaction
            {
                Amount = decimal.Parse(parts[0]),
                Description = parts[1],
                Category = parts[2],
                Date = DateTime.Parse(parts[3])
            };
        }
    }
}
