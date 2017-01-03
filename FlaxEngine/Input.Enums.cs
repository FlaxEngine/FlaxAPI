// Flax Engine scripting API

namespace FlaxEngine
{
    public enum MouseButtons
    {
        None = 0,
        Left = 1,
        Right = 2,
        Middle = 4,
        XButton1 = 8,
        XButton2 = 16,
    }

    public enum KeyCode
    {
        // TODO: create more platform independent key mappings, without OEM stuff etc.
        // Note: these are just raw values copied right from C++ source

        // BACKSPACE key
        BACK = 0x08,

        // TAB key
        TAB = 0x09,

        // CLEAR key
        CLEAR = 0x0C,

        // ENTER key
        RETURN = 0x0D,

        // SHIFT key
        SHIFT = 0x10,

        // CTRL key
        CONTROL = 0x11,

        // ALT key
        ALT = 0x12,

        // PAUSE key
        PAUSE = 0x13,

        // CAPS LOCK key
        CAPITAL = 0x14,

        // IME Kana mode
        KANA = 0x15,

        // IME Hanguel mode (maintained for compatibility; use HANGUL)
        HANGUEL = 0x15,

        // IME Hangul mode
        HANGUL = 0x15,

        // IME Junja mode
        JUNJA = 0x17,

        // IME final mode
        FINAL = 0x18,

        // IME Hanja mode
        HANJA = 0x19,

        // IME Kanji mode
        KANJI = 0x19,

        // ESC key
        ESCAPE = 0x1B,

        // IME convert
        CONVERT = 0x1C,

        // IME nonconvert
        NONCONVERT = 0x1D,

        // IME accept
        ACCEPT = 0x1E,

        // IME mode change request
        MODECHANGE = 0x1F,

        // SPACEBAR
        SPACE = 0x20,

        // PAGE UP key
        PRIOR = 0x21,

        // PAGE DOWN key
        NEXT = 0x22,

        // END key
        END = 0x23,

        // HOME key
        HOME = 0x24,

        // LEFT ARROW key
        LEFT = 0x25,

        // UP ARROW key
        UP = 0x26,

        // RIGHT ARROW key
        RIGHT = 0x27,

        // DOWN ARROW key
        DOWN = 0x28,

        // SELECT key
        SELECT = 0x29,

        // PRINT key
        PRINT = 0x2A,

        // EXECUTE key
        EXECUTE = 0x2B,

        // PRINT SCREEN key
        SNAPSHOT = 0x2C,

        // INS key
        INSERT = 0x2D,

        // DEL key
        DELETE = 0x2E,

        // HELP key
        HELP = 0x2F,

        // 0 key
        Alpha0 = 0x30,

        // 1 key
        Alpha1 = 0x31,

        // 2 key
        Alpha2 = 0x32,

        // 3 key
        Alpha3 = 0x33,

        // 4 key
        Alpha4 = 0x34,

        // 5 key
        Alpha5 = 0x35,

        // 6 key
        Alpha6 = 0x36,

        // 7 key
        Alpha7 = 0x37,

        // 8 key
        Alpha8 = 0x38,

        // 9 key
        Alpha9 = 0x39,

        // A key
        A = 0x41,

        // B key
        B = 0x42,

        // C key
        C = 0x43,

        // D key
        D = 0x44,

        // E key
        E = 0x45,

        // F key
        F = 0x46,

        // G key
        G = 0x47,

        // H key
        H = 0x48,

        // I key
        I = 0x49,

        // J key
        J = 0x4A,

        // K key
        K = 0x4B,

        // L key
        L = 0x4C,

        // M key
        M = 0x4D,

        // N key
        N = 0x4E,

        // O key
        O = 0x4F,

        // P key
        P = 0x50,

        // Q key
        Q = 0x51,

        // R key
        R = 0x52
        ,
        // S key
        S = 0x53,

        // T key
        T = 0x54,

        // U key
        U = 0x55,

        // V key
        V = 0x56,

        // W key
        W = 0x57,

        // X key
        X = 0x58,

        // Y key
        Y = 0x59,

        // Z key
        Z = 0x5A,

        // Left Windows key (Natural keyboard)
        LWIN = 0x5B,

        // Right Windows key (Natural keyboard)
        RWIN = 0x5C,

        // Applications key (Natural keyboard)
        APPS = 0x5D,

        // Computer Sleep key
        SLEEP = 0x5F,

        // Numeric keypad 0 key
        NUMPAD0 = 0x60,

        // Numeric keypad 1 key
        NUMPAD1 = 0x61,

        // Numeric keypad 2 key
        NUMPAD2 = 0x62,

        // Numeric keypad 3 key
        NUMPAD3 = 0x63,

        // Numeric keypad 4 key
        NUMPAD4 = 0x64,

        // Numeric keypad 5 key
        NUMPAD5 = 0x65,

        // Numeric keypad 6 key
        NUMPAD6 = 0x66,

        // Numeric keypad 7 key
        NUMPAD7 = 0x67,

        // Numeric keypad 8 key
        NUMPAD8 = 0x68,

