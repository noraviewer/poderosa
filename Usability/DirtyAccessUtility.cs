#if TERMCONTROL
using Poderosa.Usability;
using Poderosa.View;

namespace Poderosa.Forms
{
    /// <summary>
    /// TerminalUIPluginクラス、EditRenderProfileクラスの機能を外部から直接利用するためのクラスです。
    /// </summary>
    public class DirtyAccessUtility
    {
        /// <summary>
        /// デフォルト値を持つ表示プロファイルを取得します。
        /// </summary>
        /// <returns></returns>
        public static RenderProfile GetDefaultRenderProfile()
        {
            return TerminalUIPlugin.Instance.TerminalEmulatorPlugin.TerminalEmulatorOptions
                .CreateRenderProfile();
        }

        /// <summary>
        /// 表示プロファイルの編集画面を呼び出します。
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static RenderProfile CallEditRenderProfile(RenderProfile profile)
        {
            EditRenderProfile profileDialog = new EditRenderProfile(profile);

            if (profileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //
                // [OK]ボタンが押された場合は編集された表示プロファイルを返します。
                //
                return profileDialog.Result;
            }
            else
            {
                //
                // キャンセルされた場合はnullを返します。
                //
                return null;
            }
        }
    }
}
#endif