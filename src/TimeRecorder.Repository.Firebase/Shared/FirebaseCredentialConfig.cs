using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.Firebase
{
    /// <summary>
    /// Firebaseプロジェクトのアクセス情報を表します
    /// </summary>
    internal class FirebaseCredentialConfig
    {
        /// <summary>
        /// FirebaseのAPIキーを取得・設定します
        /// </summary>
        /// <remarks>
        /// FirebaseConsole＞プロジェクトの設定＞ウェブアプリにFirebaseを追加＞SDKの設定と構成
        /// のApiKeyを参照
        /// </remarks>
        public string ApiKey { get; set; }

        /// <summary>
        /// Firebaseのプロジェクト名を取得・設定します
        /// </summary>
        /// <remarks>
        /// FirebaseConsole＞プロジェクトの設定＞全般＞プロジェクトIDを参照
        /// </remarks>
        public string ProjectId { get; set; }


    }
}
