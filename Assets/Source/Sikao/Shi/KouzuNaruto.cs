namespace Sikao.Shi
{
    internal class KouzuNaruto : QiaoJiXie
    {
        internal const string MING_QIAN = "河津鳴人";
        internal KouzuNaruto() : base(MING_QIAN)
        {
            nao = new()
            {
                { XingGe.XUAN_SHANG, 50 },
                { XingGe.YI_PAI, 80 },
                { XingGe.SHUN_ZI, 10 },
                { XingGe.KE_ZI, 100 },
                { XingGe.LI_ZHI, 50 },
                { XingGe.MING, 80 },
                { XingGe.RAN, 10 },
                { XingGe.TAO, 40 },
            };
        }
    }
}
