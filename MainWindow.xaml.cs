using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        // define strings of buttons
        private string[,] table = new string[,] {{ "%", "", "", "" },
                                                 {"/", "^", "(", ")"},
                                                 {"7", "8", "9", "*"},
                                                 { "4", "5", "6", "-"},
                                                 { "1", "2", "3", "+"},
                                                 { "", "0", ".", ""}};
        // operation that we will use
        private string operation = "";

        public MainWindow() {
            InitializeComponent();
        }

        private void Equal_Click(object sender, RoutedEventArgs e) {
            try {
                for (int i = 0; i < operation.Length; i++) {
                    if (operation[i] == '^')
                        checkPow();
                }
                System.Data.DataTable table = new System.Data.DataTable();
                operation = Convert.ToString(Convert.ToDouble(table.Compute(operation, String.Empty)));
                Replace();
            }catch{
                operation = "Error";
            }
            entry.Text = operation;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            // define button, col and row
            var button = (Button)sender;
            var col = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            // add to operation
            operation += table[row - 1, col];

            // update entry
            Replace();
            entry.Text = operation;
        }

        private void Trans_Click(object sender, RoutedEventArgs e) {
            // if there is not a signed icon change it to "-"
            if (operation[0] != '+' && operation[0] != '-') {
                operation = "-" + operation;

                // if singed icon is "+" change it to "-"
            } else if (operation[0] == '+') {
                // change the operation operation first item would be deleted
                Change(1, 0);
                operation = "-" + operation; // change first to "-"

                // if singed icon is "-" change it to "+" 
            } else if (operation[0] == '-') {
                // change the operation operation first item would be deleted
                Change(1, 0);
                operation = "+" + operation; // change first to "+"
            }

            entry.Text = operation;
        }

        private void Back_Click(object sender, RoutedEventArgs e) {
            // delete last item of the operation
            Change(0, 1);
            entry.Text = operation;
        }

        private void CE_Click(object sender, RoutedEventArgs e) {
            // delete the last operation

            // delete numbers
            char[] arr = new char[11] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' };
            // while last char is a number delete las item
            while (In(LasOf(operation), arr) == true) {
                Change(0, 1);
            }

            // delte operation
            arr = new char[7] { '+', '-', '*', '%', '/', '^', '(' };
            // while last char is a number delete las item
            while (In(LasOf(operation), arr) == true) {
                Change(0, 1);
            }

            entry.Text = operation;
        }

        private void Delete_Click(object sender, RoutedEventArgs e) {
            // delete entys text and operation
            operation = "";
            entry.Text = operation;
        }

        private void Replace() {
            // replace some strings
            operation = operation.Replace(".", ",");
            operation = operation.Replace("/", "÷");
        }

        private void Change(int num, int minus) {
            string newOperation = "";
            for (int index = num; index < operation.Length - minus; index++) {
                newOperation += operation[index];
            }
            operation = newOperation;
        }

        private bool In(char c, char[] arr) {
            // check if c is a element of arr
            foreach (char item in arr) {
                if (c == item) {
                    return true;
                }
            }
            return false;
        }
        private char LasOf(string str) {
            char item = 'a';
            foreach (char i in str.ToCharArray()) {
                item = i;
            }
            return item;
        }

        private double LearnNum(int index) {
            string op = "";
            string num = "";
            for (int i = 0; i < index; i++) {
                op += operation[i];
            }

            char[] arr = new char[11] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' };
            while (In(LasOf(op), arr) == true) {
                num = LasOf(op) + num;
                string newOperation = "";
                for (int i = 0; i < operation.Length - 1; i++) {
                    newOperation += operation[i];
                }
                op = newOperation;
            }

            return Convert.ToDouble(num);

        }

        private double LearnPow(int index) {
            string op = "";
            string num = "";
            for (int i = index+1; i < operation.Length; i++) {
                op += operation[i];
            }
            int turn = 0;
            char[] arr = new char[11] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' };
            try {
                while (In(op[turn], arr) == true) {
                    num = num + op[turn];
                    turn += 1;
                }
            } catch {

            }

            return Convert.ToDouble(num);

        }

        private void checkPow() {
            double number = 0;
            double power = 0;
            // learn number and power
            for (int i = 0; i < operation.Length; i++) {
                try {
                    if (operation[i] == '^') {
                        number = LearnNum(i);
                        power = LearnPow(i);
                    }
                } catch{
                    operation = "Error";
                }
            }
            string op = "";
            op = Math.Pow(number, power).ToString();
            operation = op.Replace("^", op);
            Replace();
        }
    }
}
