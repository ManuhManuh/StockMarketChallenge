using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable 
{
    public void Subscribe(IObserver observer);
    public void Unsubscribe(IObserver observer);
    public void Notify();
}
