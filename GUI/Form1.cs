using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL.Entities;

namespace GUI
{
    public partial class Form1 : Form
    {
        private readonly StudentBLL studentBLL = new StudentBLL();
        private readonly FacultyBLL facultyBLL = new FacultyBLL();
        public Form1()
        {
            InitializeComponent();
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AvgScore;
                if (item.MajorID != null)
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.MajorName;
                ShowAvatar(item.Avatar);
            }
        }

        private void ShowAvatar(string ImageName)
        {
            if (string.IsNullOrEmpty(ImageName))
            {
                picAvatar.Image = null;
            }
            else
            {
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
                string imagePath = Path.Combine(parentDirectory, "HTMLOL", ImageName);
                picAvatar.Image = Image.FromFile(imagePath);
                picAvatar.Refresh();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StudentContextDB contextDB = new StudentContextDB();
            List<Faculty> faculties = contextDB.Faculty.ToList();
            try
            {
                var listFaculties = facultyBLL.GetAll();
                var listStudents = studentBLL.GetAll();
                BindGrid(listStudents);

                foreach (var item in faculties)
                {
                    cmbFaculty.Items.Add(item.FacultyName);
                }
                cmbFaculty.SelectedItem = "Công nghệ thông tin";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
