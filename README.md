
# Personal Financial Planner

A console-based application for personal financial planning. This application allows users to keep track of their transactions, income, and expenses by category, view their current balance, view transactions by category, manage categories, and calculate potential investment returns.

## Installation

1. **Clone the repository:**
   ```sh
   git clone https://github.com/glaukaliu/PersonalFinancePlanner.git
   cd PersonalFinancePlanner
   ```

2. **Build the project:**
   ```sh
   dotnet build
   ```

3. **Run the application:**
   ```sh
   dotnet run
   ```

## Usage

When you run the application, you will see the main menu with the following options:

1. **Add Transaction**:
    - **Submenu**:
      - 1. Income
      - 2. Expense
      - 3. Back to Main Menu
    - Choose whether the transaction is an income or an expense.
    - Enter the transaction amount and description.
    - Select a category from the provided list.
    - The transaction will be added to the list of transactions, and you will see a confirmation message.

2. **View Current Balance**:
    - Displays the current balance by summing all transactions.

3. **View Transactions by Category**:
    - **Submenu**:
      - 1. Income
      - 2. Expense
      - 3. Back to Main Menu
    - Choose whether you want to view income or expense transactions.
    - Select a category to view all transactions within that category.
    - The transactions for the selected category will be displayed.

4. **Manage Categories**:
    - **Submenu**:
      - 1. View Expense Categories
      - 2. View Income Categories
      - 3. Add Category
      - 4. Delete Category
      - 5. Back to Main Menu
    - View existing income and expense categories.
    - Add a new category by providing its name.
    - Delete an existing category, except for the default "Other" category.

5. **Calculate Investment Return**:
    - Enter the initial investment amount, annual interest rate, and number of years.
    - The application calculates and displays the future value and profit of the investment.

6. **View Transaction History**:
    - Displays all transactions sorted by date.

7. **Exit**:
    - Saves all data to local files and exits the application.

Throughout the usage of the application, you will be guided by prompts and messages to ensure a smooth experience.


## License

[MIT](https://choosealicense.com/licenses/mit/)

### Documentation

#### Project Structure

- **Program.cs**: This is the entry point of the application. It contains the main loop that displays the menu and handles user input.
- **FinanceManager.cs**: This class handles all financial operations, including adding transactions, viewing the balance, managing categories, and calculating investment returns. It also manages the loading and saving of data to local files.
- **Transaction.cs**: This class represents a financial transaction. It includes properties for the amount, description, category, and date of the transaction. The class also includes methods for converting a transaction to and from a string format for easy file storage.category, and date.

#### Key Methods

- `LoadData()`: Loads transactions and categories from local files.
- `SaveData()`: Saves transactions and categories to local files.
- `AddTransaction()`: Prompts the user to add a new transaction.
- `ViewCurrentBalance()`: Displays the current balance.
- `ViewTransactionsByCategory()`: Displays transactions filtered by category.
- `ManageCategories()`: Allows the user to add, view, and delete categories.
- `CalculateInvestmentReturn()`: Calculates and displays the future value and profit of an investment.
- `ViewTransactionHistory()`: Displays all transactions sorted by date. -->

#### Data Handling

- **Persistence**: Transaction and category data are stored in simple text files. When the program starts, it reads from these files, and it saves back to them when it exits.
- **Transaction Format**: Each transaction is stored as a string in the format `Amount|Description|Category|Date`.

