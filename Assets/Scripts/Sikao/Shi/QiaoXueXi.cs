using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets.Scripts.Sikao.Shi
{
    // 学習雀士
    internal class QiaoXueXi : QiaoJiXie
    {
#if DEV
        private const string HOST_URL = "http://127.0.0.1:10000/";
#else
        private const string HOST_URL = "https://maquiaobu-api.onrender.com/";
#endif
        internal const string MING_QIAN = "学習雀士";
        internal QiaoXueXi() : base(MING_QIAN)
        {
            WaiBuSikao = true;
        }

        // 思考自家
        internal override IEnumerator SiKaoZiJiaCoroutine()
        {
            yield return RequestSiKao(true, transitionZiJia.state);
        }

        // 思考他家
        internal override IEnumerator SiKaoTaJiaCoroutine()
        {
            if (transitionTaJia == null)
            {
                TaJiaYao = YaoDingYi.Wu;
                TaJiaXuanZe = 0;
                yield break;
            }
            yield return RequestSiKao(false, transitionTaJia.state);
        }

        // 思考リクエスト
        private IEnumerator RequestSiKao(bool isZiJia, State state)
        {
            AsyncStop = true;
            string json = JsonConvert.SerializeObject(state);
            byte[] raw = Encoding.UTF8.GetBytes(json);
            UnityWebRequest request = new(HOST_URL + (isZiJia ? "SiKaoZiJia" : "SiKaoTaJia"), "POST")
            {
                uploadHandler = new UploadHandlerRaw(raw),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ActionResponse response = JsonUtility.FromJson<ActionResponse>(request.downloadHandler.text);
                YaoDingYi yao = (YaoDingYi)response.action[0];
                int paiOrIndex = response.action[1];
                Debug.Log("action: yao=" + yao + " paiOrIndex=0x" + paiOrIndex.ToString("x2"));

                if (isZiJia)
                {
                    SiKaoZiJiaResponse(yao, paiOrIndex);
                }
                else
                {
                    SiKaoTaJiaResponse(yao, paiOrIndex);
                }
            }
            else
            {
                Debug.Log("リクエスト失敗:" + request.error);
                if (isZiJia)
                {
                    SiKaoZiJia();
                }
                else
                {
                    SiKaoTaJia();
                }
            }

            AsyncStop = false;
        }

        private void SiKaoZiJiaResponse(YaoDingYi yao, int paiOrIndex)
        {
            if ((yao == YaoDingYi.ZiMo && !HeLe)
                || (yao == YaoDingYi.LiZhi && LiZhiPaiWei.Count == 0)
                || (yao == YaoDingYi.AnGang && AnGangPaiWei.Count == 0)
                || (yao == YaoDingYi.JiaGang && JiaGangPaiWei.Count == 0))
            {
                Debug.Log("SikaoZiJia エラー");
                ZiJiaYao = YaoDingYi.Wu;
                ZiJiaXuanZe = ShouPai.Count - 1;
                return;
            }

            // 和了判定
            if (HeLe)
            {
                // 自摸
                ZiJiaYao = YaoDingYi.ZiMo;
                ZiJiaXuanZe = ShouPai.Count - 1;
                return;
            }

            if (LiZhi)
            {
                if (yao == YaoDingYi.AnGang)
                {
                    // 立直後暗槓
                    ZiJiaYao = yao;
                    ZiJiaXuanZe = paiOrIndex;
                    return;
                }
                // 立直後自摸切
                ZiJiaYao = YaoDingYi.Wu;
                ZiJiaXuanZe = ShouPai.Count - 1;
                return;
            }

            ZiJiaYao = yao;
            if (yao == YaoDingYi.Wu)
            {
                ZiJiaXuanZe = ShouPai.Count - 1;
                for (int i = 0; i < ShouPai.Count; i++)
                {
                    if ((ShouPai[i] & QIAO_PAI) == paiOrIndex)
                    {
                        ZiJiaXuanZe = PaiXuanZe(i);
                        break;
                    }
                }
            }
            else
            {
                ZiJiaXuanZe = paiOrIndex;
            }
        }

        private void SiKaoTaJiaResponse(YaoDingYi yao, int pai)
        {
            if ((yao == YaoDingYi.DaMingGang && DaMingGangPaiWei.Count == 0)
                || (yao == YaoDingYi.Bing && BingPaiWei.Count == 0)
                || (yao == YaoDingYi.Chi && ChiPaiWei.Count == 0))
            {
                Debug.Log("SikaoTaJia エラー");
                TaJiaYao = YaoDingYi.Wu;
                TaJiaXuanZe = 0;
                return;
            }

            // 和了判定
            if (HeLe)
            {
                // 栄和
                TaJiaYao = YaoDingYi.RongHe;
                TaJiaXuanZe = 0;
                return;
            }

            TaJiaYao = yao;
            TaJiaXuanZe = pai;
        }
    }

    [Serializable]
    public class ActionResponse
    {
        public List<int> action;
    }
}