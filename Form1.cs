using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO; //csv

namespace bvc
{
    public partial class Form1 : Form
    {

        public class voiceset
        {
            public Int16 iVoice, iVolume, iSpeed, iTone;
            public voiceset() {
                this.iVoice = 0; this.iVolume = -1; this.iSpeed = -1; this.iTone = -1;
            }
        }

        Dictionary<string, voiceset> usertable = new Dictionary<string, voiceset>();

        public Form1()
        {
            InitializeComponent();
            addDgvFromCsv();
            //var random = new RandomBoxMuller();
            //MessageBox.Show(random.next().ToString());
            /*
            if (!Utl.isProcess("BouyomiChan", "棒読みちゃん"))
            {
                System.Diagnostics.Process.Start(@"C:\Program Files (x86)\!tmp\BouyomiChan\BouyomiChan.exe");
            }
            */
        }

        public void playComment(string id, string comment, bool isAnonymity, int premium, bool isBsp)
        {
            label_id.Text = "id " + id;
            label_184.Text = "184 " + isAnonymity.ToString();
            label_premium.Text = "プレミアム " + premium.ToString();
            label_bsp.Text = "BSP " + isBsp.ToString();

            //DGVに声の登録データがある場合、その設定に基づきBouyomi.play
            foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
            {
                if (id == (string)dgvRow.Cells[0].Value)
                {

                    Int16 iVoice;
                    Int16.TryParse((string)dgvRow.Cells[2].Value, out iVoice);//声質

                    Int16 iVolume = Int16.TryParse((string)dgvRow.Cells[3].Value, out iVolume) ? iVolume : (Int16)(-1); //音量  -1はデフォルトの値                  
                    Int16 iSpeed = Int16.TryParse((string)dgvRow.Cells[4].Value, out iSpeed) ? iSpeed : (Int16)(-1);  //速度
                    Int16 iTone = Int16.TryParse((string)dgvRow.Cells[5].Value, out iTone) ? iTone : (Int16)(-1); //音程

                    Bouyomi.play(comment, iVoice, iVolume, iSpeed, iTone);
                    return;
                }
            }

            if(comment == "こえがわり")
            {
                usertable.Remove(id);
            }

            voiceset voice;
            if (usertable.TryGetValue(id, out voice))
            {
                Bouyomi.play(comment, voice.iVoice, voice.iVolume, voice.iSpeed, voice.iTone);
                return;
            }
            else
            {
                voice = new voiceset();
                voice.iVoice = (Int16)Utl.retRandom(1, 9);
                voice.iSpeed = (Int16)Utl.retRandom(60, 200); //old 30 - 200
                voice.iTone = (Int16)Utl.retRandom(70, 150);  //old 50 - 150
                
                switch (voice.iVoice)
                {
                    case 1:
                        voice.iVolume = 17;
                        break;

                    case 2:
                        voice.iVolume = 20;
                        break;

                    case 3:
                        voice.iVolume = 20;
                        break;

                    case 4:
                        voice.iVolume = 25;
                        break;

                    case 5:
                        voice.iVolume = 20;
                        break;

                    case 6:
                        voice.iVolume = 20;
                        break;

                    case 7:
                        voice.iVolume = 10;
                        break;

                    case 8:
                        voice.iVolume = 17;
                        break;

                    default:
                        voice.iVolume = 20;
                        break;
                }

                usertable.Add(id, voice);

                Bouyomi.play(comment, voice.iVoice, voice.iVolume, voice.iSpeed, voice.iTone);
                return;
            }

            //次に184やプレミアムなどの属性で調べる
            string att = "";
            if (isAnonymity) { att += "184"; } else { att += "un184"; }
            if (premium == 0) { att += "ippan"; } else if (premium == 1) { att += "premium"; } else if (premium == 3) { att = "nushi"; } else if (premium == 2) { att = "infomation"; }

            foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
            {
                if (att == (string)dgvRow.Cells[0].Value)
                {
                    Int16 iVoice;
                    Int16.TryParse((string)dgvRow.Cells[2].Value, out iVoice);//声質

                    Int16 iVolume = Int16.TryParse((string)dgvRow.Cells[3].Value, out iVolume) ? iVolume : (Int16)(-1); //音量  -1はデフォルトの値                  
                    Int16 iSpeed = Int16.TryParse((string)dgvRow.Cells[4].Value, out iSpeed) ? iSpeed : (Int16)(-1);  //速度
                    Int16 iTone = Int16.TryParse((string)dgvRow.Cells[5].Value, out iTone) ? iTone : (Int16)(-1); //音程

                    Bouyomi.play(comment, iVoice, iVolume, iSpeed, iTone);
                    return;
                }
            }


            Bouyomi.play(comment, 0, -1, -1, -1); //DGVに声の設定されていない場合

        }


        /// <summary>
        /// CSVを読み込んでDatagridViewに追加
        /// </summary>
        void addDgvFromCsv()
        {

            string path = Application.ExecutablePath;
            path = Path.GetDirectoryName(path) + @"\plugins\setting.csv";

            TextFieldParser parser = new TextFieldParser(path, System.Text.Encoding.GetEncoding("Shift_JIS"));
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(","); // 区切り文字はコンマ

            while (!parser.EndOfData)
            {
                string[] row = parser.ReadFields(); // 1行読み込み
                dataGridView1.Rows.Add(row);
            }

        }

        private void test_Click(object sender, EventArgs e)
        {
            Int16 iVoice;
            Int16.TryParse(textBox2.Text, out iVoice);//声質

            Int16 iVolume = Int16.TryParse(textBox3.Text, out iVolume) ? iVolume : (Int16)(-1); //音量  -1はデフォルトの値                  
            Int16 iSpeed = Int16.TryParse(textBox4.Text, out iSpeed) ? iSpeed : (Int16)(-1);  //速度
            Int16 iTone = Int16.TryParse(textBox5.Text, out iTone) ? iTone : (Int16)(-1); //音程


            Bouyomi.play(textBox1.Text, iVoice, iVolume, iSpeed, iTone);

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) { return; } //セルでない場合(-1)

            textBox2.Text = (string)dataGridView1[2, e.RowIndex].Value;
            textBox3.Text = (string)dataGridView1[3, e.RowIndex].Value;
            textBox4.Text = (string)dataGridView1[4, e.RowIndex].Value;
            textBox5.Text = (string)dataGridView1[5, e.RowIndex].Value;

        }

        private void addRow_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add("", "", "0", "-1", "-1", "-1");
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.ExecutablePath;
                path = Path.GetDirectoryName(path) + @"\plugins\setting.csv";

                // StreamWriter の設定
                File.Delete(path);

                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                StreamWriter writer = new StreamWriter(path, true, sjisEnc);

                foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
                {
                    string[] result = {
                (string)dgvRow.Cells[0].Value,
                (string)dgvRow.Cells[1].Value,
                (string)dgvRow.Cells[2].Value,
                (string)dgvRow.Cells[3].Value,
                (string)dgvRow.Cells[4].Value,
                (string)dgvRow.Cells[5].Value            
                                  };
                    writer.WriteLine(string.Join(",", result));
                }
                writer.Close();
            }
            catch
            {
                MessageBox.Show("保存失敗");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = openFileDialog1.FileName;
            }
        }

        public Settings get()
        {

            Settings appSettings = new Settings();

            appSettings.bouyomiPath = textBox6.Text;

            return appSettings;
        }

        public void set(Settings appSettings)
        {
            textBox6.Text = appSettings.bouyomiPath;
        }

    } //public partial class Form1 : Form
}//namespace bvc
