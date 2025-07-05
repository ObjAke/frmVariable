using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace frmVariable
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] strOriginal = lbl_Input.Text.Replace("\r\n", "\r").Split('\r');
            //string pattern = $@"{txtBef.Text}([^>]+){txtAft.Text}"; // `txtBef.Text` と `txtAft.Text` に囲まれた文字列を取得

            string pattern = $@"{txtBef.Text}(.*?){txtAft.Text}";


            List<string> strFilters = new List<string>();   

            Dictionary<string, object> dicVariable = new Dictionary<string, object>();

            for (int i = 0; i < strOriginal.Length; i++)
            {
                //間の挟まれた数字を取得して格納
                MatchCollection matches = Regex.Matches(strOriginal[i], pattern);
                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Groups[1].Value); // 囲まれた中身を取得
                    strFilters.Add(match.Groups[1].Value);
                }
            }

            List<string> strStarts = new List<string>();
            //スタートに一致するかを確認して抽出
            if (txtStart.Text.Contains($"{{{txtVariable.Text}}}"))
            {
                for (int j = 0; j < strFilters.Count(); j++)
                {
                    strStarts.Add(txtStart.Text.Replace($"{{{txtVariable.Text}}}", strFilters[j]));
                }
            }
            else
            {
                strStarts.Add(txtStart.Text);
            }

            List<string> strEnds = new List<string>();
            //エンドに一致するかを確認して抽出
            if (txtEnd.Text.Contains($"{{txtVariable}}"))
            {
                for (int j = 0; j < strFilters.Count(); j++)
                {
                    strEnds.Add(txtEnd.Text.Replace("{txtVariable}", strFilters[j]));
                }
            }
            else
            {
                strEnds.Add(txtEnd.Text);
            }

            string strResult = string.Empty;
            //strStartsとstrEndsが存在していたら抽出する
            bool flgInput = false;
            for (int i = 0; i < strOriginal.Length; i++)
            {
                for (int j = 0; j < strStarts.Count; j++)
                {
                    if (strOriginal[i].Contains(strStarts[j])) flgInput = true;
                }
                for (int j = 0; j < strEnds.Count; j++)
                {
                    if (strOriginal[i].Contains(strEnds[j]) && flgInput) flgInput = false;
                }
                if (flgInput)
                {
                    strResult += strOriginal[i] + "\r\n";
                }
                
            }
            lblResult.Text = strResult; 

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtBef.Text = @"ｲﾁ\[";
            txtAft.Text = @": ";
            txtVariable.Text = "iNo";
            txtStart.Text = "ｲﾁ[{iNo}]{";
            txtEnd.Text = "};";

            lblAlert.Text =
                "変数前文字と変数後文字に入れるときには\r\n" +
                "必ず以下の半角文字を入れる際には右側の\r\n" +
                "記載方法をして下さい。\r\n" +
                @"　.　　　　　　　　→　　　\." + "\r\n" +
                @"　^　　　　　　　　→　　　\^" + "\r\n" +
                @"　$　　　　　　　　→　　　\$|行末の位置" + "\r\n" +
                @"　|　　　　　　　　→　　　\|" + "\r\n" +
                @"　\　　　　　　　　→　　　\\" + "\r\n" +
                @"　[、または、]　　 →　　　\[, \]" + "\r\n" +
                @"　(、または、)　　 →　　　\(, \)" + "\r\n" +
                @"　{、または、}　　 →　　　\{, \}" + "\r\n" +
                @"　+　　　　　　　　→　　　\+" + "\r\n" +
                @"　*　　　　　　　　→　　　\*" + "\r\n" +
                @"　?　　　　　　　　→　　　\?" + "\r\n";

            lbl_Input.Text = 
                "  18:ｶｸｼﾞｸ ｲﾁ[1:   300.360] 100% ﾅﾒﾗｶ50    ;\r\n" +
                "  19:ﾁｮｸｾﾝ ｲﾁ[2:  1500.850] 2000mm/sec ﾅﾒﾗｶ50    ;\r\n" +
                "  20:ﾁｮｸｾﾝ ｲﾁ[3:  2000.916] 2000mm/sec ﾅﾒﾗｶ50    ;\r\n" +
                "ｲﾁ[1]{\r\n" +
                "   GP1:\r\n" +
                "\tUF : 0, UT : 1,\t\tｹｲﾀｲ : 'N U T, 0, 0, 0',\r\n" +
                "\tX =   432.123  mm,\tY =   345.678  mm,\tZ =  678.543  mm,\r\n" +
                "\tW =     1.234 deg,\tP =     1.234 deg,\tR =    23.456 deg\r\n" +
                "   GP2:\r\n\tUF : 0, UT : 1,\t\r\n\tJ1=    23.456  mm\r\n};\r\n" +
                "ｲﾁ[2]{\r\n" +
                "   GP1:\r\n" +
                "\tUF : 0, UT : 1,\t\tｹｲﾀｲ : 'N U T, 0, 0, 0',\r\n" +
                "\tX =   789.012  mm,\tY =  345.678  mm,\tZ =  901.234  mm,\r\n" +
                "\tW =    56.789 deg,\tP =   -12.345 deg,\tR =    678.901 deg\r\n" +
                "   GP2:\r\n\tUF : 0, UT : 1,\t\r\n" +
                "\tJ1=    23.456  mm\r\n" +
                "};\r\n" +
                "ｲﾁ[3]{\r\n" +
                "   GP1:\r\n" +
                "\tUF : 0, UT : 1,\t\tｹｲﾀｲ : 'N U T, 0, 0, 0',\r\n" +
                "\tX =  1234.567  mm,\tY =  1345.267  mm,\tZ =  1456.789  mm,\r\n" +
                "\tW =    12.345 deg,\tP =   -67.890 deg,\tR =    12.345 deg\r\n" +
                "   GP2:\r\n\tUF : 0, UT : 1,\t\r\n" +
                "\tJ1=    45.678  mm\r\n" +
                "};\r\n" +
                "ｲﾁ[4]{\r\n" +
                "   GP1:\r\n" +
                "\tUF : 0, UT : 1,\t\tｹｲﾀｲ : 'N U T, 0, 0, 0',\r\n" +
                "\tX =   567.890  mm,\tY =  2345.678  mm,\tZ =  1567.890  mm,\r\n" +
                "\tW =   -34.567 deg,\tP =   -89.012 deg,\tR =   34.567 deg\r\n" +
                "   GP2:\r\n" +
                "\tUF : 0, UT : 1,\t\r\n" +
                "\tJ1=    56.789  mm\r\n" +
                "};";
        }
    }
}
