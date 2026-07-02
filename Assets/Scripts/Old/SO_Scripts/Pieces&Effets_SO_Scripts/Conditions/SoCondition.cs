using System;
using System.Collections.Generic;
using UnityEngine;


public class  SoCondition : ScriptableObject
{
    public virtual bool Condition (ConditionOutput conditionOutput)
    {
        return false; 
    }

}


[Serializable]
public class ConditionOutput
{
    public Context context;
    public OutputPort port;
    public List<int> variableList;
}
