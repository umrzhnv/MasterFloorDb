CREATE TABLE Сотрудники (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ФИО NVARCHAR(255) NOT NULL,
    ДатаРождения DATE NOT NULL,
    ПаспортныеДанные VARCHAR(255) NOT NULL UNIQUE,
    БанковскиеРеквизиты VARCHAR(255) NOT NULL UNIQUE,
    НаличиеСемьи BIT NOT NULL,
    СостояниеЗдоровья NTEXT NULL,
    Доступ BIT NOT NULL,
    Должность NVARCHAR(255) NOT NULL
);



CREATE TABLE Заявка (
    Id_Заявки INT PRIMARY KEY,
    Id_Партнера INT NOT NULL,
    Id_Продукции INT NOT NULL,
    Id_Сотрудника INT NOT NULL,
    Дата_заявки DATETIME NOT NULL DEFAULT GETDATE(),
    Дата_обработки DATETIME NULL,
    Статус_заявки NVARCHAR(50) NOT NULL DEFAULT 'Новая', -- Например: "Новая", "В обработке", "Одобрена", "Отклонена"
    Комментарий_сотрудника NVARCHAR(255) NULL,
    FOREIGN KEY (Id_Партнера) REFERENCES Партнеры(Id_Партнера),
    FOREIGN KEY (Id_Продукции) REFERENCES Импортируемая_продукция(Id_Продукции),
    FOREIGN KEY (Id_Сотрудника) REFERENCES Сотрудники(ID)
);
CREATE TABLE Продажи_импортируемой_продукции_партнерам (
    Id_Продажа INT PRIMARY KEY,
    Id_Заявки INT NOT NULL,
    Количество_продукции INT,
    Дата_продажи DATE,
    FOREIGN KEY (Id_Заявки) REFERENCES Заявка(Id_Заявки)
);