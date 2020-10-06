using System;

[Serializable]
public struct EmailData
{
    public string Name;
    public string Subject;
    public string Message;
    public EmailConditionTypes ConditionTypes;
    public int AmountToRaise;
    public EmployeData EmployeData;
}