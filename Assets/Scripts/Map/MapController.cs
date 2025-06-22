using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkRadius;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    Vector3 playerLastPosition;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    public GameObject latestChunk;
    public float maxOpDist; //במכüרו קול עאיכלאן
    float opDist;
    float opCooldown;
    public float opCooldownDur;
    void Start()
    {
        playerLastPosition = player.transform.position;
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }
    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }

        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDir);

        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(directionName).position, checkRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(directionName).position);

            if(directionName.Contains("Up") && directionName.Contains("Right"))
            {
                if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Up").position);
                }
                if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Right").position);
                }
            } else if (directionName.Contains("Up") && directionName.Contains("Left"))
            {
                if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Up").position);
                }
                if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Left").position);
                }
            }else if (directionName.Contains("Down") && directionName.Contains("Right"))
            {
                if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Down").position);
                }
                if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Right").position);
                }
            }else if (directionName.Contains("Down") && directionName.Contains("Left"))
            {
                if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Down").position);
                }
                if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Left").position);
                }
            }
        }
        
    }

    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) 
        {
            if(direction.y > 0.5f)
            {
                return direction.x > 0 ? "Right Up" : "Left Up";
            }
            else if (direction.y < -0.5f)
            {
                return direction.x > 0 ? "Right Down" : "Left Down";
            }
            else
            {
                return direction.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            if (direction.x > 0.5f)
            {
                return direction.y > 0 ? "Right Up" : "Right Down";
            }
            else if (direction.x < -0.5f)
            {
                return direction.y > 0 ? "Left Up" : "Left Down";
            }
            else
            {
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 spawnPosition)
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], spawnPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }
    void ChunkOptimizer()
    {
        opCooldown -= Time.deltaTime;
        if (opCooldown <= 0f) 
        {
            opCooldown = opCooldownDur;
        }
        else
        {
            return;
        }
        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist) 
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
