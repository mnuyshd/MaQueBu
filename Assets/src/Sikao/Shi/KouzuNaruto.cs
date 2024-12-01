namespace Sikao.Shi
{
    internal class KouzuNaruto : QiaoJiXie
    {
        internal static readonly string MING_QIAN = "河津鳴人";
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
                { XingGe.GUO_SHI_WU_SHUANG, 30 },
                { XingGe.TAO, 40 },
            };
        }
    }
}
