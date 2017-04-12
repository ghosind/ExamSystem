CREATE TABLE [User] (
  [uid] INT PRIMARY KEY IDENTITY(1,1),
  [username] VARCHAR(64) UNIQUE NOT NULL,
  [password] CHAR(128) NOT NULL,
  [name] VARCHAR(32) NOT NULL,
  [rank] INT NOT NULL DEFAULT 0,
  [login_date] DATETIME NOT NULL DEFAULT GETDATE()  /* date of last login */
);

CREATE TABLE [UserInfo] (
  [uid] INT REFERENCES [user]([uid]),
  [sex] CHAR(2) NOT NULL DEFAULT '男',
  [birthday] DATETIME NOT NULL DEFAULT '1970-01-01',
  [telephone] VARCHAR(11),
  [email] VARCHAR(128) UNIQUE NOT NULL ,
  [email_valid] BIT NOT NULL DEFAULT 0,  /* valid status of email */
  [address] VARCHAR(128),
  [description] VARCHAR(256),
  [reg_date] DATETIME NOT NULL DEFAULT GETDATE()  /* register date */
);

CREATE TABLE [ForgetPassword] (
	[uid] INT REFERENCES [User]([uid]),
	[code] char(128) NOT NULL
);

CREATE TABLE [Group] (
  [gid] INT PRIMARY KEY IDENTITY(1,1),
  [group_name] VARCHAR(64) NOT NULL,
  [owner_uid] INT REFERENCES [user]([uid]),
  [number] INT NOT NULL DEFAULT 0,
  [allow_join] BIT NOT NULL DEFAULT 1,
  [allow_quit] BIT NOT NULL DEFAULT 1
);

CREATE TABLE [GroupMember] (
  [gid] INT REFERENCES [group]([gid]),
  [uid] INT REFERENCES [user]([uid]),
  [rank] INT NOT NULL DEFAULT 0
);

CREATE TABLE [Message] (
  [mid] INT PRIMARY KEY IDENTITY(1,1),
  [sender] INT REFERENCES [user]([uid]),
  [receiver] INT REFERENCES [user]([uid]),
  [title] VARCHAR(128) NOT NULL,
  [content] TEXT NOT NULL,
  [send_date] DATETIME NOT NULL,
  [read] BIT NOT NULL DEFAULT 0
);

CREATE TABLE [News] (
  [nid] INT PRIMARY KEY IDENTITY(1,1),
  [publisher] INT REFERENCES [user]([uid]),
  [title] VARCHAR(256) NOT NULL,
  [content] TEXT NOT NULL,
  [date] DATETIME NOT NULL
);

CREATE TABLE [Subject] (
  [sid] INT PRIMARY KEY IDENTITY(1,1),
  [subject_name] VARCHAR(128) UNIQUE NOT NULL
);

CREATE TABLE [Section] (
  [kid] INT PRIMARY KEY IDENTITY(1,1),
  [section_name] VARCHAR(256) NOT NULL,
  [sid] INT REFERENCES [subject]([sid])
);

CREATE TABLE [Questions] (
  [qid] INT PRIMARY KEY IDENTITY(1,1),
  [type] INT NOT NULL,
  [kid] INT REFERENCES [Section]([kid]),
  [suggest_difficulty] FLOAT NOT NULL DEFAULT 0.5,
  [difficulty] FLOAT NOT NULL DEFAULT 0.5,
  [number] INT NOT NULL DEFAULT 0
);

CREATE TABLE [ChoiceQuestions] (
  [qid] INT REFERENCES [questions]([qid]),
  [content] TEXT NOT NULL,
  [choice_num] INT NOT NULL DEFAULT 4,
  [choice_1] TEXT NOT NULL,
  [choice_2] TEXT NOT NULL,
  [choice_3] TEXT,
  [choice_4] TEXT, 
  [answer] INT NOT NULL
);

CREATE TABLE [FillQuestions] (
  [qid] INT REFERENCES [questions]([qid]),
  [content] TEXT NOT NULL,
  [answer] TEXT NOT NULL
);

CREATE TABLE [DiscussQuestions] (
  [qid] INT REFERENCES [questions]([qid]),
  [content] TEXT NOT NULL,
  [answer] TEXT NOT NULL
);

CREATE TABLE [Exams] (
  [eid] INT PRIMARY KEY IDENTITY(1,1),
  [title] VARCHAR(128) NOT NULL,
  [subject] INT REFERENCES [Subject]([sid]),
  [time] INT NOT NULL DEFAULT 60,
  [start_date] DATETIME NOT NULL,
  [end_date] DATETIME NOT NULL,
  [exam_path] VARCHAR(128) NOT NULL,  /* xml file path of exam */
  [answer_path] VARCHAR(128) NOT NULL,
  [must_take] BIT NOT NULL DEFAULT 0,
  [public] BIT NOT NULL DEFAULT 1
);

CREATE TABLE [ExamsGroup] (
  [eid] INT REFERENCES [exams]([eid]),
  [gid] INT REFERENCES [Group]([gid])  /* set group id to specify a group to take the exam. 0 means every one can take this exam and negative number means nobody can take the exam. */
);

CREATE TABLE [Result] (
  [uid] INT REFERENCES [user]([uid]),
  [eid] INT REFERENCES [exams]([eid]),
  [answer] VARCHAR(128) NOT NULL,  /* xml file path of answers */
  [reviewer] INT REFERENCES [user]([uid]),
  [score] INT NOT NULL DEFAULT -1  /* -1 means the answers is unreviewed */
);


INSERT INTO [User]([username], [password], [name], [rank]) VALUES(@username, @password, @nickname, 3);

INSERT INTO [UserInfo]([uid], [email]) VALUES(1, @email);