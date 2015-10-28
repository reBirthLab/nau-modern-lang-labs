using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessing
{
    class Student
    {
        private string lastName;
        private string firstName;
        private short course;
        private int studentId;
        private string sex;
        private string cityOfResidence;
        private int studentRecordbookId;

        public Student(string lastName, string firstName, short course, int studentId, string sex, string cityOfResidence, int studentRecordbookId)
        {
            this.lastName = lastName;
            this.firstName = firstName;
            this.course = course;
            this.studentId = studentId;
            this.sex = sex;
            this.cityOfResidence = cityOfResidence;
            this.studentRecordbookId = studentRecordbookId;
        }

        public string stringify()
        {
            return lastName + ";" + firstName + ";" + course.ToString() + ";" + studentId.ToString() + ";" + sex + ";" + cityOfResidence + ";" + studentRecordbookId.ToString();
        }

        public string getLastName()
        {
            return lastName;
        }

        public string getFirstName()
        {
            return firstName;
        }

        public short getCourse()
        {
            return course;
        }

        public int getStudentId()
        {
            return studentId;
        }

        public string getSex()
        {
            return sex;
        }

        public string getCityOfReference()
        {
            return cityOfResidence;
        }

        public int getStudentRecordbookId()
        {
            return studentRecordbookId;
        }
    }
}
