INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'1', N'David', N'Aniorte', N'david29', NULL, N'david29@gmail.com', NULL, 1, NULL, NULL, NULL, NULL, 0, 0, NULL, 1, 1)
INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'2', N'Juan', N'Perez', N'juan.perez', NULL, N'juan.perez@gmail.com', NULL, 0, NULL, NULL, NULL, NULL, 1, 1, NULL, 1, 1)
INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'3', N'Maria', N'Lopez', N'maria.lopez', NULL, N'maria.lopez@gmail.com', NULL, 0, NULL, NULL, NULL, NULL, 0, 0, NULL, 0, 3)
INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'4', N'Sofia', N'Gomez', N'sofia.desings', NULL, N'sofia.desings@outlook.com', NULL, 1, NULL, NULL, NULL, NULL, 1, 0, NULL, 0, 0)
INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'5', N'Jose', N'Martinez', N'jmartinez.hr', NULL, N'jmartinez.h@hotmail.com', NULL, 0, NULL, NULL, NULL, NULL, 1, 1, NULL, 0, 2)
INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'6', N'Ana', N'de la Guia', N'ana.guia', NULL, N'ana.guia@outlook.com', NULL, 1, NULL, NULL, NULL, NULL, 0, 0, NULL, 1, 1)

SET IDENTITY_INSERT [dbo].[Model] ON
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (1, N'Audi A4')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (2, N'Alpine A110')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (3, N'Toyota Corolla')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (4, N'Porsche 911')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (5, N'BMW Serie 3')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (6, N'Renault Clio')
SET IDENTITY_INSERT [dbo].[Model] OFF

SET IDENTITY_INSERT [dbo].[Car] ON
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [EngDisplacement], [FuelType], [MaintenanceTypes], [RimSize], [ModelId]) VALUES (17, N'Berlina', N'Gris', N'Automovil de turismo del segmento D', N'Audi', 18000, 3, 1, 85, N'150 CV', N'Gasolina', N'Aceite y ruedas', 59.5, 1)
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [EngDisplacement], [FuelType], [MaintenanceTypes], [RimSize], [ModelId]) VALUES (20, N'Deportivo', N'Azul', N'Deportivo coupe de dos puertas', N'Alpine', 19020, 4, 2, 180, N'252 CV', N'Gasolina', N'Ruedas y frenos', 40.1, 2)
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [EngDisplacement], [FuelType], [MaintenanceTypes], [RimSize], [ModelId]) VALUES (27, N'Familiar', N'Rojo', N'Automovil del segmento C', N'Toyota', 5000, 10, 7, 60, N'80 CV', N'Diesel', N'Liquido refrigerante', 45.6, 3)
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [EngDisplacement], [FuelType], [MaintenanceTypes], [RimSize], [ModelId]) VALUES (28, N'Deportivo', N'Verde', N'Deportivo de lujo aleman', N'Porsche', 20000, 1, 1, 200, N'300 CV', N'Gasolina', N'Direccion', 55, 4)
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [EngDisplacement], [FuelType], [MaintenanceTypes], [RimSize], [ModelId]) VALUES (30, N'Berlina', N'Blanco', N'Automovil aleman del segmento D', N'BMW', 15300, 3, 2, 150, N'180 CV', N'Diesel', N'Centralita', 47.8, 5)
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [EngDisplacement], [FuelType], [MaintenanceTypes], [RimSize], [ModelId]) VALUES (32, N'Compacto', N'Rojo', N'Automovil compacto frances de 5 puertas', N'Renault', 7080, 6, 3, 70, N'90 CV', N'Gasolina', N'Elevalunas', 43, 6)
SET IDENTITY_INSERT [dbo].[Car] OFF

SET IDENTITY_INSERT [dbo].[Purchase] ON
INSERT INTO [dbo].[Purchase] ([Id], [DeliveryCarDealer], [PaymentMethod], [PurchasingDate], [PurchasingPrice], [ApplicationUserId]) VALUES (5, N'C/ Albacete nº 2', 1, N'2025-10-15 00:00:00', 5000, N'1')
INSERT INTO [dbo].[Purchase] ([Id], [DeliveryCarDealer], [PaymentMethod], [PurchasingDate], [PurchasingPrice], [ApplicationUserId]) VALUES (7, N'C/La Roda nº 7', 0, N'2000-01-01 18:40:00', 7080, N'2')
INSERT INTO [dbo].[Purchase] ([Id], [DeliveryCarDealer], [PaymentMethod], [PurchasingDate], [PurchasingPrice], [ApplicationUserId]) VALUES (9, N'C/La Virgen nº 6', 0, N'2024-03-02 19:40:21', 15300, N'2')
INSERT INTO [dbo].[Purchase] ([Id], [DeliveryCarDealer], [PaymentMethod], [PurchasingDate], [PurchasingPrice], [ApplicationUserId]) VALUES (11, N'C/Cristo nº 20', 1, N'2021-05-05 08:40:37', 18000, N'4')
INSERT INTO [dbo].[Purchase] ([Id], [DeliveryCarDealer], [PaymentMethod], [PurchasingDate], [PurchasingPrice], [ApplicationUserId]) VALUES (13, N'C/Hellin nº 31', 0, N'2022-04-09 09:50:31', 20000, N'3')
SET IDENTITY_INSERT [dbo].[Purchase] OFF

