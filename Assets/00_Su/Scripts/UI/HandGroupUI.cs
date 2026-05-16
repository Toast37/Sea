using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandGroupUI : BaseSlotGroupUI
{
    [SerializeField] private HandSlotUI _cardPrefab;
    [SerializeField] private Transform  _spawnPoint;
    [SerializeField] private float _maxWidth     = 10f;
    [SerializeField] private float _maxSpacing   = 2f;
    [SerializeField] private float _detectRange  = 3f;
    [SerializeField] private float _staggerDelay = 0.05f;
    [SerializeField] private float _popOffset    = 200f;
    [SerializeField] private float _hoverScale   = 1.3f;
    [SerializeField] private float _hoverWidth   = 260f;
    [SerializeField] private float _hoverDelay   = 0.1f;

    public float HoverScale => _hoverScale;
    public float HoverWidth => _hoverWidth;
    public float HoverDelay => _hoverDelay;

    private RectTransform _rectTransform;
    private float[] _targetX = System.Array.Empty<float>();
    private int  _dragIndex  = -1;
    private int  _hoverIndex = -1;

    public bool IsDragging { get; private set; }

    private HandSlotUI Card(int i) => (HandSlotUI)_slotUIs[i];

    private float GetSpacing(int count) => count > 1
        ? Mathf.Min(_maxWidth / (count - 1), _maxSpacing)
        : _maxSpacing;

    private Vector3 CalcPosition(int visualIndex, int totalCount)
    {
        float x = (visualIndex - (totalCount - 1) / 2f) * GetSpacing(totalCount);
        return new Vector3(x, 0, 0);
    }

    [SerializeField] private int _poolSize = 10;
    private readonly List<HandSlotUI> _pool = new();

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        var found = new List<HandSlotUI>();
        GetComponentsInChildren<HandSlotUI>(true, found);
        foreach (var c in found) ReturnToPool(c);

        int toSpawn = _poolSize - _pool.Count;
        for (int i = 0; i < toSpawn; i++)
        {
            var card = Instantiate(_cardPrefab, transform);
            ReturnToPool(card);
        }
    }

    private void ReturnToPool(HandSlotUI card)
    {
        card.gameObject.SetActive(false);
        _pool.Add(card);
    }

    protected override void Init()
    {
        base.Init();
        Refresh();
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        foreach (var c in _slotUIs)
            ReturnToPool((HandSlotUI)c);
        _slotUIs.Clear();
    }

    protected override void Refresh()
    {
        var slots      = InventoryManager.Instance.Slots;
        bool fullReshow = _slotUIs.Count == 0;

        while (_slotUIs.Count > slots.Count)
        {
            ReturnToPool((HandSlotUI)_slotUIs[_slotUIs.Count - 1]);
            _slotUIs.RemoveAt(_slotUIs.Count - 1);
        }

        while (_slotUIs.Count < slots.Count)
        {
            HandSlotUI card;
            if (_pool.Count > 0)
            {
                card = _pool[_pool.Count - 1];
                _pool.RemoveAt(_pool.Count - 1);
            }
            else
            {
                card = Instantiate(_cardPrefab, transform);
            }
            if (!fullReshow && _spawnPoint != null)
                card.transform.position = _spawnPoint.position;
            card.gameObject.SetActive(true);
            _slotUIs.Add(card);
        }

        for (int i = 0; i < _slotUIs.Count; i++)
        {
            _slotUIs[i].Init(slots[i]);
            _slotUIs[i].Show();
        }

        _targetX = new float[_slotUIs.Count];
        SyncSiblingOrder();

        if (fullReshow) UpdateLayoutStagger();
        else            UpdateLayout();
    }

    public void UpdateLayout()
    {
        for (int i = 0; i < _slotUIs.Count; i++)
        {
            Vector3 pos = CalcPosition(i, _slotUIs.Count);
            _targetX[i] = pos.x;
            Card(i).MoveTo(pos);
        }
    }

    private void UpdateLayoutStagger()
    {
        for (int i = 0; i < _slotUIs.Count; i++)
        {
            Vector3 pos = CalcPosition(i, _slotUIs.Count);
            _targetX[i] = pos.x;
            Card(i).transform.localPosition = new Vector3(pos.x, -_popOffset, 0);
            Card(i).MoveTo(pos, i * _staggerDelay);
        }
    }

    public void OnCardHover(HandSlotUI card, float normalWidth)
    {
        int index = _slotUIs.IndexOf(card);
        if (index < 0) return;
        UpdateLayoutWithHover(index, normalWidth);
    }

    public void OnCardHoverExit() => UpdateLayout();

    private void UpdateLayoutWithHover(int hoverIndex, float normalWidth)
    {
        int n = _slotUIs.Count;
        if (n <= 1) return;

        float extraHalf = (_hoverWidth - normalWidth) / 2f;
        for (int i = 0; i < n; i++)
        {
            float posX = CalcPosition(i, n).x;
            if      (i < hoverIndex) posX -= extraHalf;
            else if (i > hoverIndex) posX += extraHalf;
            _targetX[i] = posX;
            Card(i).MoveTo(new Vector3(posX, 0, 0));
        }
    }

    public void BeginDrag(HandSlotUI card, PointerEventData eventData)
    {
        IsDragging  = true;
        _dragIndex  = _slotUIs.IndexOf(card);
        _hoverIndex = -1;
        card.transform.SetAsLastSibling();
        OnCardDrag(eventData);
    }

    public void OnCardDrag(PointerEventData eventData)
    {
        if (_dragIndex < 0) return;
        int hover = GetGapIndex();
        if (hover == _hoverIndex) return;
        _hoverIndex = hover;

        if (_hoverIndex < 0) UpdateLayoutCompact();
        else                 UpdateLayoutWithGap(_hoverIndex);
    }

    public void EndDrag(HandSlotUI card)
    {
        if (_dragIndex >= 0 && _hoverIndex >= 0)
        {
            int to = Mathf.Clamp(_hoverIndex, 0, _slotUIs.Count - 1);
            InventoryManager.Instance.Reorder(_dragIndex, to);
            _slotUIs.RemoveAt(_dragIndex);
            _slotUIs.Insert(to, card);
        }
        IsDragging  = false;
        _dragIndex  = -1;
        _hoverIndex = -1;
        SyncSiblingOrder();
        UpdateLayout();
    }

    private void UpdateLayoutCompact()
    {
        int n  = _slotUIs.Count - 1;
        int vi = 0;
        for (int i = 0; i < _slotUIs.Count; i++)
        {
            if (i == _dragIndex) continue;
            Vector3 pos = CalcPosition(vi, n);
            _targetX[i] = pos.x;
            Card(i).MoveTo(pos);
            vi++;
        }
    }

    private void UpdateLayoutWithGap(int gapIndex)
    {
        int n    = _slotUIs.Count;
        int vi   = 0;
        int slot = 0;
        for (int i = 0; i < _slotUIs.Count; i++)
        {
            if (i == _dragIndex) continue;
            if (vi == gapIndex) slot++;
            Vector3 pos = CalcPosition(slot, n);
            slot++;
            _targetX[i] = pos.x;
            Card(i).MoveTo(pos);
            vi++;
        }
    }

    private void SyncSiblingOrder()
    {
        for (int i = 0; i < _slotUIs.Count; i++)
            _slotUIs[i].transform.SetSiblingIndex(i);
    }

    private int GetGapIndex()
    {
        Vector2 localPos = _rectTransform.InverseTransformPoint(
            _slotUIs[_dragIndex].transform.position);

        if (Mathf.Abs(localPos.y) > _detectRange) return -1;

        int gapIndex = 0;
        int vi       = 0;
        for (int i = 0; i < _slotUIs.Count; i++)
        {
            if (i == _dragIndex) continue;
            if (localPos.x > _targetX[i]) gapIndex = vi + 1;
            vi++;
        }
        return gapIndex;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position;
        Gizmos.DrawLine(center + Vector3.left  * _maxWidth / 2f,
                        center + Vector3.right * _maxWidth / 2f);

        Gizmos.color = Color.yellow;
        foreach (var card in _slotUIs)
        {
            if (card == null) continue;
            var rt = card.GetComponent<RectTransform>();
            if (rt == null) continue;
            Vector3 worldPos    = transform.TransformPoint(rt.localPosition);
            float   worldRadius = _detectRange * transform.lossyScale.x;
            Gizmos.DrawWireSphere(worldPos, worldRadius);
        }
    }
}
