using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DebugAddCardButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_InputField;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        string _cardId = m_InputField.text;
        var desc = CardFactory.Instance.CreateDescriptor(_cardId);
        if (desc == null) return;

        CommandExecutor.Instance.Execute(new GameCommand
        {
            Type       = CommandType.AddCard,
            Descriptor = desc,
        });
    }
}
