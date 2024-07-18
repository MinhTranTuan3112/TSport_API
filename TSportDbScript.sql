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

-- Insert data into Account table
INSERT INTO Account
  (Email, [Password], [Role], [Status], SupabaseId)
VALUES
  ('staff@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Staff', 'Active', '49ab1ee8-4b75-41da-b831-cea19d171406'),
  ('admin@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Admin', 'Active', '85f1eb98-25ed-4adc-a226-2846e06a5a7d'),
  ('customer1@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Customer', 'Active', '580b1b9e-c395-467c-a4e8-ce48c0ec09d1'),
  ('customer2@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Customer', 'Active', 'b5910ce2-7c6a-4d4b-aaa0-357e32033ff3'),
  ('staff2@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Staff', 'Active', '59ab1ee8-4b75-41da-b831-cea19d171406'),
  ('admin2@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Admin', 'Active', '95f1eb98-25ed-4adc-a226-2846e06a5a7d'),
  ('customer3@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Customer', 'Active', '680b1b9e-c395-467c-a4e8-ce48c0ec09d1'),
  ('customer4@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Customer', 'Active', 'c5910ce2-7c6a-4d4b-aaa0-357e32033ff3'),
  ('staff3@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Staff', 'Active', '79ab1ee8-4b75-41da-b831-cea19d171406'),
  ('admin3@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Admin', 'Active', 'a5f1eb98-25ed-4adc-a226-2846e06a5a7d'),
  ('customer5@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Customer', 'Active', '780b1b9e-c395-467c-a4e8-ce48c0ec09d1'),
  ('customer6@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Customer', 'Active', 'd5910ce2-7c6a-4d4b-aaa0-357e32033ff3'),
  ('staff4@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Staff', 'Active', '89ab1ee8-4b75-41da-b831-cea19d171406'),
  ('admin4@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Admin', 'Active', 'b5f1eb98-25ed-4adc-a226-2846e06a5a7d');

-- Insert data into Club table
INSERT INTO Club
  (Code, [Name], LogoUrl, [Status], CreatedAccountId)
VALUES
  ('CLB001', 'Manchester City', 'https://upload.wikimedia.org/wikipedia/en/e/eb/Manchester_City_FC_badge.svg', 'Active', 1),
  ('CLB002', 'Real Madrid', 'https://upload.wikimedia.org/wikipedia/en/thumb/5/56/Real_Madrid_CF.svg/1200px-Real_Madrid_CF.svg.png', 'Active', 1),
  ('CLB003', 'Bayern Munich', 'https://www.thesportsdb.com/images/media/team/badge/r40m521686633480.png', 'Active', 1),
  ('CLB004', 'Paris Saint-Germain', 'https://upload.wikimedia.org/wikipedia/en/thumb/a/a7/Paris_Saint-Germain_F.C..svg/1200px-Paris_Saint-Germain_F.C..svg.png', 'Active', 1),
  ('CLB005', 'Barcelona', 'https://upload.wikimedia.org/wikipedia/en/thumb/4/47/FC_Barcelona_(crest).svg/1200px-FC_Barcelona_(crest).svg.png', 'Active', 1),
  ('CLB006', 'Manchester United', 'https://upload.wikimedia.org/wikipedia/en/7/7a/Manchester_United_FC_crest.svg', 'Active', 1),
  ('CLB007', 'Liverpool', 'https://upload.wikimedia.org/wikipedia/en/0/0c/Liverpool_FC.svg', 'Active', 1),
  ('CLB008', 'Chelsea', 'https://upload.wikimedia.org/wikipedia/en/c/cc/Chelsea_FC.svg', 'Active', 1),
  ('CLB009', 'Juventus', 'https://aobongda.vn/wp-content/uploads/2024/05/Logo-FC-Juventus.png', 'Active', 1),
  ('CLB010', 'Inter Milan', 'https://upload.wikimedia.org/wikipedia/commons/0/05/FC_Internazionale_Milano_2021.svg', 'Active', 1),
  ('CLB011', 'Tottenham Hotspur', 'https://upload.wikimedia.org/wikipedia/en/b/b4/Tottenham_Hotspur.svg', 'Active', 1),
  ('CLB012', 'Atletico Madrid', 'https://e7.pngegg.com/pngimages/304/16/png-clipart-atletico-madrid-fi-collection-football-player-manchester-city-f-c-crest-logo-flag-logo.png', 'Active', 1),
  ('CLB013', 'AC Milan', 'https://upload.wikimedia.org/wikipedia/commons/d/d0/Logo_of_AC_Milan.svg', 'Active', 1),
  ('CLB014', 'Arsenal', 'https://upload.wikimedia.org/wikipedia/en/5/53/Arsenal_FC.svg', 'Active', 1),
  ('CLB015', 'Borussia Dortmund', 'https://upload.wikimedia.org/wikipedia/commons/6/67/Borussia_Dortmund_logo.svg', 'Active', 1),
  ('CLB016', 'Sevilla', 'https://w7.pngwing.com/pngs/619/955/png-transparent-real-madrid-c-f-sevilla-fc-la-liga-fc-barcelona-el-clasico-fc-barcelona-sport-heart-logo.png', 'Active', 1),
  ('CLB017', 'Lazio', 'https://w7.pngwing.com/pngs/698/443/png-transparent-s-s-lazio-logo-dream-league-soccer-s-s-lazio-youth-sector-serie-a-a-s-roma-1000-text-logo-juventus-fc-thumbnail.png', 'Active', 1),
  ('CLB018', 'Napoli', 'https://logowik.com/content/uploads/images/ssc-napoli3874.logowik.com.webp', 'Active', 1),
  ('CLB019', 'Roma', 'https://logowik.com/content/uploads/images/t_as-roma5337.jpg', 'Active', 1),
  ('CLB020', 'RB Leipzig', 'https://upload.wikimedia.org/wikipedia/en/0/04/RB_Leipzig_2014_logo.svg', 'Active', 1);

