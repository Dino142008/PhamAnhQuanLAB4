using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exercise_1.Model;

namespace Exercise_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                List<Faculty> listFalcultys = context.Faculties.ToList(); //lấy các khoa
                List<Student> listStudent = context.Students.ToList(); //lấy sinh viên
                FillFalcultyCombobox(listFalcultys);
                BindGrid(listStudent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Hàm binding list có tên hiện thị là tên khoa, giá trị là Mã khoa
        private void FillFalcultyCombobox(List<Faculty> listFalcultys)
        {
            this.cmbFaculty.DataSource = listFalcultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }
        //Hàm binding gridView từ list sinh viên
        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                List<Student> studentLst = context.Students.ToList();

                if (studentLst.Any(s => s.StudentID == txtStudentId.Text))
                {
                    MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newStudent = new Student
                {
                    StudentID = txtStudentId.Text,
                    FullName = txtFullname.Text,
                    AverageScore = double.Parse(txtAverageScore.Text),
                    FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString())
                };

                // Add the new student to the list
                context.Students.Add(newStudent);
                context.SaveChanges();

                // Reload the data
                BindGrid(context.Students.ToList());
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB db = new StudentContextDB();
                List<Student> studentLst = db.Students.ToList();
                var student = studentLst.FirstOrDefault(s => s.StudentID == txtStudentId.Text);

                if (student != null)
                {
                    // Update student details
                    student.FullName = txtFullname.Text;
                    student.AverageScore = double.Parse(txtAverageScore.Text);
                    student.FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString());

                    // Save changes to the database
                    db.SaveChanges();

                    // Reload the data
                    BindGrid(db.Students.ToList());

                    MessageBox.Show("Chỉnh sửa thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Handle student not found
                    MessageBox.Show("Sinh viên không tìm thấy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (studentLst.Any(s => s.StudentID == txtStudentId.Text && s.StudentID != student.StudentID))
                {
                    MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                List<Student> studentList = context.Students.ToList();

                // Find the student by Student ID
                var student = studentList.FirstOrDefault(s => s.StudentID == txtStudentId.Text);

                if (student != null)
                {
                    // Remove the student from the list
                    context.Students.Remove(student);
                    context.SaveChanges();
                    BindGrid(context.Students.ToList());
                    MessageBox.Show("Sinh viên đã được xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sinh viên không tìm thấy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvStudent.Rows[e.RowIndex];
                txtStudentId.Text = selectedRow.Cells[0].Value.ToString();
                txtFullname.Text = selectedRow.Cells[1].Value.ToString();
                cmbFaculty.Text = selectedRow.Cells[2].Value.ToString();
                txtAverageScore.Text = selectedRow.Cells[3].Value.ToString();
            }
        }

        private void frmQuanLyKhoa_Click(object sender, EventArgs e)
        {
            frmFalculty frmFalculty = new frmFalculty();

            frmFalculty.Show();

            Hide(); 
        }

        private void frmTim_Click(object sender, EventArgs e)
        {
            frmTim frmTim = new frmTim();

            frmTim.Show();
        }
    }
    }
