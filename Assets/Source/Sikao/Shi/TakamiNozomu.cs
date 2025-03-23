namespace Assets.Source.Sikao.Shi
{
    internal class TakamiNozomu : QiaoJiXie
    {
        internal const string MING_QIAN = "高見望";
        internal TakamiNozomu() : base(MING_QIAN)
        {
            nao = new()
            {
                { XingGe.XUAN_SHANG, 70 },
                { XingGe.YI_PAI, 70 },
                { XingGe.SHUN_ZI, 50 },
                { XingGe.KE_ZI, 50 },
                { XingGe.LI_ZHI, 90 },
                { XingGe.MING, 30 },
                { XingGe.RAN, 40 },
                { XingGe.TAO, 50 },
            };
        }
    }
}