-- Insert data into Player table
INSERT INTO Player
  (Code, [Name], [Status], ClubId, CreatedAccountId)
VALUES
  ('PLY001', 'Kylian Mbappé', 'Active', 4, 1),
  ('PLY002', 'Lionel Messi', 'Active', 5, 1),
  ('PLY003', 'Cristiano Ronaldo', 'Active', 6, 1),
  ('PLY004', 'Kevin De Bruyne', 'Active', 1, 1),
  ('PLY005', 'Robert Lewandowski', 'Active', 5, 1),
  ('PLY006', 'Neymar', 'Active', 4, 1),
  ('PLY007', 'Erling Haaland', 'Active', 1, 1),
  ('PLY008', 'Mohamed Salah', 'Active', 7, 1),
  ('PLY009', 'Karim Benzema', 'Active', 2, 1),
  ('PLY010', 'Harry Kane', 'Active', 3, 1),
  ('PLY011', 'Son Heung-min', 'Active', 11, 1),
  ('PLY012', 'Antoine Griezmann', 'Active', 12, 1),
  ('PLY013', 'Zlatan Ibrahimović', 'Active', 13, 1),
  ('PLY014', 'Pierre-Emerick Aubameyang', 'Active', 14, 1),
  ('PLY015', 'Jadon Sancho', 'Active', 15, 1),
  ('PLY016', 'Ivan Rakitic', 'Active', 16, 1),
  ('PLY017', 'Ciro Immobile', 'Active', 17, 1),
  ('PLY018', 'Dries Mertens', 'Active', 18, 1),
  ('PLY019', 'Edin Džeko', 'Active', 19, 1),
  ('PLY020', 'Timo Werner', 'Active', 20, 1);

-- Insert data into Season table
INSERT INTO Season
  (Code, [Name], ClubId, CreatedAccountId, [Status])
VALUES
  ('SES001', '2022/2023', 1, 1, 'Active'),
  ('SES002', '2021/2022', 1, 1, 'Active'),
  ('SES003', '2020/2021', 2, 1, 'Active'),
  ('SES004', '2019/2020', 2, 1, 'Active'),
  ('SES005', '2018/2019', 3, 1, 'Active'),
  ('SES006', '2017/2018', 3, 1, 'Active'),
  ('SES007', '2016/2017', 4, 1, 'Active'),
  ('SES008', '2015/2016', 4, 1, 'Active'),
  ('SES009', '2014/2015', 5, 1, 'Active'),
  ('SES010', '2013/2014', 5, 1, 'Active'),
  ('SES011', '2022/2023', 6, 1, 'Active'),
  ('SES012', '2021/2022', 6, 1, 'Active'),
  ('SES013', '2020/2021', 7, 1, 'Active'),
  ('SES014', '2019/2020', 7, 1, 'Active'),
  ('SES015', '2018/2019', 8, 1, 'Active'),
  ('SES016', '2017/2018', 8, 1, 'Active'),
  ('SES017', '2016/2017', 9, 1, 'Active'),
  ('SES018', '2015/2016', 9, 1, 'Active'),
  ('SES019', '2014/2015', 10, 1, 'Active'),
  ('SES020', '2013/2014', 10, 1, 'Active');

-- Insert data into SeasonPlayer table
INSERT INTO SeasonPlayer
  (SeasonId, PlayerId)
VALUES
  (1, 1),
  (1, 2),
  (2, 3),
  (2, 4),
  (3, 5),
  (3, 6),
  (4, 7),
  (4, 8),
  (5, 9),
  (5, 10),
  (6, 11),
  (6, 12),
  (7, 13),
  (7, 14),
  (8, 15),
  (8, 16),
  (9, 17),
  (9, 18),
  (10, 19),
  (10, 20),
  (1, 11),
  (1, 12),
  (2, 13),
  (2, 14),
  (3, 15),
  (3, 16),
  (4, 17),
  (4, 18),
  (5, 19),
  (5, 20),
  (6, 1),
  (6, 2),
  (7, 3),
  (7, 4),
  (8, 5),
  (8, 6),
  (9, 7),
  (9, 8),
  (10, 9),
  (10, 10),
  (1, 3),
  (1, 4),
  (2, 5),
  (2, 6),
  (3, 7),
  (3, 8),
  (4, 9),
  (4, 10),
  (5, 11),
  (5, 12),
  (6, 13),
  (6, 14),
  (7, 15),
  (7, 16),
  (8, 17),
  (8, 18),
  (9, 19),
  (9, 20),
  (10, 1),
  (10, 2);

