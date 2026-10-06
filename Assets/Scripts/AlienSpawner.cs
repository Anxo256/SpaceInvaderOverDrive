using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    public GameObject AlienPrefab;
    public GameObject[] RowPrefabs;
    public int Rows = 5;
    public int Columns = 11;
    public float SpacingX = 1.2f;
    public float SpacingY = 1.0f;
    public Transform AliensParent;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float StartX = -((Columns - 1) * SpacingX) / 2f;
        float StartY = transform.position.y;

        for (int Row = 0; Row < Rows; Row++)
        {
            float PositionY = StartY - (Row * SpacingY);

            GameObject PrefabForRow = AlienPrefab;

            if (RowPrefabs != null && RowPrefabs.Length > 0)
            {
                PrefabForRow = RowPrefabs[Row % RowPrefabs.Length];
            }

            for (int Col = 0; Col < Columns; Col++)
            {
                float PositionX = StartX + (Col * SpacingX);
                Vector3 SpawnPosition = new Vector3(PositionX, PositionY, 0f);
                
                GameObject NewAlien = Instantiate(PrefabForRow, SpawnPosition, Quaternion.identity);
                
                if (AliensParent != null)
                {
                    NewAlien.transform.SetParent(AliensParent);
                }
            }
        }
    }
}
