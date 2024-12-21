namespace Sikao.Shi
{
    internal class HikitaMamoru : QiaoXiaoLu
    {
        internal const string MING_QIAN = "引田守";
        internal HikitaMamoru() : base(MING_QIAN)
        {
            nao = new()
            {
                { XingGe.XUAN_SHANG, 10 },
                { XingGe.YI_PAI, 60 },
                { XingGe.LI_ZHI, 0 },
                { XingGe.MING, 0 },
                { XingGe.TAO, 100 },
            };
        }
    }
}
