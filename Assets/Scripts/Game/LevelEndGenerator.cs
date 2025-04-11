using UnityEngine;

namespace MojoCase.Game
{
    public class LevelEndGenerator : MonoBehaviour
    {
        [SerializeField] private Block _blockPrefab;
        [SerializeField] private int[] _blockHealthValues;
        [SerializeField] private float _blockSpacingX;
        [SerializeField] private float _blockSpacingZ;
        [SerializeField] private int _blockCountInRow;

        [SerializeField] private Transform _finishLine;

        private void Start()
        {
            GenerateBlocks();
        }
        
        private void GenerateBlocks()
        {
            var blockXStartPosition = (_blockSpacingX * _blockCountInRow / 2 - _blockSpacingX / 2) * Vector3.left;
            var finishLinePos = transform.position;
            for (int blockRowIndex = 0; blockRowIndex < _blockHealthValues.Length; blockRowIndex++)
            {
                var blockZPosition = Vector3.forward * _blockSpacingZ * blockRowIndex;
                for (int blockIndexInRow = 0; blockIndexInRow < _blockCountInRow; blockIndexInRow++)
                {
                    var block = Instantiate(_blockPrefab, transform).GetComponent<Block>();
                    block.SetBlockHealth(_blockHealthValues[blockRowIndex]);
                    var blockPosition = Vector3.right * _blockSpacingX * blockIndexInRow + blockXStartPosition + blockZPosition + transform.position;
                    block.transform.position = blockPosition;
                }
                finishLinePos = blockZPosition + Vector3.forward * _blockSpacingZ;
            }
            _finishLine.position += finishLinePos;
        }
    }
}