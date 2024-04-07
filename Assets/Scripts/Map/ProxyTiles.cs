using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "ProxyTiles", menuName = "ProxyTile", order = 0)]
public class ProxyTiles : TileBase
{
    public Sprite DefaultSprite;
    public GameObject DefaultGameObject;
    public Tile.ColliderType DefaultColliderType = Tile.ColliderType.Sprite;
    public TileWeight[] TileWeights;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
    {
        if (instantiatedGameObject != null)
        {
            Tilemap tmpMap = tilemap.GetComponent<Tilemap>();
            Matrix4x4 orientMatrix = tmpMap.orientationMatrix;

            Vector3 gameObjectTranslation = new Vector3(orientMatrix.m03, orientMatrix.m13, orientMatrix.m23);
            Quaternion gameObjectRotation = Quaternion.LookRotation(new Vector3(orientMatrix.m02, orientMatrix.m12, orientMatrix.m22), new Vector3(orientMatrix.m01, orientMatrix.m11, orientMatrix.m21));
            Vector3 gameObjectScale = orientMatrix.lossyScale;

            instantiatedGameObject.transform.localPosition = gameObjectTranslation + tmpMap.CellToLocalInterpolated(position + tmpMap.tileAnchor);
            instantiatedGameObject.transform.localRotation = gameObjectRotation;
            instantiatedGameObject.transform.localScale = gameObjectScale;
        }

        return true;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        var iden = Matrix4x4.identity;

        tileData.sprite = DefaultSprite;
        tileData.gameObject = DefaultGameObject;
        tileData.colliderType = DefaultColliderType;
        tileData.flags = TileFlags.LockTransform;
        tileData.transform = iden;

        Matrix4x4 transform = iden;
        int totalWeight = TileWeights.Sum(x => x.weight);
        int randomInt = UnityEngine.Random.Range(0, totalWeight);

        foreach (TileWeight rule in TileWeights)
        {
            if (randomInt < rule.weight)
            {
                tileData.sprite = rule.Sprite;
                tileData.transform = transform;
                tileData.gameObject = rule.GameObject;
                tileData.colliderType = rule.ColliderType;
                break;
            }
            randomInt -= rule.weight;
        }
    }

    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        Matrix4x4 transform = Matrix4x4.identity;
        // foreach (TilingRule rule in m_TilingRules)
        // {
        //     if (rule.m_Output == TilingRuleOutput.OutputSprite.Animation)
        //     {
        //         if (RuleMatches(rule, position, tilemap, ref transform))
        //         {
        //             tileAnimationData.animatedSprites = rule.m_Sprites;
        //             tileAnimationData.animationSpeed = Random.Range( rule.m_MinAnimationSpeed, rule.m_MaxAnimationSpeed);
        //             return true;
        //         }
        //     }
        // }
        return false;
    }
}

[Serializable]
public struct TileWeight
{
    public Sprite Sprite;
    public GameObject GameObject;
    public Tile.ColliderType ColliderType;
    public int weight;
}