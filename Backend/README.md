PostgreSQL is a powerful, open-source object-relational database system with a strong reputation for reliability, feature robustness, and performance.
Table of Contents

    System Requirements
    Step 1: Update the System
    Step 2: Install PostgreSQL
    Step 3: Access the PostgreSQL CLI
    Step 4: Configure PostgreSQL (Local Use)
    Step 5: Create a Database and Table
    Step 6: Manage PostgreSQL Services
    Step 7: Test the Setup
    Conclusion

System Requirements

    Operating System: Linux (Ubuntu 20.04 or later recommended)
    Processor: 1 GHz or faster
    Memory: 1 GB RAM or more
    Disk Space: Minimum 100 MB for PostgreSQL installation

Step 1: Update the System

Before installing PostgreSQL, ensure your system is up-to-date. Run the following commands:

sudo apt update
sudo apt upgrade -y

Step 2: Install PostgreSQL

Install PostgreSQL and its additional utilities:

sudo apt install postgresql postgresql-contrib -y

Verify the installation by checking the service status:

sudo systemctl status postgresql

    Note: The status should display active (running).

Step 3: Access the PostgreSQL CLI

    Switch to the default PostgreSQL user:

sudo -u postgres psql

Check the connection details:

\conninfo

Exit the PostgreSQL terminal:

    \q

Step 4: Configure PostgreSQL (Local Use)

For local use, PostgreSQL is pre-configured. No additional configuration is needed unless specific settings are required.
Step 5: Create a Database and Table

Follow these steps to set up a database and table:

    Access the PostgreSQL terminal:

sudo -u postgres psql

Create a new database:

CREATE DATABASE my_database;

Switch to the new database:

\c my_database

Create a users table:

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50),
    email VARCHAR(100)
);

Insert a sample record:

INSERT INTO users (name, email) VALUES ('Alice', 'alice@example.com');

Query the table:

    SELECT * FROM users;

Step 6: Manage PostgreSQL Services
