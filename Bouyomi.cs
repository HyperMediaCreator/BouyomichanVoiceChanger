using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace bvc
{
    public static class Bouyomi
    {
        public static string sHost { get; set; }//棒読みちゃんが動いているホスト
        public static int iPort { get; set; }//棒読みちゃんのTCPサーバのポート番号(デフォルト値)

        static Bouyomi(){
            Bouyomi.sHost = "127.0.0.1";
            Bouyomi.iPort = 50001;
        }

        static void play(string message) {
            Bouyomi.play(message, 0, -1, -1, -1);
        }

        public static void play(string message, Int16 iVoice, Int16 iVolume, Int16 iSpeed, Int16 iTone)
        {
            byte bCode = 0;
            Int16 iCommand = 0x0001;
            byte[] bMessage = Encoding.UTF8.GetBytes(message);
            Int32 iLength = bMessage.Length;

            //棒読みちゃんのTCPサーバへ接続
            TcpClient tc = null;

            try
            {
                tc = new TcpClient(sHost, iPort);
            }
            catch (Exception)
            {
                Console.WriteLine("接続失敗");
            }

            if (tc != null)
            {
                //メッセージ送信
                using (NetworkStream ns = tc.GetStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ns))
                    {
                        bw.Write(iCommand); //コマンド（ 0:メッセージ読み上げ）
                        bw.Write(iSpeed);   //速度    （-1:棒読みちゃん画面上の設定）
                        bw.Write(iTone);    //音程    （-1:棒読みちゃん画面上の設定）
                        bw.Write(iVolume);  //音量    （-1:棒読みちゃん画面上の設定）
                        bw.Write(iVoice);   //声質    （ 0:棒読みちゃん画面上の設定、1:女性1、2:女性2、3:男性1、4:男性2、5:中性、6:ロボット、7:機械1、8:機械2、10001～:SAPI5）
                        bw.Write(bCode);    //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
                        bw.Write(iLength);  //文字列のbyte配列の長さ
                        bw.Write(bMessage); //文字列のbyte配列
                    }
                }
                tc.Close();
            }
        }

    }


}
