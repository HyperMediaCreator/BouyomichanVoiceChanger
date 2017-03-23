using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO; //csv

namespace bvc
{
    /// <summary>
    /// NCVのプラグインのインターフェースを実装してるクラス
    /// </summary>
    public class Class1 : IPlugin
    {
        private IPluginHost _host = null;
        private Form1 _form = null;

        /// <summary>
        /// IsAutoRunがtrueの場合、アプリケーション起動時に自動実行される
        /// </summary>
        public void AutoRun()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// プラグインの説明
        /// </summary>
        public string Description
        {
            get { return "コメントする人（ユーザーID）ごとに棒読みちゃんの声を設定します"; }
        }

        /// <summary>
        /// プラグインのホスト
        /// </summary>
        public IPluginHost Host
        {
            get
            {
                return this._host;
            }
            set
            {
                this._host = value;
            }
        }

        /// <summary>
        /// アプリケーション起動時にプラグインを自動実行するかどうか
        /// </summary>
        public bool IsAutoRun
        {
            get { return true; }
        }

        /// <summary>
        /// プラグインの名前
        /// </summary>
        public string Name
        {
            get { return "棒読みボイスチェンジャー"; }
        }

        /// <summary>
        /// プラグインのバージョン
        /// </summary>
        public string Version
        {
            get { return "1.0"; }
        }

        /// <summary>
        /// プラグインを実行する
        /// </summary>
        public void Run()
        {
            
            if(_form == null){
            //フォームの生成、表示

                _form = new Form1();

                //コメント受信時のイベントハンドラ追加
                _host.ReceivedComment += new ReceivedCommentEventHandler(_host_ReceivedComment);
                //addDgvFromCsv();

                //フォームが閉じられた際のイベントハンドラを登録
                _form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(_form_FormClosed);

                /*
                //放送接続イベントハンドラ追加
                _host.BroadcastConnected += new BroadcastConnectedEventHandler(_host_BroadcastConnected);

                //放送切断イベントハンドラ追加
                _host.BroadcastDisConnected += new BroadcastDisConnectedEventHandler(_host_BroadcastDisConnected);

                //コメント受信時のイベントハンドラ追加
                _host.ReceivedComment += new ReceivedCommentEventHandler(_host_ReceivedComment);

                */

                _form.Show((System.Windows.Forms.IWin32Window)_host.MainForm);

                string path = Application.ExecutablePath;
                path = Path.GetDirectoryName(path) + @"\plugins\setting.config";

                if (File.Exists(path)) {
                    _form.set(Utl.load(path));
                }

            }
        }

        /// <summary>
        //フォームが閉じられた際のイベントハンドラ
        void _form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {

            //フォームが閉じられた際のイベントハンドラ削除
            _form.FormClosed -= _form_FormClosed;

            //コメント受信時のイベントハンドラ削除
            _host.ReceivedComment -= _host_ReceivedComment;
            
            /*
            
             * //フォームが閉じられた際のイベントハンドラ削除
            _form.FormClosed -= _form_FormClosed;

            //放送開始直後イベントハンドラ削除
            _host.BroadcastConnected -= _host_BroadcastConnected;

            //放送終了直後のイベントハンドラ削除
            _host.BroadcastDisConnected -= _host_BroadcastDisConnected;

            */

            _form = null;
        }

        /// <summary>
        /// 放送接続時イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _host_BroadcastConnected(object sender, EventArgs e)
        {
            //MessageBox.Show("放送に接続しました", Name);//動作確認用MessageBox          
        }

        /// <summary>
        /// 放送切断時イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _host_BroadcastDisConnected(object sender, EventArgs e)
        {
            //MessageBox.Show("放送から切断しました", Name);//動作確認用MessageBox
        }

        /// <summary>
        /// コメント受信時イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _host_ReceivedComment(object sender, ReceivedCommentEventArgs e)
        {
            //受信したコメント数を取り出す
            int count = e.CommentDataList.Count;
            if (count == 0)
            {
                return;
            }
            
            //最新のコメントデータを取り出す
            NicoLibrary.NicoLiveData.LiveCommentData commentData = e.CommentDataList[count - 1];
            
            string comment = commentData.Comment;
            string userId = commentData.UserId;
            bool isAnonymity = commentData.IsAnonymity;
            int premium = commentData.Premium;
            bool isBsp = commentData.IsBSP;
            
            _form.playComment(userId, comment, isAnonymity, premium, isBsp);

            //コメントに"ぬるぽ"が含まれているか判定し、
            //含まれていたら、"ｶﾞｯ"とコメントする
            if (comment.Contains("ぬるぽ"))
            {
                    bool result = _host.SendComment(commentData.No + " >> " +  "ｶﾞｯ");
            }
            if (comment.Contains("いつするの") || comment.Contains("いつやるの") || comment.Contains("いつやるか") || comment.Contains("いつするか"))
            {
                bool result = _host.SendComment(commentData.No + " >> " + "今でしょ。");
            }
        }
    }//public class Class1 : IPlugin

    

}


