-- DATABASE MANIPULATION
CREATE TABLE EXERCICE(
CodEXER INT primary key, 
DatBED DATE,
DATFIN DATE);

CREATE TABLE Clients(
ID INT IDENTITY(1, 1) primary key,
FullName varchar(30) NOT NULL,
phone varchar(20),
email varchar(30),
Address_Name varchar(70),
city varchar(20),
country varchar(20) Not null);

CREATE TABLE BRANCH(
BranchID int primary key,
BranchName varchar(10) not null,
city varchar(18) not null,
country varchar(18) not null,
phone varchar(20) not null);

CREATE TABLE EMPLOYEE(
Emp_Matricule INT primary key, 
FullName varchar(45) not null,
Position varchar(12) not null,
HireDate Date,
Salary float(3) Not null,
BranchID INT CONSTRAINT FK_BREMP FOREIGN KEY (BranchID) REFERENCES BRANCH (BranchID));

CREATE TABLE Credit_Card(
RIB Varchar(20) primary key,
Expiration_date varchar(5),
cvv INT CONSTRAINT CHK_cvv_len3 CHECK (LEN(cvv) = 3 AND CVV NOT LIKE '%[^0-9]%' ),
ID INT CONSTRAINT FK_IDClient FOREIGN KEY (ID) REFERENCES Clients (ID));

CREATE TABLE BNK_Acc(
IDACC INT PRIMARY KEY,
RIB Varchar(20),
Balance DECIMAL(15,2) DEFAULT 0 not null,
Max_Withdraw DECIMAL(15,2),
Acc_Status char(1) CONSTRAINT CK_STATUS CHECK (Acc_Status = 'O' or Acc_Status = 'C') ,
Constraint FK_RIMCRD_BL FOREIGN KEY (RIB) REFERENCES Credit_Card (RIB)
);

CREATE TABLE Transactions(
RIB Varchar(20) CONSTRAINT FK_RIB_BA2TRAN FOREIGN KEY (RIB) REFERENCES Credit_Card (RIB),
RIB_TO   VARCHAR(20) NULL CONSTRAINT FK_RIB_TO FOREIGN KEY REFERENCES Credit_Card(RIB),
Amount DECIMAL(15,2) not  null, 
Tran_Type char(1) CONSTRAINT CK_WD CHECK (Tran_Type = 'W' or Tran_Type = 'D' or Tran_Type = 'T') not null,
Tran_Date DATETIME DEfault GETDATE());

CREATE TABLE LOAN(
LOANID INT primary key,
LoanType varchar(1) not null,
LoanAmount float(3) not null,
Loan_Status char(1) NOT NULL CHECK (Loan_Status IN ('A', 'R', 'P')),
Client_Id INT CONSTRAINT FK_CLLOAN FOREIGN KEY (Client_Id) REFERENCES Clients (ID));

CREATE TABLE LOAN_PAYMENT(
PAYMENTID INT primary key,
PaymentDte Date,
AmountPaid DECIMAL(15,2) not null,
RmainingBalance DECIMAL(15,2) not null,
LoanID INT CONSTRAINT FK_LOANPAY FOREIGN KEY (LoanID) REFERENCES LOAN (LOANID));


-- INSERTING DATA

-- Insert into EXERCICE
INSERT INTO EXERCICE (CodEXER, DatBED, DATFIN) VALUES
(2025, '2025-01-01', '2025-12-31'),
(2024, '2024-01-01', '2024-12-31');

-- Insert into Clients
INSERT INTO Clients (FullName, phone, email, Address_Name, city, country) VALUES
('John Smith', '+1-202-555-0174', 'john.smith@email.com', '123 Maple Street', 'New York', 'USA'),
('Emma Johnson', '+44-20-7946-0855', 'emma.johnson@email.co.uk', '45 High Street', 'London', 'UK'),
('Ali Ben Salah', '+216-98-456-789', 'ali.bensalah@email.tn', '12 Avenue Habib Bourguiba', 'Tunis', 'Tunisia'),
('Maria Gonzalez', '+34-91-555-0123', 'maria.gonzalez@email.es', 'Calle Mayor 56', 'Madrid', 'Spain'),
('David Kim', '+82-2-555-0147', 'david.kim@email.kr', '78 Gangnam-daero', 'Seoul', 'South Korea');

-- Insert into BRANCH
INSERT INTO BRANCH (BranchID, BranchName, city, country, phone) VALUES
(10, 'NYC01', 'New York', 'USA', '+1-202-555-0188'),
(20, 'LDN01', 'London', 'UK', '+44-20-7946-0555'),
(30, 'TUN01', 'Tunis', 'Tunisia', '+216-71-123-456');

-- Insert into EMPLOYEE
INSERT INTO EMPLOYEE (Emp_Matricule, FullName, Position, HireDate, Salary, BranchID) VALUES
(1001, 'Sarah Connor', 'Manager', '2015-03-15', 7500.00, 10),
(1002, 'James Miller', 'Teller', '2018-06-20', 3200.00, 10),
(1003, 'Ahmed Mansour', 'Manager', '2017-09-05', 6800.00, 30),
(1004, 'Laura White', 'Teller', '2019-11-01', 3000.00, 20);

