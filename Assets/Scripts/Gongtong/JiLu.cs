namespace Assets.Scripts.Gongtong
{
    // 記録
    public class JiLu
    {
        // 脳 懸賞
        public int naoXuanShang = -1;
        // 脳 役牌
        public int naoYiPai = -1;
        // 脳 順子
        public int naoShunZi = -1;
        // 脳 刻子
        public int naoKeZi = -1;
        // 脳 立直
        public int naoLiZhi = -1;
        // 脳 鳴き
        public int naoMing = -1;
        // 脳 染め
        public int naoRan = -1;
        // 脳 逃げ
        public int naoTao = -1;

        // 半荘数
        public int banZhuangShu = 0;
        // 対局数
        public int duiJuShu = 0;

        // 集計点
        public int jiJiDian = 0;
        // 順位１
        public int shunWei1 = 0;
        // 順位２
        public int shunWei2 = 0;
        // 順位３
        public int shunWei3 = 0;
        // 順位４
        public int shunWei4 = 0;

        // 和了数
        public int heLeShu = 0;
        // 親和了数
        public int qinHeLeShu = 0;
        // 放銃数
        public int fangChongShu = 0;

        // 流局数
        public int liuJuShu = 0;
        // 聴牌数
        public int tingPaiShu = 0;
        // 不聴数
        public int buTingShu = 0;

        // 和了点
        public int heLeDian = 0;
        // 放銃点
        public int fangChongDian = 0;

        // 役満数
        public int[] yiManShu = new int[Sikao.QueShi.YiManMing.Count];
        // 役数
        public int[] yiShu = new int[Sikao.QueShi.YiMing.Count];
    }
}
