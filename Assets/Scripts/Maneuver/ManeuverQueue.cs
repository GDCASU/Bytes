/*
 * Author: Cristion Dominguez
 * Date: 9 Jan. 2023
 */

using System;
using UnityEngine;

public class Maneuver
{
    // Only the Manuever Queue should alter these variables.
    public Maneuver prev;
    public Maneuver next;
    public Action<Maneuver> dequeuing;
    // These variables are able to be freely read and modified.
    public readonly string name;
    public readonly int priority;
    Action<bool> _performing;
    Action _pausing;
    Action _resuming;
    Action _halting;

    public bool InQueue => prev != null;

    public Maneuver(string name, int priority, Action<bool> performing, Action pausing, Action resuming, Action halting)
    {
        this.name = name;
        this.priority = priority;
        _performing = performing;
        _pausing = pausing;
        _resuming = resuming;
        _halting = halting;
    }

    public void Perform(bool pauseImmediately) => _performing?.Invoke(pauseImmediately);
    public void Pause() => _pausing?.Invoke();
    public void Resume() => _resuming?.Invoke();
    public void Halt() => _halting?.Invoke();
    public void Dequeue() => dequeuing?.Invoke(this);
}

public class ManeuverQueue : MonoBehaviour
{
    [SerializeField] int _max;
    [SerializeField] bool _shouldPrint;
    int _count;
    Maneuver _entry;

    private void Awake()
    {
        _entry = new Maneuver("Queue Entry", -1, null, null, null, null);
    }

    public void Enqueue(Maneuver maneuver)
    {
        if (_count >= _max || maneuver.prev != null)
            return;

        maneuver.dequeuing = Dequeue;
        Maneuver temp, iterator = _entry;
        while (true)
        {
            if (iterator.next == null)
            {
                iterator.next = maneuver;
                maneuver.prev = iterator;
                break;
            }
            else if (iterator.next.priority > maneuver.priority)
            {
                temp = iterator.next;
                iterator.next = maneuver;
                maneuver.prev = iterator;
                maneuver.next = temp;
                temp.prev = maneuver;
                break;
            }

            iterator = iterator.next;
        }

        _count++;
        if (_shouldPrint) Print();

        if (maneuver.prev.prev == null)
        {
            maneuver.Perform(false);
        }
        else
        {
            maneuver.Perform(true);
        }
    }

    public void Dequeue(Maneuver maneuver)
    {
        if (_count <= 0 || maneuver.prev == null)
            return;

        maneuver.dequeuing = null;
        Maneuver temp = maneuver.next;
        maneuver.prev.next = temp;
        if (temp != null)
            temp.prev = maneuver.next;

        maneuver.prev = null;
        maneuver.next = null;

        _count--;
        if (_shouldPrint) Print();

        if (temp != null && temp.prev.prev == null)
            temp.Resume();
    }

    public void Clear()
    {
        _count = 0;

        Maneuver temp, iterator = _entry;
        while (iterator != null)
        {
            temp = iterator.next;
            iterator.prev = null;
            iterator.next = null;
            iterator.Halt();
            iterator = temp;
        }
    }

    void Print()
    {
        string output = "Queue: ";
        Maneuver iterator = _entry;
        while (iterator != null)
        {
            output += iterator.name + ", ";
            iterator = iterator.next;
        }
        Debug.Log(output);
    }
}