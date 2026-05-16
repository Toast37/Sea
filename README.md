# Sea

一个用 Unity 开发的卡牌驱动游戏项目，所有内容（角色、道具、Buff、技能、事件）都做成可数据配置 + Mod 友好的结构。

---

## 整体架构一句话

```
ScriptableObject 数据 → Factory 实例化 → Manager 管理状态 → Command 改状态 → Event 广播 → UI 监听刷新
```

- **数据层**：`BaseCard / BaseCharacter / BaseSkill / BaseEvent` 都是 ScriptableObject，Inspector 配置
- **运行时**：`CardFactory` 按 id 实例化数据，各 `Manager` 持有运行时状态
- **修改入口**：所有数据变动走 `GameCommand` → `CommandExecutor`，方便事件、剧情、调试统一调用
- **同步机制**：状态变了 `GameManager.NotifyStateChanged()`，UI 订阅这个事件刷新自己

---

## Managers（每个 Manager 干什么）

### `BaseManager<T>`
泛型单例基类，所有 Manager 继承它。`Instance` 直接取。

### `GameManager`
**全局状态变更广播站**。任何数据修改完都 `GameManager.Instance.NotifyStateChanged()`，UI / 检查器订阅它的 `OnStateChanged` 事件。也持有 `TagManager` 和 `MetaStats` 的引用。

### `UIManager`
**面板注册与展示中心**。Inspector 配 `List<Panel>`，每个 Panel = `{Id, List<BasePanel> Members, OnShow, OnHide 事件}`。调 `Show(id, data)` 激活面板并广播；`Hide(id)` 关闭并广播。

### `CardFactory`
**SO 注册表 + 实例化工厂**。Inspector 配每个卡 / 角色 / 技能的 id 对应的 SO。提供：
- `Create(descriptor)`：按 descriptor 实例化运行时卡
- `CreateDescriptor(cardId)`：按 id 造一个默认 descriptor（用于"加新卡"场景）
- `CreateSkill(skillId)`：实例化技能

### `InventoryManager`
**手牌容器**。`Slots` 是当前手牌列表，提供 `Add / Remove / Reorder / Save` 操作。任何变动后通知 `GameManager`。

### `CharacterManager`
**队伍 + 装备协调器**。
- `Party`：当前队伍角色列表
- `CurrentCharacter`：当前激活角色
- `Equip / Unequip`：把卡装到角色的某个槽，调 `card.OnEquip(owner)` 注入归属
- `ApplyExtra / RemoveExtra`：处理 IExtra（道具/Buff）的属性修正

### `CommandExecutor`
**指令分发中心**。所有数据修改通过 `Execute(GameCommand)` 进入，内部按 `CommandType` 找 handler 跑。支持运行时 `Register` 新 handler（Mod 用）。

### `EventManager`
**事件池子**。Inspector 配永久池 / 随机池 / 计划池 / 当前池。提供按 Tag 索引的事件查询。

### `ConditionChecker`
**条件检查器**。把事件注册进来，每次条件可能满足时（例如 Tag 变化）检查，满足就触发事件的 `OnConditionMet`。

### `TagManager`
**标签集合**。`Add / Remove / Has`，配合事件系统的条件用。

### `DayLoop / DayState`
**日循环系统**。`DayState` 记录当前日，`DayLoop` 推进。

### `DontDestroy`
**跨场景持久化**。挂上的 GameObject 不会随场景切换销毁。

---

## Interfaces（每个接口定义什么）

### 基础接口（`BaseInterface/`）

| 接口 | 职责 |
|---|---|
| `IVisual` | 任何能显示的东西：`Icon` + `Description` |
| `IStats` | 属性查询：`maxHealth / Strength / Wisdom / Charisma`，按 `StatType` 查值 |
| `ICard` | **卡牌通用契约**：`CardId / Name / GetPanelKey / Capture / Restore / NewDescriptor / OnEquip(owner) / OnUnequip(owner)` |
| `ISlot` | **单格容器**：装 `ICard`，有 `Add / Remove / Has` 和 `ShouldAddCard / ShouldRemoveCard` 规则钩子 |
| `ISlotGroup` | **一组格子**：`Slots / AddCard / RemoveCard / GetAllCards` |
| `ISkill` | 技能定义：`OnGain / OnExpire / OnTrigger` 等钩子 |
| `IExtra` | 属性提供者：`StatModifiers` + `MetaStatModifiers`（物品 / Buff 实现） |
| `IEventResult` | 事件结果：`List<GameCommand> Commands`，跑完一组指令 |

### 组合接口（`AssembleInterface/`）

