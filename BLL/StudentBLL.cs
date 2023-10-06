using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class StudentBLL
    {
        public List<Student> GetAll()
        {
            StudentContextDB contextDB = new StudentContextDB();
            return contextDB.Student.ToList();
        }

        public List<Student> GetAllHasNoMajor()
        {
            StudentContextDB contextDB = new StudentContextDB();
            return contextDB.Student.Where(s => s.MajorID == null).ToList();
        }

        public List<Student> GetAllHasNoMajor(int facultyID)
        {
            StudentContextDB contextDB = new StudentContextDB();
            return contextDB.Student.Where(s => s.MajorID == null && s.FacultyID == facultyID).ToList();
        }

        public Student FindByID(string studentID)
        {
            StudentContextDB contextDB = new StudentContextDB();
            return contextDB.Student.FirstOrDefault(s => s.StudentID == studentID);
        }

        public void InsertUpdate(Student student)
        {
            StudentContextDB contextDB = new StudentContextDB();
            contextDB.Student.AddOrUpdate(student);
            contextDB.SaveChanges();
        }
    }
}
