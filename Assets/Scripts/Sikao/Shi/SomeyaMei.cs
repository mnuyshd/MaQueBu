namespace Assets.Scripts.Sikao.Shi
{
    internal class SomeyaMei : QiaoJiXie
    {
        internal const string MING_QIAN = "染谷鳴";
        internal SomeyaMei() : base(MING_QIAN)
        {
            nao = new()
            {
                { XingGe.XUAN_SHANG, 10 },
                { XingGe.YI_PAI, 50 },
                { XingGe.SHUN_ZI, 30 },
                { XingGe.KE_ZI, 30 },
                { XingGe.LI_ZHI, 50 },
                { XingGe.MING, 90 },
                { XingGe.RAN, 100 },
                { XingGe.TAO, 30 },
            };
        }
    }
}
