using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Pr7_Part2_PAT
{
    public partial class MainWindow : Window
    {
        // Полные алфавиты (без пробелов)
        private readonly List<char> LowEng = Enumerable.Range('a', 26).Select(c => (char)c).ToList();
        private readonly List<char> UppEng = Enumerable.Range('A', 26).Select(c => (char)c).ToList();
        private readonly List<char> LowRu = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToList();
        private readonly List<char> UppRu = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ".ToList();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Шифрование/дешифрование текста шифром Цезаря
        /// </summary>
        /// <param name="txt">Исходный текст</param>
        /// <param name="shift">Величина сдвига (может быть положительной, отрицательной или нулём)</param>
        /// <returns>Преобразованный текст (небуквенные символы не изменяются)</returns>
        private string Coder(string txt, int shift)
        {
            if (string.IsNullOrEmpty(txt)) return txt;

            char[] result = new char[txt.Length];

            for (int i = 0; i < txt.Length; i++)
            {
                char c = txt[i];
                char newChar = c;

                if (LowEng.Contains(c))
                {
                    int idx = (LowEng.IndexOf(c) + shift) % LowEng.Count;
                    if (idx < 0) idx += LowEng.Count;
                    newChar = LowEng[idx];
                }
                else if (UppEng.Contains(c))
                {
                    int idx = (UppEng.IndexOf(c) + shift) % UppEng.Count;
                    if (idx < 0) idx += UppEng.Count;
                    newChar = UppEng[idx];
                }
                else if (LowRu.Contains(c))
                {
                    int idx = (LowRu.IndexOf(c) + shift) % LowRu.Count;
                    if (idx < 0) idx += LowRu.Count;
                    newChar = LowRu[idx];
                }
                else if (UppRu.Contains(c))
                {
                    int idx = (UppRu.IndexOf(c) + shift) % UppRu.Count;
                    if (idx < 0) idx += UppRu.Count;
                    newChar = UppRu[idx];
                }
                // Пробелы, цифры, знаки препинания — остаются без изменений

                result[i] = newChar;
            }

            return new string(result);
        }

        /// <summary>
        /// Проверяет, что в поле введено корректное целое число (положительное, отрицательное или ноль)
        /// </summary>
        private bool TryGetShift(out int shift)
        {
            shift = 0;
            if (string.IsNullOrWhiteSpace(PaternText.Text))
            {
                MessageBox.Show("Поле сдвига не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(PaternText.Text, out shift))
            {
                MessageBox.Show("Сдвиг должен быть целым числом (например, 3, -5, 0)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        // Зашифровать
        private void CodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetShift(out int shift)) return;

            string input = InputText.Text;
            string encoded = Coder(input, shift);
            OutoutText.Text = encoded;
        }

        // Дешифровать
        private void DeCodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetShift(out int shift)) return;

            string input = InputText.Text;
            // Для дешифровки используем сдвиг с противоположным знаком
            string decoded = Coder(input, -shift);
            OutoutText.Text = decoded;
        }

        // Очистить все поля
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            InputText.Text = "";
            OutoutText.Text = "";
            PaternText.Text = "";
        }

        // Выход из приложения с подтверждением
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите выйти?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}