-- Insert data into ShirtEdition table
INSERT INTO ShirtEdition
  (Code, Size, HasSignature, StockPrice, DiscountPrice, Color, [Status], Origin, Material, SeasonId, CreatedAccountId, Quantity)
VALUES
  ('SE001', 'S', 0, 329000,286000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 34),
  ('SE002', 'M', 0, 312000,259000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 45),
  ('SE003', 'L', 0, 360000,280000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 67),
  ('SE004', 'XL', 0, 520000,500000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 87),
  ('SE005', 'S', 1, 650000,450000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 76),
  ('SE006', 'M', 1, 660000,620000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 32),
  ('SE007', 'L', 1, 250000,100000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 15),
  ('SE008', 'XL', 1, 270000,260000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 54),
  ('SE009', 'S', 0, 280000,269000, 'White', 'Active', 'Made in Spain', 'Polyester', 5, 1, 30),
  ('SE010', 'M', 0, 299000,285000, 'White', 'Active', 'Made in Spain', 'Polyester', 5, 1, 40),
  ('SE011', 'S', 0, 300000,256000, 'Yellow', 'Active', 'Made in Germany', 'Polyester', 6, 1, 34),
  ('SE012', 'M', 0, 420000,320000, 'Yellow', 'Active', 'Made in Germany', 'Polyester', 6, 1, 45),
  ('SE013', 'L', 0, 550000,510000, 'Yellow', 'Active', 'Made in Germany', 'Polyester', 6, 1, 67),
  ('SE014', 'XL', 0, 580000,250000, 'Yellow', 'Active', 'Made in Germany', 'Polyester', 6, 1, 87),
  ('SE015', 'S', 1, 590000,501000, 'Green', 'Active', 'Made in Italy', 'Cotton', 8, 1, 76),
  ('SE016', 'M', 1, 609000,426000, 'Green', 'Active', 'Made in Italy', 'Cotton', 8, 1, 32),
  ('SE017', 'L', 1, 380000,359000, 'Green', 'Active', 'Made in Italy', 'Cotton', 8, 1, 15),
  ('SE018', 'XL', 1, 290000,269000, 'Green', 'Active', 'Made in Italy', 'Cotton', 8, 1, 54),
  ('SE019', 'S', 0, 365000,259000, 'Black', 'Active', 'Made in Spain', 'Polyester', 10, 1, 30),
  ('SE020', 'M', 0, 300000,259000, 'Black', 'Active', 'Made in Spain', 'Polyester', 10, 1, 40),
  ('SE021', 'S', 0, 320000, 275000, 'Red', 'Active', 'Made in Vietnam', 'Cotton', 7, 1, 25),
  ('SE022', 'M', 0, 330000, 280000, 'Red', 'Active', 'Made in Vietnam', 'Cotton', 7, 1, 35),
  ('SE023', 'L', 0, 340000, 290000, 'Red', 'Active', 'Made in Vietnam', 'Cotton', 7, 1, 40),
  ('SE024', 'XL', 0, 350000, 295000, 'Red', 'Active', 'Made in Vietnam', 'Cotton', 7, 1, 45),
  ('SE025', 'S', 1, 450000, 420000, 'Blue', 'Active', 'Made in Vietnam', 'Cotton', 8, 1, 50),
  ('SE026', 'M', 1, 460000, 430000, 'Blue', 'Active', 'Made in Vietnam', 'Cotton', 8, 1, 60),
  ('SE027', 'L', 1, 470000, 440000, 'Blue', 'Active', 'Made in Vietnam', 'Cotton', 8, 1, 70),
  ('SE028', 'XL', 1, 480000, 450000, 'Blue', 'Active', 'Made in Vietnam', 'Cotton', 8, 1, 80),
  ('SE029', 'S', 0, 300000, 270000, 'White', 'Active', 'Made in Vietnam', 'Polyester', 9, 1, 20),
  ('SE030', 'M', 0, 310000, 280000, 'White', 'Active', 'Made in Vietnam', 'Polyester', 9, 1, 30),
  ('SE031', 'L', 0, 320000, 290000, 'White', 'Active', 'Made in Vietnam', 'Polyester', 9, 1, 40),
  ('SE032', 'XL', 0, 330000, 300000, 'White', 'Active', 'Made in Vietnam', 'Polyester', 9, 1, 50),
  ('SE033', 'S', 1, 440000, 410000, 'Black', 'Active', 'Made in Vietnam', 'Polyester', 10, 1, 25),
  ('SE034', 'M', 1, 450000, 420000, 'Black', 'Active', 'Made in Vietnam', 'Polyester', 10, 1, 35),
  ('SE035', 'L', 1, 460000, 430000, 'Black', 'Active', 'Made in Vietnam', 'Polyester', 10, 1, 45),
  ('SE036', 'XL', 1, 470000, 440000, 'Black', 'Active', 'Made in Vietnam', 'Polyester', 10, 1, 55),
  ('SE037', 'S', 0, 280000, 250000, 'Green', 'Active', 'Made in Vietnam', 'Cotton', 11, 1, 30),
  ('SE038', 'M', 0, 290000, 260000, 'Green', 'Active', 'Made in Vietnam', 'Cotton', 11, 1, 35),
  ('SE039', 'L', 0, 300000, 270000, 'Green', 'Active', 'Made in Vietnam', 'Cotton', 11, 1, 40),
  ('SE040', 'XL', 0, 310000, 280000, 'Green', 'Active', 'Made in Vietnam', 'Cotton', 11, 1, 45);

