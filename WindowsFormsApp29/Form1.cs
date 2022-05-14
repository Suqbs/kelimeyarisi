using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp29
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Kısıtlı süre içerisinde yazabildiğin kadar kelime yaz!\n\nKurallar:\n1. Süre bitmeden en az bir kelime yazmalısın.\n2. Aynı kelimeyi yazmak oyunun bitmesi demektir.\n\nİyi şanslar!");
            button2.Enabled = false;
        }
        
        //Input olarak sadece harf ve control (backspace, ctrl vs.) değerlerini alır. 
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        //Global değerler
        int puan = 0;
        int index;
        int listboxDegeri1;
        int listboxDegeri2;
        int zaman = 60;

        private void button1_Click_1(object sender, EventArgs e)
        {
            zaman = 60;
            textBox1.Clear();
            timer1.Start();
            listboxDegeri1 = listBox1.Items.Count;
            button1.Enabled = false;
            button2.Enabled = true;

            //Random atanan bir index değeriyle ımageListten bir string değeri alıp pictureBox1'e yazdırdı.
            Random random = new Random();
            index = random.Next(ımageList1.Images.Count);
            pictureBox1.Image = ımageList1.Images[index];
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            zaman--;

            if (zaman == 0)
            {
                timer1.Stop();
                listboxDegeri2 = listBox1.Items.Count;
                button1.Enabled = true;
                button2.Enabled = false;

                //Harf seç butonuyla aldığımız listboxDegeri1 ile zaman == 0 olduğunda aldığımız listboxDegeri2 'i karşılaştırıyoruz. Eğer eşitlerse herhangi bir kelime eklenmediği için puan = 0 oluyor.
                if (zaman == 0 && listboxDegeri1 == listboxDegeri2) 
                {
                    puan = 0;
                    MessageBox.Show("Kelime girmediğiniz için kaybettiniz.");
                    textBox1.Text = string.Empty;
                    listBox1.Items.Clear();
                }
            }
            Lbltime.Text = Convert.ToString(zaman);
            Lblpuan.Text = Convert.ToString(puan);
        }

        private void Kelime_Datasini_Al(string harftxt)
        {
            string kelime = textBox1.Text.ToLower(); //ToLower metodu: büyük-küçük harf duyarlılığını ortadan kaldırır.
            //string imageName = ımageList1.Images.Keys[index];
            //int pictureBoxIndex = ımageList1.Images.IndexOfKey(imageName);

            //Dosya yolunu kendine göre girmelisin.
            using (StreamReader sr = new StreamReader(@"C:\Users\windows1\source\repos\Suqbs\kelimeyarisi\WindowsFormsApp29\Resources\" + harftxt)) // StreamReader fonksiyonu ile okunacak dosyamızı açtırıyoruz. Kelime başına ortalama 4000 satırdır.
            {
                string satir; // Burada okuduğunuz her satırı atamamız için gerekli değişkeni tanımlıyoruz.

                while ((satir = sr.ReadLine()) != null) // Döngü kurup; eğer satır boş değilse, satir değişkenine ekleme yapıyoruz.
                {
                    if (satir == kelime && !listBox1.Items.Contains(textBox1.Text.ToLower()))
                    {
                        textBox1.Text = "";
                        listBox1.Items.Add(satir);
                        puan += 5;
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string imageName = ımageList1.Images.Keys[index];
            Kelime_Datasini_Al(imageName + ".txt");

            //Eğer listboxa aynı değeri girdiysek bunları gerçekleştir. Bunu eventin sonunda yazmamın sebebi, eğer kelime tekrar yazılırken kelimenin baş harfi resimdeki harfle aynıysa while döngüsüne bir daha girip listbox'a onu giriyor. Bunu sonda yazmak o gelen değeri de siliyor. 
            if (listBox1.Items.Contains(textBox1.Text.ToLower()))
            {
                puan = 0;
                zaman = 60;
                button1.Enabled = true;
                timer1.Stop();
                textBox1.Text = "";
                MessageBox.Show("Aynı kelimeyi girdiğin için kaybettin. Tekrar başlamak için harf seç.");
                textBox1.Text = string.Empty;
                listBox1.Items.Clear();
            }
            Lblpuan.Text = Convert.ToString(puan);
            Lbltime.Text = Convert.ToString(zaman);
        }
    }
}
