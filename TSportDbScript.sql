if exists (select [name]
from sys.databases
where [name] = N'TSportDb')

begin
  use master;
  drop database TSportDb;
end

go

create database TSportDb;
go

use TSportDb;

go
create table Account
(
  Id int identity(1,1) not null,
  Username nvarchar(255),
  Email nvarchar(255) not null,
  [Password] nvarchar(max) not null,
  FirstName nvarchar(255),
  LastName nvarchar(255),
  Gender nvarchar(10),
  [Address] nvarchar(255),
  Phone nvarchar(50),
  Dob date,
  -- RefreshToken nvarchar(max),
  -- RefreshTokenExpiryTime datetime,
  SupabaseId nvarchar(max) not null,
  [Role] nvarchar(50) not null,
  [Status] nvarchar(100),

  -- Keys
  primary key (Id)
);

create table [Order]
(
  Id int identity(1,1) not null,
  Code nvarchar(255),
  OrderDate datetime not null default GETDATE(),
  [Status] nvarchar(100) not null,
  Total money not null default 0,
  CreatedDate datetime not null default GETDATE(),
  CreatedAccountId int not null,
  ModifiedDate datetime null,
  ModifiedAccountId int null,

  -- Keys
  primary key (Id),
  foreign key (CreatedAccountId) references dbo.Account(Id),
  foreign key (ModifiedAccountId) references dbo.Account(Id),
);


create table Payment
(
  Id int identity(1,1) not null,
  Code nvarchar(255),
  PaymentMethod nvarchar(255),
  PaymentName nvarchar(255),
  Amount money not null,
  [Status] nvarchar(100) not null,
  OrderId int null,
  CreatedDate datetime not null default GETDATE(),
  CreatedAccountId int not null,
  ModifiedDate datetime null,
  ModifiedAccountId int null,

  -- Keys
  primary key(Id),
  foreign key (OrderId) references dbo.[Order](Id),
  foreign key (CreatedAccountId) references dbo.Account(Id),
  foreign key (ModifiedAccountId) references dbo.Account(Id),
);

create table Club
(
  Id int identity(1,1) not null,
  Code nvarchar(255),
  [Name] nvarchar(255) not null,
  LogoUrl nvarchar(max),
  [Status] nvarchar(100) not null,
  CreatedDate datetime not null default GETDATE(),
  CreatedAccountId int not null,
  ModifiedDate datetime null,
  ModifiedAccountId int null,

  -- Keys
  primary key (Id),
  foreign key (CreatedAccountId) references dbo.Account(Id),
  foreign key (ModifiedAccountId) references dbo.Account(Id),
);

create table Player
(
  Id int identity(1,1) not null,
  Code nvarchar(255),
  [Name] nvarchar(255) not null,
  [Status] nvarchar(100) not null,
  ClubId int null,
  CreatedDate datetime not null default GETDATE(),
  CreatedAccountId int not null,
  ModifiedDate datetime null,
  ModifiedAccountId int null,


  -- Keys
  primary key (Id),
  foreign key (CreatedAccountId) references dbo.Account(Id),
  foreign key (ModifiedAccountId) references dbo.Account(Id),
  foreign key (ClubId) references dbo.Club(Id)
);

create table Season
(
  Id int identity(1,1) not null,
  Code nvarchar(255),
  [Name] nvarchar(255) not null,
  ClubId int null,
  CreatedDate datetime not null default GETDATE(),
  CreatedAccountId int not null,
  ModifiedDate datetime null,
  ModifiedAccountId int null,
  [Status] nvarchar(100) not null

  -- Keys
  primary key (Id),
  foreign key (ClubId) references dbo.Club(Id),
  foreign key (CreatedAccountId) references dbo.Account(Id),
  foreign key (ModifiedAccountId) references dbo.Account(Id),
);

create table SeasonPlayer
(
  Id int identity(1,1) not null,
  SeasonId int not null,
  PlayerId int not null

    -- Keys
  primary key (Id),
  foreign key (SeasonId) references dbo.Season(Id),
  foreign key (PlayerId) references dbo.Player(Id)
);