-- Insert data into Shirt table
INSERT INTO Shirt
  (Code, [Name], [Description], [Status], ShirtEditionId, SeasonPlayerId, CreatedAccountId, Quantity)
VALUES
  ('SRT001', 'Manchester City Home Jersey', 'Manchester City Home Jersey', 'Active', 1, 1, 1, 32),
  ('SRT002', 'Real Madrid Home Jersey', 'Real Madrid Home Jersey', 'Active', 2, 2, 1, 45),
  ('SRT003', 'Barcelona Home Jersey', 'Barcelona Home Jersey', 'Active', 3, 3, 1, 23),
  ('SRT004', 'PSG Home Jersey', 'PSG Home Jersey', 'Active', 4, 4, 1, 43),
  ('SRT005', 'Bayern Munich Home Jersey', 'Bayern Munich Home Jersey', 'Active', 5, 5, 1, 56),
  ('SRT006', 'Manchester United Home Jersey', 'Manchester United Home Jersey', 'Active', 6, 6, 1, 23),
  ('SRT007', 'Liverpool Home Jersey', 'Liverpool Home Jersey', 'Active', 7, 7, 1, 30),
  ('SRT008', 'Chelsea Home Jersey', 'Chelsea Home Jersey', 'Active', 8, 8, 1, 32),
  ('SRT009', 'Tottenham Hotspur Home Jersey', 'Tottenham Hotspur Home Jersey', 'Active', 11, 6, 1, 32),
  ('SRT010', 'Inter Milan Home Jersey', 'Inter Milan Home Jersey', 'Active', 10, 10, 1, 30),
  ('SRT011', 'Atletico Madrid Home Jersey', 'Atletico Madrid Home Jersey', 'Active', 12, 12, 1, 40),
  ('SRT012', 'AC Milan Home Jersey', 'AC Milan Home Jersey', 'Active', 13, 13, 1, 50),
  ('SRT013', 'Arsenal Home Jersey', 'Arsenal Home Jersey', 'Active', 14, 14, 1, 60),
  ('SRT014', 'Borussia Dortmund Home Jersey', 'Borussia Dortmund Home Jersey', 'Active', 15, 15, 1, 70),
  ('SRT015', 'Sevilla Home Jersey', 'Sevilla Home Jersey', 'Active', 16, 16, 1, 80),
  ('SRT016', 'Lazio Home Jersey', 'Lazio Home Jersey', 'Active', 17, 17, 1, 90),
  ('SRT017', 'Napoli Home Jersey', 'Napoli Home Jersey', 'Active', 18, 18, 1, 100),
  ('SRT018', 'Roma Home Jersey', 'Roma Home Jersey', 'Active', 19, 19, 1, 110),
  ('SRT019', 'RB Leipzig Home Jersey', 'RB Leipzig Home Jersey', 'Active', 20, 20, 1, 120),
  ('SRT020', 'Manchester City Away Jersey', 'Manchester City Away Jersey', 'Active', 1, 1, 1, 32),
  ('SRT021', 'Real Madrid Away Jersey', 'Real Madrid Away Jersey', 'Active', 2, 2, 1, 45),
  ('SRT022', 'Barcelona Away Jersey', 'Barcelona Away Jersey', 'Active', 3, 3, 1, 23),
  ('SRT023', 'PSG Away Jersey', 'PSG Away Jersey', 'Active', 4, 4, 1, 43),
  ('SRT024', 'Bayern Munich Away Jersey', 'Bayern Munich Away Jersey', 'Active', 5, 5, 1, 56),
  ('SRT025', 'Manchester United Away Jersey', 'Manchester United Away Jersey', 'Active', 6, 6, 1, 23),
  ('SRT026', 'Liverpool Away Jersey', 'Liverpool Away Jersey', 'Active', 7, 7, 1, 30),
  ('SRT027', 'Chelsea Away Jersey', 'Chelsea Away Jersey', 'Active', 8, 8, 1, 32),
  ('SRT028', 'Juventus Away Jersey', 'Juventus Away Jersey', 'Active', 9, 9, 1, 28),
  ('SRT029', 'Inter Milan Away Jersey', 'Inter Milan Away Jersey', 'Active', 10, 10, 1, 30);

