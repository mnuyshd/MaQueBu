namespace Assets.Scripts.Sikao.Shi
{
    public class SomeyaMei : QiaoJiXie
    {
        public const string MING_QIAN = "染谷鳴";
        public SomeyaMei() : base(MING_QIAN)
        {
            naos[(int)XingGe.XUAN_SHANG].score = 10;
            naos[(int)XingGe.YI_PAI].score = 50;
            naos[(int)XingGe.SHUN_ZI].score = 30;
            naos[(int)XingGe.KE_ZI].score = 30;
            naos[(int)XingGe.LI_ZHI].score = 50;
            naos[(int)XingGe.MING].score = 90;
            naos[(int)XingGe.RAN].score = 100;
            naos[(int)XingGe.TAO].score = 30;
        }
    }
}
