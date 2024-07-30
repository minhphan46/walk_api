USE [NZWalksDb]
GO

-- Clear existing data
DELETE FROM [NZWalksDb].[dbo].[WalkCategories];
DELETE FROM [NZWalksDb].[dbo].[Categories];
DELETE FROM [NZWalksDb].[dbo].[Walks];
DELETE FROM [NZWalksDb].[dbo].[Regions];
DELETE FROM [NZWalksDb].[dbo].[Difficulties];
DELETE FROM [NZWalksDb].[dbo].[Users];
GO

-- Insert Difficulties
INSERT [dbo].[Difficulties] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (N'f808ddcd-b5e5-4d80-b732-1ca523e48434', N'Hard', GETDATE(), GETDATE())
GO
INSERT [dbo].[Difficulties] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (N'54466f17-02af-48e7-8ed3-5a4a8bfacf6f', N'Easy', GETDATE(), GETDATE())
GO
INSERT [dbo].[Difficulties] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (N'ea294873-7a8c-4c0f-bfa7-a2eb492cbf8c', N'Medium', GETDATE(), GETDATE())
GO

-- Insert Regions
INSERT [dbo].[Regions] ([Id], [Code], [Name], [RegionImageUrl], [CreatedAt], [UpdatedAt]) VALUES (N'906cb139-415a-4bbb-a174-1a1faf9fb1f6', N'NSN', N'Nelson', N'https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', GETDATE(), GETDATE())
GO
INSERT [dbo].[Regions] ([Id], [Code], [Name], [RegionImageUrl], [CreatedAt], [UpdatedAt]) VALUES (N'f7248fc3-2585-4efb-8d1d-1c555f4087f6', N'AKL', N'Auckland', N'https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', GETDATE(), GETDATE())
GO
INSERT [dbo].[Regions] ([Id], [Code], [Name], [RegionImageUrl], [CreatedAt], [UpdatedAt]) VALUES (N'14ceba71-4b51-4777-9b17-46602cf66153', N'BOP', N'Bay Of Plenty', 'https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', GETDATE(), GETDATE())
GO
INSERT [dbo].[Regions] ([Id], [Code], [Name], [RegionImageUrl], [CreatedAt], [UpdatedAt]) VALUES (N'6884f7d7-ad1f-4101-8df3-7a6fa7387d81', N'NTL', N'Northland', 'https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', GETDATE(), GETDATE())
GO
INSERT [dbo].[Regions] ([Id], [Code], [Name], [RegionImageUrl], [CreatedAt], [UpdatedAt]) VALUES (N'f077a22e-4248-4bf6-b564-c7cf4e250263', N'STL', N'Southland', 'https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', GETDATE(), GETDATE())
GO
INSERT [dbo].[Regions] ([Id], [Code], [Name], [RegionImageUrl], [CreatedAt], [UpdatedAt]) VALUES (N'cfa06ed2-bf65-4b65-93ed-c9d286ddb0de', N'WGN', N'Wellington', N'https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', GETDATE(), GETDATE())
GO

-- Insert Walks
INSERT INTO [NZWalksDb].[dbo].[Walks]
(Id, Name, Description, LengthInKm, WalkImageUrl, DifficultyId, RegionId, CreatedAt, UpdatedAt)
VALUES
('327aa9f7-26f7-4ddb-8047-97464374bb63', 'Mount Victoria Loop', 'This scenic walk takes you around the top of Mount Victoria, offering stunning views of Wellington and its harbor.', 3.5, 'https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', '54466F17-02AF-48E7-8ED3-5A4A8BFACF6F','CFA06ED2-BF65-4B65-93ED-C9D286DDB0DE', GETDATE(), GETDATE());
GO
-- Repeat the above INSERT INTO [NZWalksDb].[dbo].[Walks] for the other walks
-- ...

-- Insert Categories
INSERT INTO [dbo].[Categories] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (NEWID(), N'Nature', GETDATE(), GETDATE())
GO
INSERT INTO [dbo].[Categories] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (NEWID(), N'Scenic', GETDATE(), GETDATE())
GO
INSERT INTO [dbo].[Categories] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (NEWID(), N'Historical', GETDATE(), GETDATE())
GO
-- Add more categories as needed

-- Insert WalkCategories (Linking Walks and Categories)
INSERT INTO [dbo].[WalkCategories] ([WalkId], [CategoryId])
VALUES 
('327aa9f7-26f7-4ddb-8047-97464374bb63', (SELECT [Id] FROM [dbo].[Categories] WHERE [Name] = 'Scenic')),
('327aa9f7-26f7-4ddb-8047-97464374bb63', (SELECT [Id] FROM [dbo].[Categories] WHERE [Name] = 'Nature'));
GO
-- Repeat the above INSERT INTO [dbo].[WalkCategories] for the other walk-category associations
-- ...

-- Insert Users
INSERT INTO [dbo].[Users] 
(Id, IdentityId, Email, Username, Fullname, Dob, Address, Phone, EmailVerified, RoleId, Status, CreatedAt, UpdatedAt) 
VALUES 
('6035a10a-d052-4c93-8bbf-16e235e48dcf','4a459ae6-5f38-4a47-b257-0e79e504550f', 'test@test.com', 'admin', 'ADMIN', '1990-01-01', '123 Main St', '1234567890', 1, '9048e7ee-b63b-44ee-a93a-79cf1a904d86', 1, GETDATE(), GETDATE())
GO