-- Insert data into Image table
INSERT INTO [Image] (ShirtId, [Url])
VALUES
  (1, 'https://shop.mancity.com/dw/image/v2/BDWJ_PRD/on/demandware.static/-/Sites-master-catalog-MAN/default/dw887b23ea/images/large/701230876001_pp_01_mcfc.png?sw=1600&sh=1600&sm=fit'),
  (2, 'https://shop.realmadrid.com/_next/image?url=https%3A%2F%2Flegends.broadleafcloud.com%2Fapi%2Fasset%2Fcontent%2FRMCFMZ0195-01-1.jpg%3FcontextRequest%3D%257B%2522forceCatalogForFetch%2522%3Afalse%2C%2522applicationId%2522%3A%252201H4RD9NXMKQBQ1WVKM1181VD8%2522%2C%2522tenantId%2522%3A%2522REAL_MADRID%2522%257D&w=1920&q=75'),
  (3, 'https://store.fcbarcelona.com/cdn/shop/files/25100MC_PEDRI_2_db046606-b5b7-406c-bb7c-8bcad85a5562.jpg?v=1721277069'),
  (4, 'https://images.footballfanatics.com/paris-saint-germain/psg-nike-away-stadium-shirt-2024-2025-kids-with-barcola-29-printing_ss5_p-201913643+pv-1+u-2aiynatoy3vi1gs0ryy1+v-zdvmsygjxnkcnpxpwipd.jpg?_hv=2&w=900'),
  (5, 'https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.adidas.com.vn%2Fen%2Ffc-bayern-22-23-home-jersey%2FH39900.html&psig=AOvVaw0UAUumEMWoeZfzSacYutzW&ust=1721404095032000&source=images&cd=vfe&opi=89978449&ved=0CBEQjRxqFwoTCJD-kY_4sIcDFQAAAAAdAAAAABAE'),
  (6, 'https://images.footballfanatics.com/manchester-united/manchester-united-adidas-home-authentic-shirt-2024-25_ss5_p-200954448+pv-1+u-1kema30vdpjpvdj2lech+v-dgmtxkecxubiknowzxyc.jpg?_hv=2&w=900'),
  (7, 'https://store.liverpoolfc.com/media/catalog/product/cache/dd504f005c0b90ffe1b9fb2344db1a87/f/n/fn8798g_1.jpg'),
  (8, 'https://static.nike.com/a/images/t_PDP_1280_v1/f_auto,q_auto:eco/fd0830a3-678e-4ceb-9b48-45bf010b609c/chelsea-fc-2023-24-stadium-away-dri-fit-football-shirt-HzkDbt.png'),
  (9, 'https://img.freepik.com/free-vector/sports-shirt-design-ready-print-football-shirt-sublimation_29096-4311.jpg'),
  (10, 'https://img.freepik.com/premium-vector/sports-shirt-design-ready-print-football-shirt-sublimation_29096-4294.jpg'),
  (11, 'https://img.freepik.com/free-vector/sports-shirt-design-ready-print-football-shirt-sublimation_29096-4165.jpg'),
  (12, 'https://img.freepik.com/premium-vector/sports-shirt-design-ready-print-football-shirt-sublimation_29096-4053.jpg'),
  (13, 'https://img.freepik.com/premium-vector/sports-shirt-design-ready-print-football-shirt-sublimation_29096-4205.jpg'),
  (14, 'https://img.freepik.com/free-vector/sports-shirt-design-ready-print-football-shirt-sublimation_29096-4303.jpg'),
  (15, 'https://img.freepik.com/free-vector/soccer-jersey-template-sport-t-shirt-design_29096-1247.jpg?t=st=1721317578~exp=1721321178~hmac=41ff73b71bf54981e0ca7eda7e7186e7a8a6cf1ad16f6a21d48616a0e737bc66&w=1800'),
  (16, 'https://img.freepik.com/free-vector/soccer-jersey-template-sport-t-shirt-design_29096-1293.jpg?t=st=1721318067~exp=1721321667~hmac=db97028945f08ccc076f3ac093978125703a39af647b2e9c7a207e7cc5a84d74&w=1800'),
  (17, 'https://img.freepik.com/free-vector/soccer-jersey-template-sport-t-shirt-design_29096-1338.jpg?t=st=1721318108~exp=1721321708~hmac=c3bc7f4c136fecedfa70570785672a695d638cfc8c8d51bb0503eb692d0a0e42&w=1800'),
  (18, 'https://img.freepik.com/free-vector/soccer-jersey-template-sport-t-shirt-design_29096-1299.jpg?t=st=1721318109~exp=1721321709~hmac=b74b629db8151596ba336aaa276d2a1d29d83d8e062a0d4fa601a3fe9dd4b54a&w=1800'),
  (19, 'https://img.freepik.com/free-vector/shirt-template-racing-jersey-design-soccer-jersey_29096-1208.jpg?t=st=1721318110~exp=1721321710~hmac=695465a9509ba75b5f45badc4268242f6c49de452281b8e5709f743e74ad1c91&w=1800'),
  (20, 'https://fanatics.frgimages.com/paris-saint-germain/youth-nike-sergio-ramos-white-paris-saint-germain-2022/23-third-breathe-stadium-replica-player-jersey_pi5045000_altimages_ff_5045802-dbe488e80ddabc118266alt1_full.jpg?_hv=2&w=900'),
  (21, 'https://images.footballfanatics.com/paris-saint-germain/psg-home-stadium-shirt-2023-24-with-champions-league-printing-odemb%C3%A9l%C3%A9-10_ss5_p-200596413+pv-1+u-wt1bxyuv8kdhvd229khi+v-3mnbpb735ckimgqmmra1.jpg?_hv=2&w=900'),
  (22, 'https://store.fcbarcelona.com/cdn/shop/files/BLMP0007401720_6.jpg?v=1697727110'),
  (23, 'https://store.fcbarcelona.com/cdn/shop/files/2023-09-28-BLM-GAVI-AC203149.jpg?v=1697724308'),
  (24, 'https://images.footballfanatics.com/bayern-munich/mens-adidas-jamal-musiala-red-bayern-munich-2024/25-home-replica-long-sleeve-player-jersey_ss5_p-201645208+pv-1+u-wajkddtwdlo0kno1ngkg+v-gis143hq3la2iim9fxw0.jpg?_hv=2&w=900'),
  (25, 'https://bodysports.co/wp-content/uploads/2022/07/PSG-Home.jpeg'),
  (26, 'https://i.pinimg.com/736x/0c/6c/1b/0c6c1b39ae7a2f2b900e885bfa920248.jpg'),
  (27, 'https://cdn.fifakitcreator.com/kits/2022/01/11/61dcbb7771475.jpg'),
  (28, 'https://i.pinimg.com/736x/62/e7/74/62e7742fcd2fc9a8d4b70d7fe8aaec1a.jpg'),
  (29, 'https://i.pinimg.com/736x/22/64/1c/22641c3b151555f2926774c2f2628be7.jpg');

