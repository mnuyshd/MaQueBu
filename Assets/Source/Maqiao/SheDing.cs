namespace Assets.Source.Maqiao
{
    // 設定
    public class SheDing
    {
        // 鳴無し
        public bool mingWu = false;
        // 立直後自動
        public bool liZhiAuto = true;
        // 打牌方法(0:１タップ打牌 1:２タップ打牌)
        public int daPaiFangFa = 1;
        // ドラマーク表示
        public bool xuanShangYin = true;
        // ツモ切表示有り
        public bool ziMoQieBiaoShi = true;
        // 待ち牌表示有り
        public bool daiPaiBiaoShi = true;
        // 向聴数表示有り
        public bool xiangTingShuBiaoShi = true;
        // 鳴パスはボタン
        public bool mingQuXiao = true;
        // 手牌点表示無し
        public bool shouPaiDianBiaoShi = false;
        // 相手牌オープン
        public bool xiangShouPaiOpen = false;
        // 学習データ作成無し
        public bool learningData = false;
    }
}