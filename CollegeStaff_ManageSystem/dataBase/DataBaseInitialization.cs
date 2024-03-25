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
                            subjectName TEXT PRIMARY KEY,
                            information TEXT NOT NULL
                        );";

                    string createPositionsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS positions(
                            positionName TEXT PRIMARY KEY,
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
                            position TEXT,
                            subject TEXT,
                            characteristic TEXT,
                            workHours INTEGER,
                            FOREIGN KEY (position) REFERENCES positions(positionName),
                            FOREIGN KEY (subject) REFERENCES subjects(subjectName)
                        );";

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createSubjectsTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createPositionsTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createTeachersTableQuery;
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
                            INSERT INTO teachers (name, lastName, otherName, address, position, subject, characteristic, workHours) 
                            VALUES 
                                ('Иван', 'Иванов', 'Иванович', 'ул. Пушкина, д. 10', 'Преподаватель', 'Математика', 'Преподаватель математики',40),
                                ('Петр', 'Петров', 'Петрович', 'пр. Ленина, д. 5', 'Преподаватель', 'Физика', 'Преподаватель физики',99),
                                ('Александр', 'Сидоров', 'Александрович', 'ул. Гагарина, д. 15', 'Преподаватель', 'История', 'Преподаватель истории',106),
                                ('Елена', 'Васильева', 'Андреевна', 'ул. Мира, д. 8', 'Преподаватель', 'Литература', 'Преподаватель литературы',40),
                                ('Марина', 'Смирнова', 'Сергеевна', 'пр. Кирова, д. 20', 'Преподаватель', 'Химия', 'Преподаватель химии',40),
                                ('Сергей', 'Козлов', 'Игоревич', 'ул. Советская, д. 3', 'Преподаватель', 'Биология', 'Преподаватель биологии',34),
                                ('Ольга', 'Николаева', 'Владимировна', 'ул. Лермонтова, д. 25', 'Преподаватель', 'География', 'Преподаватель географии',1),
                                ('Андрей', 'Морозов', 'Алексеевич', 'пр. Горького, д. 12', 'Преподаватель', 'Информатика', 'Преподаватель информатики',40),
                                ('Евгений', 'Белов', 'Дмитриевич', 'ул. Калинина, д. 6', 'Преподаватель', 'Иностранный язык', 'Преподаватель иностранных языков',40),
                                ('Татьяна', 'Ковалева', 'Ивановна', 'пр. Фрунзе, д. 30', 'Преподаватель', 'Музыка', 'Преподаватель музыки',2);";
                        command.CommandText = insertTeachersDataQuery;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
