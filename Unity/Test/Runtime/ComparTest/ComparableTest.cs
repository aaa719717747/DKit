using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComparableTest : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
       List<Master> mList= new List<Master>
        {
            new Master{Age = 60,Name = "好"},
            new Master{Age = 18,Name = "李四"},
            new Master{Age = 32,Name = "代号XT"},
            new Master{Age = 18,Name = "西藏高原卫视"},
            new Master{Age = 18,Name = "山东台"},
            new Master{Age = 18,Name = "东南卫视"},
        };


       Dictionary<string, int> dict = new Dictionary<string, int>();
       dict.Add("你",2);
       dict.Add("你好",1);
       dict.Add("你好啊",5);
       dict.Add("你好啊!",4);
       dict.Add("你好啊!兄弟",3);

       List<KeyValuePair<string, int>> pairs = new List<KeyValuePair<string, int>>(dict);
       
       pairs.Sort((x,y)=>x.Value.CompareTo(y.Value));
       
       // mList.Sort((x,y)=>x.Age.CompareTo(y.Age));
       // mList.Sort((x, y) =>
       // {
       //     if (x.Age.CompareTo(y.Age)==0)
       //     {
       //         return x.Name.Length.CompareTo(y.Name.Length);
       //     }
       //     return x.Age.CompareTo(y.Age);
       // });
       
       
       foreach (var VARIABLE in pairs)
       {
           Debug.LogWarning(VARIABLE.Key);
       }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
