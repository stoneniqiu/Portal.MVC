using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Niqiu.Core.Helpers
{
    public class ImageManageHelper
    {
        /// <summary>   
        /// 添加图片水印   
        /// </summary>   
        /// <param name="sourcePicture">源图片文件名</param>   
        /// <param name="waterImage">水印图片文件名</param>   
        /// <param name="alpha">透明度(0.1-1.0数值越小透明度越高)</param>   
        /// <param name="position">位置</param>   
        /// <param name="picturePath" >图片的路径</param>   
        /// <returns>返回生成于指定文件夹下的水印文件名</returns>   
        public static string DrawImage(string sourcePicture, string waterImage, double alpha, ImagePosition position, string picturePath)
        {
            if (sourcePicture == string.Empty || waterImage == string.Empty || Math.Abs(alpha * 10) - 1 < 0 || picturePath == string.Empty)
            {
                return sourcePicture;
            }
            var imgPhoto = Image.FromFile(sourcePicture);
            //   
            // 确定其长宽   
            //   
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;
            // 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。   
            var bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
            // 设定分辨率   
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // 定义一个绘图画面用来装载位图   
            var grPhoto = Graphics.FromImage(bmPhoto);

            //同样，由于水印是图片，我们也需要定义一个Image来装载它   
            Image imgWatermark = new Bitmap(waterImage);
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。   
            // 成员名称   说明    
            // AntiAlias      指定消除锯齿的呈现。     
            // Default        指定不消除锯齿。     
            // HighQuality  指定高质量、低速度呈现。     
            // HighSpeed   指定高速度、低质量呈现。     
            // Invalid        指定一个无效模式。     
            // None          指定不消除锯齿。    
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            // 第一次描绘，将我们的底图描绘在绘图画面上   
            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, phWidth, phHeight), 0, 0, phWidth, phHeight, GraphicsUnit.Pixel);

            var bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //   
            // 继续，将水印图片装载到一个绘图画面grWatermark   
            //   
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //   
            //ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。   
            //          
            var imageAttributes = new ImageAttributes();

            //   
            //Colormap: 定义转换颜色的映射   
            //   
            var colorMap = new ColorMap
            {
                OldColor = Color.FromArgb(255, 0, 255, 0),
                NewColor = Color.FromArgb(0, 0, 0, 0)
            };

            //   
            //我的水印图被定义成拥有绿色背景色的图片被替换成透明   

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {    
           new[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f}, // red红色   
           new[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f}, //green绿色   
           new[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f}, //blue蓝色          
           new[] {0.0f,  0.0f,  0.0f,  (float)alpha, 0.0f}, //透明度        
           new[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};//   

            //  ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。   
            //  ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。   
            var wmColorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
             ColorAdjustType.Bitmap);
            //   
            //上面设置完颜色，下面开始设置位置   
            //   
            int xPosOfWm;
            int yPosOfWm;
            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.Center:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = (phHeight - wmHeight) / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = 10;
                    break;
                default:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
            }
            grWatermark.DrawImage(imgWatermark, new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), 0, 0, wmWidth, wmHeight, GraphicsUnit.Pixel, imageAttributes);
            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            // 保存文件到服务器的文件夹里面   
            imgPhoto.Save(picturePath, ImageFormat.Jpeg);
            imgPhoto.Dispose();
            imgWatermark.Dispose();
            return picturePath;
        }

        /// <summary>
        /// 存放缩略图
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="saveFileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void GetThumbnail(string sourceFileName, string saveFileName, int width, int height)
        {
            var iSource = Image.FromFile(sourceFileName);
            var samllimg = iSource.GetThumbnailImage(width, height, null, IntPtr.Zero);
            samllimg.Save(saveFileName);
        }

        /// <summary>
        /// 按比例 存放缩略图片
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="saveFileName"></param>
        /// <param name="width"></param>
        public static void GetThumbnail(string sourceFileName, string saveFileName, int width)
        {
            var iSource = Image.FromFile(sourceFileName);
            var height = width * iSource.Height / iSource.Width;
            var samllimg = iSource.GetThumbnailImage(width, height, null, IntPtr.Zero);
            samllimg.Save(saveFileName);
        }

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth"></param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            var iSource = Image.FromFile(sFile);
            var tFormat = iSource.RawFormat;
            int sW, sH;
            //按比例缩放
            var temSize = new Size(iSource.Width, iSource.Height);

            if (temSize.Width > dHeight || temSize.Width > dWidth) //将**改成c#中的或者操作符号
            {
                if ((temSize.Width * dHeight) > (temSize.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * temSize.Height) / temSize.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (temSize.Width * dHeight) / temSize.Height;
                }
            }

            else
            {
                sW = temSize.Width;
                sH = temSize.Height;
            }
            var ob = new Bitmap(dWidth, dHeight);
            var g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            var ep = new EncoderParameters();
            var qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            var eParam = new EncoderParameter(Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                var arrayICI = ImageCodecInfo.GetImageEncoders();
                var jpegICIinfo = arrayICI.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }

            catch
            {
                return false;
            }

            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }

        /// <summary>   
        /// 在图片上添加水印文字   
        /// </summary>   
        /// <param name="sourcePicture">源图片文件</param>   
        /// <param name="waterWords">需要添加到图片上的文字</param>   
        /// <param name="alpha">透明度</param>   
        /// <param name="position">位置</param>   
        /// <param name="picturePath">文件路径</param>   
        /// <returns></returns>   
        public static string DrawWords(string sourcePicture, string waterWords, double alpha, ImagePosition position, string picturePath)
        {
            // 判断参数是否有效   
            if (sourcePicture == string.Empty || waterWords == string.Empty || Math.Abs(alpha * 10) - 1 < 0 || picturePath == string.Empty)
            {
                return sourcePicture;
            }
            //创建一个图片对象用来装载要被添加水印的图片   
            Image imgPhoto = Image.FromFile(sourcePicture);

            //获取图片的宽和高   
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //   
            //建立一个bitmap，和我们需要加水印的图片一样大小   
            var bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            //SetResolution：设置此 Bitmap 的分辨率   
            //这里直接将我们需要添加水印的图片的分辨率赋给了bitmap   
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //Graphics：封装一个 GDI+ 绘图图面。   
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //设置图形的品质   
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //将我们要添加水印的图片按照原始大小描绘（复制）到图形中   
            grPhoto.DrawImage(
             imgPhoto,                                           //   要添加水印的图片   
             new Rectangle(0, 0, phWidth, phHeight), //  根据要添加的水印图片的宽和高   
             0,                                                     //  X方向从0点开始描绘   
             0,                                                     // Y方向    
             phWidth,                                            //  X方向描绘长度   
             phHeight,                                           //  Y方向描绘长度   
             GraphicsUnit.Pixel);                              // 描绘的单位，这里用的是像素   

            //根据图片的大小我们来确定添加上去的文字的大小   
            //在这里我们定义一个数组来确定   
            var sizes = new[] { 16, 14, 12, 10, 8, 6, 4 };

            //字体   
            Font crFont = null;
            //矩形的宽度和高度，SizeF有三个属性，分别为Height高，width宽，IsEmpty是否为空   
            var crSize = new SizeF();

            //利用一个循环语句来选择我们要添加文字的型号   
            //直到它的长度比图片的宽度小   
            for (int i = 0; i < 7; i++)
            {
                crFont = new Font("arial", sizes[i], FontStyle.Bold);

                //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。   
                crSize = grPhoto.MeasureString(waterWords, crFont);

                // ushort 关键字表示一种整数数据类型   
                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            //截边5%的距离，定义文字显示(由于不同的图片显示的高和宽不同，所以按百分比截取)   

            //定义在图片上文字的位置   
            float wmHeight = crSize.Height;
            float wmWidth = crSize.Width;

            float xPosOfWm;
            float yPosOfWm;

            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.Center:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = phHeight / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = wmWidth;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = wmWidth / 2;
                    yPosOfWm = wmHeight / 2;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = wmHeight;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = wmWidth;
                    break;
                default:
                    xPosOfWm = wmWidth;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
            }

            //封装文本布局信息（如对齐、文字方向和 Tab 停靠位），显示操作（如省略号插入和国家标准 (National) 数字替换）和 OpenType 功能。   
            var strFormat = new StringFormat {Alignment = StringAlignment.Center};

            //定义需要印的文字居中对齐   

            //SolidBrush:定义单色画笔。画笔用于填充图形形状，如矩形、椭圆、扇形、多边形和封闭路径。   
            //这个画笔为描绘阴影的画笔，呈灰色   
            var mAlpha = Convert.ToInt32(256 * alpha);
            var semiTransBrush2 = new SolidBrush(Color.FromArgb(mAlpha, 0, 0, 0));

            //描绘文字信息，这个图层向右和向下偏移一个像素，表示阴影效果   
            //DrawString 在指定矩形并且用指定的 Brush 和 Font 对象绘制指定的文本字符串。   
            grPhoto.DrawString(waterWords,                                    //string of text   
                                       crFont,                                         //font   
                                       semiTransBrush2,                            //Brush   
                                       new PointF(xPosOfWm + 1, yPosOfWm + 1),  //Position   
                                       strFormat);

            //从四个 ARGB 分量（alpha、红色、绿色和蓝色）值创建 Color 结构，这里设置透明度为153   
            //这个画笔为描绘正式文字的笔刷，呈白色   
            var semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            //第二次绘制这个图形，建立在第一次描绘的基础上   
            grPhoto.DrawString(waterWords,                 //string of text   
                                       crFont,                                   //font   
                                       semiTransBrush,                           //Brush   
                                       new PointF(xPosOfWm, yPosOfWm),  //Position   
                                       strFormat);

            //imgPhoto是我们建立的用来装载最终图形的Image对象   
            //bmPhoto是我们用来制作图形的容器，为Bitmap对象   
            imgPhoto = bmPhoto;
            //释放资源，将定义的Graphics实例grPhoto释放，grPhoto功德圆满   
            grPhoto.Dispose();

            //将grPhoto保存   
            imgPhoto.Save(picturePath, ImageFormat.Jpeg);
            imgPhoto.Dispose();

            return picturePath;
        }

        ///<summary>
        /// 生成验证码
        ///</summary>
        ///<param name="length">指定验证码的长度</param>
        ///<returns></returns>
        public static string CreateValidateCode(int length)
        {

            var randMembers = new int[length];
            var validateNums = new int[length];
            var validateNumberStr = "";

            //生成起始序列值

            var seekSeek = unchecked((int)DateTime.Now.Ticks);
            var seekRand = new Random(seekSeek);
            var beginSeek = seekRand.Next(0, Int32.MaxValue - length * 10000);
            var seeks = new int[length];
            for (var i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }

            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                var rand = new Random(seeks[i]);
                var pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }

            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                var numStr = randMembers[i].ToString(CultureInfo.InvariantCulture);
                var numLength = numStr.Length;
                var rand = new Random();
                var numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }

            //生成验证码
            for (var i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString(CultureInfo.InvariantCulture);
            }
            return validateNumberStr;
        }

        /// <summary>
        ///  创建验证码的图片
        /// </summary>
        /// <param name="validateCode"></param>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            var image = new Bitmap((int)Math.Ceiling(validateCode.Length * 14.0), 22);
            var g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                var random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (var i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                var font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
                var brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                                                                    Color.Blue, Color.DarkRed, 1.5f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }

            finally
            {
                g.Dispose();
                image.Dispose();
            }

        }
    }

    public enum ImagePosition
    {
        LeftTop,        //左上   
        LeftBottom,    //左下   
        RightTop,       //右上   
        RigthBottom,  //右下   
        TopMiddle,     //顶部居中   
        BottomMiddle, //底部居中   
        Center           //中心   
    }

}
