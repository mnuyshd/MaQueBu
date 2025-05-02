using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets.Source.Sikao.Shi
{
    // 学習雀士
    internal class QiaoXueXi : QiaoJiXie
    {
        private const string HOST_URL = "http://127.0.0.1:5000/";
        internal const string MING_QIAN = "学習雀士";
        internal QiaoXueXi() : base(MING_QIAN)
        {
            WaiBuSikao = true;
        }

        private IEnumerator RequestSikao(string sikao, State state)
        {
            AsyncStop = true;

            string json = JsonConvert.SerializeObject(state);
            byte[] raw = Encoding.UTF8.GetBytes(json);
            UnityWebRequest request = new(HOST_URL + sikao, "POST")
            {
                uploadHandler = new UploadHandlerRaw(raw),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ActionResponse response = JsonUtility.FromJson<ActionResponse>(request.downloadHandler.text);
                Debug.Log("action: yao=" + response.action[0] + " pai=0x" + response.action[1].ToString("x2"));
                YaoDingYi yao = (YaoDingYi)response.action[0];
                int pai = response.action[1];
                if (sikao == "SiKaoZiJia")
                {
                    ZiJiaYao = YaoDingYi.Wu;
                    ZiJiaXuanZe = ShouPai.Count - 1;
                    for (int i = 0; i < ShouPai.Count; i++)
                    {
                        if ((ShouPai[i] & QIAO_PAI) == pai)
                        {
                            ZiJiaXuanZe = i;
                            break;
                        }
                    }
                }
                else
                {
                    TaJiaYao = yao;
                    TaJiaXuanZe = pai;
                }
            }
            else
            {
                // Debug.Log(request.error);
                if (sikao == "SiKaoZiJia")
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

        // 思考自家
        internal override IEnumerator SiKaoZiJiaCoroutine()
        {
            if (LiZhi)
            {
                // 立直後自摸切
                ZiJiaYao = YaoDingYi.Wu;
                ZiJiaXuanZe = ShouPai.Count - 1;
                yield break;
            }

            yield return RequestSikao("SiKaoZiJia", transitionZiJia.state);
        }

        // 思考他家
        internal override IEnumerator SiKaoTaJiaCoroutine()
        {
            yield return RequestSikao("SiKaoTaJia", null);//transitionTaJia.state);
        }
    }

    [Serializable]
    public class ActionResponse
    {
        public List<int> action;
    }
}