create table ShirtEdition
(
  Id int identity(1,1) not null,
  Code nvarchar(255),
  Size nvarchar(10) not null,
  HasSignature bit not null default 0,
  StockPrice money not null,
  DiscountPrice money,
  Color nvarchar(255),
  [Status] nvarchar(100) not null,
  Origin nvarchar(255),
  Quantity int not null,
  Material nvarchar(255),
  SeasonId int not null,
  CreatedDate datetime not null default GETDATE(),
  CreatedAccountId int not null,
  ModifiedDate datetime null,
  ModifiedAccountId int null,

  -- Keys
  primary key (Id),
  foreign key (SeasonId) references dbo.Season(Id),
  foreign key (CreatedAccountId) references dbo.Account(Id),
  foreign key (ModifiedAccountId) references dbo.Account(Id)
);

create table Shirt
(
  Id int identity(1,1) not null,
  Code nvarchar(255) not null,
  [Name] nvarchar(255) not null,
  [Description] nvarchar(255),
  Quantity int,
  [Status] nvarchar(100) not null,
  ShirtEditionId int not null,
  SeasonPlayerId int not null,
  CreatedDate datetime not null default GETDATE(),
  CreatedAccountId int not null,
  ModifiedDate datetime null,
  ModifiedAccountId int null,

  -- Keys
  primary key (Id),
  foreign key (CreatedAccountId) references dbo.Account(Id),
  foreign key (ModifiedAccountId) references dbo.Account(Id),
  foreign key (ShirtEditionId) references dbo.ShirtEdition(Id),
  foreign key (SeasonPlayerId) references dbo.SeasonPlayer(Id)
);

create table OrderDetail
(
  OrderId int not null,
  ShirtId int not null,
  Code nvarchar(255),
  Subtotal money not null default 0,
  Quantity int not null default 0,
  Size nvarchar(100) not null,
  [Status] nvarchar(100),

  -- Keys
  primary key (OrderId, ShirtId),
  foreign key (OrderId) references dbo.[Order](Id),
  foreign key (ShirtId) references dbo.Shirt(Id)
);

create table [Image]
(
  Id int identity(1,1) not null,
  [Url] nvarchar(max),
  ShirtId int not null,

  primary key (Id),
  foreign key (ShirtId) references dbo.Shirt(Id)
);

---- Insert data into Account table
INSERT INTO Account
  (Email, [Password], [Role], [Status], SupabaseId)
VALUES
  ('staff@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Staff', 'Active', '49ab1ee8-4b75-41da-b831-cea19d171406'),
  ('admin@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Admin', 'Active', '85f1eb98-25ed-4adc-a226-2846e06a5a7d'),
  ('minh@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Customer', 'Active', '580b1b9e-c395-467c-a4e8-ce48c0ec09d1'),
  ('kiritominhswordartonline@gmail.com','548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Customer', 'Active', 'b5910ce2-7c6a-4d4b-aaa0-357e32033ff3');

-- Insert data into Club table
INSERT INTO Club
  (Code, [Name], LogoUrl, [Status], CreatedAccountId)
VALUES
  ('CLB001', 'Real Madrid', 'https://upload.wikimedia.org/wikipedia/en/thumb/5/56/Real_Madrid_CF.svg/1200px-Real_Madrid_CF.svg.png', 'Active', 1),
  ('CLB002', 'Barcelona', 'https://upload.wikimedia.org/wikipedia/vi/thumb/9/91/FC_Barcelona_logo.svg/220px-FC_Barcelona_logo.svg.png', 'Active', 1),
  ('CLB003', 'Manchester United', 'https://upload.wikimedia.org/wikipedia/vi/a/a1/Man_Utd_FC_.svg', 'Active', 1);

-- Insert data into Player table
INSERT INTO Player
  (Code, [Name], [Status], ClubId, CreatedAccountId)
VALUES
  ('PLY001', 'Lionel Messi', 'Active', 2, 1),
  ('PLY002', 'Cristiano Ronaldo', 'Active', 1, 1),
  ('PLY003', 'Neymar', 'Active', 2, 1),
  ('PLY004', 'Kylian Mbappe', 'Active', 2, 1),
  ('PLY005', 'Robert Lewandowski', 'Active', 2, 1);

-- Insert data into Season table
INSERT INTO Season
  (Code, [Name], ClubId, CreatedAccountId, [Status])
VALUES
  ('SES001', '2022/2023', 1, 1, 'Active'),
  ('SES002', '2021/2022', 1, 1, 'Active'),
  ('SES003', '2022/2023', 2, 1, 'Active'),
  ('SES004', '2021/2022', 2, 1, 'Active');

