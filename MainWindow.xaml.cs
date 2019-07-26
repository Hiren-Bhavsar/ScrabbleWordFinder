using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace ScrabbleWordFinder {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly int[] pointVals = { 1, 3, 3, 2, 1, 4, 2, 4, 1, 8, 5, 1, 3, 1, 1, 3, 10, 1, 1, 1, 1, 4, 4, 8, 4, 10 };
        private readonly string fileName = "ScrabbleDictionary.txt";
        private Dictionary<String, int> wordOptions = new Dictionary<string, int>();
        private IOrderedEnumerable<System.Collections.Generic.KeyValuePair<string, int>> ordered;

        public MainWindow() {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e) {
            wordOptions.Clear();
            WordBox.Clear();
            bool isDescending = SortChooser.SelectedIndex == 0 ? true : false;
            string lettersToSearch = LetterBox.Text;

            wordSearcher(lettersToSearch, isDescending);
        }

        private void wordSearcher(string lettersToSearch, bool isDescending) {
            lettersToSearch = lettersToSearch.Replace(" ", string.Empty);

            foreach (string line in File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, fileName))) {
                if (line.Length <= lettersToSearch.Length) {
                    if (String_Contains_String(line, lettersToSearch)) {
                        addStringToDictionary(line, isDescending);
                        
                    }
                }
            }
            foreach (KeyValuePair<string, int> kv in ordered) {
                WordBox.AppendText(kv.Key + " "+ kv.Value + "\n");
                //Console.WriteLine(kv.Key + " " + kv.Value);
            }

        }

        private bool String_Contains_String(string compareTo, string compareSet) {
            compareTo = compareTo.ToUpper();
            ArrayList letterSet = new ArrayList(compareSet.ToUpper().ToArray());

            foreach (char c in compareTo) {
                if (letterSet.Contains(c)) {
                    letterSet.Remove(c);
                } else if (letterSet.Contains('?')) {
                    letterSet.Remove('?');
                } else {
                    return false;
                }
            }
            return true;
        }

        private void addStringToDictionary(string toAdd, bool isDescending) {
            wordOptions.Add(toAdd, calculatePointValue(toAdd));
            if (isDescending) {
                ordered = wordOptions.OrderByDescending(x => x.Value);
            } else { 
                ordered = wordOptions.OrderBy(x => x.Value);
            }
        }

        private int calculatePointValue(string toCal) {
            int toReturn = 0;
            foreach (char c in toCal.ToCharArray()) {
                toReturn += pointVals[((int)c) - 65];
            }
            return toReturn;
        }


    }
}
