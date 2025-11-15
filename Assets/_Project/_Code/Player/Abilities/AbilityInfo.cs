using UnityEngine;

public static class AbilityInfo
{
    public static string Name(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.JumpModule:
                return "Прыжковый модуль";
            case AbilityType.Dash:
                return "Реактивные сапоги";
            default:
                break;
        }
        return "Нет данных";
    }

    public static string Info(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.JumpModule:
                return "Позволяет прыгать на клавишу Space";
            case AbilityType.Dash:
                return "Позволяет ускоряться в направлении движения на Alt";
            default:
                break;
        }
        return "Нет данных";
    }
}
