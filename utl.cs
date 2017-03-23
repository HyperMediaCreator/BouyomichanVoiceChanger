using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography; //RNGCryptoServiceProvider

namespace bvc
{
    public static class Utl
    {


        /// <summary>
        /// プロセスが起動しているか
        /// </summary>
        /// <param name="processName">プロセス名(タスクマネージャの)</param>
        /// <param name="windowTitle">ウィンドウタイトルにwindowTitleの文字列が含まれている</param>
        /// <returns>paramが２つとも合致していればtrue</returns>
        public static bool isProcess(string processName, string windowTitle)
        {

            //ローカルコンピュータ上で実行されている processName という名前のすべてのプロセスを取得
            System.Diagnostics.Process[] ps =
                System.Diagnostics.Process.GetProcessesByName(processName); //"BouyomiChan"

            foreach (System.Diagnostics.Process p in ps)
            {
                //IDとメインウィンドウのキャプションを出力する
                //Console.WriteLine("{0}/{1}", p.Id, p.MainWindowTitle);
                if (p.MainWindowTitle.Contains(windowTitle))//"棒読みちゃん"
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Settingsクラスをテキストファイルに保存する
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="fileName">保存するファイルのパス+ファイル名</param>
        public static void save(Settings appSettings, string fileName)
        {

            //＜XMLファイルに書き込む＞
            //XmlSerializerオブジェクトを作成
            //書き込むオブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer1 =
                new System.Xml.Serialization.XmlSerializer(typeof(Settings));
            //ファイルを開く
            System.IO.FileStream fs1 =
                new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            //シリアル化し、XMLファイルに保存する
            serializer1.Serialize(fs1, appSettings);
            //閉じる
            fs1.Close();
        }

        /// <summary>
        /// Settingsクラスを開く
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Settings load(string fileName)
        {

            //＜XMLファイルから読み込む＞
            //XmlSerializerオブジェクトの作成
            System.Xml.Serialization.XmlSerializer serializer2 =
                new System.Xml.Serialization.XmlSerializer(typeof(Settings));
            //ファイルを開く
            System.IO.FileStream fs2 =
                new System.IO.FileStream(fileName, System.IO.FileMode.Open);
            //XMLファイルから読み込み、逆シリアル化する
            Settings appSettings =
                (Settings)serializer2.Deserialize(fs2);
            //閉じる
            fs2.Close();

            return appSettings;
        }
        
        /// <summary>
        /// Settingsクラスをテキストファイルに保存する binで
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="fileName">保存するファイルのパス+ファイル名</param>
        public static void saveBinary(Settings appSettings, string fileName) {

            //＜バイナリファイルに書き込む＞
            //BinaryFormatterオブジェクトを作成
            BinaryFormatter bf1 = new BinaryFormatter();
            //ファイルを開く
            System.IO.FileStream fs1 =
                new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            //シリアル化し、バイナリファイルに保存する
            bf1.Serialize(fs1, appSettings);
            //閉じる
            fs1.Close();
        }

        /// <summary>
        /// Settingsクラスを開くBinで
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Settings loadBinary(string fileName) {
            //＜バイナリファイルから読み込む＞
            //BinaryFormatterオブジェクトの作成
            BinaryFormatter bf2 = new BinaryFormatter();
            //ファイルを開く
            System.IO.FileStream fs2 =
                new System.IO.FileStream(fileName, System.IO.FileMode.Open);
            //バイナリファイルから読み込み、逆シリアル化する
            Settings appSettings =
                (Settings)bf2.Deserialize(fs2);
            //閉じる
            fs2.Close();

            return appSettings;
        }




        /// <summary>
        /// /指定した範囲内の乱数を返します。
        /// </summary>
        /// <param name="minValue">返される乱数の包括的下限値。</param>
        /// <param name="maxValue"返される乱数の排他的上限値。 maxValue は minValue 以上である必要があります。 ></param>
        /// <returns></returns>
        public static int retRandom(int minValue, int maxValue)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                // 厳密にランダムなInt32を作る
                var buffer = new byte[sizeof(int)];
                rng.GetBytes(buffer);
                var seed = BitConverter.ToInt32(buffer, 0);
                // そのseedを基にRandomを作る
                var rand = new Random(seed);
                return rand.Next(minValue, maxValue);
            }

        }
    }

    [Serializable()]
    public class Settings
    {
        public string bouyomiPath;
    }


}
