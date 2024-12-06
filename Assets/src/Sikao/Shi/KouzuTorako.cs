namespace Sikao.Shi
{
    internal class KouzuTorako : QiaoJiXie
    {
        internal const string MING_QIAN = "河津虎子";
        internal KouzuTorako() : base(MING_QIAN)
        {
            nao = new()
            {
                { XingGe.XUAN_SHANG, 100 },
                { XingGe.YI_PAI, 80 },
                { XingGe.SHUN_ZI, 10 },
                { XingGe.KE_ZI, 100 },
                { XingGe.LI_ZHI, 70 },
                { XingGe.MING, 0 },
                { XingGe.RAN, 10 },
                { XingGe.GUO_SHI_WU_SHUANG, 30 },
                { XingGe.TAO, 40 },
            };
        }
    }
}
