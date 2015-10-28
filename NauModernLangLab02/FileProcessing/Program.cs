using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating list of students...");
            Console.Write("Enter path and/or name of list file to write >>> ");
            string filePath = Console.ReadLine();

            StreamWriter fileToWrite = null;

            try
            {
                fileToWrite = new StreamWriter(filePath, true);
                string choice1 = "y";
                Console.WriteLine("\nEnter student's data...");

                do
                {
                    try
                    {
                        string lastName;
                        string firstName;
                        short course;
                        int studentId;
                        string sex;
                        string cityOfResidence;
                        int studentRecordbookId;

                        do
                        {
                            try
                            {
                                Console.Write("Enter the student's last name >>> ");

                                lastName = Console.ReadLine();

                                if (lastName.Equals("")) throw new FormatException("Last name field can't be empty");

                                for (int i = 0; i < lastName.Length; i++)
                                {
                                    if (!Char.IsLetter(lastName[i]) || Char.IsLower(lastName[0]))
                                        throw new FormatException("Wrong last name. Not all characters are letters and/or a first letter is lowercase");
                                }
                                break;
                            }
                            catch (FormatException fEx)
                            {
                                Console.WriteLine("[ERROR] Wrong data format: {0}", fEx.Message);
                                Console.WriteLine("Please try again");
                            }
                        } while (true);

                        do
                        {
                            try
                            {
                                Console.Write("Enter the student's first name >>> ");

                                firstName = Console.ReadLine();

                                if (firstName.Equals("")) throw new FormatException("First name field can't be empty");

                                for (int i = 0; i < firstName.Length; i++)
                                {
                                    if (!Char.IsLetter(firstName[i]) || Char.IsLower(firstName[0]))
                                        throw new FormatException("Wrong first name. Not all characters are letters and/or a first letter is lowercase");
                                }
                                break;
                            }
                            catch (FormatException fEx)
                            {
                                Console.WriteLine("[ERROR] Wrong data format: {0}", fEx.Message);
                                Console.WriteLine("Please try again");
                            }
                        } while (true);

                        do
                        {
                            try
                            {
                                Console.Write("Enter the student's year of studying [1-5] >>> ");

                                course = Convert.ToInt16(Console.ReadLine());

                                if (course < 1 || course > 5)
                                    throw new FormatException("The year of studying is out of range");
                                break;
                            }
                            catch (FormatException fEx)
                            {
                                Console.WriteLine("[ERROR] Wrong data format: {0}", fEx.Message);
                                Console.WriteLine("Please try again");
                            }
                        } while (true);

                        do
                        {
                            try
                            {
                                Console.Write("Enter the student ID number [1000-9000] >>> ");

                                studentId = Convert.ToInt32(Console.ReadLine());

                                if (studentId < 1000 || studentId > 9000)
                                    throw new FormatException("The student ID number is out of range");
                                break;
                            }
                            catch (FormatException fEx)
                            {
                                Console.WriteLine("[ERROR] Wrong data format: {0}", fEx.Message);
                                Console.WriteLine("Please try again");
                            }
                        } while (true);

                        do
                        {
                            try
                            {
                                Console.Write("Enter the sex of student [male/female] >>> ");

                                sex = Console.ReadLine();

                                if (sex.Equals("")) throw new FormatException("Sex field can't be empty");

                                if (!sex.Equals("male") && !sex.Equals("female")) throw new FormatException("Sex field must be \"male\" or \"female\" only");

                                break;
                            }
                            catch (FormatException fEx)
                            {
                                Console.WriteLine("[ERROR] Wrong data format: {0}", fEx.Message);
                                Console.WriteLine("Please try again");
                            }
                        } while (true);

                        do
                        {
                            try
                            {
                                Console.Write("Enter the student's city of residence >>> ");

                                cityOfResidence = Console.ReadLine();

                                if (cityOfResidence.Equals("")) throw new FormatException("City of residence field can't be empty");

                                for (int i = 0; i < cityOfResidence.Length; i++)
                                {
                                    Console.Write(cityOfResidence[i]);
                                    if ((!Char.IsLetter(cityOfResidence[i]) && !cityOfResidence[i].Equals(' ')) || Char.IsLower(cityOfResidence[0]))
                                        throw new FormatException("Wrong city of residence. Not all characters are letters and/or a first letter is lowercase");
                                }
                                break;
                            }
                            catch (FormatException fEx)
                            {
                                Console.WriteLine("[ERROR] Wrong data format: {0}", fEx.Message);
                                Console.WriteLine("Please try again");
                            }
                        } while (true);

                        do
                        {
                            try
                            {
                                Console.Write("Enter the student's Recordbook ID number [1000-9000] >>> ");

                                studentRecordbookId = Convert.ToInt32(Console.ReadLine());

                                if (studentRecordbookId < 1000 || studentRecordbookId > 9000)
                                    throw new FormatException("The student's Recordbook ID number is out of range");
                                break;
                            }
                            catch (FormatException fEx)
                            {
                                Console.WriteLine("[ERROR] Wrong data format: {0}", fEx.Message);
                                Console.WriteLine("Please try again");
                            }
                        } while (true);

                        Student st = new Student(lastName, firstName, course, studentId, sex, cityOfResidence, studentRecordbookId);

                        fileToWrite.WriteLine(st.stringify());

                        Console.Write("Do you want to add another student to list? [y/n] >>> ");
                        choice1 = Console.ReadLine();

                        while (!choice1.Equals("y") && !choice1.Equals("n"))
                        {
                            Console.WriteLine("Enter \"y\" for Yes or \"n\" for No >>> ");
                            choice1 = Console.ReadLine();
                        }

                        Console.WriteLine();
                    }
                    catch (IOException ioEx)
                    {
                        Console.WriteLine("[ERROR] I/O Exception: {0}", ioEx.Message);
                        return;
                    }
                } while (choice1.Equals("y"));

                fileToWrite.Close();
                Console.WriteLine("Student's data was successfully written to file {0}\n", filePath);
            }
            catch (DirectoryNotFoundException dnfEx)
            {
                Console.WriteLine("[ERROR] Wrong file path: {0}", dnfEx.Message);
                return;
            }
            catch (IOException ioEx)
            {
                Console.WriteLine("[ERROR] I/O Exception: {0}", ioEx.Message);
                return;
            }


            Console.WriteLine("Reading list of students...");

            Console.Write("Path and/or name of list file to read >>> ");
            filePath = Console.ReadLine();

            StreamReader fileToRead;
            Student[] studentList;
            int index = 0;
            int studentCounter = 0;
            List<Student> selection = new List<Student>();

            try
            {
                fileToRead = new StreamReader(@filePath);
                studentCounter = fileToRead.ReadToEnd().Split('\n').Length;

                fileToRead = new StreamReader(@filePath);
                studentList = new Student[studentCounter - 1];

                while (fileToRead.EndOfStream != true)
                {
                    String[] data = fileToRead.ReadLine().Split(';');
                    studentList[index] = new Student(data[0], data[1], Convert.ToInt16(data[2]), Convert.ToInt32(data[3]), data[4], data[5], Convert.ToInt32(data[6]));

                    if (studentList[index].getCourse().Equals(5) && studentList[index].getCityOfReference().Equals("Kiev"))
                    {
                        selection.Add(studentList[index]);
                    }

                    index++;
                }

                fileToRead.Close();

                Console.WriteLine("\nList of students of 5th year of studying whose city of residence is Kiev:");

                int counter = 1;
                foreach (Student st in selection)
                {
                    Console.WriteLine("[Student {0}] Name: {1} {2}, Year of studying: {3}, Student ID: {4}, Sex: {5}, City of residence: {6}, Recordbook ID: {7}\n",
                        counter, st.getFirstName(), st.getLastName(), st.getCourse(), st.getStudentId(), st.getSex(), st.getCityOfReference(), st.getStudentRecordbookId());
                    ++counter;
                }
            }
            catch (FileNotFoundException fnfEx)
            {
                Console.WriteLine("[ERROR] File not found: {0}", fnfEx.Message);
            }
            catch (IndexOutOfRangeException iorEx)
            {
                Console.WriteLine("[ERROR] Index out of range. Added {0} elements to array only. {1}", studentCounter, iorEx.Message);
            }
            catch (IOException ioEx)
            {
                Console.WriteLine("[ERROR] I/O Exception: {0}", ioEx.Message);
            }
            catch (ArgumentException argEx) {
                Console.WriteLine("[ERROR] File path wasn't define: {0}", argEx.Message);
            }

            Console.Read();
        }
    }
}