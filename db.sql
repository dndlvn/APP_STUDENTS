CREATE DATABASE StudentsDB;
GO

USE StudentsDB;
GO

CREATE TABLE Student (
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    LastName NVARCHAR(100) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    MiddleName NVARCHAR(100)
);

CREATE TABLE Subject (
    SubjectId INT IDENTITY(1,1) PRIMARY KEY,
    SubjectName NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Term (
    TermId INT IDENTITY(1,1) PRIMARY KEY,
    TermName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Grade (
    GradeId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL,
    SubjectId INT NOT NULL,
    TermId INT NOT NULL,
    GradeValue TINYINT NOT NULL CHECK (GradeValue BETWEEN 1 AND 10),

    CONSTRAINT FK_Grade_Student FOREIGN KEY (StudentId) REFERENCES Student(StudentId) ON DELETE CASCADE,
    CONSTRAINT FK_Grade_Subject FOREIGN KEY (SubjectId) REFERENCES Subject(SubjectId),
    CONSTRAINT FK_Grade_Term FOREIGN KEY (TermId) REFERENCES Term(TermId),

    CONSTRAINT UQ_Grade UNIQUE (StudentId, SubjectId, TermId)
);


INSERT INTO Student (LastName, FirstName, MiddleName) VALUES
('Иванов', 'Михаил', NULL),
('Петрова', 'Ольга', NULL),
('Сидорова', 'Алла', NULL);

INSERT INTO Subject (SubjectName) VALUES
('Информатика'),
('Математика');

INSERT INTO Term (TermName) VALUES
('1 четверть'),
('2 четверть');

INSERT INTO Grade (StudentId, SubjectId, TermId, GradeValue) VALUES
(1, 1, 1, 4),
(1, 1, 2, 5),
(1, 2, 1, 5),
(1, 2, 2, 4);

INSERT INTO Grade (StudentId, SubjectId, TermId, GradeValue) VALUES
(2, 1, 1, 5),
(2, 1, 2, 5),
(2, 2, 1, 4),
(2, 2, 2, 4);

INSERT INTO Grade (StudentId, SubjectId, TermId, GradeValue) VALUES
(3, 1, 1, 4),
(3, 1, 2, 4),
(3, 2, 1, 4),
(3, 2, 2, 5);
