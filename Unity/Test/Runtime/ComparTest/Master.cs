using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour// IComparable<Master>
{
    public int Age;
    public string Name;


    class MyClass
    {
        
    }
    // public int CompareTo(Master m)
    // {
    //     if (this.Age == m.Age)
    //     {
    //         if (this.Name.Length > m.Name.Length)
    //         {
    //             return 1;
    //         }
    //         else if(this.Name.Length == m.Name.Length)
    //         {
    //             return 0;
    //         }
    //         else
    //         {
    //             return -1;
    //         }
    //     }
    //     else
    //     {
    //         if (this.Age > m.Age)
    //         {
    //             return 1;
    //         }
    //         else if (this.Age == m.Age)
    //         {
    //             return 0;
    //         }
    //         else
    //         {
    //             return -1;
    //         }
    //     }
    // }

    public override string ToString()
    {
        return $"名字:{Name},Age:{this.Age}";
    }
}