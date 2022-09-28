using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GamePieceUi : MonoBehaviour
    {
        public AbstractGamePieceController AbstractGamePieceController { get; private set; }
        private GameObject _gamePiece;
        public List<Vector3> TargetPosition = new List<Vector3>();
        public float speed;
        public bool IsMoving { get; private set; } = false;
        public bool IsDestroyable { get; private set; } = false;
        void Update()
        {
            if (IsMoving)
            {
                MoveCloserToTargetPosition();
            }
        }
        public void Initialize(GameObject map, GameObject GamePiecePrefab, Vector3 pixelPosition, float size, AbstractGamePieceController abstractGamePieceController)
        {
            AbstractGamePieceController = abstractGamePieceController;
            _gamePiece = Instantiate(GamePiecePrefab) as GameObject;
            _gamePiece.transform.position = pixelPosition;
            _gamePiece.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            _gamePiece.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            _gamePiece.transform.SetParent(map.transform, false);
        }
        public static void ReactivateMovementOfAll(List<GamePieceUi> gamePieces)
        {
            foreach (var gamePiece in gamePieces)
            {
                gamePiece.IsMoving = true;
                gamePiece.UpdateStatus();
            }
        }
        public void ReactivateMovement()
        {
            IsMoving = true;
        }

        public void MoveCloserToTargetPosition()
        {
            var newPosition = Vector2.MoveTowards(_gamePiece.transform.localPosition, TargetPosition[0], speed * (float)0.005);
            _gamePiece.transform.localPosition = new Vector3(newPosition.x, newPosition.y, 0);
            if (_gamePiece.transform.localPosition == TargetPosition[0])
            {
                TargetPosition.RemoveAt(0);
                IsMoving = false;
            }
        }
        public void SetTargetPosition(Vector3 position)
        {
            TargetPosition.Add(position);
        }

        public Vector2 GetAnchoredPosition()
        {
            return _gamePiece.GetComponent<RectTransform>().anchoredPosition;
        }

        public Vector2 GetSize()
        {
            return _gamePiece.GetComponent<RectTransform>().sizeDelta;
        }
        public static void SetSpeedOfAll(float speed, List<GamePieceUi> gamePieces)
        {
            foreach (var gamePiece in gamePieces)
            {
                gamePiece.speed = speed;
            }
        }

        public static GamePieceUi GetGamePieceUiByAbstractGamePieceController(List<GamePieceUi> gamePieceUis, AbstractGamePieceController abstractGamePieceController)
        {
            foreach(var gamePieceUi in gamePieceUis)
            {
                if(gamePieceUi.AbstractGamePieceController == abstractGamePieceController)
                {
                    return gamePieceUi;
                }
            }
            return null;
        }

        public static int GetLargestCountOfTargetedPositions(List<GamePieceUi> gamePieces)
        {
            int mostTargetPositions = 0;
            foreach (var gamePiece in gamePieces)
            {
                if (mostTargetPositions < gamePiece.TargetPosition.Count)
                {
                    mostTargetPositions = gamePiece.TargetPosition.Count;
                }
            }
            return mostTargetPositions;
        }

        public static bool HaveAllFinishedMovement(List<GamePieceUi> gamePieces)
        {
            bool allFinishedMovment = true;
            foreach (var gamePiece in gamePieces)
            {
                if (gamePiece.IsMoving || gamePiece.TargetPosition.Count == 0)
                {
                    allFinishedMovment = false;
                }
            }
            return allFinishedMovment;
        }

        public static GameObject GetGameObjectByGamePieceController(List<GamePieceUi> gamePieceUis, AbstractGamePieceController gamePieceController)
        {
            foreach(var gamePieceUi in gamePieceUis)
            {
                if(gamePieceUi.AbstractGamePieceController == gamePieceController)
                {
                    return gamePieceUi._gamePiece;
                } 
            }
            return null;
        }

        private void UpdateStatus()
        {
            if (AbstractGamePieceController is GamePieceCloneController && !IsDestroyable)
            {
                GameObject hpGameObject;
                var i = 5;
                do
                {
                    i--;
                    hpGameObject = HelperMethods.GetChildByName(_gamePiece, i + "hp");
                } while (!hpGameObject.activeInHierarchy);
                if(i > 0)
                {
                    var nextHpGameObject = HelperMethods.GetChildByName(_gamePiece, i-1 + "hp");
                    nextHpGameObject.SetActive(true);
                }
                else
                {
                    IsDestroyable = true;
                }
                hpGameObject.SetActive(false);
            }
        }

        public void Destroy()
        {
            IsDestroyable = true;
            _gamePiece.SetActive(false);
        }
    }
}
