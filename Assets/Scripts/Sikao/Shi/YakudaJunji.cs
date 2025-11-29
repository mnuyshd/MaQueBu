namespace Assets.Scripts.Sikao.Shi
{
    public class YakudaJunji : QiaoJiXie
    {
        public const string MING_QIAN = "役田順字";
        public YakudaJunji() : base(MING_QIAN)
        {
            naos[(int)XingGe.XUAN_SHANG].score = 50;
            naos[(int)XingGe.YI_PAI].score = 90;
            naos[(int)XingGe.SHUN_ZI].score = 90;
            naos[(int)XingGe.KE_ZI].score = 30;
            naos[(int)XingGe.LI_ZHI].score = 40;
            naos[(int)XingGe.MING].score = 50;
            naos[(int)XingGe.RAN].score = 60;
            naos[(int)XingGe.TAO].score = 50;
        }
    }
}