        // Numeric keypad 9 key
        NUMPAD9 = 0x69,

        // Multiply key
        MULTIPLY = 0x6A,

        // Add key
        ADD = 0x6B,

        // Separator key
        SEPARATOR = 0x6C,

        // Subtract key
        SUBTRACT = 0x6D,

        // Decimal key
        DECIMAL = 0x6E,

        // Divide key
        DIVIDE = 0x6F,

        // F1 key
        F1 = 0x70,

        // F2 key
        F2 = 0x71,

        // F3 key
        F3 = 0x72,

        // F4 key
        F4 = 0x73,

        // F5 key
        F5 = 0x74,

        // F6 key
        F6 = 0x75,

        // F7 key
        F7 = 0x76,

        // F8 key
        F8 = 0x77,

        // F9 key
        F9 = 0x78,

        // F10 key
        F10 = 0x79,

        // F11 key
        F11 = 0x7A,

        // F12 key
        F12 = 0x7B,

        // F13 key
        F13 = 0x7C,

        // F14 key
        F14 = 0x7D,

        // F15 key
        F15 = 0x7E,

        // F16 key
        F16 = 0x7F,

        // F17 key
        F17 = 0x80,

        // F18 key
        F18 = 0x81,

        // F19 key
        F19 = 0x82,

        // F20 key
        F20 = 0x83,

        // F21 key
        F21 = 0x84,

        // F22 key
        F22 = 0x85,

        // F23 key
        F23 = 0x86,

        // F24 key
        F24 = 0x87,

        // NUM LOCK key
        NUMLOCK = 0x90,

        // SCROLL LOCK key
        SCROLL = 0x91,

        // Left SHIFT key
        LSHIFT = 0xA0,

        // Right SHIFT key
        RSHIFT = 0xA1,

        // Left CONTROL key
        LCONTROL = 0xA2,

        // Right CONTROL key
        RCONTROL = 0xA3,

        // Left MENU key
        LMENU = 0xA4,

        // Right MENU key
        RMENU = 0xA5,

        // Browser Back key
        BROWSER_BACK = 0xA6,

        // Browser Forward key
        BROWSER_FORWARD = 0xA7,

        // Browser Refresh key
        BROWSER_REFRESH = 0xA8,

        // Browser Stop key
        BROWSER_STOP = 0xA9,

        // Browser Search key
        BROWSER_SEARCH = 0xAA,

        // Browser Favorites key
        BROWSER_FAVORITES = 0xAB,

        // Browser Start and Home key
        BROWSER_HOME = 0xAC,

        // Volume Mute key
        VOLUME_MUTE = 0xAD,

        // Volume Down key
        VOLUME_DOWN = 0xAE,

        // Volume Up key
        VOLUME_UP = 0xAF,

        // Next Track key
        MEDIA_NEXT_TRACK = 0xB0,

        // Previous Track key
        MEDIA_PREV_TRACK = 0xB1,

        // Stop Media key
        MEDIA_STOP = 0xB2,

        // Play/Pause Media key
        MEDIA_PLAY_PAUSE = 0xB3,

        // Start Mail key
        LAUNCH_MAIL = 0xB4,

        // Select Media key
        LAUNCH_MEDIA_SELECT = 0xB5,

        // Start Application 1 key
        LAUNCH_APP1 = 0xB6,

        // Start Application 2 key
        LAUNCH_APP2 = 0xB7,

        // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard the ';:' key
        OEM_1 = 0xBA,

        // For any country/region the '+' key
        OEM_PLUS = 0xBB,

        // For any country/region the '' key
        OEM_COMMA = 0xBC,

        // For any country/region the '-' key
        OEM_MINUS = 0xBD,

        // For any country/region the '.' key
        OEM_PERIOD = 0xBE,

        // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard the '/?' key
        OEM_2 = 0xBF,

        // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard the '`~' key
        OEM_3 = 0xC0,

        // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard the '[{' key
        OEM_4 = 0xDB,

        // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard the '\\|' key
        OEM_5 = 0xDC,

        // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard the ']}' key
        OEM_6 = 0xDD,

        // Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard the 'single-quote/double-quote' key
        OEM_7 = 0xDE,

        // Used for miscellaneous characters; it can vary by keyboard.
        OEM_8 = 0xDF,

        // Either the angle bracket key or the backslash key on the RT 102-key keyboard
        OEM_102 = 0xE2,

        // IME PROCESS key
        PROCESSKEY = 0xE5,

        // Used to pass Unicode characters as if they were keystrokes. The PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods.
        PACKET = 0xE7,

        // Attn key
        ATTN = 0xF6,

        // CrSel key
        CRSEL = 0xF7,

        // ExSel key
        EXSEL = 0xF8,

        // Erase EOF key
        EREOF = 0xF9,

        // Play key
        PLAY = 0xFA,

        // Zoom key
        ZOOM = 0xFB,

        // PA1 key
        PA1 = 0xFD,

        // Clear key
        OEM_CLEAR = 0xFE,
    }
}
