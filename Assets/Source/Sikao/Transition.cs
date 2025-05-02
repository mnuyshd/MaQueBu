using System.Collections.Generic;

namespace Assets.Source.Sikao
{
    // 遷移
    public class Transition
    {
        // 雀士
        public string mingQian;
        // 状態
        public State state;
        // 行動
        public List<int> action;
    }

    // 状態
    [System.Serializable]
    public class State
    {
        // 場風
        public int changFeng;
        // 局
        public int ju;

        // 懸賞牌
        public List<int> xuanShangPai;

        // 手牌数
        public List<int> shouPaiShu;
        // 副露牌数
        public List<int> fuLuPaiShu;
        // 捨牌
        public List<int> shePai;
        // 立直
        public bool liZhi;

        // 他家捨牌
        public List<List<int>> taJiaShePai = new();
    }
}
