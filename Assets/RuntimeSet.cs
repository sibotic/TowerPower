using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    public List<T> Items = new List<T>(); 
    public GameEvent ChangeEvent;

    public void Add(T t){
        if(!Items.Contains(t)){Items.Add(t);}
        if(ChangeEvent != null){ChangeEvent.Raise(Items.Count);}
    }

    public void Remove(T t){
        if(Items.Contains(t)){Items.Remove(t);}
        if(ChangeEvent != null){ChangeEvent.Raise(Items);}
    }

}



