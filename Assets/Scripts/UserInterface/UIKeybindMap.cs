using UnityEngine;

public static class UIKeybindMap
{
    /***********************************************
    * KeybindToString: Returns usable string for UI from keycode
    * @author: Nathan Hunt
    * @parameter: Keycode
    * @return: static string
    ************************************************/

    public static string KeybindToString(KeyCode keycode)
    {
        switch(keycode)
        {
            case KeyCode.Alpha1: return "1";
            case KeyCode.Alpha2: return "2";
            case KeyCode.Alpha3: return "3";
            case KeyCode.Alpha4: return "4";
            case KeyCode.Alpha5: return "5";
            case KeyCode.Alpha6: return "6";
            case KeyCode.Alpha7: return "7";
            case KeyCode.Alpha8: return "8";
            case KeyCode.Alpha9: return "9";
            case KeyCode.Alpha0: return "0";
        }
        return keycode.ToString();
    }
}