-- Insert data into Order table
INSERT INTO [Order] (Code, OrderDate, [Status], Total, CreatedAccountId)
VALUES
  ('OD001', '2024-07-18 12:45:42.067', 'Pending', 777000.00, 4),
  ('OD002', '2024-07-18 13:00:00.000', 'Processed', 800000.00, 3),
  ('OD003', '2024-07-18 13:15:00.000', 'Shipped', 900000.00, 2),
  ('OD004', '2024-07-18 13:30:00.000', 'Delivered', 1000000.00, 1),
  ('OD005', '2024-07-18 13:45:00.000', 'Pending', 1100000.00, 4),
  ('OD006', '2024-07-18 14:00:00.000', 'Processed', 1200000.00, 3),
  ('OD007', '2024-07-18 14:15:00.000', 'Shipped', 1300000.00, 2),
  ('OD008', '2024-07-18 14:30:00.000', 'Delivered', 1400000.00, 1),
  ('OD009', '2024-07-18 14:45:00.000', 'Pending', 1500000.00, 4),
  ('OD010', '2024-07-18 15:00:00.000', 'Processed', 1600000.00, 3);

-- Insert data into Payment table
INSERT INTO Payment (Code, PaymentMethod, PaymentName, [Status], OrderId, CreatedAccountId)
VALUES
  ('PAY001', 'Credit Card', 'Visa ending in 1234', 'Paid', 1, 1),
  ('PAY002', 'PayPal', 'user2@example.com', 'Paid', 2, 2),
  ('PAY003', 'Credit Card', 'Mastercard ending in 5678', 'Pending', 3, 3),
  ('PAY004', 'Bank Transfer', 'Chase ending in 9876', 'Paid', 4, 4),
  ('PAY005', 'Credit Card', 'Visa ending in 1234', 'Pending', 5, 1),
  ('PAY006', 'PayPal', 'user2@example.com', 'Paid', 6, 2),
  ('PAY007', 'Credit Card', 'Mastercard ending in 5678', 'Pending', 7, 3),
  ('PAY008', 'Bank Transfer', 'Chase ending in 9876', 'Paid', 8, 4),
  ('PAY009', 'Credit Card', 'Visa ending in 1234', 'Pending', 9, 1),
  ('PAY010', 'PayPal', 'user2@example.com', 'Paid', 10, 2);

