using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver 
{

    public void UpdatedInfo(IObservable subject);

}
