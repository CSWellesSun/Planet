using UnityEngine;
using System.Linq;
using System.Numerics;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;

public class Land :MonoBehaviour, IClickable
//地块类
{
    public Vector3Int pos;
    public Planet planet;
    public bool OnClick(Vector3 startPos,Vector3 activeData)
    {
        GameManager.instance.EditPlanet(this,activeData);
        return false;
    }
    public Vector3Int GetPos(Vector3 dir)
    {
        Vector3 nextPos = transform.InverseTransformDirection(dir);
        return new Vector3Int(pos.x + Mathf.RoundToInt(nextPos.x), pos.y + Mathf.RoundToInt(nextPos.y), pos.z + Mathf.RoundToInt(nextPos.z));
    }
}

/// <summary>
/// 地块类型
/// </summary>
public class LandData: NftObject
{
    public string landKind;
    public Texture landTexture;

    public bool IsSurface()
    {
        return !(landKind == "empty" || landKind == "underground");
    }

    public override JObject Serialize()
    {
        JObject res = new JObject();
        res["tokenID"] = tokenId.ToString();
        res["landKind"] = landKind;
        return res;
    }

    public override IEnumerator DeSerialize(JObject jobj)
    {
        tokenId = BigInteger.Parse(jobj["tokenID"].ToString());
        landKind = jobj["landKind"].ToString();

        while (BlockchainManager.instance.nfts == null)
            yield return null;
        if (!nftObjDic.ContainsKey(tokenId))
        {
            Nft nft = BlockchainManager.instance.nfts.Select(x => x.tokenId = tokenId) as Nft;

            AssetBundle nftBundle = AssetBundle.LoadFromMemory(nft.imgData);
            nftImage = nftBundle.LoadAllAssets<Texture>()[0];
            nftBundle = AssetBundle.LoadFromMemory(nft.nftData);
            landTexture = nftBundle.LoadAllAssets<Texture>()[0];
            nftObjDic.Add(tokenId, this);
        }
        else
        {
            var obj = nftObjDic[tokenId] as LandData;
            nftImage = obj.nftImage;
            landTexture = obj.landTexture;
            nftObjDic[tokenId].nftImage = null;
        }
    }
}