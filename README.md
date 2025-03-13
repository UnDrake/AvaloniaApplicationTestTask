# **Avalonia Application Test Task**
This is an **Avalonia** application that interacts with a **MySQL** database and allows users to save UI settings.  
The application supports **saving and loading** values from the database, as well as generating random shapes.

---

## **Features**
- **Loads the last saved state** on startup.
- **Saves changes** (slider value, numeric input, checkbox, radio button, dropdown selection).
- **Generates a random shape** when clicking the *"Generate Graph"* button.
- **Automatically updates the last record in the database** instead of creating new entries.

---

## **Prerequisites**
### **1. Install MySQL**
If MySQL is not installed, download and install it from:
- [**Download MySQL Community Server**](https://dev.mysql.com/downloads/mysql/)
- Start **MySQL Server** and configure the `root` user with a password.

### **2. Connect to MySQL**
Use **MySQL Shell** or **phpMyAdmin** to execute the following commands.

---

## **Database Setup**
### **1. Create the Database**
```sql
CREATE DATABASE TestDB;
```

### **2. Create the Settings Table**
```sql
USE TestDB;

CREATE TABLE Settings (
    id INT PRIMARY KEY AUTO_INCREMENT,
    slider_value INT NOT NULL,
    numeric_value DECIMAL(10,1) NOT NULL,
    checkbox_state BOOLEAN NOT NULL,
    selected_radio VARCHAR(10) NOT NULL,
    selected_combo VARCHAR(50) NOT NULL
);
```

### **3. Create the ComboBoxItems Table**
```sql
CREATE TABLE ComboBoxItems (
    id INT PRIMARY KEY AUTO_INCREMENT,
    item_name VARCHAR(50) NOT NULL
);
```

### **4. Populate ComboBoxItems with Initial Values**
```sql
INSERT INTO ComboBoxItems (item_name) VALUES
('Опція 1'),
('Опція 2'),
('Опція 3');
```

## **Update the Database Connection**
Open the DatabaseService.cs file and update the connection string:
```
private readonly string _connectionString = "Server=localhost;Database=TestDB;User=root;Password=YOUR_PASSWORD;";
```
