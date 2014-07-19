using System;
using System.Drawing;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Poderosa.ConnectionParam;
using Poderosa.View;

namespace Poderosa.TerminalControl
{
    /// <summary>
    /// ログインプロファイルを格納するクラスです。
    /// </summary>
    public class LoginProfile
    {
        // --------------------------------------------------------------------
        // ログインパラメータ
        // --------------------------------------------------------------------

        /// <summary>
        /// 接続方式です。
        /// </summary>
        [XmlAttribute]
        public ConnectionMethod ConnectionMethod { get; set; }
        
        /// <summary>
        /// 接続先ホストです。
        /// </summary>
        [XmlAttribute]
        public string Host { get; set; }
        
        /// <summary>
        /// ポート番号です。
        /// </summary>
        [XmlAttribute]
        public int Port { get; set; }
        
        /// <summary>
        /// 端末タイプです。
        /// </summary>
        [XmlAttribute]
        public TerminalType TerminalType { get; set; }
        
        /// <summary>
        /// ユーザ名です。
        /// </summary>
        [XmlAttribute]
        public string UserName { get; set; }
        
        /// <summary>
        /// パスワードです。
        /// </summary>
        [XmlIgnore]
        public SecureString Password { get; set; }
        
        /// <summary>
        /// 鍵ファイルです。
        /// </summary>
        [XmlAttribute]
        public string IdentityFile { get; set; }

        /// <summary>
        /// エンコーディングです。
        /// </summary>
        [XmlAttribute]
        public EncodingType EncodingType { get; set; }

        /// <summary>
        /// ローカルエコーを行うかどうかです。
        /// </summary>
        [XmlAttribute]
        public bool LocalEcho { get; set; }

        /// <summary>
        /// 送信時の改行の種類です。
        /// </summary>
        [XmlAttribute]
        public NewLine TransmitNL { get; set; }

        // --------------------------------------------------------------------
        // 表示プロファイル
        // --------------------------------------------------------------------

        /// <summary>
        /// フォント名です。
        /// </summary>
        [XmlAttribute]
        public string RpFontName { get; set; }
        
        /// <summary>
        /// CJKフォント名です。
        /// </summary>
        [XmlAttribute]
        public string RpCjkFontName { get; set; }
        
        /// <summary>
        /// フォントサイズです。
        /// </summary>
        [XmlAttribute]
        public float RpFontSize { get; set; }
        
        /// <summary>
        /// 可能であれば常にClearTypeフォントを使うかどうかです。
        /// </summary>
        [XmlAttribute]
        public bool RpUseClearType { get; set; }
        
        /// <summary>
        /// ボールドフォントを有効にするかどうかです。
        /// </summary>
        [XmlAttribute]
        public bool RpEnableBoldStyle { get; set; }
        
        /// <summary>
        /// ボールドフォントを強制するかどうかです。
        /// </summary>
        [XmlAttribute]
        public bool RpForceBoldStyle { get; set; }
        
        /// <summary>
        /// テキスト色です。
        /// </summary>
        [XmlIgnore]
        public Color RpForeColor { get; set; }

