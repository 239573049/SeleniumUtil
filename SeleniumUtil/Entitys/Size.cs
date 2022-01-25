namespace SeleniumUtil.Entitys
{
    public class Size
    {
        /// <summary>
        /// 宽度
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Size(double width,double height)
        {
            Width=width;
            Height=height;
        }
    }
}