-- Insert data into OrderDetail table
INSERT INTO OrderDetail (OrderId, ShirtId, Code, Subtotal, Quantity, Size, [Status])
VALUES
  (1, 1, 'OD001', 299000.00, 1,'M',  'Fulfilled'),
  (1, 2, 'OD002', 299000.00, 1,'L', 'Fulfilled'),
  (2, 3, 'OD003', 299000.00, 1,'XL',  'Fulfilled'),
  (2, 4, 'OD004', 299000.00, 1,'S',  'Fulfilled'),
  (3, 5, 'OD005', 399000.00, 1,'M',  'Pending'),
  (3, 6, 'OD006', 399000.00, 1,'L',  'Pending'),
  (4, 7, 'OD007', 399000.00, 1,'M',  'Pending'),
  (4, 8, 'OD008', 399000.00, 1,'M',  'Pending'),
  (5, 1, 'OD009', 299000.00, 1,'S',  'Fulfilled'),
  (5, 2, 'OD010', 299000.00, 1,'XL',  'Fulfilled'),
  (1, 3, 'OD011', 299000.00, 1, 'L', 'Fulfilled'),
  (1, 4, 'OD012', 299000.00, 1, 'XL', 'Fulfilled'),
  (2, 5, 'OD013', 399000.00, 1, 'S', 'Pending'),
  (2, 6, 'OD014', 399000.00, 1, 'M', 'Pending'),
  (3, 7, 'OD015', 399000.00, 1, 'L', 'Pending'),
  (3, 8, 'OD016', 399000.00, 1, 'XL', 'Pending'),
  (4, 9, 'OD017', 299000.00, 1, 'S', 'Fulfilled'),
  (4, 10, 'OD018', 299000.00, 1, 'M', 'Fulfilled'),
  (5, 11, 'OD019', 299000.00, 1, 'L', 'Fulfilled'),
  (5, 12, 'OD020', 299000.00, 1, 'XL', 'Fulfilled'),
  (6, 13, 'OD021', 399000.00, 1, 'S', 'Pending'),
  (6, 14, 'OD022', 399000.00, 1, 'M', 'Pending'),
  (7, 15, 'OD023', 399000.00, 1, 'L', 'Pending'),
  (7, 16, 'OD024', 399000.00, 1, 'XL', 'Pending'),
  (8, 17, 'OD025', 299000.00, 1, 'S', 'Fulfilled'),
  (8, 18, 'OD026', 299000.00, 1, 'M', 'Fulfilled'),
  (9, 19, 'OD027', 299000.00, 1, 'L', 'Fulfilled'),
  (9, 20, 'OD028', 299000.00, 1, 'XL', 'Fulfilled'),
  (10, 21, 'OD029', 399000.00, 1, 'S', 'Pending'),
  (10, 22, 'OD030', 399000.00, 1, 'M', 'Pending');

