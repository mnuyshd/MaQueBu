namespace Assets.Scripts.Sikao.Shi
{
    public class UchidaKou : QiaoJiXie
    {
        public const string MING_QIAN = "打田攻";
        public UchidaKou() : base(MING_QIAN)
        {
            naos[(int)XingGe.XUAN_SHANG].score = 40;
            naos[(int)XingGe.YI_PAI].score = 60;
            naos[(int)XingGe.SHUN_ZI].score = 20;
            naos[(int)XingGe.KE_ZI].score = 20;
            naos[(int)XingGe.LI_ZHI].score = 80;
            naos[(int)XingGe.MING].score = 30;
            naos[(int)XingGe.RAN].score = 30;
            naos[(int)XingGe.TAO].score = 0;
        }
    }
}
