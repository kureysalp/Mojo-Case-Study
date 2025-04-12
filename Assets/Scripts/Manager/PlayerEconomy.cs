using System;
using TMPro;
using UnityEngine;

namespace MojoCase.Manager
{
    public class PlayerEconomy : Singleton<PlayerEconomy>
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        private int _currentCoin;

        private void Start()
        {
            SetCoinText();
        }

        public int GetPlayerCoinAmount()
        {
            return PlayerPrefs.GetInt("Player_Coin");
        }

        public void AddCoin(int amount)
        {
            PlayerPrefs.SetInt("Player_Coin", GetPlayerCoinAmount() + amount);
            SetCoinText();
        }

        private void SetCoinText()
        {
            _coinText.text = GetPlayerCoinAmount().ToString();
        }
    }
}