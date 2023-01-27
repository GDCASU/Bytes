/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System;
using UnityEngine;

public abstract class PoolItem: MonoBehaviour
{
    public Action ReturningToPool { protected get; set; }
    public abstract void OnGet();
    public abstract void OnPoolDisposed();
}