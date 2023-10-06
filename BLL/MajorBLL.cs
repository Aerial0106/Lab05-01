using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
namespace BLL
{
    public class MajorBLL
    {
        public List<Major> GetAllByFaculty(int facultyID)
        {
            StudentContextDB contextDB = new StudentContextDB();
            return contextDB.Major.Where(m => m.FacultyID == facultyID).ToList();
        }
    }
}