------ Insert data into Account table
--INSERT INTO Account
--  (Email, [Password], [Role], [Status], SupabaseId)
--VALUES
--  ('staff@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Staff', 'Active', '49ab1ee8-4b75-41da-b831-cea19d171406'),
--  ('admin@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Admin', 'Active', '85f1eb98-25ed-4adc-a226-2846e06a5a7d'),
--  ('minh@gmail.com', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Customer', 'Active', '580b1b9e-c395-467c-a4e8-ce48c0ec09d1'),
--  ('kiritominhswordartonline@gmail.com','548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Customer', 'Active', 'b5910ce2-7c6a-4d4b-aaa0-357e32033ff3');

---- Insert data into Club table
--INSERT INTO Club
--  (Code, [Name], LogoUrl, [Status], CreatedAccountId)
--VALUES
--  ('CLB001', 'Real Madrid', 'https://upload.wikimedia.org/wikipedia/en/thumb/5/56/Real_Madrid_CF.svg/1200px-Real_Madrid_CF.svg.png', 'Active', 1),
--  ('CLB002', 'Barcelona', 'https://upload.wikimedia.org/wikipedia/vi/thumb/9/91/FC_Barcelona_logo.svg/220px-FC_Barcelona_logo.svg.png', 'Active', 1),
--  ('CLB003', 'Manchester United', 'https://upload.wikimedia.org/wikipedia/vi/a/a1/Man_Utd_FC_.svg', 'Active', 1);

---- Insert data into Player table
--INSERT INTO Player
--  (Code, [Name], [Status], ClubId, CreatedAccountId)
--VALUES
--  ('PLY001', 'Lionel Messi', 'Active', 2, 1),
--  ('PLY002', 'Cristiano Ronaldo', 'Active', 1, 1),
--  ('PLY003', 'Neymar', 'Active', 2, 1),
--  ('PLY004', 'Kylian Mbappe', 'Active', 2, 1),
--  ('PLY005', 'Robert Lewandowski', 'Active', 2, 1);

---- Insert data into Season table
--INSERT INTO Season
--  (Code, [Name], ClubId, CreatedAccountId, [Status])
--VALUES
--  ('SES001', '2022/2023', 1, 1, 'Active'),
--  ('SES002', '2021/2022', 1, 1, 'Active'),
--  ('SES003', '2022/2023', 2, 1, 'Active'),
--  ('SES004', '2021/2022', 2, 1, 'Active');

---- Insert data into SeasonPlayer table
--INSERT INTO SeasonPlayer
--  (SeasonId, PlayerId)
--VALUES
--  (1, 2),
--  (1, 3),
--  (3, 1),
--  (3, 4),
--  (3, 5);

---- Insert data into ShirtEdition table
--INSERT INTO ShirtEdition
--  (Code, Size, HasSignature, StockPrice, DiscountPrice, Color, [Status], Origin, Material, SeasonId, CreatedAccountId, Quantity)
--VALUES
--  ('SE001', 'S', 0, 299000.00,259000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 34),
--  ('SE002', 'M', 0, 299000.00,259000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 45),
--  ('SE003', 'L', 0, 299000.00,259000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 67),
--  ('SE004', 'XL', 0, 299000.00,259000, 'Red', 'Active', 'Made in China', 'Cotton', 1, 1, 87),
--  ('SE005', 'S', 1, 399000.00,359000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 76),
--  ('SE006', 'M', 1, 399000.00,359000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 32),
--  ('SE007', 'L', 1, 399000.00,359000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 15),
--  ('SE008', 'XL', 1, 399000.00,359000, 'Blue', 'Active', 'Made in Italy', 'Cotton', 3, 1, 54);

---- Insert data into Shirt table
--INSERT INTO Shirt
--  (Code, [Name], [Description], [Status], ShirtEditionId, SeasonPlayerId, CreatedAccountId, Quantity)
--VALUES
--  ('SRT001', 'Real Madrid Home Jersey','Real Madrid Home Jersey', 'Active', 1, 1, 1, 32),
--  ('SRT002', 'Real Madrid Home Jersey','Real Madrid Home Jersey', 'Active', 2, 1, 1, 45),
--  ('SRT003', 'Real Madrid Home Jersey','Real Madrid Home Jersey', 'Active', 3, 1, 1, 23),
--  ('SRT004', 'Real Madrid Home Jersey','Real Madrid Home Jersey', 'Active', 4, 1, 1, 43),
--  ('SRT005', 'Barcelona Home Jersey','Barcelona Home Jersey', 'Active', 5, 3, 1, 56),
--  ('SRT006', 'Barcelona Home Jersey','Barcelona Home Jersey', 'Active', 6, 3, 1, 23),
--  ('SRT007', 'Barcelona Home Jersey','Barcelona Home Jersey', 'Active', 7, 3, 1, 30),
--  ('SRT008', 'Barcelona Home Jersey','Barcelona Home Jersey', 'Active', 8, 3, 1, 32);

--insert into dbo.[Image](ShirtId, [Url])
--values(1, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/TSport/shirt.jpg'),
--	 (2, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg'),
--	 (3, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg'),
--	 (4, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/TSport/shirt.jpg'),
--	 (5, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg'),
--	 (6, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg'),
--	 (7, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg'),
--	 (8, 'https://iptnrpnttdzsfgozjmum.supabase.co/storage/v1/object/public/Shirts/shirts_02.jpg')

--insert into dbo.[Order](Code,OrderDate,[Status],Total,CreatedAccountId)
--values('OD8d29677b-f8b9-429e-b0b6-0cee511aeef1','2024-07-18 12:45:42.067','Pending','777000.00','2024-07-18 12:45:42.067',4);
------ Insert data into Order table
----INSERT INTO [Order] (Id, Code, OrderDate, [Status], Total, CreatedAccountId)
----VALUES
----  (1, 'ORD001', '2023-04-01 10:30:00', 'Processed', 159.98, 2),
----  (2, 'ORD002', '2023-04-15 15:45:00', 'Shipped', 199.98, 2),
----  (3, 'ORD003', '2023-05-01 12:00:00', 'Pending', 99.99, 1);

------ Insert data into Payment table
----INSERT INTO Payment (Id, Code, PaymentMethod, PaymentName, [Status], OrderId, CreatedAccountId)
----VALUES
----  (1, 'PAY001', 'Credit Card', 'Visa ending in 1234', 'Paid', 1, 2),
----  (2, 'PAY002', 'PayPal', 'user2@example.com', 'Paid', 2, 2),
----  (3, 'PAY003', 'Credit Card', 'Mastercard ending in 5678', 'Pending', 3, 1);

------ Insert data into OrderDetail table
----INSERT INTO OrderDetail (OrderId, ShirtId, Code, Subtotal, Quantity, [Status])
----VALUES
----  (1, 1, 'OD001', 79.99, 1, 'Fulfilled'),
----  (1, 2, 'OD002', 79.99, 1, 'Fulfilled'),
----  (2, 5, 'OD003', 99.99, 1, 'Fulfilled'),
----  (2, 6, 'OD004', 99.99, 1, 'Fulfilled'),
----  (3, 7, 'OD005', 99.99, 1, 'Pending');

--go