using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
                string imagePath = Path.Combine(parentDirectory, "Images", ImageName);
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

        private void btnQuit_Click(object sender, EventArgs e)
        {
            DialogResult dl = MessageBox.Show("Bạn có muốn thoát", "thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dl == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            StudentContextDB ContextDB = new StudentContextDB();
            List<Student> listStudent = ContextDB.Student.ToList();

            DialogResult dl = MessageBox.Show("Bạn có muốn xóa sinh viên", "thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dl == DialogResult.Yes)
            {
                Student dbDeltete = ContextDB.Student.FirstOrDefault(nv => nv.StudentID == txtStudentID.Text);
                if (dbDeltete != null)
                {
                    ContextDB.Student.Remove(dbDeltete);
                    ContextDB.SaveChanges();
                    List<Student> listNewNhanvien = ContextDB.Student.ToList();
                    dgvStudent.DataSource = null;
                    BindGrid(listNewNhanvien);
                    txtStudentID.Text = "";
                    txtFullName.Text = "";
                    txtAvgScore.Text = "";
                    MessageBox.Show("Xóa nhân viên thành công", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB ContextDB = new StudentContextDB();
                if (txtStudentID.Text == "" || txtFullName.Text == "" || txtAvgScore.Text == "")
                {
                    throw new Exception("Hãy điền đủ thông tin !!!");
                }
                List<Student> listStudent = ContextDB.Student.ToList();
                Student checkID = ContextDB.Student.FirstOrDefault(nv => nv.StudentID == txtStudentID.Text);
                if (checkID != null)
                {
                    throw new Exception("Mã số nhân viên đã tồn tại!!!");
                }

                string selectedFaculty = cmbFaculty.Text;
                Faculty selectedFacultyObj = ContextDB.Faculty.FirstOrDefault(NV => NV.FacultyName == selectedFaculty);
                int facultyID = selectedFacultyObj.FacultyID;

                Student student = new Student() { StudentID = txtStudentID.Text, FullName = txtFullName.Text, FacultyID = facultyID, AvgScore = Double.Parse(txtAvgScore.Text) };
                ContextDB.Student.Add(student);
                ContextDB.SaveChanges();

                List<Student> listNewStudent = ContextDB.Student.ToList();
                dgvStudent.DataSource = null;
                BindGrid(listNewStudent);
                txtStudentID.Text = "";
                txtFullName.Text = "";
                txtAvgScore.Text = "";
                throw new Exception("Thêm nhân viên thành công !!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            StudentContextDB ContextDB = new StudentContextDB();
            string selectedFaculty = cmbFaculty.Text;
            Faculty selectedFacultyObj = ContextDB.Faculty.FirstOrDefault(NV => NV.FacultyName == selectedFaculty);
            int facultyID = selectedFacultyObj.FacultyID;
            try
            {
                Student dbUpdate = ContextDB.Student.FirstOrDefault(nv => nv.StudentID == txtStudentID.Text);
                if (dbUpdate != null)
                {
                    dbUpdate.FullName = txtFullName.Text;
                    dbUpdate.FacultyID = facultyID;
                    dbUpdate.AvgScore = Double.Parse(txtAvgScore.Text);
                    ContextDB.SaveChanges();
                    List<Student> listnewStudent = ContextDB.Student.ToList();
                    dgvStudent.DataSource = null;
                    BindGrid(listnewStudent);
                    throw new Exception("Cập nhật thành công !!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgvStudent.CurrentRow.Index;
            txtStudentID.Text = dgvStudent.Rows[i].Cells[0].Value.ToString();
            txtFullName.Text = dgvStudent.Rows[i].Cells[1].Value.ToString();
            cmbFaculty.Text = dgvStudent.Rows[i].Cells[2].Value.ToString();
            txtAvgScore.Text = dgvStudent.Rows[i].Cells[3].Value.ToString();
        }
    }
}
