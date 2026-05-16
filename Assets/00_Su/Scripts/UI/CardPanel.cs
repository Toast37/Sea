using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPanel : BasePanel
{
    [SerializeField] private Image           _icon;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private Button          _closeButton;

    private void OnEnable()  => _closeButton.onClick.AddListener(OnClose);
    private void OnDisable() => _closeButton.onClick.RemoveListener(OnClose);

    protected override void OnReceive(object data)
    {
        if (data is not ICard card) return;

        _icon.sprite      = card.Icon;
        _description.text = card.Description;

        if (card is ICharacter character)
        {
            _health.gameObject.SetActive(true);
            _health.text = $"{character.CurrentHealth}/{character.maxHealth}";
        }
        else
        {
            _health.gameObject.SetActive(false);
        }
    }

    private void OnClose() => Hide();
}