-- Insert into Credit_Card
INSERT INTO Credit_Card (RIB, Expiration_date, cvv, ID) VALUES
('4532 9876 5432 1098', '12/27', 324, 6),
('4485 7654 2345 9087', '08/26', 512, 7),
('5309 8765 4321 7654', '05/28', 289, 8),
('3714 567890 12345', '11/26', 731, 9),
('6011 2345 6789 4321', '09/27', 465, 10);

-- Insert into BNK_Acc
INSERT INTO BNK_Acc (IDACC, RIB, Balance, Max_Withdraw, Acc_Status) VALUES
(2005, '6011 2345 6789 4321', 1200.50, 1500.00, 'O'),
(2007, '5309 8765 4321 7654', 8540.00, 1500.00, 'O'),
(2010, '4532 9876 5432 1098', 300.00, 1500.00, 'O');

-- Insert into Transactions
INSERT INTO Transactions (RIB, Amount, Tran_Type) VALUES
('4532 9876 5432 1098', 500.00, 'W'),
('4485 7654 2345 9087', 1000.00, 'D'),
('5309 8765 4321 7654', 200.00, 'W');

-- Insert into LOAN
INSERT INTO LOAN (LOANID, LoanType, LoanAmount, Loan_Status, Client_Id) VALUES
(1, 'H', 150000.00, 'A', 6), -- H for Home
(2, 'C', 20000.00, 'A', 9),  -- C for Car
(3, 'B', 50000.00, 'P', 10);  -- B for Business

-- Insert into LOAN_PAYMENT
INSERT INTO LOAN_PAYMENT (PAYMENTID, PaymentDte, AmountPaid, RmainingBalance, LoanID) VALUES
(1, '2025-02-01', 10000.00, 140000.00, 1),
(2, '2025-02-15', 5000.00, 15000.00, 2),
(3, '2025-03-01', 25000.00, 25000.00, 3);

-- Procedures :
USE [Banking_Management]
GO
/****** Object:  StoredProcedure [dbo].[Tan_UPD]    Script Date: 11/08/2025 20:20:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE Tan_UPD
    @RIB_From CHAR(24),  -- Account making the transaction
    @Tran_Type CHAR(1),         -- 'D', 'W', 'T'
    @Amount DECIMAL(18, 2),
    @RIB_To CHAR(24) = NULL     -- Only used for Transfers
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRY
      BEGIN TRANSACTION;

       IF TRY_CONVERT(DATE, (SELECT Expiration_date FROM Credit_Card WHERE RIB= @RIB_From), 103) < GETDATE()
          throw 5000,'credit card is already expired! Renew it to have the ability to do a transfer',1;

      IF @Tran_Type = 'D'
      BEGIN
      UPDATE BNK_Acc
      SET Balance = Balance + @Amount
      WHERE RIB =@RIB_From;
      END

      ELSE IF @Tran_Type = 'W'
      BEGIN 
         UPDATE BNK_Acc
         SET Balance = Balance - @Amount
         WHERE RIB = @RIB_From;

         --REquired
         IF (SELECT Balance FROM BNK_Acc WHERE RIB = @RIB_From) <= 0
            THROW 5001,'Insufficient funds for Withdrawwal',1;
      END


      IF @Tran_Type = 'T'
      BEGIN
         IF @RIB_To IS NULL 
            THROW 5002,'Target Account RIB is required for transfer',1;
         ELSE 
         -- Deduct from sender
         UPDATE BNK_Acc
         SET Balance = Balance - @Amount
         WHERE RIB = @RIB_From;

          --REquired
         IF (SELECT Balance FROM BNK_Acc WHERE RIB = @RIB_From) <= 0
            THROW 5001,'Insufficient funds for Withdrawwal',1;

         --Add to reciever
         UPDATE BNK_Acc
         SET Balance = Balance + @Amount
         WHERE RIB = @RIB_To;

          --REquired
          IF @@ROWCOUNT = 0
          THROW 50004, 'Receiver account not found.', 1;
      END

      IF @Tran_Type = 'T'
      BEGIN
        INSERT INTO Transactions (RIB,RIB_TO ,Tran_Type, Amount, Tran_Date)
         VALUES (@RIB_From, @RIB_To ,'D', @Amount, GETDATE()); -- record the incoming money
      END
      ELSE
      BEGIN
        -- Record the transaction in Transactions table
        INSERT INTO Transactions (RIB, RIB_TO, Tran_Type, Amount, Tran_Date)
        VALUES (@RIB_From, null ,@Tran_Type, @Amount, GETDATE());
      END

      COMMIT TRANSACTION;
      END TRY

     BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH

END;

EXEC Tan_UPD @RIB_From="4532 9876 5432 1098" , @Tran_Type='D', @Amount= 600, @RIB_To= NULL;
