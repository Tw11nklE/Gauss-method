using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Kursovaya_rabota
{
    public partial class NewForm : Form
    {
        private int numberOfUnknowns;
        public NewForm(int numberOfUnknowns)
        {
            InitializeComponent();
            this.numberOfUnknowns = numberOfUnknowns;
            int fontSize = 14;
            int verticalSpacing = 10;

            System.Windows.Forms.Label labelMatrixA = new System.Windows.Forms.Label();
            labelMatrixA.Text = "Матрица A";
            int matrixAWidth = (this.Width - 20) / (numberOfUnknowns + 2); // Общая ширина для матрицы A
            labelMatrixA.Width = matrixAWidth * numberOfUnknowns; // Установка ширины, учитывая несколько полей
            labelMatrixA.Location = new System.Drawing.Point(0, 5); // Вертикальный отступ
            labelMatrixA.Font = new Font("Arial", 10, FontStyle.Bold);
            labelMatrixA.TextAlign = ContentAlignment.MiddleCenter; // Выравнивание по центру
            this.Controls.Add(labelMatrixA);

            System.Windows.Forms.Label labelMatrixB = new System.Windows.Forms.Label();
            labelMatrixB.Text = "Матрица B";
            labelMatrixB.Width = (this.Width - 20) / (numberOfUnknowns + 2) - 10; // Установка ширины
            labelMatrixB.Location = new System.Drawing.Point(10 + numberOfUnknowns * (this.Width - 20) / (numberOfUnknowns + 2), 5); // Вертикальный отступ
            labelMatrixB.Font = new Font("Arial", 10, FontStyle.Bold);
            labelMatrixB.TextAlign = ContentAlignment.MiddleCenter; // Выравнивание по центру
            this.Controls.Add(labelMatrixB);

            System.Windows.Forms.Label labelAnswer = new System.Windows.Forms.Label();
            labelAnswer.Text = "Ответ";
            labelAnswer.Width = (this.Width - 20) / (numberOfUnknowns + 2) - 10; // Установка ширины
            labelAnswer.Location = new System.Drawing.Point(10 + (numberOfUnknowns + 1) * (this.Width - 20) / (numberOfUnknowns + 2), 5); // Вертикальный отступ
            labelAnswer.Font = new Font("Arial", 10, FontStyle.Bold);
            labelAnswer.TextAlign = ContentAlignment.MiddleCenter; // Выравнивание по центру
            this.Controls.Add(labelAnswer);

            // Цикл создания полей ввода для матриц
            for (int i = 0; i < numberOfUnknowns; i++)
            {
                // Создание полей ввода для матрицы A
                System.Windows.Forms.TextBox[] matrixATextBoxes = new System.Windows.Forms.TextBox[numberOfUnknowns];
                for (int j = 0; j < numberOfUnknowns; j++)
                {
                    matrixATextBoxes[j] = new System.Windows.Forms.TextBox();
                    matrixATextBoxes[j].Location = new System.Drawing.Point(10 + j * (this.Width - 20) / (numberOfUnknowns + 2), 30 * (i + 1) + i * verticalSpacing);
                    matrixATextBoxes[j].Width = (this.Width - 20) / (numberOfUnknowns + 2) - 10;
                    matrixATextBoxes[j].Name = "A_" + i + "_" + j;
                    matrixATextBoxes[j].Font = new Font("Arial", fontSize);
                    this.Controls.Add(matrixATextBoxes[j]);
                }

                // Создание полей ввода для матрицы B
                System.Windows.Forms.TextBox matrixBTextBox = new System.Windows.Forms.TextBox();
                matrixBTextBox.Location = new System.Drawing.Point(10 + numberOfUnknowns * (this.Width - 20) / (numberOfUnknowns + 2), 30 * (i + 1) + i * verticalSpacing);
                matrixBTextBox.Width = (this.Width - 20) / (numberOfUnknowns + 2) - 10;
                matrixBTextBox.Name = "B_" + i;
                matrixBTextBox.Font = new Font("Arial", fontSize);
                this.Controls.Add(matrixBTextBox);

                // Создание полей для отображения ответа
                System.Windows.Forms.TextBox answerTextBox = new System.Windows.Forms.TextBox();
                answerTextBox.Location = new System.Drawing.Point(10 + (numberOfUnknowns + 1) * (this.Width - 20) / (numberOfUnknowns + 2), 30 * (i + 1) + i * verticalSpacing);
                answerTextBox.Width = (this.Width - 20) / (numberOfUnknowns + 2) - 10;
                answerTextBox.Name = "answer_" + i;
                answerTextBox.Font = new Font("Arial", fontSize);
                answerTextBox.BackColor = Color.DarkGray;
                this.Controls.Add(answerTextBox);
            }
            int totalHeight = (numberOfUnknowns + 1) * (30 + verticalSpacing) + 150; // 30 - высота текстового поля, 50 - дополнительные отступы
            this.Height = totalHeight;

        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void nazad_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
        private void Result_Click(object sender, EventArgs e)
        {
            // Получение значений из текстовых полей матрицы A
            double[,] matrixA = new double[numberOfUnknowns, numberOfUnknowns];
            for (int i = 0; i < numberOfUnknowns; i++)
            {
                for (int j = 0; j < numberOfUnknowns; j++)
                {
                    string textBoxName = "A_" + i + "_" + j;
                    if (!double.TryParse(this.Controls[textBoxName].Text, out matrixA[i, j]))
                    {
                        MessageBox.Show("Ошибка ввода в матрицу A.");
                        return;
                    }
                }
            }

            // Получение значений из текстовых полей матрицы B
            double[] matrixB = new double[numberOfUnknowns];
            for (int i = 0; i < numberOfUnknowns; i++)
            {
                string textBoxName = "B_" + i;
                if (!double.TryParse(this.Controls[textBoxName].Text, out matrixB[i]))
                {
                    MessageBox.Show("Ошибка ввода в матрицу B.");
                    return;
                }
            }

            // Выполнение метода Гаусса
            double[] result = SolveGauss(matrixA, matrixB);

            // Вывод результатов в текстовые поля ответа
            for (int i = 0; i < numberOfUnknowns; i++)
            {
                string textBoxName = "answer_" + i;
                this.Controls[textBoxName].Text = result[i].ToString("F3", CultureInfo.InvariantCulture);
            }
        }

        // Метод Гаусса для решения системы линейных уравнений
        private double[] SolveGauss(double[,] matrixA, double[] matrixB)
        {
            int n = matrixA.GetLength(0);

            // Прямой ход
            for (int k = 0; k < n - 1; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    double factor = matrixA[i, k] / matrixA[k, k];
                    matrixB[i] -= factor * matrixB[k];

                    for (int j = k; j < n; j++)
                    {
                        matrixA[i, j] -= factor * matrixA[k, j];
                    }
                }
            }

            // Обратный ход
            double[] result = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                result[i] = matrixB[i] / matrixA[i, i];
                for (int j = i - 1; j >= 0; j--)
                {
                    matrixB[j] -= matrixA[j, i] * result[i];
                }
            }

            return result;
        }
    }
}

