namespace SeleniumUtil.Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class Size
    {
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Size(int width, int height)
        {
            Width=width;
            Height=height;
        }
    }
}