| 接口 | 组合 |
|---|---|
| `ICharacter` | `ICard + IStats + IConditionOwner`：再加 `Skills / SlotGroup / CurrentHealth / TakeDamage / Heal / AddMod / RemoveMod / OnSpawn / OnDead` |
| `IEvent` | 事件定义：`Name / Visibility / ActionCost / RequiresRoll / OnSuccess / OnFail / Conditions` |
| `ICondition` | 条件：`bool IsMet(...)` |
| `IReactiveCondition` | 响应式条件：能主动触发检查（不只是被动判断） |
| `IConditionOwner` | 持有条件的对象：`RequireAll / RequireAny` |

---

## 槽位系统（重要细节）

`ISlot` 是一个有规则的格子：

- `ItemSlot` 只收 `BaseItemCard`
- `BuffSlot` 只收 `BaseBuffCard`
- `CharacterSlot` 只收 `ICharacter`
- `HandSlot` 什么都收

`ISlotGroup` 有两种实现：

- **`FixedSlotGroup`**：构造时传死固定的 `List<ISlot>`，`AddCard` 找第一个空位塞（典型 RPG 装备栏）
- **`DynamicSlotGroup`**：构造时传 `slotFactory` + `maxCount`，`AddCard` 新建槽追加，支持 `Insert(index)` / `Move(from, to)`（顺序敏感的组合系统用）

---

## 数据流示例：玩家点按钮摸一张卡

```
按钮 OnClick
    ↓
CommandExecutor.Execute({ Type=AddCard, Descriptor=... })
    ↓
InventoryManager.Add(descriptor)
    ├─ CardFactory.Create(descriptor) → ICard 实例
    ├─ 加入 _slots / _descriptors
    └─ GameManager.NotifyStateChanged()
                ↓
HandGroupUI（订阅了 OnStateChanged）
    → Refresh()
        ├─ 比对 InventoryManager.Slots，从池取卡 / 归池多余卡
        ├─ 每张卡 SetActive + Init(slot) + Show()
        └─ UpdateLayout 飞到目标位置
```

---

## 数据流示例：玩家点卡片打开详情面板

```
HandSlotUI.OnClick
    ↓
UIManager.Show(card.GetPanelKey(), card)
    ↓
查 _registry 找到对应 Panel
    ├─ 遍历 Panel.Members 全部 SetActive(true)
    └─ Panel.OnShow?.Invoke(card)
                ↓
CardPanel（继承 BasePanel，订阅 OnShow）
    → OnReceive(card)
        ├─ _icon.sprite = card.Icon
        ├─ _description.text = card.Description
        └─ 是 ICharacter 时显示 血量/上限
```

---

## 数据流示例：装备道具触发 Buff 加属性

```
CharacterManager.Equip(character, swordCard, slot)
    ├─ slot.Add(swordCard)
    ├─ ApplyExtra(character, sword)  ← 加 Sword 自带的属性 mod
    ├─ swordCard.OnEquip(character)  ← BaseItemCard 看 _buffs 列表
    │       ├─ ApplyExtra(character, buff) ← 加 Buff 的属性 mod
    │       └─ buff.OnEquip(character)
    │             └─ 把 Buff 提供的 skill 加进 character.Skills
    └─ NotifyStateChanged
```

每张卡都通过 `OnEquip(owner)` 知道自己被谁装上，方便条件系统反向查询"我现在在谁身上"。

---

## Mod 友好设计点

1. **CardFactory 是 BaseManager 单例**：Mod 运行时可以 `CardFactory.Instance.Register(...)` 加新卡 / 新角色 / 新技能
2. **`CommandExecutor.Register(type, handler)`**：Mod 可以覆盖或新增指令实现
3. **`UIManager.Register(id, go)`**：Mod 可以运行时注册新面板
4. **CardDescriptor 用 `[SerializeReference]` 多态序列化**：Mod 可以自己定义 `XxxDescriptor : CardDescriptor` 类型，加自己的字段
5. **`Panel.OnShow / OnHide` 事件**：Mod 可以监听任何面板的显隐做副作用（音效、统计、教程引导、剧情演出）
6. **TagManager + ConditionChecker**：剧情事件不写死触发条件，配标签即可
7. **保存与恢复走 `Capture / Restore`**：Mod 加的新数据只要正确实现这两个方法就能存档

---

## 目录结构

```
Assets/00_Su/Scripts/
├── BaseClass/        # SO 基类 (BaseCard, BaseCharacter, BaseSkill...)
├── CardsTemple/      # 具体内容
│   ├── Buffs/        # 具体 Buff
│   ├── Characters/   # 具体角色
│   ├── Events/       # 具体事件
│   ├── Items/        # 具体道具
│   ├── Skills/       # 具体技能
│   └── Slots/        # ISlot / ISlotGroup 实现
├── Interface/        # 接口定义
├── Manager/          # 各种 Manager 单例
├── UI/               # UI 组件（BasePanel, HandGroupUI, CardPanel...）
└── Editor/           # 自定义 PropertyDrawer
```
