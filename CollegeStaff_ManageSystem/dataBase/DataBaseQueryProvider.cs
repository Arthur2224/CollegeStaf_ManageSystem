using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeStaff_ManageSystem.dataBase
{
    internal class DataBaseQueryProvider
    {
        private string connectionString = @"Data Source=..\..\\Files\\CollegeStaff.db;Version=3;";
        public void AddNewEntity(string query)
        {
            using(var connection=new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteRecord(string tableName,string PKName,string primaryKeyValue)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = $"DELETE FROM {tableName} WHERE {PKName} = @PrimaryKeyValue";
                using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                {  
                    command.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);
                    command.ExecuteNonQuery();
                }
            }
        }
        // Сформировать запрос на выборку, в котором, используя групповые операции,
        // определить среднее количество часов, отработанных каждым преподавателем.
        // Название запроса - "Среднее количество часов"
        public DataTable GetAverageWorkHours()
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        SELECT 
                            name || ' ' || lastName AS [Преподаватель],
                            AVG(workHours) AS [Среднее количество часов]
                        FROM 
                            teachers
                        GROUP BY 
                            name, lastName";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        // Сформировать запрос на выборку, который выдает на экран список
        // преподавателей, проживающих в указанном городе.
        // Название запроса - "Преподаватели по городу"
        public DataTable GetTeachersByCity(string city)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        SELECT 
                            name || ' ' || lastName AS [Преподаватель],
                            address AS [Адрес]
                        FROM 
                            teachers
                        WHERE 
                            address LIKE '%' || @City || '%'";
                    command.Parameters.AddWithValue("@City", city);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
        // Сформировать запрос на выборку, который выводит на экран список
        // предметов, проводимых преподавателями на должности "Преподаватель".
        // Название запроса - "Предметы преподавателей"
        public DataTable GetSubjectsForTeachers()
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                SELECT 
                    t.lastName AS [Фамилия преподавателя],
                    s.subjectName AS [Предмет]
                FROM 
                    teacherSubjects AS ts
                INNER JOIN 
                    subjects AS s ON ts.subjectID = s.subjectID
                INNER JOIN 
                    teachers AS t ON ts.teacherID = t.id
                WHERE 
                    t.positionID IN (SELECT positionID FROM positions WHERE positionName = 'Преподаватель')";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }



        // Сформировать запрос на выборку, который выводит на экран список
        // преподавателей, чьи фамилии начинаются с заданной буквы.
        // Название запроса - "Преподаватели по букве фамилии"
        public DataTable GetTeachersByLastNameInitial(string initial)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        SELECT 
                            name || ' ' || lastName AS [Преподаватель]
                        FROM 
                            teachers
                        WHERE 
                            lastName LIKE @Initial || '%'";
                    command.Parameters.AddWithValue("@Initial", initial);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        // Сформировать запрос на выборку, который выдает на экран список
        // преподавателей, чья должностная почасовая ставка меньше заданного значения.
        // Название запроса - "Преподаватели по почасовой ставке"
        public DataTable GetTeachersByHourlyRate(decimal maxHourlyRate)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        SELECT 
                            name || ' ' || lastName AS [Преподаватель],
                            positions.hourPaid AS [Почасовая ставка]
                        FROM 
                            teachers
                        JOIN 
                            positions ON teachers.position = positions.positionName
                        WHERE 
                            positions.hourPaid < @MaxHourlyRate";
                    command.Parameters.AddWithValue("@MaxHourlyRate", maxHourlyRate);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        // Сформировать запрос на выборку, который выдает на экран список преподавателей,
        // проводивших занятия по указанному предмету, и сумму к выплате в USD, исходя из заданного курса доллара.
        // Запрос должен содержать два параметра: "Название предмета" и "Курс доллара".
        // Название запроса - "Запрос с параметрами".
        // Сумму к выплате вывести с точностью до двух обязательных знаков после запятой.
        public DataTable QueryWithParameters(string subjectName, decimal exchangeRate)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        SELECT 
                            name || ' ' || lastName || ' ' || COALESCE(otherName, '') AS [Имя преподавателя],
                            ROUND(positions.hourPaid * teachers.workHours * @ExchangeRate, 2) AS [Сумма к выплате в USD]
                        FROM 
                            teachers
                        JOIN 
                            positions ON teachers.position = positions.positionName
                        WHERE 
                            teachers.subject = @SubjectName";
                    command.Parameters.AddWithValue("@SubjectName", subjectName);
                    command.Parameters.AddWithValue("@ExchangeRate", exchangeRate);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        // Сформировать запрос на выборку, который выдает на экран список фамилий и
        // инициалов всех преподавателей, хранящихся в базе данных.
        // Название запроса - "Фамилии и инициалы".
        public DataTable FetchLastNamesAndInitials()
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        SELECT 
                            lastName || ' ' || SUBSTR(name, 1, 1) || '.' || COALESCE(SUBSTR(otherName, 1, 1) || '.', '') AS [Фамилия и инициалы]
                        FROM 
                            teachers";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        // Сформировать запрос на выборку, который выводит на экран содержимое поля
        // "Название должности преподавателя", преобразованное к верхнему (нижнему)
        // регистру.
        // Название запроса - "Изменение регистра"
            public DataTable ChangeCaseOfPositionName(string newCase)
            {
                DataTable dataTable = new DataTable();
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                    command.CommandText = @"
    SELECT 
        " + newCase + "(positions.positionName) AS [Название должности в " + newCase + @"]
    FROM 
        positions";


                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {   
                            adapter.Fill(dataTable);
                        }

                    }
                    foreach (DataRow row in dataTable.Rows)
            {   if(newCase=="UPPER")
                    {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    row[i] = row[i].ToString().ToUpper();
                }
                    }
                    else
                    {
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            row[i] = row[i].ToString().ToLower();
                        }
                    }
               
            }
                }
                return dataTable;
            }

        // Сформировать запрос на выборку, в котором, используя групповые операции,
        // определить максимальную должностную почасовую ставку.
        // Название запроса - "Максимальная почасовая ставка"
        public DataTable GetMaxHourlyRate()
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        SELECT 
                            MAX(hourPaid) AS [Максимальная почасовая ставка]
                        FROM 
                            positions";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        // Сформировать запрос на выборку, в котором, используя групповые операции,
        // определить суммарное количество часов, отработанных каждым преподавателем.
        // Название запроса - "Суммарное количество часов"
        public DataTable GetTotalWorkHours()
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        SELECT 
                            name || ' ' || lastName AS [Преподаватель],
                            SUM(workHours) AS [Суммарное количество часов]
                        FROM 
                            teachers
                        GROUP BY 
                            name, lastName";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        // Сформировать запрос на выборку, в котором, используя групповые операции,
        // определить по скольким предметам проводит занятия каждый преподаватель.
        // Название запроса - "Количество предметов"
        public DataTable CountSubjectsPerTeacher()
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                SELECT 
                    t.name || ' ' || t.lastName AS [Преподаватель],
                    COUNT(DISTINCT ts.subjectID) AS [Количество предметов]
                FROM 
                    teachers AS t
                LEFT JOIN 
                    teacherSubjects AS ts ON t.id = ts.teacherID
                GROUP BY 
                    t.name, t.lastName";

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            return dataTable;
        }

    }
}
