namespace Assets.Source.Sikao.Shi
{
    internal class UchidaKou : QiaoJiXie
    {
        internal const string MING_QIAN = "打田攻";
        internal UchidaKou() : base(MING_QIAN)
        {
            nao = new()
            {
                { XingGe.XUAN_SHANG, 40 },
                { XingGe.YI_PAI, 60 },
                { XingGe.SHUN_ZI, 20 },
                { XingGe.KE_ZI, 20 },
                { XingGe.LI_ZHI, 80 },
                { XingGe.MING, 30 },
                { XingGe.RAN, 30 },
                { XingGe.TAO, 0 },
            };
        }
    }
}
