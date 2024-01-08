CREATE DATABASE REC
ON (NAME = 'Rec_Data',
FILENAME = 'F:\Rec_Data.mdf',
SIZE = 20 MB,
FILEGROWTH = 0)
LOG ON (NAME = 'Rec_Log',
FILENAME = 'F:\Rec_Data.ldf',
SIZE = 5 MB,
FILEGROWTH = 0)


CREATE TABLE Client(
  ID_Client int IDENTITY(1,1) NOT NULL,
  Fio varchar(70) NOT NULL,
  Number varchar(20) NOT NULL,
  PRIMARY KEY (ID_Client)
  )



INSERT INTO Client VALUES
('Петрова Ангелина Семёновна','7440056246'),
('Кольцов Мирон Львович','70684710123'),
('Харитонова Варвара Данииловна','71348230683'),
('Павлов Адам Арсентьевич','799411448332'),
('Иванов Марк Даниилович','7538689024'),
('Лобанов Тимофей Валерьевич','757822061475'),
('Князев Лука Егорович','72943778006'),
('Горбунов Владимир Вячеславаович','79244781694');

select * from OrderRec

CREATE TABLE OrderRec(
  id int IDENTITY(1,1) NOT NULL,
  Product varchar(40) NOT NULL,
  Stat varchar(40) NOT NULL,
  Price varchar(20) NOT NULL,
  ID_Client int NOT NULL,
  Date_Inst date 
  PRIMARY KEY (id)
  )





CREATE TABLE Users(
id_User int IDENTITY(1,1) NOT NULL,
  User_Login varchar(70) NOT NULL,
  User_Password varchar(20) NOT NULL,
  )

  INSERT INTO Users VALUES
('zxc','zxc');

select * from OrderRec