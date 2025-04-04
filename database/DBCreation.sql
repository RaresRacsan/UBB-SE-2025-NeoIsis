USE Workout
DROP TABLE IF EXISTS UserClasses
DROP TABLE IF EXISTS Rankings
DROP TABLE IF EXISTS UserWorkouts
DROP TABLE IF EXISTS CompleteWorkouts
DROP TABLE IF EXISTS Workouts
DROP TABLE IF EXISTS Exercises  
DROP TABLE IF EXISTS MuscleGroups
DROP TABLE IF EXISTS Classes
DROP TABLE IF EXISTS WorkoutTypes
DROP TABLE IF EXISTS Users
DROP TABLE IF EXISTS ClassTypes
DROP TABLE IF EXISTS PersonalTrainers

CREATE TABLE MuscleGroups (
	MGID INT PRIMARY KEY IDENTITY(1, 1),
	[Name] VARCHAR(256)
	--enough?
)

--hardcoded
CREATE TABLE Exercises (
	EID INT PRIMARY KEY IDENTITY(1, 1),
	[Name] VARCHAR(256) UNIQUE,
	[Description] VARCHAR(MAX),
	--Difficulty VARCHAR(256)
	Difficulty INT CHECK (Difficulty BETWEEN 1 AND 10),
	MGID INT REFERENCES MuscleGroups(MGID)
)

CREATE TABLE WorkoutTypes (
	WTID INT PRIMARY KEY IDENTITY(1, 1),
	[Name] VARCHAR(256) UNIQUE
	--enough?
)

--when added, it is visible for all users
CREATE TABLE Workouts (
	WID INT PRIMARY KEY IDENTITY(1, 1),
	--unique?
	[Name] VARCHAR(256) UNIQUE,
	WTID INT REFERENCES WorkoutTypes(WTID)
)

--intermediary for Workouts and Excercises
CREATE TABLE CompleteWorkouts (
	WID INT REFERENCES Workouts(WID),
	EID INT REFERENCES Exercises(EID),
	PRIMARY KEY (WID, EID),
	[Sets] INT,
	RepsPerSet INT
)

CREATE TABLE [Users] (
	[UID] INT PRIMARY KEY IDENTITY(1, 1)
	--what else?
)

--useful for calendar
CREATE TABLE UserWorkouts (
	[UID] INT REFERENCES [Users]([UID]),
	WID INT REFERENCES Workouts(WID),
	[Date] DATE,
	PRIMARY KEY ([UID], WID, [Date]),
	--boolean ---> workout completed or not
	Completed BIT NOT NULL
)

--individual ranking(per user)??
CREATE TABLE Rankings (
	[UID] INT REFERENCES [Users]([UID]),
	MGID INT REFERENCES MuscleGroups(MGID),
	PRIMARY KEY ([UID], MGID),
	[Rank] INT CHECK ([Rank] BETWEEN 0 AND 10000)
)

--hardcoded
CREATE TABLE PersonalTrainers (
	PTID INT PRIMARY KEY IDENTITY(1, 1),
	LastName VARCHAR(50),
	FirstName VARCHAR(50),
	WorksSince DATE
)

--hardcoded
CREATE TABLE ClassTypes (
	CTID INT PRIMARY KEY IDENTITY(1, 1),
	[Name] VARCHAR(50) UNIQUE
)

--hardcoded
CREATE TABLE Classes (
	CID INT PRIMARY KEY IDENTITY(1, 1),
	[Name] VARCHAR(50) UNIQUE,
	[Description] VARCHAR(MAX),
	CTID INT REFERENCES ClassTypes(CTID),
	PTID INT REFERENCES PersonalTrainers(PTID)
)

--keeps the evidence of each user in a class
--it is like UserWorkouts but with classes
CREATE TABLE UserClasses (
	[UID] INT REFERENCES [Users]([UID]),
	CID INT REFERENCES Classes(CID),
	[Date] DATE
	PRIMARY KEY ([UID], CID, [Date])
)

INSERT INTO MuscleGroups([Name])
VALUES ('Chest'), ('Legs'), ('Arms'), ('Abs'), ('Back')

INSERT INTO Exercises([Name], [Description], Difficulty, MGID)
VALUES ('Bench Press', 'a', 8, 1), ('Pull Ups', 'a', 8, 5), ('Cable Flys', 'a', 6, 1)

INSERT INTO WorkoutTypes([Name])
VALUES ('upper'), ('lower')

INSERT INTO Workouts([Name], WTID)
VALUES ('workout1', 1), ('workout2', 1), ('workout3', 1), ('workout4', 1), ('workout5', 2)


INSERT INTO CompleteWorkouts(WID, EID, [Sets], RepsPerSet)
VALUES (1, 1, 4, 10), (1, 3, 4, 12), (2, 2, 5, 8)

INSERT INTO ClassTypes([Name])
VALUES ('dance'), ('fight'), ('stretch')

INSERT INTO PersonalTrainers([FirstName], [LastName], WorksSince)
VALUES ('Zelu', 'Popa', '2024-02-10'),('Rares', 'Racsan', '2024-03-11'), ('Mihai', 'Predescu', '2022-05-11')

INSERT INTO Classes([Name], [Description], CTID, PTID)
VALUES ('Samba', 'danceeee', 1, 1), ('Box', 'Guts', 2, 2), ('MMA', 'fightttt', 2, 2), ('Yoga', 'relax', 3, 3)

INSERT INTO Users DEFAULT VALUES;

select * from UserClasses