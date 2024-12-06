namespace Sikao.Shi
{
    internal class MenzenJunko : QiaoJiXie
    {
        internal const string MING_QIAN = "面前順子";
        internal MenzenJunko() : base(MING_QIAN)
        {
            LiPaiDongZuo = false;

            nao = new()
            {
                { XingGe.XUAN_SHANG, 50 },
                { XingGe.YI_PAI, 50 },
                { XingGe.SHUN_ZI, 70 },
                { XingGe.KE_ZI, 30 },
                { XingGe.LI_ZHI, 10 },
                { XingGe.MING, 10 },
                { XingGe.RAN, 20 },
                { XingGe.GUO_SHI_WU_SHUANG, 50 },
                { XingGe.TAO, 80 },
            };
        }
    }
}
