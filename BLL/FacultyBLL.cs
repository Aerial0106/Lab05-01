using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BLL
{
    public class FacultyBLL
    {
        public List<Faculty> GetAll()
        {
            StudentContextDB contextDB = new StudentContextDB();
            return contextDB.Faculty.ToList();
        }
    }
}
