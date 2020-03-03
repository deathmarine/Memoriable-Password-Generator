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

namespace DictionaryBasedPasswords {
    public partial class Form1 : Form {
        Random gen = new Random();
        string[] dictionary = { };
        string[] common = { };
        string[] common_alt = { };
        string[] swear_words = { };
        public Form1() {
            InitializeComponent();
            dictionary = Properties.Resources.words_alpha.Split('\n');
            common = Properties.Resources.common_words.Split('\n');
            common_alt = Properties.Resources.common_words_alt.Split('\n');
            swear_words = Properties.Resources.swear_words.Split('\n');
            minWordSizeCB.SelectedItem = minWordSizeCB.Items[2];
            maxWordSizeCB.SelectedItem = maxWordSizeCB.Items[5];
            comboBox2.SelectedItem = comboBox2.Items[3];
            comboBox1.SelectedItem = comboBox1.Items[3];
        }

        private void button1_Click(object sender, EventArgs e) {
            string password = "";
            int word_count = int.Parse((string) comboBox2.SelectedItem);
            int min_word_size = int.Parse((string) minWordSizeCB.SelectedItem);
            int max_word_size = int.Parse((string) maxWordSizeCB.SelectedItem);
            string[] book = dictionary;
            if (comboBox1.SelectedItem.ToString().Equals("Common"))
                book = common;
            if (comboBox1.SelectedItem.ToString().Equals("Common Alt"))
                book = common_alt;
            if (comboBox1.SelectedItem.ToString().Equals("Common Mix"))
                book = common.Concat(common_alt).ToArray();
            if (comboBox1.SelectedItem.ToString().Equals("Add Cursewords"))
                book = common.Concat(common_alt).ToArray().Concat(swear_words).ToArray();
            if (comboBox1.SelectedItem.ToString().Equals("Only Cursewords"))
                book = swear_words;
            double brutesum = 0;
            int brutechar = 26;
            for (int i=0; i<word_count; i++) {
                string word = book[gen.Next(book.Length - 1)];
                if(!checkBox1.Checked)
                    while(word.Length < min_word_size || word.Length > max_word_size) {
                        word = book[gen.Next(book.Length - 1)];
                    }
                if (checkBox2.Checked) {
                        brutechar *= 2;
                        word = word.Substring(0, 1).ToUpper() + word.Substring(1);
                }
                if (checkBox3.Checked) {
                    brutechar *= 2;
                    string build = "";
                    for(int ii = 0; ii < word.Length; ii++) {
                        if (gen.Next(2) == 0) {
                            build += char.ToUpper(word.ToCharArray()[ii]);
                        } else {
                            build += char.ToLower(word.ToCharArray()[ii]);
                        }
                    }
                    word = build;
                }


                brutesum += Math.Pow(brutechar, (double) word.Length);
                password += word;
                if (i != word_count - 1) {
                    password += textBox2.Text;
                }
            }
            textBox1.Text = password;
            Clipboard.SetText(password);
            toolStripStatusLabel1.Text = $"{ThousandPassToYears(Math.Pow(brutesum, word_count))}/~Years Bruteforce";


        }

        private void textBox2_Enter(object sender, EventArgs e) {
            textBox2.Text = "";

        }

        private void minWordSizeCB_SelectedIndexChanged(object sender, EventArgs e) {
            if (maxWordSizeCB.SelectedIndex >= 0 && minWordSizeCB.SelectedIndex > maxWordSizeCB.SelectedIndex) {
                minWordSizeCB.SelectedItem = minWordSizeCB.Items[maxWordSizeCB.SelectedIndex];
            }
        }

        double ThousandPassToYears(double d) {
            return d / 1000 / 60 / 60 / 24 / 365.4;
        }

        private void label5_Click(object sender, EventArgs e) {
            toolStripStatusLabel1.Text = "Curse Words add to list!";
            comboBox1.Items.Add("Add Cursewords");
            comboBox1.Items.Add("Only Cursewords");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
        }
    }
}
