using UnityEngine;

public class Weapon : MonoBehaviour
{
    public class SkillNode
    {
        public IWeaponSkill skill;
        public SkillNode next;
        public SkillNode prev;
    }

    [SerializeField] AnimatorOverrideController _overrideController;

    public WeaponHandler Handler { get; private set; }
    public AmmoInventory Inventory { get; private set; }

    IWeaponSkill _primarySkill, _secondarySkill, _tertiarySkill, _reloadSkill;
    SkillNode[] _nodes = new SkillNode[4];
    SkillNode _head = null;
    int _stackSize = 0;

    private void Awake()
    {
        IWeaponSkill[] skills = GetComponents<IWeaponSkill>();
        foreach (IWeaponSkill skill in skills)
        {
            switch (skill.Type)
            {
                case SkillType.Primary:
                    _primarySkill = skill;
                    break;
                case SkillType.Secondary:
                    _secondarySkill = skill;
                    break;
                case SkillType.Tertiary:
                    _tertiarySkill = skill;
                    break;
                case SkillType.Reload:
                    _reloadSkill = skill;
                    break;
                default:
                    continue;
            }
        }

        _primarySkill = _primarySkill ?? new EmptySkill();
        _secondarySkill = _secondarySkill ?? new EmptySkill();
        _tertiarySkill = _tertiarySkill ?? new EmptySkill();
        _reloadSkill = _reloadSkill ?? new EmptySkill();

        for (int i = 0; i < _nodes.Length; i++)
            _nodes[i] = new SkillNode();
        _nodes[0].skill = _primarySkill;
        _nodes[1].skill = _secondarySkill;
        _nodes[2].skill = _tertiarySkill;
        _nodes[3].skill = _reloadSkill;

        Inventory = GetComponent<AmmoInventory>();
    }

    private void OnEnable()
    {
        _primarySkill.Deactivated += PopFromStack;
        _secondarySkill.Deactivated += PopFromStack;
        _tertiarySkill.Deactivated += PopFromStack;
        _reloadSkill.Deactivated += PopFromStack;

        _primarySkill.ResourceExpended += PerformReload;
        _secondarySkill.ResourceExpended += PerformReload;
        _tertiarySkill.ResourceExpended += PerformReload;
    }

    private void OnDisable()
    {
        _primarySkill.Deactivated -= PopFromStack;
        _secondarySkill.Deactivated -= PopFromStack;
        _tertiarySkill.Deactivated -= PopFromStack;
        _reloadSkill.Deactivated -= PopFromStack;

        _primarySkill.ResourceExpended += PerformReload;
        _secondarySkill.ResourceExpended += PerformReload;
        _tertiarySkill.ResourceExpended += PerformReload;
    }

    public void PerformPrimaryAttack(bool isStarting)
    {
        PushToStack(_primarySkill, isStarting);
    }

    public void PerformSecondaryAttack(bool isStarting)
    {
        PushToStack(_secondarySkill, isStarting);
    }

    public void PerformTertiaryAttack(bool isStarting)
    {
        PushToStack(_tertiarySkill, isStarting);
    }

    public void PerformReload()
    {
        PushToStack(_reloadSkill);
    }

    public void Ready(WeaponHandler handler)
    {
        Handler = handler;
    }

    public void Store()
    {
        ClearStack();
        gameObject.SetActive(false);
    }

    public IWeaponSkill GetSkill(SkillType type)
    {
        switch (type)
        {
            case SkillType.Primary:
                return _primarySkill.Type != SkillType.Empty ? _primarySkill : null;
            case SkillType.Secondary:
                return _secondarySkill.Type != SkillType.Empty ? _secondarySkill : null;
            case SkillType.Tertiary:
                return _tertiarySkill.Type != SkillType.Empty ? _tertiarySkill : null;
            case SkillType.Reload:
                return _reloadSkill.Type != SkillType.Empty ? _reloadSkill : null;
            default:
                return null;
        }
    }

    void PushToStack(IWeaponSkill skill, bool isStarting = true)
    {
        if (_stackSize >= _nodes.Length)
            return;

        SkillNode correspondingNode = _nodes[(int)skill.Type];
        SkillNode temp, iterator;

        if (!skill.CanPerform(isStarting))
            return;

        if (skill.Obstructs(isStarting))
        {
            if (_head == null)
            {
                _head = correspondingNode;
                skill.Perform(isStarting);
                goto SkipIteration;
            }
            else if (_head.skill.Type == skill.Type)
            {
                skill.Perform(isStarting);
                goto SkipIteration;
            }
            else if (_head.skill.Priority > skill.Priority)
            {
                temp = _head;
                _head = correspondingNode;
                correspondingNode.prev = null;
                correspondingNode.next = temp;
                temp.prev = correspondingNode;
                temp.skill.Pause();
                skill.Perform(isStarting);
                goto SkipIteration;
            }

            iterator = _head;
            while (true)
            {
                if (iterator.next == null)
                {
                    iterator.next = correspondingNode;
                    correspondingNode.prev = iterator;
                    correspondingNode.skill.Perform(isStarting, true);
                    break;
                }
                else if (iterator.next.skill.Priority > skill.Priority)
                {
                    temp = iterator.next;
                    iterator.next = correspondingNode;
                    correspondingNode.prev = iterator;
                    correspondingNode.next = temp;
                    temp.prev = correspondingNode;
                    iterator.next.skill.Perform(isStarting, true);
                    break;
                }

                iterator = iterator.next;
            }

            SkipIteration:
            _stackSize++;
        }
        else
            skill.Perform(isStarting);

        PrintStack();
    }

    void PopFromStack(IWeaponSkill skill)
    {
        SkillNode correspondingNode = _nodes[(int)skill.Type];
        SkillNode temp;

        if (_head == null)
            return;

        temp = correspondingNode.next;
        if (correspondingNode.prev != null)
            correspondingNode.prev.next = temp;
        if (temp != null)
            temp.prev = correspondingNode.prev;

        correspondingNode.next = null;
        correspondingNode.prev = null;

        if (_head.skill.Type == skill.Type)
        {
            _head = temp;
            if (_head != null)
                _head.skill.Resume();
        }
        _stackSize--;

        PrintStack();
    }

    void ClearStack()
    {
        SkillNode iterator = _head, temp;
        while (iterator != null)
        {
            temp = iterator.next;
            iterator.next = null;
            iterator.prev = null;
            iterator.skill.Halt();
            iterator = temp;
        }
        _stackSize = 0;

        PrintStack();
    }

    void PrintStack()
    {
        string output = "Stack: ";
        SkillNode temp = _head;
        while (temp != null)
        {
            output += temp.skill.Type.ToString() + " ";
            temp = temp.next;
        }
        print(output);
    }
}