-- Insert data into SeasonPlayer table
INSERT INTO SeasonPlayer
  (SeasonId, PlayerId)
VALUES
  (1, 2),
  (1, 3),
  (3, 1),
  (3, 4),
  (3, 5);

-- Insert data into ShirtEdition table
INSERT INTO ShirtEdition
  (Code, Size, HasSignature, StockPrice, DiscountPrice, Color, [Status], Origin, Material, SeasonId, CreatedAccountId, Quantity)
VALUES
  ('SE001', 'S', 0, 299000.00,259000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 34),
  ('SE002', 'M', 0, 299000.00,259000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 45),
  ('SE003', 'L', 0, 299000.00,259000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 67),
  ('SE004', 'XL', 0, 299000.00,259000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 87),
  ('SE005', 'S', 1, 399000.00,359000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 76),
  ('SE006', 'M', 1, 399000.00,359000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 32),
  ('SE007', 'L', 1, 399000.00,359000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 15),
  ('SE008', 'XL', 1, 399000.00,359000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 54);

-- Insert data into Shirt table
INSERT INTO Shirt
  (Code, [Name], [Description], [Status], ShirtEditionId, SeasonPlayerId, CreatedAccountId, Quantity)
VALUES
  ('SRT001', 'Real Madrid Home Jersey','Real Madrid Home Jersey', 'Active', 1, 1, 1, 32),
  ('SRT002', 'Real Madrid Home Jersey','Real Madrid Home Jersey', 'Active', 2, 1, 1, 45),
  ('SRT003', 'Real Madrid Home Jersey','Real Madrid Home Jersey', 'Active', 3, 1, 1, 23),
  ('SRT004', 'Real Madrid Home Jersey','Real Madrid Home Jersey', 'Active', 4, 1, 1, 43),
  ('SRT005', 'Barcelona Home Jersey','Barcelona Home Jersey', 'Active', 5, 3, 1, 56),
  ('SRT006', 'Barcelona Home Jersey','Barcelona Home Jersey', 'Active', 6, 3, 1, 23),
  ('SRT007', 'Barcelona Home Jersey','Barcelona Home Jersey', 'Active', 7, 3, 1, 30),
  ('SRT008', 'Barcelona Home Jersey','Barcelona Home Jersey', 'Active', 8, 3, 1, 32);

insert into dbo.[Image](ShirtId, [Url])
values(1, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/TSport/shirt.jpg'),
	 (2, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg'),
	 (3, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg'),
	 (4, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/TSport/shirt.jpg'),
	 (5, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg'),
	 (6, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg'),
	 (7, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg'),
	 (8, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg')

insert into dbo.[Order](Code,OrderDate,[Status],Total,CreatedAccountId)
values('OD8d29677b-f8b9-429e-b0b6-0cee511aeef1','2024-07-18 12:45:42.067','Pending',777000.00,4);
Insert data into Order table
INSERT INTO [Order] (Id, Code, OrderDate, [Status], Total, CreatedAccountId)
VALUES
  (1, 'ORD001', '2023-04-01 10:30:00', 'Processed', 159.98, 2),
  (2, 'ORD002', '2023-04-15 15:45:00', 'Shipped', 199.98, 2),
  (3, 'ORD003', '2023-05-01 12:00:00', 'Pending', 99.99, 1);

---- Insert data into Payment table
--INSERT INTO Payment (Id, Code, PaymentMethod, PaymentName, [Status], OrderId, CreatedAccountId)
--VALUES
--  (1, 'PAY001', 'Credit Card', 'Visa ending in 1234', 'Paid', 1, 2),
--  (2, 'PAY002', 'PayPal', 'user2@example.com', 'Paid', 2, 2),
--  (3, 'PAY003', 'Credit Card', 'Mastercard ending in 5678', 'Pending', 3, 1);

---- Insert data into OrderDetail table
--INSERT INTO OrderDetail (OrderId, ShirtId, Code, Subtotal, Quantity, [Status])
--VALUES
--  (1, 1, 'OD001', 79.99, 1, 'Fulfilled'),
--  (1, 2, 'OD002', 79.99, 1, 'Fulfilled'),
--  (2, 5, 'OD003', 99.99, 1, 'Fulfilled'),
--  (2, 6, 'OD004', 99.99, 1, 'Fulfilled'),
--  (3, 7, 'OD005', 99.99, 1, 'Pending');