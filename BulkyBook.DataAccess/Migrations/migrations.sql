SELECT TOP (1000) * FROM [Bulky].[dbo].[Categories]
SET IDENTITY_INSERT [Categories] OFF;
SET IDENTITY_INSERT [Bulky].[dbo].[Categories] ON;

-- 3. Insert the missing categories (replace with actual data)
INSERT INTO [Bulky].[dbo].[Categories] ([Id], [Name], [DisplayOrder])
VALUES (1, 'Action', 2);

-- 4. Turn off identity insert
SET IDENTITY_INSERT [Bulky].[dbo].[Categories] OFF;