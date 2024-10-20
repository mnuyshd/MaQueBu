namespace Sikao.Shi
{
    internal class YakudaJunji : QiaoJiXie
    {
        internal const string MING_QIAN = "役田順字";
        internal YakudaJunji() : base(MING_QIAN)
        {
            nao = new()
            {
                { XingGe.XUAN_SHANG, 50 },
                { XingGe.YI_PAI, 90 },
                { XingGe.SHUN_ZI, 90 },
                { XingGe.KE_ZI, 30 },
                { XingGe.LI_ZHI, 40 },
                { XingGe.MING, 50 },
                { XingGe.RAN, 60 },
                { XingGe.GUO_SHI_WU_SHUANG, 60 },
                { XingGe.TAO, 50 },
            };
        }
    }
}