        /// <summary>
        /// テキスト色です。
        /// </summary>
        [XmlAttribute("RpForeColor")]
        public string RpForeColorText
        {
            get { return ColorTranslator.ToHtml(RpForeColor); }
            set { RpForeColor = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// 背景色です。
        /// </summary>
        [XmlIgnore]
        public Color RpBackColor { get; set; }

        /// <summary>
        /// 背景色です。
        /// </summary>
        [XmlAttribute("RpBackColor")]
        public string RpBackColorText
        {
            get { return ColorTranslator.ToHtml(RpBackColor); }
            set { RpBackColor = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// 背景画像ファイルです。
        /// </summary>
        [XmlAttribute]
        public string RpBackgroundImageFileName { get; set; }
        
        /// <summary>
        /// 背景画像の配置です。
        /// </summary>
        [XmlAttribute]
        public ImageStyle RpImageStyle { get; set; }

        // --------------------------------------------------------------------
        // 色指定エスケープシーケンス
        // --------------------------------------------------------------------

        /// <summary>
        /// 色指定エスケープシーケンス0です。
        /// </summary>
        [XmlIgnore]
        public Color EsColor0 { get; set; }

        /// <summary>
        /// 色指定エスケープシーケンス0です。
        /// </summary>
        [XmlAttribute("EsColor0")]
        public string EsColor0Text
        {
            get { return ColorTranslator.ToHtml(EsColor0); }
            set { EsColor0 = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// 色指定エスケープシーケンス1です。
        /// </summary>
        [XmlIgnore]
        public Color EsColor1 { get; set; }

        /// <summary>
        /// 色指定エスケープシーケンス1です。
        /// </summary>
        [XmlAttribute("EsColor1")]
        public string EsColor1Text
        {
            get { return ColorTranslator.ToHtml(EsColor1); }
            set { EsColor1 = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// 色指定エスケープシーケンス2です。
        /// </summary>
        [XmlIgnore]
        public Color EsColor2 { get; set; }

        /// <summary>
        /// 色指定エスケープシーケンス2です。
        /// </summary>
        [XmlAttribute("EsColor2")]
        public string EsColor2Text
        {
            get { return ColorTranslator.ToHtml(EsColor2); }
            set { EsColor2 = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// 色指定エスケープシーケンス3です。
        /// </summary>
        [XmlIgnore]
        public Color EsColor3 { get; set; }

        /// <summary>
        /// 色指定エスケープシーケンス3です。
        /// </summary>
        [XmlAttribute("EsColor3")]
        public string EsColor3Text
        {
            get { return ColorTranslator.ToHtml(EsColor3); }
            set { EsColor3 = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// 色指定エスケープシーケンス4です。
        /// </summary>
        [XmlIgnore]
        public Color EsColor4 { get; set; }

        /// <summary>
        /// 色指定エスケープシーケンス4です。
        /// </summary>
        [XmlAttribute("EsColor4")]
        public string EsColor4Text
        {
            get { return ColorTranslator.ToHtml(EsColor4); }
            set { EsColor4 = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// 色指定エスケープシーケンス5です。
        /// </summary>
        [XmlIgnore]
        public Color EsColor5 { get; set; }

        /// <summary>
        /// 色指定エスケープシーケンス5です。
        /// </summary>
        [XmlAttribute("EsColor5")]
        public string EsColor5Text
        {
            get { return ColorTranslator.ToHtml(EsColor5); }
            set { EsColor5 = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// 色指定エスケープシーケンス6です。
        /// </summary>
        [XmlIgnore]
        public Color EsColor6 { get; set; }

        /// <summary>
        /// 色指定エスケープシーケンス6です。
        /// </summary>
        [XmlAttribute("EsColor6")]
        public string EsColor6Text
        {
            get { return ColorTranslator.ToHtml(EsColor6); }
            set { EsColor6 = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// 色指定エスケープシーケンス7です。
        /// </summary>
        [XmlIgnore]
        public Color EsColor7 { get; set; }

        /// <summary>
        /// 色指定エスケープシーケンス7です。
        /// </summary>
        [XmlAttribute("EsColor7")]
        public string EsColor7Text
        {
            get { return ColorTranslator.ToHtml(EsColor7); }
            set { EsColor7 = ColorTranslator.FromHtml(value); }
        }

        // --------------------------------------------------------------------
        // その他
        // --------------------------------------------------------------------

        /// <summary>
        /// 接続先と端末設定をロックするかどうかを示します。
        /// </summary>
        [XmlIgnore]
        public bool LockDestinationAndTerminal { get; set; }

        /// <summary>
        /// 終了時に保存するかどうかを示します。
        /// </summary>
        [XmlIgnore]
        public bool SaveOnExit { get; set; }

        // --------------------------------------------------------------------
        // シリアライズ＆デシリアライズ
        // --------------------------------------------------------------------

        /// <summary>
        /// ログインプロファイルをXMLデシリアライズします。
        /// </summary>
        /// <remarks>
        /// デシリアライズできない場合はデフォルト値を持つログインプロファイルを返します。
        /// </remarks>
        /// <param name="terminalControl"></param>
        /// <param name="profileDir"></param>
        /// <returns></returns>
        public static LoginProfile Deserialize(string profileName, string profileDir)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LoginProfile));
                string profilePath = Path.Combine(profileDir, profileName + ".xml");
                if (File.Exists(profilePath))
                {
                    using (XmlTextReader reader = new XmlTextReader(profilePath))
                    {
                        return (LoginProfile)serializer.Deserialize(reader);
                    }
                }
                else
                {
                    return new Poderosa.TerminalControl.LoginProfile();
                }
            }
            catch
            {
                return new Poderosa.TerminalControl.LoginProfile();
            }
        }

        /// <summary>
        /// ログインプロファイルをXMLシリアライズします。
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="profileName"></param>
        /// <param name="profileDir"></param>
        public static void Serialize(LoginProfile profile, string profileName, string profileDir)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LoginProfile));
            string profilePath = Path.Combine(profileDir, profileName + ".xml");
            using (XmlTextWriter writer = new XmlTextWriter(profilePath, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, profile);
            }
        }

        // --------------------------------------------------------------------
        // コンストラクタとメソッド
        // --------------------------------------------------------------------

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public LoginProfile()
        {
            //
            // デフォルト値を持つログインプロファイルを作成します。
            //
            ConnectionMethod = ConnectionMethod.SSH2;
            Host = String.Empty;
            Port = 22;
            TerminalType = TerminalType.XTerm;
            UserName = String.Empty;
            Password = new SecureString();
            IdentityFile = String.Empty;
            EncodingType = ConnectionParam.EncodingType.UTF8;
            LocalEcho = false;
            TransmitNL = NewLine.CR;
            ImportRenderProfile(PoderosaAccessPoint.GetDefaultRenderProfile());
            LockDestinationAndTerminal = false;
            SaveOnExit = true;
        }

        /// <summary>
        /// 表示プロファイルを取り込みます。
        /// </summary>
        /// <param name="profile"></param>
        public void ImportRenderProfile(Poderosa.View.RenderProfile profile)
        {
            RpFontName = profile.FontName;
            RpCjkFontName = profile.CJKFontName;
            RpFontSize = profile.FontSize;
            RpUseClearType = profile.UseClearType;
            RpEnableBoldStyle = profile.EnableBoldStyle;
            RpForceBoldStyle = profile.ForceBoldStyle;
            RpForeColor = profile.ForeColor;
            RpBackColor = profile.BackColor;
            RpBackgroundImageFileName = profile.BackgroundImageFileName;
            RpImageStyle = profile.ImageStyle;

            ImportEscapeSequenceColorSet(profile.ESColorSet);
        }

        /// <summary>
        /// 色指定エスケープシーケンスセットを取り込みます。
        /// </summary>
        /// <param name="colorSet"></param>
        public void ImportEscapeSequenceColorSet(Poderosa.View.EscapesequenceColorSet colorSet)
        {
            EsColor0 = colorSet[0].Color;
            EsColor1 = colorSet[1].Color;
            EsColor2 = colorSet[2].Color;
            EsColor3 = colorSet[3].Color;
            EsColor4 = colorSet[4].Color;
            EsColor5 = colorSet[5].Color;
            EsColor6 = colorSet[6].Color;
            EsColor7 = colorSet[7].Color;
        }

        /// <summary>
        /// 表示プロファイルを取り出します。
        /// </summary>
        /// <returns></returns>
        public Poderosa.View.RenderProfile ExportRenderProfile()
        {
            Poderosa.View.RenderProfile profile = PoderosaAccessPoint.GetDefaultRenderProfile();
            profile.FontName = RpFontName;
            profile.CJKFontName = RpCjkFontName;
            profile.FontSize = RpFontSize;
            profile.UseClearType = RpUseClearType;
            profile.EnableBoldStyle = RpEnableBoldStyle;
            profile.ForceBoldStyle = RpForceBoldStyle;
            profile.ForeColor = RpForeColor;
            profile.BackColor = RpBackColor;
            profile.BackgroundImageFileName = RpBackgroundImageFileName;
            profile.ImageStyle = RpImageStyle;
            profile.ESColorSet = ExportEscapeSequenceColorSet();
            return profile;
        }

        /// <summary>
        /// 色指定エスケープシーケンスセットを取り出します。
        /// </summary>
        /// <returns></returns>
        public Poderosa.View.EscapesequenceColorSet ExportEscapeSequenceColorSet()
        {
            Poderosa.View.EscapesequenceColorSet colorSet = new View.EscapesequenceColorSet();
            colorSet.ResetToDefault();
            colorSet[0] = new View.ESColor(EsColor0, false);
            colorSet[1] = new View.ESColor(EsColor1, false);
            colorSet[2] = new View.ESColor(EsColor2, false);
            colorSet[3] = new View.ESColor(EsColor3, false);
            colorSet[4] = new View.ESColor(EsColor4, false);
            colorSet[5] = new View.ESColor(EsColor5, false);
            colorSet[6] = new View.ESColor(EsColor6, false);
            colorSet[7] = new View.ESColor(EsColor7, false);
            return colorSet;
        }

        /// <summary>
        /// 端末タイトルを取得します。
        /// </summary>
        /// <returns></returns>
        public string GetTerminalTitle()
        {
            string method = ConnectionMethod.ToString();
            if (ConnectionMethod != ConnectionParam.ConnectionMethod.Telnet)
            {
                method += (string.IsNullOrEmpty(IdentityFile)) ? "-Pass" : "-Key"; 
            }
            return (String.IsNullOrEmpty(Host)) ? 
                "(接続先が未設定)" : string.Format("{0} ({1})", Host, method);
        }
    }
}
