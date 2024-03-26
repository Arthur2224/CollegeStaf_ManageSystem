using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace WindowsFormsApp1
{
    class DataBaseInitialization
    {
        private static string connectionString = @"Data Source=..\\..\\Files\\CollegeStaff.db;Version=3;";

        public static void InitializeDataBase()
        {
            if (!File.Exists(@"..\\..\\Files\\CollegeStaff.db"))
            {
                SQLiteConnection.CreateFile(@"..\\..\\Files\\CollegeStaff.db");

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string createSubjectsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS subjects(
                            subjectID INTEGER PRIMARY KEY AUTOINCREMENT,
                            subjectName TEXT NOT NULL,
                            information TEXT NOT NULL
                        );";

                    string createPositionsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS positions(
                            positionID INTEGER PRIMARY KEY AUTOINCREMENT,
                            positionName TEXT NOT NULL,
                            hourPaid MONEY NOT NULL,
                            information TEXT NOT NULL
                        );";

                    string createTeachersTableQuery = @"
                        CREATE TABLE IF NOT EXISTS teachers(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            name TEXT NOT NULL,
                            lastName TEXT NOT NULL,
                            otherName TEXT,
                            address TEXT NOT NULL,
                            positionID INTEGER,
                            characteristic TEXT,
                            workHours INTEGER,
                            FOREIGN KEY (positionID) REFERENCES positions(positionID)
                        );";

                    string createTeacherSubjectsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS teacherSubjects(
                            teacherID INTEGER,
                            subjectID INTEGER,
                            FOREIGN KEY (teacherID) REFERENCES teachers(id),
                            FOREIGN KEY (subjectID) REFERENCES subjects(subjectID),
                            PRIMARY KEY (teacherID, subjectID)
                        );";

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createSubjectsTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createPositionsTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createTeachersTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createTeacherSubjectsTableQuery;
                        command.ExecuteNonQuery();

                        string insertSubjectsDataQuery = @"
                            INSERT INTO subjects (subjectName, information) 
                            VALUES 
                                ('Математика', 'Информация о предмете Математика'),
                                ('Физика', 'Информация о предмете Физика'),
                                ('История', 'Информация о предмете История'),
                                ('Литература', 'Информация о предмете Литература'),
                                ('Химия', 'Информация о предмете Химия'),
                                ('Биология', 'Информация о предмете Биология'),
                                ('География', 'Информация о предмете География'),
                                ('Информатика', 'Информация о предмете Информатика'),
                                ('Иностранный язык', 'Информация о предмете Иностранный язык'),
                                ('Музыка', 'Информация о предмете Музыка');";
                        command.CommandText = insertSubjectsDataQuery;
                        command.ExecuteNonQuery();

                        
                        string insertPositionsDataQuery = @"
                            INSERT INTO positions (positionName, hourPaid, information) 
                            VALUES 
                                ('Преподаватель', 100, 'Информация о должности Преподаватель'),
                                ('Профессор', 150, 'Информация о должности Профессор');";
                        command.CommandText = insertPositionsDataQuery;
                        command.ExecuteNonQuery();


                        string insertTeachersDataQuery = @"
    INSERT INTO teachers (name, lastName, otherName, address, positionID, characteristic, workHours) 
    VALUES 
        ('Иван', 'Иванов', 'Иванович', 'ул. Пушкина, д. 10', 1, 'Преподаватель математики', 40),
        ('Петр', 'Петров', 'Петрович', 'пр. Ленина, д. 5', 1, 'Преподаватель физики', 99),
        ('Александр', 'Сидоров', 'Александрович', 'ул. Гагарина, д. 15', 1, 'Преподаватель истории', 106),
        ('Елена', 'Васильева', 'Андреевна', 'ул. Мира, д. 8', 1, 'Преподаватель литературы', 40),
        ('Марина', 'Смирнова', 'Сергеевна', 'пр. Кирова, д. 20', 1, 'Преподаватель химии', 40),
        ('Сергей', 'Козлов', 'Игоревич', 'ул. Советская, д. 3', 1, 'Преподаватель биологии', 34),
        ('Ольга', 'Николаева', 'Владимировна', 'ул. Лермонтова, д. 25', 1, 'Преподаватель географии', 1),
        ('Андрей', 'Морозов', 'Алексеевич', 'пр. Горького, д. 12', 1, 'Преподаватель информатики', 40),
        ('Евгений', 'Белов', 'Дмитриевич', 'ул. Калинина, д. 6', 1, 'Преподаватель иностранных языков', 40),
        ('Татьяна', 'Ковалева', 'Ивановна', 'пр. Фрунзе, д. 30', 1, 'Преподаватель музыки', 2);";
                        command.CommandText = insertTeachersDataQuery;
                        command.ExecuteNonQuery();

                        string insertTeacherSubjectsDataQuery = @"
                INSERT INTO teacherSubjects (teacherID, subjectID) 
                VALUES 
                    (1, 1),
                    (1, 2),
                    (2, 2),
                    (3, 3),
                    (4, 4),
                    (5, 5),
                    (5, 7),
                    (6, 6),
                    (7, 7),
                    (8, 8),
                    (9, 9),
                    (10, 10);";
                        command.CommandText = insertTeacherSubjectsDataQuery;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
