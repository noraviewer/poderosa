using System.Drawing;
using System.Windows.Forms;
using Poderosa.View;

namespace Poderosa.TerminalControl
{
    /// <summary>
    /// 表示プロファイルの背景画像表示に対応したラベルコントロールです。
    /// </summary>
    /// <remarks>
    /// DrawBackgroundImageメソッドはPoderosa.View.CharacterDocumentViewerクラスの同名メソッドを
    /// ベースに作成しています。
    /// </remarks>
    [System.ComponentModel.DesignerCategory("Code")]
    public class ImageStyleLabel : Label
    {
        // --------------------------------------------------------------------
        // フィールド定義
        // --------------------------------------------------------------------

        private Image _image;
        private ImageStyle _imageStyle;

        // --------------------------------------------------------------------
        // プロパティ定義
        // --------------------------------------------------------------------

        /// <summary>
        /// 背景画像です。
        /// </summary>
        /// <remarks>
        /// newキーワードの指定により、Label.Imageを隠しています。
        /// これにより、背景色による塗りつぶしをLabel.OnPaintBackgroundメソッドに任せています。
        /// </remarks>
        public new Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 背景画像のレイアウトです。
        /// </summary>
        public ImageStyle ImageStyle
        {
            get { return _imageStyle; }
            set
            {
                _imageStyle = value;
                Invalidate();
            }
        }

        // --------------------------------------------------------------------
        // 背景画像の描画
        // --------------------------------------------------------------------

        /// <summary>
        /// 背景を描画します。
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            if (Image != null)
            {
                DrawBackgroundImage(pevent.Graphics, Image, ImageStyle, pevent.ClipRectangle);
            }
        }

        /// <summary>
        /// 背景画像を描画します。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="img"></param>
        /// <param name="style"></param>
        /// <param name="clip"></param>
        private void DrawBackgroundImage(Graphics g, Image img, ImageStyle style, Rectangle clip)
        {
            if (style == ImageStyle.HorizontalFit)
            {
                this.DrawBackgroundImageScaled(g, img, clip, true, false);
            }
            else if (style == ImageStyle.VerticalFit)
            {
                this.DrawBackgroundImageScaled(g, img, clip, false, true);
            }
            else if (style == ImageStyle.Scaled)
            {
                this.DrawBackgroundImageScaled(g, img, clip, true, true);
            }
            else
            {
                DrawBackgroundImageNormal(g, img, style, clip);
            }
        }

        /// <summary>
        /// 背景画像をスケーリングして描画します。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        /// <param name="clip"></param>
        /// <param name="fitWidth"></param>
        /// <param name="fitHeight"></param>
        private void DrawBackgroundImageScaled(Graphics g, Image image, Rectangle clip,
            bool fitWidth, bool fitHeight)
        {
            PointF drawPoint;
            SizeF drawSize;

            if (fitWidth && fitHeight)
            {
                drawSize = new SizeF(ClientSize.Width, ClientSize.Height);
                drawPoint = new PointF(0, 0);
            }
            else if (fitWidth)
            {
                float drawWidth = ClientSize.Width;
                float drawHeight = drawWidth * image.Height / image.Width;
                drawSize = new SizeF(drawWidth, drawHeight);
                drawPoint = new PointF(0, (ClientSize.Height - drawSize.Height) / 2f);
            }
            else
            {
                float drawHeight = ClientSize.Height;
                float drawWidth = drawHeight * image.Width / image.Height;
                drawSize = new SizeF(drawWidth, drawHeight);
                drawPoint = new PointF((ClientSize.Width - drawSize.Width) / 2f, 0);
            }

            Region oldClip = g.Clip;
            using (Region newClip = new Region(clip))
            {
                g.Clip = newClip;
                g.DrawImage(image, new RectangleF(drawPoint, drawSize),
                    new RectangleF(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                g.Clip = oldClip;
            }
        }

        /// <summary>
        /// 背景画像を指定の位置に描画します。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        /// <param name="style"></param>
        /// <param name="clip"></param>
        private void DrawBackgroundImageNormal(Graphics g, Image image, ImageStyle style,
            Rectangle clip)
        {
            int offsetX, offsetY;
            if (style == ImageStyle.Center)
            {
                offsetX = (this.Width - image.Width) / 2;
                offsetY = (this.Height - image.Height) / 2;
            }
            else
            {
                offsetX = (style == ImageStyle.TopLeft || style == ImageStyle.BottomLeft)
                    ? 0 : (this.ClientSize.Width - image.Width);
                offsetY = (style == ImageStyle.TopLeft || style == ImageStyle.TopRight)
                    ? 0 : (this.ClientSize.Height - image.Height);
            }

            Rectangle target = Rectangle.Intersect(
                new Rectangle(clip.Left - offsetX, clip.Top - offsetY, clip.Width, clip.Height),
                new Rectangle(0, 0, image.Width, image.Height));
            if (target != Rectangle.Empty)
            {
                g.DrawImage(image, new Rectangle(
                    target.Left + offsetX, target.Top + offsetY, target.Width, target.Height),
                    target, GraphicsUnit.Pixel);
            }
        }
    }
}
