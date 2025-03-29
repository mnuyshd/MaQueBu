namespace Assets.Source.Gongtong
{
    // 規則
    public class GuiZe
    {
        // 半荘戦
        public bool banZhuang = true;
        // 喰断
        public bool shiDuan = true;
        // 自摸平和
        public bool ziMoPingHe = true;
        // 食い替え
        public bool shiTi = false;
        // 箱(ドボン)
        public bool xiang = true;
        // 赤牌数
        public int[] chiPaiShu = new int[] { 1, 1, 1 };
        // W栄和(0:無し(頭ハネ) 1:有り)
        public int wRongHe = 1;
        // トリプル栄和(0:無し(頭ハネ) 1:有り 2:流局(親連荘) 3:流局(親流れ))
        public int tRongHe = 1;
        // 借金立直
        public bool jieJinLiZhi = false;
        // 南場不聴連荘
        public bool nanChangBuTingLianZhuang = false;
        // 開始点
        public int kaiShiDian = 25000;
        // 返し点
        public int fanDian = 30000;
        // 九種九牌(0:無し 1:流局(親連荘) 2:流局(親流れ))
        public int jiuZhongJiuPaiLianZhuang = 1;
        // 四家立直(0:無し 1:流局(親連荘) 2:流局(親流れ))
        public int siJiaLiZhiLianZhuang = 1;
        // 四風子連打(0:無し 1:流局(親連荘) 2:流局(親流れ))
        public int siFengZiLianDaLianZhuang = 1;
        // 四開槓(0:流局(親連荘) 1:流局(親流れ))
        public int siKaiGangLianZhuang = 1;
        // 包則
        public bool baoZe = true;
        // 流し満貫
        public bool liuManGuan = false;
        // 三連刻
        public bool sanLianKe = false;
        // 燕返し
        public bool yanFan = false;
        // 開立直
        public bool kaiLiZhi = false;
        // 十三不塔
        public bool shiSanBuTa = false;
        // 八連荘
        public bool baLianZhuang = false;
        // ローカル役満
        public bool localYiMan = false;
    }
}
