-- Tasks определение

CREATE TABLE Tasks (
	Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	Title NVARCHAR,
	Description NVARCHAR,
	IsCompleted bit,
	CreatedAt datetime
);