INSERT INTO [dbo].[PurchaseItem] ([CarId], [PurchaseId], [Quantity]) VALUES (17, 11, 1)
INSERT INTO [dbo].[PurchaseItem] ([CarId], [PurchaseId], [Quantity]) VALUES (27, 5, 1)
INSERT INTO [dbo].[PurchaseItem] ([CarId], [PurchaseId], [Quantity]) VALUES (28, 13, 1)
INSERT INTO [dbo].[PurchaseItem] ([CarId], [PurchaseId], [Quantity]) VALUES (30, 9, 1)
INSERT INTO [dbo].[PurchaseItem] ([CarId], [PurchaseId], [Quantity]) VALUES (32, 7, 1)

SET IDENTITY_INSERT [dbo].[Rental] ON
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [PaymentMethod], [RentingDate], [StartDate], [TotalPrice], [ApplicationUserId]) VALUES (5, N'C/La Gineta nº 9', N'2025-10-16 14:14:18', 2, N'2025-10-14 15:25:23', N'2025-10-14 15:25:23', 140, N'1')
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [PaymentMethod], [RentingDate], [StartDate], [TotalPrice], [ApplicationUserId]) VALUES (7, N'C/Valencia nº 99', N'2025-10-04 18:02:14', 1, N'2025-10-01 17:38:00', N'2025-10-01 17:38:00', 255, N'3')
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [PaymentMethod], [RentingDate], [StartDate], [TotalPrice], [ApplicationUserId]) VALUES (9, N'C/Villarrobledo nº 1', N'2025-01-01 07:25:00', 0, N'2024-12-30 13:59:59', N'2024-12-30 13:59:59', 400, N'6')
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [PaymentMethod], [RentingDate], [StartDate], [TotalPrice], [ApplicationUserId]) VALUES (11, N'C/Albacete nº 5', N'2025-04-18 09:45:12', 1, N'2025-04-14 20:14:25', N'2025-04-14 20:14:25', 720, N'4')
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [PaymentMethod], [RentingDate], [StartDate], [TotalPrice], [ApplicationUserId]) VALUES (12, N'C/C. Criptana nº 8', N'2021-07-06 14:08:59', 0, N'2021-07-04 23:57:14', N'2021-07-04 23:57:14', 170, N'2')
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [PaymentMethod], [RentingDate], [StartDate], [TotalPrice], [ApplicationUserId]) VALUES (16, N'C/Perrote nº 8', N'2020-04-19 00:00:00', 0, N'2020-04-10 18:50:25', N'2020-04-10 18:50:25', 540, N'3')
SET IDENTITY_INSERT [dbo].[Rental] OFF

INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (17, 7, 1)
INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (17, 12, 1)
INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (20, 11, 1)
INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (27, 16, 1)
INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (28, 9, 1)
INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (32, 5, 1)

SET IDENTITY_INSERT [dbo].[Review] ON
INSERT INTO [dbo].[Review] ([Id], [Country], [Created], [DriverType], [ApplicationUserId]) VALUES (2, N'Espana', N'2025-10-16 16:54:13', N'Novato', N'1')
INSERT INTO [dbo].[Review] ([Id], [Country], [Created], [DriverType], [ApplicationUserId]) VALUES (3, N'Argentina', N'2025-10-15 14:18:56', N'Experto', N'2')
INSERT INTO [dbo].[Review] ([Id], [Country], [Created], [DriverType], [ApplicationUserId]) VALUES (4, N'Alemania', N'2025-10-04 21:37:37', N'Novato', N'4')
INSERT INTO [dbo].[Review] ([Id], [Country], [Created], [DriverType], [ApplicationUserId]) VALUES (5, N'Portugal', N'2025-09-17 17:48:47', N'Novato', N'5')
INSERT INTO [dbo].[Review] ([Id], [Country], [Created], [DriverType], [ApplicationUserId]) VALUES (6, N'Italia', N'2025-09-14 18:25:50', N'Experto', N'6')
SET IDENTITY_INSERT [dbo].[Review] OFF


INSERT INTO [dbo].[ReviewItem] ([CarId], [ReviewId], [Description], [Rating]) VALUES (17, 3, NULL, 4)
INSERT INTO [dbo].[ReviewItem] ([CarId], [ReviewId], [Description], [Rating]) VALUES (20, 2, NULL, 1)
INSERT INTO [dbo].[ReviewItem] ([CarId], [ReviewId], [Description], [Rating]) VALUES (27, 6, NULL, 2)
INSERT INTO [dbo].[ReviewItem] ([CarId], [ReviewId], [Description], [Rating]) VALUES (30, 4, NULL, 4)
INSERT INTO [dbo].[ReviewItem] ([CarId], [ReviewId], [Description], [Rating]) VALUES (32, 5, N'Magnifico coche, la conduccion es espectacular y el manejo se siente muy suave.', 5)